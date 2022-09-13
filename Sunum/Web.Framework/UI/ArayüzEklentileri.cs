using System.Web.Mvc;
using Core.Altyapı;

namespace Web.Framework.UI
{
    public static class ArayüzEklentileri
    {
        public static void BaşlıkParçasıEkle(this HtmlHelper html, string parça)
        {
            var sayfaHeadOluşturucu = EngineContext.Current.Resolve<ISayfaHeadOluşturucu>();
            sayfaHeadOluşturucu.BaşlıkParçasıEkle(parça);
        }
        public static void BaşlıkParçasıIlaveEt(this HtmlHelper html, string parça)
        {
            var sayfaHeadOluşturucu = EngineContext.Current.Resolve<ISayfaHeadOluşturucu>();
            sayfaHeadOluşturucu.BaşlıkParçasıIlaveEt(parça);
        }
        public static MvcHtmlString Başlık(this HtmlHelper html, bool VarsayılanBaşlıkEkle = true, string parça = "")
        {
            var sayfaHeadOluşturucu = EngineContext.Current.Resolve<ISayfaHeadOluşturucu>();
            html.BaşlıkParçasıIlaveEt(parça);
            return MvcHtmlString.Create(html.Encode(sayfaHeadOluşturucu.BaşlıkOluştur(VarsayılanBaşlıkEkle)));
        }
        public static void MetaDescriptionParçasıEkle(this HtmlHelper html, string parça)
        {
            var sayfaHeadOluşturucu = EngineContext.Current.Resolve<ISayfaHeadOluşturucu>();
            sayfaHeadOluşturucu.MetaDescriptionParçasıEkle(parça);
        }
        public static void MetaDescriptionParçasıIlaveEt(this HtmlHelper html, string parça)
        {
            var sayfaHeadOluşturucu = EngineContext.Current.Resolve<ISayfaHeadOluşturucu>();
            sayfaHeadOluşturucu.MetaDescriptionParçasıIlaveEt(parça);
        }
        public static MvcHtmlString MetaDescription(this HtmlHelper html, string parça = "")
        {
            var sayfaHeadOluşturucu = EngineContext.Current.Resolve<ISayfaHeadOluşturucu>();
            html.MetaDescriptionParçasıIlaveEt(parça);
            return MvcHtmlString.Create(html.Encode(sayfaHeadOluşturucu.MetaDescriptionOluştur()));
        }
        public static void MetaKeywordParçasıEkle(this HtmlHelper html, string parça)
        {
            var sayfaHeadOluşturucu = EngineContext.Current.Resolve<ISayfaHeadOluşturucu>();
            sayfaHeadOluşturucu.MetaKeywordParçasıEkle(parça);
        }
        public static void MetaKeywordParçasıIlaveEt(this HtmlHelper html, string parça)
        {
            var sayfaHeadOluşturucu = EngineContext.Current.Resolve<ISayfaHeadOluşturucu>();
            sayfaHeadOluşturucu.MetaKeywordParçasıIlaveEt(parça);
        }
        public static MvcHtmlString MetaKeywords(this HtmlHelper html, string parça = "")
        {
            var sayfaHeadOluşturucu = EngineContext.Current.Resolve<ISayfaHeadOluşturucu>();
            html.MetaKeywordParçasıIlaveEt(parça);
            return MvcHtmlString.Create(html.Encode(sayfaHeadOluşturucu.MetaKeywordsOluştur()));
        }
        public static void ScriptParçasıEkle(this HtmlHelper html, string parça, bool pakettenÇıkar = false, bool async = false)
        {
            ScriptParçasıEkle(html, KaynakKonumu.Head, parça, pakettenÇıkar, async);
        }
        public static void ScriptParçasıEkle(this HtmlHelper html, KaynakKonumu konum, string parça, bool pakettenÇıkar = false, bool async = false)
        {
            var sayfaHeadOluşturucu = EngineContext.Current.Resolve<ISayfaHeadOluşturucu>();
            sayfaHeadOluşturucu.ScriptParçasıEkle(konum, parça, pakettenÇıkar, async);
        }
        public static void ScriptParçasıIlaveEt(this HtmlHelper html, string parça, bool pakettenÇıkar = false, bool async = false)
        {
            ScriptParçasıIlaveEt(html, KaynakKonumu.Head, parça, pakettenÇıkar, async);
        }
        public static void ScriptParçasıIlaveEt(this HtmlHelper html, KaynakKonumu konum, string parça, bool pakettenÇıkar = false, bool async = false)
        {
            var sayfaHeadOluşturucu = EngineContext.Current.Resolve<ISayfaHeadOluşturucu>();
            sayfaHeadOluşturucu.ScriptParçasıIlaveEt(konum, parça, pakettenÇıkar, async);
        }
        public static MvcHtmlString Scripts(this HtmlHelper html, UrlHelper urlHelper,
            KaynakKonumu konum, bool? paketDosyaları = null)
        {
            var sayfaHeadOluşturucu = EngineContext.Current.Resolve<ISayfaHeadOluşturucu>();
            return MvcHtmlString.Create(sayfaHeadOluşturucu.ScriptsOluştur(urlHelper, konum, paketDosyaları));
        }
        public static void CssParçasıEkle(this HtmlHelper html, string parça, bool pakettenÇıkar = false)
        {
            CssParçasıEkle(html, KaynakKonumu.Head, parça, pakettenÇıkar);
        }
        public static void CssParçasıEkle(this HtmlHelper html, KaynakKonumu konum, string parça, bool pakettenÇıkar = false)
        {
            var sayfaHeadOluşturucu = EngineContext.Current.Resolve<ISayfaHeadOluşturucu>();
            sayfaHeadOluşturucu.CssParçasıEkle(konum, parça, pakettenÇıkar);
        }
        public static void CssParçasıIlaveEt(this HtmlHelper html, string parça, bool pakettenÇıkar = false)
        {
            CssParçasıIlaveEt(html, KaynakKonumu.Head, parça, pakettenÇıkar);
        }
        public static void CssParçasıIlaveEt(this HtmlHelper html, KaynakKonumu konum, string parça, bool pakettenÇıkar = false)
        {
            var sayfaHeadOluşturucu = EngineContext.Current.Resolve<ISayfaHeadOluşturucu>();
            sayfaHeadOluşturucu.CssParçasıIlaveEt(konum, parça, pakettenÇıkar);
        }
        public static MvcHtmlString CssFiles(this HtmlHelper html, UrlHelper urlHelper,
            KaynakKonumu konum, bool? paketDosyaları = null)
        {
            var sayfaHeadOluşturucu = EngineContext.Current.Resolve<ISayfaHeadOluşturucu>();
            return MvcHtmlString.Create(sayfaHeadOluşturucu.CssOluştur(urlHelper, konum, paketDosyaları));
        }
        public static void CanonicalUrlParçasıEkle(this HtmlHelper html, string parça)
        {
            var sayfaHeadOluşturucu = EngineContext.Current.Resolve<ISayfaHeadOluşturucu>();
            sayfaHeadOluşturucu.CanonicalUrlParçasıEkle(parça);
        }
        public static void CanonicalUrlParçasıIlaveEt(this HtmlHelper html, string parça)
        {
            var sayfaHeadOluşturucu = EngineContext.Current.Resolve<ISayfaHeadOluşturucu>();
            sayfaHeadOluşturucu.CanonicalUrlParçasıIlaveEt(parça);
        }
        public static MvcHtmlString CanonicalUrls(this HtmlHelper html, string parça = "")
        {
            var sayfaHeadOluşturucu = EngineContext.Current.Resolve<ISayfaHeadOluşturucu>();
            html.CanonicalUrlParçasıIlaveEt(parça);
            return MvcHtmlString.Create(sayfaHeadOluşturucu.CanonicalUrlOluştur());
        }
        public static void ÖzelHeadParçasıEkle(this HtmlHelper html, string parça)
        {
            var sayfaHeadOluşturucu = EngineContext.Current.Resolve<ISayfaHeadOluşturucu>();
            sayfaHeadOluşturucu.ÖzelHeadParçasıEkle(parça);
        }
        public static void ÖzelHeadParçasıIlaveEt(this HtmlHelper html, string parça)
        {
            var sayfaHeadOluşturucu = EngineContext.Current.Resolve<ISayfaHeadOluşturucu>();
            sayfaHeadOluşturucu.ÖzelHeadParçasıIlaveEt(parça);
        }
        public static MvcHtmlString ÖzelHead(this HtmlHelper html)
        {
            var sayfaHeadOluşturucu = EngineContext.Current.Resolve<ISayfaHeadOluşturucu>();
            return MvcHtmlString.Create(sayfaHeadOluşturucu.ÖzelHeadOluştur());
        }
        public static void SayfaCssSınıfıParçasıEkle(this HtmlHelper html, string parça)
        {
            var sayfaHeadOluşturucu = EngineContext.Current.Resolve<ISayfaHeadOluşturucu>();
            sayfaHeadOluşturucu.SayfaCssSınıfıParçasıEkle(parça);
        }
        public static void SayfaCssSınıfıParçasıIlaveEt(this HtmlHelper html, string parça)
        {
            var sayfaHeadOluşturucu = EngineContext.Current.Resolve<ISayfaHeadOluşturucu>();
            sayfaHeadOluşturucu.SayfaCssSınıfıParçasıIlaveEt(parça);
        }
        public static MvcHtmlString SayfaCssSınıfı(this HtmlHelper html, string parça = "", bool sınıfElemanıDahilEt = true)
        {
            var sayfaHeadOluşturucu = EngineContext.Current.Resolve<ISayfaHeadOluşturucu>();
            html.SayfaCssSınıfıParçasıIlaveEt(parça);
            var sınıflar = sayfaHeadOluşturucu.SayfaCssSınıfıOluştur();

            if (string.IsNullOrEmpty(sınıflar))
                return null;

            var sonuç = sınıfElemanıDahilEt ? string.Format("class=\"{0}\"", sınıflar) : sınıflar;
            return MvcHtmlString.Create(sonuç);
        }
        public static void AktifMenuÖğesiSistemAdıBelirle(this HtmlHelper html, string sistemAdı)
        {
            var sayfaHeadOluşturucu = EngineContext.Current.Resolve<ISayfaHeadOluşturucu>();
            sayfaHeadOluşturucu.AktifMenuÖğesiSistemAdıBelirle(sistemAdı);
        }
        public static string AktifMenuÖğesiSistemAdıAl(this HtmlHelper html)
        {
            var sayfaHeadOluşturucu = EngineContext.Current.Resolve<ISayfaHeadOluşturucu>();
            return sayfaHeadOluşturucu.AktifMenuÖğesiSistemAdıAl();
        }
    }
}

