using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using Core;
using Core.Domain.Seo;

namespace Web.Framework.UI
{
    public partial class SayfaHeadOluşturucu : ISayfaHeadOluşturucu
    {
        #region Fields

        private static readonly object s_lock = new object();

        private readonly SeoAyarları _seoAyarları;
        private readonly List<string> _baslikParçaları;
        private readonly List<string> _metaDescriptionParçaları;
        private readonly List<string> _metaKeywordParçaları;
        private readonly Dictionary<KaynakKonumu, List<ScriptReferenceMeta>> _scriptParçaları;
        private readonly Dictionary<KaynakKonumu, List<CssReferenceMeta>> _cssParçaları;
        private readonly List<string> _canonicalUrlParçaları;
        private readonly List<string> _özelHeadParçaları;
        private readonly List<string> _sayfaCssSınıfıParçaları;
        private string _düzenleSayfaUrlsi;
        private string _aktifAdminMenuSistemAdı;

        #endregion

        #region Ctor

        public SayfaHeadOluşturucu(SeoAyarları seoAyarları)
        {
            this._seoAyarları = seoAyarları;
            this._baslikParçaları = new List<string>();
            this._metaDescriptionParçaları = new List<string>();
            this._metaKeywordParçaları = new List<string>();
            this._scriptParçaları = new Dictionary<KaynakKonumu, List<ScriptReferenceMeta>>();
            this._cssParçaları = new Dictionary<KaynakKonumu, List<CssReferenceMeta>>();
            this._canonicalUrlParçaları = new List<string>();
            this._özelHeadParçaları = new List<string>();
            this._sayfaCssSınıfıParçaları = new List<string>();
        }

        #endregion

        #region Utilities

        protected virtual string PaketSanalYolunuAl(string sonek, string uzantı, string[] parçalar)
        {
            if (parçalar == null || parçalar.Length == 0)
                throw new ArgumentException("parçalar");

            //hash hesapla
            var hash = "";
            using (SHA256 sha = new SHA256Managed())
            {
                // string birleştirme
                var hashGirdisi = "";
                foreach (var parça in parçalar)
                {
                    hashGirdisi += parça;
                    hashGirdisi += ",";
                }

                byte[] girdi = sha.ComputeHash(Encoding.Unicode.GetBytes(hashGirdisi));
                hash = HttpServerUtility.UrlTokenEncode(girdi);
            }
            //Sadece uygun karekterler olduğunu denetle
            //hash = SeoExtensions.GetSeName(hash);

            var sb = new StringBuilder(sonek);
            sb.Append(hash);
            return sb.ToString();
        }

        protected virtual IItemTransform CssDönüşümüAl()
        {
            return new CssRewriteUrlTransform();
        }

        #endregion

        #region Methods

        public virtual void BaşlıkParçasıEkle(string parça)
        {
            if (string.IsNullOrEmpty(parça))
                return;

            _baslikParçaları.Add(parça);
        }
        public virtual void BaşlıkParçasıIlaveEt(string parça)
        {
            if (string.IsNullOrEmpty(parça))
                return;

            _baslikParçaları.Insert(0, parça);
        }

        public virtual string BaşlıkOluştur(bool varsayılanBaşlığıEkle)
        {
            string sonuç = "";
            
            var özelBaşlık = string.Join(_seoAyarları.SayfaBaşlığıAyırıcısı, _baslikParçaları.AsEnumerable().Reverse().ToArray());
            if (!String.IsNullOrEmpty(özelBaşlık))
            {
                if (varsayılanBaşlığıEkle)
                {
                    //site adı + sayfa başlığı
                    switch (_seoAyarları.SayfaBaşlığıSeoAyarı)
                    {
                        case SayfaBaşlığıSeoAyarı.SayfaAdıSonraSiteAdı:
                            {
                                sonuç = string.Join(_seoAyarları.SayfaBaşlığıAyırıcısı, _seoAyarları.VarsayılanBaşlık, özelBaşlık);
                            }
                            break;
                        case SayfaBaşlığıSeoAyarı.SiteAdıSonraSayfaAdı:
                        default:
                            {
                                sonuç = string.Join(_seoAyarları.SayfaBaşlığıAyırıcısı, özelBaşlık, _seoAyarları.VarsayılanBaşlık);
                            }
                            break;
                    }
                }
                else
                {
                    //sadece sayfa balığı
                    sonuç = özelBaşlık;
                }
            }
            else
            {
                //sadece site adı
                sonuç = _seoAyarları.VarsayılanBaşlık;
            }
            return sonuç;
        }

        public virtual void MetaDescriptionParçasıEkle(string parça)
        {
            if (string.IsNullOrEmpty(parça))
                return;

            _metaDescriptionParçaları.Add(parça);
        }
        public virtual void MetaDescriptionParçasıIlaveEt(string parça)
        {
            if (string.IsNullOrEmpty(parça))
                return;

            _metaDescriptionParçaları.Insert(0, parça);
        }
        public virtual string MetaDescriptionOluştur()
        {
            var metaDescription = string.Join(", ", _metaDescriptionParçaları.AsEnumerable().Reverse().ToArray());
            var sonuç = !String.IsNullOrEmpty(metaDescription) ? metaDescription : "";
            return sonuç;
        }


        public virtual void MetaKeywordParçasıEkle(string parça)
        {
            if (string.IsNullOrEmpty(parça))
                return;

            _metaKeywordParçaları.Add(parça);
        }
        public virtual void MetaKeywordParçasıIlaveEt(string parça)
        {
            if (string.IsNullOrEmpty(parça))
                return;

            _metaKeywordParçaları.Insert(0, parça);
        }
        public virtual string MetaKeywordsOluştur()
        {
            var metaKeyword = string.Join(", ", _metaKeywordParçaları.AsEnumerable().Reverse().ToArray());
            var sonuç = !String.IsNullOrEmpty(metaKeyword) ? metaKeyword : "";
            return sonuç;
        }


        public virtual void ScriptParçasıEkle(KaynakKonumu konum, string parça, bool pakettinDışında, bool isAsync)
        {
            if (!_scriptParçaları.ContainsKey(konum))
                _scriptParçaları.Add(konum, new List<ScriptReferenceMeta>());

            if (string.IsNullOrEmpty(parça))
                return;

            _scriptParçaları[konum].Add(new ScriptReferenceMeta
            {
                pakettinDışında = pakettinDışında,
                IsAsync = isAsync,
                Parça = parça
            });
        }
        public virtual void ScriptParçasıIlaveEt(KaynakKonumu konum, string parça, bool pakettinDışında, bool isAsync)
        {
            if (!_scriptParçaları.ContainsKey(konum))
                _scriptParçaları.Add(konum, new List<ScriptReferenceMeta>());

            if (string.IsNullOrEmpty(parça))
                return;

            _scriptParçaları[konum].Insert(0, new ScriptReferenceMeta
            {
                pakettinDışında = pakettinDışında,
                IsAsync = isAsync,
                Parça = parça
            });
        }
        public virtual string ScriptsOluştur(UrlHelper urlHelper, KaynakKonumu konum, bool? paketDosyaları = null)
        {
            if (!_scriptParçaları.ContainsKey(konum) || _scriptParçaları[konum] == null)
                return "";

            if (!_scriptParçaları.Any())
                return "";

            if (!paketDosyaları.HasValue)
            {
                //Herhangi bir değer belirtilmemişse ayarı kullanın
                paketDosyaları = _seoAyarları.JSPaketlemeyeIzinVer && BundleTable.EnableOptimizations;
            }

            if (paketDosyaları.Value)
            {
                var pakettenParçalar = _scriptParçaları[konum]
                    .Where(x => !x.pakettinDışında)
                    .Select(x => x.Parça)
                    .Distinct()
                    .ToArray();
                var paketinDışındakiParçalar = _scriptParçaları[konum]
                    .Where(x => x.pakettinDışında)
                    .Select(x => new { x.Parça, x.IsAsync })
                    .Distinct()
                    .ToArray();


                var sonuç = new StringBuilder();

                if (pakettenParçalar.Length > 0)
                {
                    string paketSanalYolu = PaketSanalYolunuAl("~/bundles/scripts/", ".js", pakettenParçalar);
                    //paket oluştur
                    lock (s_lock)
                    {
                        var paketIçin = BundleTable.Bundles.GetBundleFor(paketSanalYolu);
                        if (paketIçin == null)
                        {
                            var paket = new ScriptBundle(paketSanalYolu);
                            //bundle.Transforms.Clear();

                            //sıralanmış olarak
                            paket.Orderer = new PaketSıralayıcı();
                            //disable file extension replacements. renders scripts which were specified by a developer
                            paket.EnableFileExtensionReplacements = false;
                            paket.Include(pakettenParçalar);
                            BundleTable.Bundles.Add(paket);
                        }
                    }

                    //paket parçaları
                    sonuç.AppendLine(Scripts.Render(paketSanalYolu).ToString());
                }

                //paketten olmayan parçalar
                foreach (var item in paketinDışındakiParçalar)
                {
                    sonuç.AppendFormat("<script {2}src=\"{0}\" type=\"{1}\"></script>", urlHelper.Content(item.Parça), MimeTipleri.TextJavascript, item.IsAsync ? "async " : "");
                    sonuç.Append(Environment.NewLine);
                }
                return sonuç.ToString();
            }
            else
            {
                //bundling is disabled
                var sonuç = new StringBuilder();
                foreach (var item in _scriptParçaları[konum].Select(x => new { x.Parça, x.IsAsync }).Distinct())
                {
                    sonuç.AppendFormat("<script {2}src=\"{0}\" type=\"{1}\"></script>", urlHelper.Content(item.Parça), MimeTipleri.TextJavascript, item.IsAsync ? "async " : "");
                    sonuç.Append(Environment.NewLine);
                }
                return sonuç.ToString();
            }
        }


        public virtual void CssParçasıEkle(KaynakKonumu konum, string parça, bool pakettinDışında = false)
        {
            if (!_cssParçaları.ContainsKey(konum))
                _cssParçaları.Add(konum, new List<CssReferenceMeta>());

            if (string.IsNullOrEmpty(parça))
                return;

            _cssParçaları[konum].Add(new CssReferenceMeta
            {
                pakettinDışında = pakettinDışında,
                Parça = parça
            });
        }
        public virtual void CssParçasıIlaveEt(KaynakKonumu konum, string parça, bool pakettinDışında = false)
        {
            if (!_cssParçaları.ContainsKey(konum))
                _cssParçaları.Add(konum, new List<CssReferenceMeta>());

            if (string.IsNullOrEmpty(parça))
                return;

            _cssParçaları[konum].Insert(0, new CssReferenceMeta
            {
                pakettinDışında = pakettinDışında,
                Parça = parça
            });
        }
        public virtual string CssOluştur(UrlHelper urlHelper, KaynakKonumu konum, bool? paketDosyaları = null)
        {
            if (!_cssParçaları.ContainsKey(konum) || _cssParçaları[konum] == null)
                return "";

            if (!_cssParçaları.Any())
                return "";

            if (!paketDosyaları.HasValue)
            {
                //Herhangi bir değer belirtilmemişse ayarı kullanın
                paketDosyaları = _seoAyarları.CssPaketlemeyeIzinVer && BundleTable.EnableOptimizations;
            }

            if (paketDosyaları.Value)
            {
                var pakettenParçalar = _cssParçaları[konum]
                    .Where(x => !x.pakettinDışında)
                    .Select(x => x.Parça)
                    .Distinct()
                    .ToArray();
                var paketinDışındakiParçalar = _cssParçaları[konum]
                    .Where(x => x.pakettinDışında)
                    .Select(x => x.Parça)
                    .Distinct()
                    .ToArray();


                var sonuç = new StringBuilder();

                if (pakettenParçalar.Length > 0)
                {
                    // ÖNEMLİ: Sanal dizinlerdeki CSS paketlemeyi kullanma
                    string paketSanalYolu = PaketSanalYolunuAl("~/bundles/styles/", ".css", pakettenParçalar);

                    //paket oluştur
                    lock (s_lock)
                    {
                        var paketIçin = BundleTable.Bundles.GetBundleFor(paketSanalYolu);
                        if (paketIçin == null)
                        {
                            var paket = new StyleBundle(paketSanalYolu);
                            //bundle.Transforms.Clear();

                            //sıralanmış olarak
                            paket.Orderer = new PaketSıralayıcı();
                            //dosya uzantısı değiştirmelerini devre dışı bırak. Bir geliştirici tarafından belirtilen betikleri işle
                            paket.EnableFileExtensionReplacements = false;
                            foreach (var ptb in pakettenParçalar)
                            {
                                paket.Include(ptb, CssDönüşümüAl());
                            }
                            BundleTable.Bundles.Add(paket);
                        }
                    }

                    //paket parçaları
                    sonuç.AppendLine(Styles.Render(paketSanalYolu).ToString());
                }

                //paketin dışındaki parçalar
                foreach (var item in paketinDışındakiParçalar)
                {
                    sonuç.AppendFormat("<link href=\"{0}\" rel=\"stylesheet\" type=\"{1}\" />", urlHelper.Content(item), MimeTipleri.TextCss);
                    sonuç.Append(Environment.NewLine);
                }

                return sonuç.ToString();
            }
            else
            {
                //paketleme devredışı
                var sonuç = new StringBuilder();
                foreach (var yol in _cssParçaları[konum].Select(x => x.Parça).Distinct())
                {
                    sonuç.AppendFormat("<link href=\"{0}\" rel=\"stylesheet\" type=\"{1}\" />", urlHelper.Content(yol), MimeTipleri.TextCss);
                    sonuç.AppendLine();
                }
                return sonuç.ToString();
            }
        }


        public virtual void CanonicalUrlParçasıEkle(string parça)
        {
            if (string.IsNullOrEmpty(parça))
                return;

            _canonicalUrlParçaları.Add(parça);
        }
        public virtual void CanonicalUrlParçasıIlaveEt(string parça)
        {
            if (string.IsNullOrEmpty(parça))
                return;

            _canonicalUrlParçaları.Insert(0, parça);
        }
        public virtual string CanonicalUrlOluştur()
        {
            var sonuç = new StringBuilder();
            foreach (var canonicalUrl in _canonicalUrlParçaları)
            {
                sonuç.AppendFormat("<link rel=\"canonical\" href=\"{0}\" />", canonicalUrl);
                sonuç.Append(Environment.NewLine);
            }
            return sonuç.ToString();
        }


        public virtual void ÖzelHeadParçasıEkle(string parça)
        {
            if (string.IsNullOrEmpty(parça))
                return;

            _özelHeadParçaları.Add(parça);
        }
        public virtual void ÖzelHeadParçasıIlaveEt(string parça)
        {
            if (string.IsNullOrEmpty(parça))
                return;

            _özelHeadParçaları.Insert(0, parça);
        }
        public virtual string ÖzelHeadOluştur()
        {
            //Sadece farklı satırlar kullan
            var farklıParçalar = _özelHeadParçaları.Distinct().ToList();
            if (!farklıParçalar.Any())
                return "";

            var sonuç = new StringBuilder();
            foreach (var yol in farklıParçalar)
            {
                sonuç.Append(yol);
                sonuç.Append(Environment.NewLine);
            }
            return sonuç.ToString();
        }


        public virtual void SayfaCssSınıfıParçasıEkle(string parça)
        {
            if (string.IsNullOrEmpty(parça))
                return;

            _sayfaCssSınıfıParçaları.Add(parça);
        }
        public virtual void SayfaCssSınıfıParçasıIlaveEt(string parça)
        {
            if (string.IsNullOrEmpty(parça))
                return;

            _sayfaCssSınıfıParçaları.Insert(0, parça);
        }
        public virtual string SayfaCssSınıfıOluştur()
        {
            string sonuç = string.Join(" ", _sayfaCssSınıfıParçaları.AsEnumerable().Reverse().ToArray());
            return sonuç;
        }
        public virtual void DüzenleSayfaURLsiEkle(string url)
        {
            _düzenleSayfaUrlsi = url;
        }
        public virtual string DüzenleSayfaURLsiAl()
        {
            return _düzenleSayfaUrlsi;
        }
        public virtual void AktifMenuÖğesiSistemAdıBelirle(string systemName)
        {
            _aktifAdminMenuSistemAdı = systemName;
        }
        public virtual string AktifMenuÖğesiSistemAdıAl()
        {
            return _aktifAdminMenuSistemAdı;
        }

        #endregion

        #region Nested classes

        private class ScriptReferenceMeta
        {
            public bool pakettinDışında { get; set; }

            public bool IsAsync { get; set; }

            public string Parça { get; set; }
        }

        private class CssReferenceMeta
        {
            public bool pakettinDışında { get; set; }

            public string Parça { get; set; }
        }
        #endregion
    }
}
