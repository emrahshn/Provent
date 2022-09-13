using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Core;
using Core.Domain;
using Core.Domain.Kullanıcılar;
using Core.Domain.Mesajlar;
using Core.Eklentiler;
using Services.Genel;
using Services.Güvenlik;
using Services.KimlikDoğrulama;
using Services.Klasör;
using Services.Kullanıcılar;
using Services.Logging;
using Services.Mesajlar;
using Services.Olaylar;
using Services.Siteler;
using Services.Yardımcılar;
using Services.Yetkilendirme.Harici;
using Web.Fabrika;
using Web.Framework;
using Web.Framework.Controllers;
using Web.Framework.Güvenlik.Captcha;
using Web.Framework.Kendoui;
using Web.Framework.Mvc;
using Web.Models.Kullanıcılar;

namespace Web.Controllers
{

    public class KullanıcıController : TemelPublicController
    {
        private readonly IKullanıcıModelFabrikası _kullanıcıModelFabrikası;
        private readonly KullanıcıAyarları _kullanıcıAyarları;
        private readonly IWorkContext _workContext;
        private readonly IKimlikDoğrulamaServisi _kimlikDoğrulamaServisi;
        private readonly IOlayYayınlayıcı _olayYayınlayıcı;
        private readonly IKullanıcıServisi _kullanıcıServisi;
        private readonly ISiteContext _siteContext;
        private readonly IKullanıcıKayıtServisi _kullanıcıKayıtServisi;
        private readonly IGenelÖznitelikServisi _genelÖznitelikServisi;
        private readonly IWebYardımcısı _webYardımcısı;
        private readonly SiteBilgiAyarları _siteBilgiAyarları;
        private readonly IİzinServisi _izinServisi;
        private readonly ITarihYardımcısı _tarihYardımcısı;
        private readonly IEklentiBulucu _eklentiBulucu;
        private readonly IAçıkYetkilendirmeServisi _açıkYetkilendirmeServisi;
        private readonly ISiteServisi _siteServisi;
        private readonly IBültenAbonelikServisi _bültenAbonelikServisi;
        private readonly IÜlkeServisi _ülkeServisi;
        private readonly IKullanıcıİşlemServisi _kullanıcıİşlemServisi;
        public KullanıcıController(IKullanıcıModelFabrikası kullanıcıModelFabrikası,
            KullanıcıAyarları kullanıcıAyarları,
            IWorkContext workContext,
            IKimlikDoğrulamaServisi kimlikDoğrulamaServisi,
            IOlayYayınlayıcı olayYayınlayıcı,
            IKullanıcıServisi kullanıcıServisi,
            ISiteContext siteContext,
            IKullanıcıKayıtServisi kullanıcıKayıtServisi,
            IGenelÖznitelikServisi genelÖznitelikServisi,
            IWebYardımcısı webYardımcısı,
            SiteBilgiAyarları siteBilgiAyarları,
            IİzinServisi izinServisi,
            ITarihYardımcısı tarihYardımcısı,
            IAçıkYetkilendirmeServisi açıkYetkilendirmeServisi,
            ISiteServisi siteServisi,
            IBültenAbonelikServisi bültenAbonelikServisi,
            IÜlkeServisi ülkeServisi,
            IKullanıcıİşlemServisi kullanıcıİşlemServisi)
        {
            this._kullanıcıModelFabrikası = kullanıcıModelFabrikası;
            this._kullanıcıAyarları = kullanıcıAyarları;
            this._workContext = workContext;
            this._kimlikDoğrulamaServisi = kimlikDoğrulamaServisi;
            this._olayYayınlayıcı = olayYayınlayıcı;
            this._kullanıcıServisi = kullanıcıServisi;
            this._siteContext = siteContext;
            this._kullanıcıKayıtServisi = kullanıcıKayıtServisi;
            this._genelÖznitelikServisi = genelÖznitelikServisi;
            this._webYardımcısı = webYardımcısı;
            this._siteBilgiAyarları = siteBilgiAyarları;
            this._izinServisi = izinServisi;
            this._tarihYardımcısı = tarihYardımcısı;
            this._açıkYetkilendirmeServisi = açıkYetkilendirmeServisi;
            this._siteServisi = siteServisi;
            this._bültenAbonelikServisi = bültenAbonelikServisi;
            this._ülkeServisi = ülkeServisi;
            this._kullanıcıİşlemServisi = kullanıcıİşlemServisi;
        }

        [NonAction]
        protected virtual string KullanıcıRolAdlarıAl(IList<KullanıcıRolü> kullanıcıRolleri, string ayırıcı = ",")
        {
            var sb = new StringBuilder();
            for (int i = 0; i < kullanıcıRolleri.Count; i++)
            {
                sb.Append(kullanıcıRolleri[i].Adı);
                if (i != kullanıcıRolleri.Count - 1)
                {
                    sb.Append(ayırıcı);
                    sb.Append(" ");
                }
            }
            return sb.ToString();
        }

        [NonAction]
        protected virtual string KullanıcıRolüDoğrula(IList<KullanıcıRolü> kullanıcıRolleri)
        {
            if (kullanıcıRolleri == null)
                throw new ArgumentNullException("kullanıcıRolleri");

            bool ziyaretçiRolü = kullanıcıRolleri.FirstOrDefault(cr => cr.SistemAdı == SistemKullanıcıRolAdları.Ziyaretçi) != null;
            bool kayıtlıRolü = kullanıcıRolleri.FirstOrDefault(cr => cr.SistemAdı == SistemKullanıcıRolAdları.Kayıtlı) != null;
            if (kayıtlıRolü && ziyaretçiRolü)
                return "Müşteri hem Ziyaretçi hem de Kayıtlı müşteri rollerinde bulunamaz";
            if (!kayıtlıRolü && !ziyaretçiRolü)
                return "Müşteriyi Ziyaretçi veya Kayıtlı müşteri rolüne ekleyin";

            //no errors
            return "";
        }

        [NonAction]
        protected virtual KullanıcıModel ListeİçinKullanıcıModelHazırla(Kullanıcı kullanıcı)
        {
            return new KullanıcıModel
            {
                Id = kullanıcı.Id,
                Email = kullanıcı.IsRegistered() ? kullanıcı.Email : "Ziyaretçi",
                KullanıcıAdı = kullanıcı.KullanıcıAdı,
                Adı = kullanıcı.TamAdAl(),
                Şirket = kullanıcı.ÖznitelikAl<string>(SistemKullanıcıÖznitelikAdları.Şirket),
                Tel = kullanıcı.ÖznitelikAl<string>(SistemKullanıcıÖznitelikAdları.Tel),
                PostaKodu = kullanıcı.ÖznitelikAl<string>(SistemKullanıcıÖznitelikAdları.PostaKodu),
                KullanıcıRolAdları = KullanıcıRolAdlarıAl(kullanıcı.KullanıcıRolleri.ToList()),
                Aktif = kullanıcı.Aktif,
                OluşturulmaTarihi = _tarihYardımcısı.KullanıcıZamanınaDönüştür(kullanıcı.ŞuTarihdeOluşturuldu, DateTimeKind.Utc),
                SonİşlemTarihi = _tarihYardımcısı.KullanıcıZamanınaDönüştür(kullanıcı.SonİşlemTarihi, DateTimeKind.Utc),
            };
        }

        [NonAction]
        protected virtual IList<KullanıcıModel.İlişkilendirilmişHariciKimlikDoğrulamaModeli> İlişkilendirilmişHariciKimlikDoğrulamaKayıtlarınıAl(Kullanıcı kullanıcı)
        {
            if (kullanıcı == null)
                throw new ArgumentNullException("kullanıcı");

            var result = new List<KullanıcıModel.İlişkilendirilmişHariciKimlikDoğrulamaModeli>();
            foreach (var kayıt in _açıkYetkilendirmeServisi.HariciTanımlayıcıAl(kullanıcı))
            {
                var method = _açıkYetkilendirmeServisi.HariciYetkilendirmeMetodlarınıYükleSistemAdı(kayıt.SağlayıcıSistemAdı);
                if (method == null)
                    continue;

                result.Add(new KullanıcıModel.İlişkilendirilmişHariciKimlikDoğrulamaModeli
                {
                    Id = kayıt.Id,
                    Email = kayıt.Email,
                    HariciDoğrulayıcı = kayıt.HariciTanımlayıcı,
                    HariciMetodAdı = method.EklentiTanımlayıcı.KısaAd
                });
            }

            return result;
        }

        [NonAction]
        private bool BaşkaYöneticiMevcut(Kullanıcı kullanıcı)
        {
            var kullanıcılar = _kullanıcıServisi.TümKullanıcılarıAl(kullanıcıRolIdleri: new[] { _kullanıcıServisi.KullanıcıRolüAlSistemAdı(SistemKullanıcıRolAdları.Yönetici).Id });
            return kullanıcılar.Any(c => c.Aktif && c.Id != kullanıcı.Id);
        }
        [NonAction]
        protected virtual void KullanıcıModelHazırla(KullanıcıModel model, Kullanıcı kullanıcı, bool özellikleriDışla)
        {
            var tümSiteler = _siteServisi.TümSiteler();
            if (kullanıcı != null)
            {
                model.Id = kullanıcı.Id;
                if (!özellikleriDışla)
                {
                    model.Email = kullanıcı.Email;
                    model.KullanıcıAdı = kullanıcı.KullanıcıAdı;
                    model.AdminYorumu = kullanıcı.AdminYorumu;
                    model.Aktif = kullanıcı.Aktif;

                    if (kullanıcı.KayıtOlduSiteId == 0 || tümSiteler.All(s => s.Id != kullanıcı.KayıtOlduSiteId))
                        model.SitedeKayıtlı = string.Empty;
                    else
                        model.SitedeKayıtlı = tümSiteler.First(s => s.Id == kullanıcı.KayıtOlduSiteId).Adı;


                    model.OluşturulmaTarihi = _tarihYardımcısı.KullanıcıZamanınaDönüştür(kullanıcı.ŞuTarihdeOluşturuldu, DateTimeKind.Utc);
                    model.SonİşlemTarihi = _tarihYardımcısı.KullanıcıZamanınaDönüştür(kullanıcı.SonİşlemTarihi, DateTimeKind.Utc);
                    model.SonIpAdresi = kullanıcı.SonIPAdresi;
                    model.SonZiyaretEdilenSayfa = kullanıcı.ÖznitelikAl<string>(SistemKullanıcıÖznitelikAdları.SonZiyaretEdilenSayfa);

                    model.SeçiliKullanıcıRolIdleri = kullanıcı.KullanıcıRolleri.Select(cr => cr.Id).ToList();

                    //abonelik 
                    if (!String.IsNullOrEmpty(kullanıcı.Email))
                    {
                        var abonelikKayıtlarıSiteId = new List<int>();
                        foreach (var site in tümSiteler)
                        {
                            var bültenAbonelikKaydı = _bültenAbonelikServisi
                                .BültenAboneliğiAlEmailVeSiteId(kullanıcı.Email, site.Id);
                            if (bültenAbonelikKaydı != null)
                                abonelikKayıtlarıSiteId.Add(site.Id);
                            model.SeçiliAbonleikKayıtları = abonelikKayıtlarıSiteId.ToArray();
                        }
                    }

                    //form fields
                    model.Adı = kullanıcı.ÖznitelikAl<string>(SistemKullanıcıÖznitelikAdları.Adı);
                    model.Soyadı = kullanıcı.ÖznitelikAl<string>(SistemKullanıcıÖznitelikAdları.Soyadı);
                    model.Cinsiyet = kullanıcı.ÖznitelikAl<string>(SistemKullanıcıÖznitelikAdları.Cinsiyet);
                    model.DoğumGünü = kullanıcı.ÖznitelikAl<DateTime?>(SistemKullanıcıÖznitelikAdları.DoğumTarihi);
                    model.Şirket = kullanıcı.ÖznitelikAl<string>(SistemKullanıcıÖznitelikAdları.Şirket);
                    model.SokakAdresi = kullanıcı.ÖznitelikAl<string>(SistemKullanıcıÖznitelikAdları.SokakAdresi1);
                    model.SokakAdresi2 = kullanıcı.ÖznitelikAl<string>(SistemKullanıcıÖznitelikAdları.SokakAdresi2);
                    model.PostaKodu = kullanıcı.ÖznitelikAl<string>(SistemKullanıcıÖznitelikAdları.PostaKodu);
                    model.Şehir = kullanıcı.ÖznitelikAl<string>(SistemKullanıcıÖznitelikAdları.Şehir);
                    model.ÜlkeId = kullanıcı.ÖznitelikAl<int>(SistemKullanıcıÖznitelikAdları.ÜlkeId);
                    model.Tel = kullanıcı.ÖznitelikAl<string>(SistemKullanıcıÖznitelikAdları.Tel);
                    model.Faks = kullanıcı.ÖznitelikAl<string>(SistemKullanıcıÖznitelikAdları.Fax);
                }
            }

            model.KullanıcıAdlarıEtkin = _kullanıcıAyarları.KullanıcıAdlarıEtkin;
            //model. = _tarihYardımcısı.iz;

            //kullanıcı öznitelikleri
            //KullanıcıÖznitelikModelHazırla(model, kullanıcı);

            model.CinsiyetEtkin = _kullanıcıAyarları.CinsiyetEtkin;
            model.DoğumGünüEtkin = _kullanıcıAyarları.DoğumTarihiEtkin;
            model.ŞirketEtkin = _kullanıcıAyarları.ŞirketEtkin;
            model.SokakAdresiEtkin = _kullanıcıAyarları.SokakAdresiEtkin;
            model.SokakAdresi2Etkin = _kullanıcıAyarları.SokakAdresi2Etkin;
            model.PostaKoduEtkin = _kullanıcıAyarları.PostaKoduEtkin;
            model.ŞehirEtkin = _kullanıcıAyarları.ŞehirEtkin;
            model.ÜlkeEtkin = _kullanıcıAyarları.ÜlkeEtkin;
            model.TelEtkin = _kullanıcıAyarları.TelEtkin;
            model.FaksEtkin = _kullanıcıAyarları.FaksEtkin;

            //ülkeler
            if (_kullanıcıAyarları.ÜlkeEtkin)
            {
                model.KullanılabilirÜlkeler.Add(new SelectListItem { Text = "Ülke seçiniz", Value = "0" });
                foreach (var c in _ülkeServisi.TümÜlkeleriAl(gizliOlanıGöster: true))
                {
                    model.KullanılabilirÜlkeler.Add(new SelectListItem
                    {
                        Text = c.Adı,
                        Value = c.Id.ToString(),
                        Selected = c.Id == model.ÜlkeId
                    });
                }

            }

            //abonelik kayıtları
            model.AbonelikKayıtlarıKullanılabilirSiteler = tümSiteler
                .Select(s => new KullanıcıModel.SiteModel() { Id = s.Id, Adı = s.Adı })
                .ToList();

            //kullanıcı rolleri
            var tümRoller = _kullanıcıServisi.TümKullanıcıRolleriniAl(true);
            var adminRolü = tümRoller.FirstOrDefault(c => c.SistemAdı == SistemKullanıcıRolAdları.Kayıtlı);
            if (kullanıcı == null && adminRolü != null)
            {
                model.SeçiliKullanıcıRolIdleri.Add(adminRolü.Id);
            }
            foreach (var rol in tümRoller)
            {
                model.KullanılabilirKullanıcıRolleri.Add(new SelectListItem
                {
                    Text = rol.Adı,
                    Value = rol.Id.ToString(),
                    Selected = model.SeçiliKullanıcıRolIdleri.Contains(rol.Id)
                });
            }

            //ödül puanı
            if (kullanıcı != null)
            {
                //model.ÖdülPuanıGeçmişiniGörüntüle = _rewardPointsSettings.Enabled;
                model.ÖdülPuanıDeğeriEkle = 0;
                model.ÖdülPuanıMesajıEkle = "Ödül puanı mesajı...";

                //siteler
                foreach (var store in tümSiteler)
                {
                    model.ÖdülPuanıKullanılabilirSiteler.Add(new SelectListItem
                    {
                        Text = store.Adı,
                        Value = store.Id.ToString(),
                        Selected = (store.Id == _siteContext.MevcutSite.Id)
                    });
                }
            }
            else
            {
                model.ÖdülPuanıGeçmişiniGörüntüle = false;
            }
            //harici yetkilendirme kayıtları
            if (kullanıcı != null)
            {
                model.İlişkilendirilmişHariciKimlikDoğrulamaKayıtları = İlişkilendirilmişHariciKimlikDoğrulamaKayıtlarınıAl(kullanıcı);
            }

            model.HoşgeldinizMesajıGönderimiEtkin = _kullanıcıAyarları.KullanıcıKayıtTipi == KullanıcıKayıtTipi.YöneticiOnayı &&
                kullanıcı != null &&
                kullanıcı.IsRegistered();

            model.AktivasyonMesajıGönderimiEtkin = _kullanıcıAyarları.KullanıcıKayıtTipi == KullanıcıKayıtTipi.EmailDoğrulaması &&
                kullanıcı != null &&
                kullanıcı.IsRegistered() &&
                !kullanıcı.Aktif;
        }

        [GenelGezinimeİzinVer(true)]
        public virtual ActionResult Giriş(bool? ziyaretçi)
        {
            var model = _kullanıcıModelFabrikası.GirişModelHazırla();
            return View(model);
        }

        [HttpPost]
        [CaptchaDoğrulayıcı]
        //[StoreClosed(true)]
        [GenelGezinimeİzinVer(true)]
        public virtual ActionResult Giriş(GirişModel model, string dönenUrl, bool captchaDoğrulandı)
        {
            /*
            //CAPTCHA doğrula
            if (_captchaSettings.Enabled && _captchaSettings.ShowOnLoginPage && !captchaValid)
            {
                ModelState.AddModelError("", _captchaSettings.GetWrongCaptchaMessage(_localizationService));
            }
            */
            if (ModelState.IsValid)
            {
                if (_kullanıcıAyarları.KullanıcıAdlarıEtkin && model.KullanıcıAdı != null)
                {
                    model.KullanıcıAdı = model.KullanıcıAdı.Trim();
                }
                var girişSonucu =
                    _kullanıcıKayıtServisi.KullanıcıDoğrula(
                        _kullanıcıAyarları.KullanıcıAdlarıEtkin ? model.KullanıcıAdı : model.Email, model.Şifre);
                switch (girişSonucu)
                {
                    case KullanıcıGirişSonuçları.Başarılı:
                        {
                            var kullanıcı = _kullanıcıAyarları.KullanıcıAdlarıEtkin
                                ? _kullanıcıServisi.KullanıcıAlSistemAdı(model.KullanıcıAdı)
                                : _kullanıcıServisi.KullanıcıAlEmail(model.Email);

                            //yeni kullanıcı olarak giriş yapıldı
                            _kimlikDoğrulamaServisi.Giriş(kullanıcı, model.BeniHatırla);

                            //olay      
                            _olayYayınlayıcı.Yayınla(new KullanıcıBağlandıOlayı(kullanıcı));

                            //log ekle
                            //_kullanıcıAktiviteServisi.AktiviteEkle(kullanıcı, "GenelSite.Login", _localizationService.GetResource("ActivityLog.PublicStore.Login"));

                            if (String.IsNullOrEmpty(dönenUrl) || !Url.IsLocalUrl(dönenUrl))
                                return RedirectToRoute("HomePage");

                            return Redirect(dönenUrl);
                        }
                    case KullanıcıGirişSonuçları.KullanıcıMevcutDeğil:
                        ModelState.AddModelError("", "Kullanıcı bulunamadı");
                        break;
                    case KullanıcıGirişSonuçları.Silindi:
                        ModelState.AddModelError("", "Kullanıcı silindi");
                        break;
                    case KullanıcıGirişSonuçları.AktifDeğil:
                        ModelState.AddModelError("", "Kullanıcı aktifleştirilmedi");
                        break;
                    case KullanıcıGirişSonuçları.KayıtlıDeğil:
                        ModelState.AddModelError("", "Kullanıcı henüz kayıtlı değil");
                        break;
                    case KullanıcıGirişSonuçları.Kilitlendi:
                        ModelState.AddModelError("", "Kullanıcı kilitlendi");
                        break;
                    case KullanıcıGirişSonuçları.HatalıŞifre:
                    default:
                        ModelState.AddModelError("", "Hatalı şifre");
                        break;
                }
            }

            //If we got this far, something failed, redisplay form
            model = _kullanıcıModelFabrikası.GirişModelHazırla();
            return View(model);
        }
        [GenelGezinimeİzinVer(true)]
        public virtual ActionResult Çıkış()
        {
            _kimlikDoğrulamaServisi.Çıkış();
            _olayYayınlayıcı.Yayınla(new KullanıcıÇıkışYaptıOlayı(_workContext.MevcutKullanıcı));
            return RedirectToRoute("HomePage");
        }
        public virtual ActionResult Index()
        {
            return RedirectToAction("Liste");
        }
        public virtual ActionResult Liste()
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KullanıcılarıYönet))
                return ErişimEngellendiView();
            var varsayılanRolIdleri = new List<int> { _kullanıcıServisi.KullanıcıRolüAlSistemAdı(SistemKullanıcıRolAdları.Kayıtlı).Id };
            var model = new KullanıcılarListeModel
            {
                KullanıcıAdlarıEtkin = _kullanıcıAyarları.KullanıcıAdlarıEtkin,
                DoğumGünüEtkin = _kullanıcıAyarları.DoğumTarihiEtkin,
                PostaKoduEtkin = _kullanıcıAyarları.PostaKoduEtkin,
                TelEtkin = _kullanıcıAyarları.TelEtkin,
                SirketEtkin = _kullanıcıAyarları.ŞirketEtkin
            };
            var tümRoller = _kullanıcıServisi.TümKullanıcıRolleriniAl(true);
            foreach (var rol in tümRoller)
            {
                model.KullanılabilirKullanıcıRolleri.Add(new SelectListItem
                {
                    Text = rol.Adı,
                    Value = rol.Id.ToString(),
                    Selected = varsayılanRolIdleri.Any(l => l == rol.Id)
                });
            }
            return View(model);
        }
        [HttpPost]
        public virtual ActionResult KullanıcıListe(DataSourceİsteği komut, KullanıcılarListeModel model,
            [ModelBinder(typeof(VirgülleAyrılmışModelBinder))] int[] kullaniciRolleriAra)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KullanıcılarıYönet))
                return ErişimEngellendiKendoGridJson();
            var doğumGünüAra = 0;
            int doğumAyıAra = 0;
            if (!String.IsNullOrWhiteSpace(model.DoğumGünüAra))
                doğumGünüAra = Convert.ToInt32(model.DoğumGünüAra);
            if (!String.IsNullOrWhiteSpace(model.DoğumAyıAra))
                doğumAyıAra = Convert.ToInt32(model.DoğumAyıAra);

            var kullanıcılar = _kullanıcıServisi.TümKullanıcılarıAl(
                kullanıcıRolIdleri: kullaniciRolleriAra,
                email: model.EmailAra,
                kullanıcıAdı: model.KullanıcıAdıAra,
                ad: model.AdAra,
                soyadı: model.SoyadAra,
                doğumTarihi: doğumGünüAra,
                doğumAyı: doğumAyıAra,
                şirket: model.SirketAra,
                tel: model.TelAra,
                postaKodu: model.PostaKoduAra,
                ipAdresi: model.IpAdresiAra,
                sayfaIndeksi: komut.Page - 1,
                sayfaBüyüklüğü: komut.PageSize);
            var gridModel = new DataSourceSonucu
            {
                Data = kullanıcılar.Select(ListeİçinKullanıcıModelHazırla),
                Toplam = kullanıcılar.TotalCount
            };

            return Json(gridModel);
        }
        public virtual ActionResult Ekle()
        {
            var model = new KullanıcıModel();
            KullanıcıModelHazırla(model, null, false);
            return View(model);
        }

        [HttpPost, FormAdıParametresi("kaydet-devam", "düzenlemeyeDevam")]
        [FormDeğeriGerekli("kaydet", "kaydet-devam")]
        [ValidateInput(false)]
        public virtual ActionResult Ekle(KullanıcıModel model, bool düzenlemeyeDevam, FormCollection form)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KullanıcılarıYönet))
                return ErişimEngellendiView();

            if (!String.IsNullOrWhiteSpace(model.Email))
            {
                var user2 = _kullanıcıServisi.KullanıcıAlEmail(model.Email);
                if (user2 != null)
                    ModelState.AddModelError("", "Email zaten kayıtlı");
            }
            if (!String.IsNullOrWhiteSpace(model.KullanıcıAdı) & _kullanıcıAyarları.KullanıcıAdlarıEtkin)
            {
                var user2 = _kullanıcıServisi.KullanıcıAlKullanıcıAdı(model.KullanıcıAdı);
                if (user2 != null)
                    ModelState.AddModelError("", "Kullanıcı adı zaten alındı");
            }

            //kullanıcı rolü doğrula
            var tümKullanıcıRolleri = _kullanıcıServisi.TümKullanıcıRolleriniAl(true);
            var yeniKullanıcıRolleri = new List<KullanıcıRolü>();
            foreach (var kullanıcıRolü in tümKullanıcıRolleri)
                if (model.SeçiliKullanıcıRolIdleri.Contains(kullanıcıRolü.Id))
                    yeniKullanıcıRolleri.Add(kullanıcıRolü);
            var kullanıcıRolüHatası = KullanıcıRolüDoğrula(yeniKullanıcıRolleri);
            if (!String.IsNullOrEmpty(kullanıcıRolüHatası))
            {
                ModelState.AddModelError("", kullanıcıRolüHatası);
                HataBildirimi(kullanıcıRolüHatası, false);
            }

            //email adresini doğrula
            if (yeniKullanıcıRolleri.Any() && yeniKullanıcıRolleri.FirstOrDefault(c => c.SistemAdı == SistemKullanıcıRolAdları.Kayıtlı) != null && !GenelYardımcı.GeçerliMail(model.Email))
            {
                ModelState.AddModelError("", "Müşterinin 'Kayıtlı' rolü olması için geçerli e-posta gereklidir.");
                HataBildirimi("Müşterinin 'Kayıtlı' rolü olması için geçerli e-posta gereklidir", false);
            }

            //özel kullanıcı özniteliği
            /*
            var kullanıcıAttributesXml = ParseCustomCustomerAttributes(form);
            if (newCustomerRoles.Any() && newCustomerRoles.FirstOrDefault(c => c.SystemName == SystemCustomerRoleNames.Registered) != null)
            {
                var kullanıcıAttributeWarnings = _kullanıcıAttributeParser.GetAttributeWarnings(kullanıcıAttributesXml);
                foreach (var error in kullanıcıAttributeWarnings)
                {
                    ModelState.AddModelError("", error);
                }
            }
            */
            if (ModelState.IsValid)
            {
                var kullanıcı = new Kullanıcı
                {
                    KullanıcıGuid = Guid.NewGuid(),
                    Email = model.Email,
                    KullanıcıAdı = model.KullanıcıAdı,
                    AdminYorumu = model.AdminYorumu,
                    Aktif = model.Aktif,
                    ŞuTarihdeOluşturuldu = DateTime.UtcNow,
                    SonİşlemTarihi = DateTime.UtcNow,
                    KayıtOlduSiteId = _siteContext.MevcutSite.Id
                };
                _kullanıcıServisi.KullanıcıEkle(kullanıcı);

                //form alanları
                if (_kullanıcıAyarları.CinsiyetEtkin)
                    _genelÖznitelikServisi.ÖznitelikKaydet(kullanıcı, SistemKullanıcıÖznitelikAdları.Cinsiyet, model.Cinsiyet);
                _genelÖznitelikServisi.ÖznitelikKaydet(kullanıcı, SistemKullanıcıÖznitelikAdları.Adı, model.Adı);
                _genelÖznitelikServisi.ÖznitelikKaydet(kullanıcı, SistemKullanıcıÖznitelikAdları.Soyadı, model.Soyadı);
                if (_kullanıcıAyarları.DoğumTarihiEtkin)
                    _genelÖznitelikServisi.ÖznitelikKaydet(kullanıcı, SistemKullanıcıÖznitelikAdları.DoğumTarihi, model.DoğumGünü);
                if (_kullanıcıAyarları.ŞirketEtkin)
                    _genelÖznitelikServisi.ÖznitelikKaydet(kullanıcı, SistemKullanıcıÖznitelikAdları.Şirket, model.Şirket);
                if (_kullanıcıAyarları.SokakAdresiEtkin)
                    _genelÖznitelikServisi.ÖznitelikKaydet(kullanıcı, SistemKullanıcıÖznitelikAdları.SokakAdresi1, model.SokakAdresi);
                if (_kullanıcıAyarları.SokakAdresi2Etkin)
                    _genelÖznitelikServisi.ÖznitelikKaydet(kullanıcı, SistemKullanıcıÖznitelikAdları.SokakAdresi2, model.SokakAdresi2);
                if (_kullanıcıAyarları.PostaKoduEtkin)
                    _genelÖznitelikServisi.ÖznitelikKaydet(kullanıcı, SistemKullanıcıÖznitelikAdları.PostaKodu, model.PostaKodu);
                if (_kullanıcıAyarları.ŞehirEtkin)
                    _genelÖznitelikServisi.ÖznitelikKaydet(kullanıcı, SistemKullanıcıÖznitelikAdları.Şehir, model.Şehir);
                if (_kullanıcıAyarları.ÜlkeEtkin)
                    _genelÖznitelikServisi.ÖznitelikKaydet(kullanıcı, SistemKullanıcıÖznitelikAdları.ÜlkeId, model.ÜlkeId);
                if (_kullanıcıAyarları.TelEtkin)
                    _genelÖznitelikServisi.ÖznitelikKaydet(kullanıcı, SistemKullanıcıÖznitelikAdları.Tel, model.Tel);
                if (_kullanıcıAyarları.FaksEtkin)
                    _genelÖznitelikServisi.ÖznitelikKaydet(kullanıcı, SistemKullanıcıÖznitelikAdları.Fax, model.Faks);

                //özel kullanıcı öznitelikleri
                //_genelÖznitelikServisi.ÖznitelikKaydet(kullanıcı, SistemKullanıcıÖznitelikAdları.ÖzelKullanıcıÖznitelikleri, kullanıcıÖznitelikXml);


                //abonelik kaydı
                if (!String.IsNullOrEmpty(kullanıcı.Email))
                {
                    var tümSiteler = _siteServisi.TümSiteler();
                    foreach (var site in tümSiteler)
                    {
                        var bültenAboneliği = _bültenAbonelikServisi
                            .BültenAboneliğiAlEmailVeSiteId(kullanıcı.Email, site.Id);
                        if (model.SeçiliAbonleikKayıtları != null &&
                            model.SeçiliAbonleikKayıtları.Contains(site.Id))
                        {
                            //abone olundu
                            if (bültenAboneliği == null)
                            {
                                _bültenAbonelikServisi.BültenAboneliğiEkle(new BültenAboneliği
                                {
                                    BültenAboneliğiGuid = Guid.NewGuid(),
                                    Email = kullanıcı.Email,
                                    Aktif = true,
                                    SiteId = site.Id,
                                    OluşturulmaTarihi = DateTime.UtcNow
                                });
                            }
                        }
                        else
                        {
                            //abone olunmadı
                            if (bültenAboneliği != null)
                            {
                                _bültenAbonelikServisi.BültenAboneliğiSil(bültenAboneliği);
                            }
                        }
                    }
                }

                //şifre
                if (!String.IsNullOrWhiteSpace(model.Şifre))
                {
                    var şifreİsteğiniDeğiştir = new ŞifreDeğiştirmeİsteği(model.Email, false, _kullanıcıAyarları.VarsayılanŞifreFormatı, model.Şifre);
                    var şifreDeğiştiSonucu = _kullanıcıKayıtServisi.ŞifreDeğiştir(şifreİsteğiniDeğiştir);
                    if (!şifreDeğiştiSonucu.Başarılı)
                    {
                        foreach (var şifreDeğiştirmeHatası in şifreDeğiştiSonucu.Hatalar)
                            HataBildirimi(şifreDeğiştirmeHatası);
                    }
                }

                //kullanıcı rolleri
                foreach (var kullanıcıRolü in yeniKullanıcıRolleri)
                {
                    //Yönetici olmadığından emin ol
                    if (kullanıcıRolü.SistemAdı == SistemKullanıcıRolAdları.Yönetici &&
                        !_workContext.MevcutKullanıcı.Yönetici())
                        continue;

                    kullanıcı.KullanıcıRolleri.Add(kullanıcıRolü);
                }
                _kullanıcıServisi.KullanıcıGüncelle(kullanıcı);

                //işlem kaydı
                _kullanıcıİşlemServisi.İşlemEkle("YeniKullanıcı", "Yeni kullanıcı eklendi", kullanıcı.Id);

                BaşarılıBildirimi("Kullanıcı Eklendi");

                if (düzenlemeyeDevam)
                {
                    //seçili tab
                    SeçiliTabKaydet();

                    return RedirectToAction("Düzenli", new { id = kullanıcı.Id });
                }
                return RedirectToAction("Liste");
            }

            //sorun olduysa form yeniden doldur
            KullanıcıModelHazırla(model, null, true);
            return View(model);
        }

        public virtual ActionResult Düzenle(int id)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KullanıcılarıYönet))
                return ErişimEngellendiView();

            var kullanıcı = _kullanıcıServisi.KullanıcıAlId(id);
            if (kullanıcı == null || kullanıcı.Silindi)
                return RedirectToAction("Liste");

            var model = new KullanıcıModel();
            KullanıcıModelHazırla(model, kullanıcı, false);
            return View(model);
        }

        [HttpPost, FormAdıParametresi("kaydet-devam", "düzenlemeyeDevam")]
        [FormDeğeriGerekli("kaydet", "kaydet-devam")]
        [ValidateInput(false)]
        public virtual ActionResult Düzenle(KullanıcıModel model, bool düzenlemeyeDevam, FormCollection form)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KullanıcılarıYönet))
                return ErişimEngellendiView();

            var kullanıcı = _kullanıcıServisi.KullanıcıAlId(model.Id);
            if (kullanıcı == null || kullanıcı.Silindi)
                return RedirectToAction("Liste");

            //kullanıcı rollerini düzenle
            var tümKullanıcıRolleri = _kullanıcıServisi.TümKullanıcıRolleriniAl(true);
            var yeniKullanıcıRolleri = new List<KullanıcıRolü>();
            foreach (var kullanıcıRolü in tümKullanıcıRolleri)
                if (model.SeçiliKullanıcıRolIdleri.Contains(kullanıcıRolü.Id))
                    yeniKullanıcıRolleri.Add(kullanıcıRolü);
            var kullanıcıRolleriHatası = KullanıcıRolüDoğrula(yeniKullanıcıRolleri);
            if (!String.IsNullOrEmpty(kullanıcıRolleriHatası))
            {
                ModelState.AddModelError("", kullanıcıRolleriHatası);
                HataBildirimi(kullanıcıRolleriHatası, false);
            }

            if (yeniKullanıcıRolleri.Any() && yeniKullanıcıRolleri.FirstOrDefault(c => c.SistemAdı == SistemKullanıcıRolAdları.Kayıtlı) != null && !GenelYardımcı.GeçerliMail(model.Email))
            {
                ModelState.AddModelError("", "Müşterinin 'Kayıtlı' rolü olması için geçerli e-posta gereklidir.");
                HataBildirimi("Müşterinin 'Kayıtlı' rolü olması için geçerli e-posta gereklidir", false);
            }

            //özel kullanıcı öznitelikleri

            if (ModelState.IsValid)
            {
                try
                {
                    kullanıcı.AdminYorumu = model.AdminYorumu;
                    if (!kullanıcı.Yönetici() || model.Aktif || BaşkaYöneticiMevcut(kullanıcı))
                        kullanıcı.Aktif = model.Aktif;
                    else
                        HataBildirimi("Son yöneticiyi devre dışı bırakamazsınız. En az bir yönetici hesabı olmalıdır.");

                    //email
                    if (!String.IsNullOrWhiteSpace(model.Email))
                    {
                        _kullanıcıKayıtServisi.EmailAyarla(kullanıcı, model.Email, false);
                    }
                    else
                    {
                        kullanıcı.Email = model.Email;
                    }

                    //kullanıcıadı
                    if (_kullanıcıAyarları.KullanıcıAdlarıEtkin)
                    {
                        if (!String.IsNullOrWhiteSpace(model.KullanıcıAdı))
                        {
                            _kullanıcıKayıtServisi.KullanıcıAdıAyarla(kullanıcı, model.KullanıcıAdı);
                        }
                        else
                        {
                            kullanıcı.KullanıcıAdı = model.KullanıcıAdı;
                        }
                    }

                    //form alanları
                    //form alanları
                    if (_kullanıcıAyarları.CinsiyetEtkin)
                        _genelÖznitelikServisi.ÖznitelikKaydet(kullanıcı, SistemKullanıcıÖznitelikAdları.Cinsiyet, model.Cinsiyet);
                    _genelÖznitelikServisi.ÖznitelikKaydet(kullanıcı, SistemKullanıcıÖznitelikAdları.Adı, model.Adı);
                    _genelÖznitelikServisi.ÖznitelikKaydet(kullanıcı, SistemKullanıcıÖznitelikAdları.Soyadı, model.Soyadı);
                    if (_kullanıcıAyarları.DoğumTarihiEtkin)
                        _genelÖznitelikServisi.ÖznitelikKaydet(kullanıcı, SistemKullanıcıÖznitelikAdları.DoğumTarihi, model.DoğumGünü);
                    if (_kullanıcıAyarları.ŞirketEtkin)
                        _genelÖznitelikServisi.ÖznitelikKaydet(kullanıcı, SistemKullanıcıÖznitelikAdları.Şirket, model.Şirket);
                    if (_kullanıcıAyarları.SokakAdresiEtkin)
                        _genelÖznitelikServisi.ÖznitelikKaydet(kullanıcı, SistemKullanıcıÖznitelikAdları.SokakAdresi1, model.SokakAdresi);
                    if (_kullanıcıAyarları.SokakAdresi2Etkin)
                        _genelÖznitelikServisi.ÖznitelikKaydet(kullanıcı, SistemKullanıcıÖznitelikAdları.SokakAdresi2, model.SokakAdresi2);
                    if (_kullanıcıAyarları.PostaKoduEtkin)
                        _genelÖznitelikServisi.ÖznitelikKaydet(kullanıcı, SistemKullanıcıÖznitelikAdları.PostaKodu, model.PostaKodu);
                    if (_kullanıcıAyarları.ŞehirEtkin)
                        _genelÖznitelikServisi.ÖznitelikKaydet(kullanıcı, SistemKullanıcıÖznitelikAdları.Şehir, model.Şehir);
                    if (_kullanıcıAyarları.ÜlkeEtkin)
                        _genelÖznitelikServisi.ÖznitelikKaydet(kullanıcı, SistemKullanıcıÖznitelikAdları.ÜlkeId, model.ÜlkeId);
                    if (_kullanıcıAyarları.TelEtkin)
                        _genelÖznitelikServisi.ÖznitelikKaydet(kullanıcı, SistemKullanıcıÖznitelikAdları.Tel, model.Tel);
                    if (_kullanıcıAyarları.FaksEtkin)
                        _genelÖznitelikServisi.ÖznitelikKaydet(kullanıcı, SistemKullanıcıÖznitelikAdları.Fax, model.Faks);

                    //özel kullanıcı öznitelikleri
                    //_genelÖznitelikServisi.ÖznitelikKaydet(kullanıcı, SistemKullanıcıÖznitelikAdları.ÖzelKullanıcıÖznitelikleri, kullanıcıAttributesXml);

                    //abonelik kaydı
                    if (!String.IsNullOrEmpty(kullanıcı.Email))
                    {
                        var tümSiteler = _siteServisi.TümSiteler();
                        foreach (var site in tümSiteler)
                        {
                            var bültenAboneliği = _bültenAbonelikServisi
                                .BültenAboneliğiAlEmailVeSiteId(kullanıcı.Email, site.Id);
                            if (model.SeçiliAbonleikKayıtları != null &&
                                model.SeçiliAbonleikKayıtları.Contains(site.Id))
                            {
                                //abone olundu
                                if (bültenAboneliği == null)
                                {
                                    _bültenAbonelikServisi.BültenAboneliğiEkle(new BültenAboneliği
                                    {
                                        BültenAboneliğiGuid = Guid.NewGuid(),
                                        Email = kullanıcı.Email,
                                        Aktif = true,
                                        SiteId = site.Id,
                                        OluşturulmaTarihi = DateTime.UtcNow
                                    });
                                }
                            }
                            else
                            {
                                //abone olunmadı
                                if (bültenAboneliği != null)
                                {
                                    _bültenAbonelikServisi.BültenAboneliğiSil(bültenAboneliği);
                                }
                            }
                        }
                    }

                    //kullanıcı roles
                    foreach (var kullanıcıRolü in tümKullanıcıRolleri)
                    {
                        if (kullanıcıRolü.SistemAdı == SistemKullanıcıRolAdları.Yönetici &&
                            !_workContext.MevcutKullanıcı.Yönetici())
                            continue;

                        if (model.SeçiliKullanıcıRolIdleri.Contains(kullanıcıRolü.Id))
                        {
                            //yeni rol
                            if (kullanıcı.KullanıcıRolleri.Count(cr => cr.Id == kullanıcıRolü.Id) == 0)
                                kullanıcı.KullanıcıRolleri.Add(kullanıcıRolü);
                        }
                        else
                        {
                            if (kullanıcıRolü.SistemAdı == SistemKullanıcıRolAdları.Yönetici && !BaşkaYöneticiMevcut(kullanıcı))
                            {
                                HataBildirimi("Yönetici rolünü kaldıramazsınız. En az bir yönetici hesabı olmalıdır.");
                                continue;
                            }

                            //rolü sil
                            if (kullanıcı.KullanıcıRolleri.Count(cr => cr.Id == kullanıcıRolü.Id) > 0)
                                kullanıcı.KullanıcıRolleri.Remove(kullanıcıRolü);
                        }
                    }
                    _kullanıcıServisi.KullanıcıGüncelle(kullanıcı);

                    //işlem kaydı
                    _kullanıcıİşlemServisi.İşlemEkle("KullanıcıDüzenle", "Bir müşteri düzenlendi (Id = {0})", kullanıcı.Id);
                    BaşarılıBildirimi("Kullanıcı başarıyla güncellendi");
                    if (düzenlemeyeDevam)
                    {
                        //saçili tab
                        SeçiliTabKaydet();
                        return RedirectToAction("Düzenle", new { id = kullanıcı.Id });
                    }
                    return RedirectToAction("Liste");
                }
                catch (Exception exc)
                {
                    HataBildirimi(exc.Message, false);
                }
            }

            KullanıcıModelHazırla(model, kullanıcı, true);
            return View(model);
        }
        [HttpPost]
        public virtual ActionResult Sil(int id)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KullanıcılarıYönet))
                return ErişimEngellendiView();

            var kullanıcı = _kullanıcıServisi.KullanıcıAlId(id);
            if (kullanıcı == null)
                return RedirectToAction("Liste");

            try
            {
                if (kullanıcı.Yönetici() && !BaşkaYöneticiMevcut(kullanıcı))
                {
                    HataBildirimi("Son yöneticiyi devre dışı bırakamazsınız. En az bir yönetici hesabı olmalıdır.");
                    return RedirectToAction("Düzenle", new { id = kullanıcı.Id });
                }

                //ensure that the current kullanıcı cannot delete "Administrators" if he's not an admin himself
                if (kullanıcı.Yönetici() && !_workContext.MevcutKullanıcı.Yönetici())
                {
                    HataBildirimi("Sadece yönetici kullanıcı silebilir.");
                    return RedirectToAction("Düzenle", new { id = kullanıcı.Id });
                }

                //sil
                _kullanıcıServisi.KullanıcıSil(kullanıcı);

                //abonelik sil
                foreach (var site in _siteServisi.TümSiteler())
                {
                    var abonelik = _bültenAbonelikServisi.BültenAboneliğiAlEmailVeSiteId(kullanıcı.Email, site.Id);
                    if (abonelik != null)
                        _bültenAbonelikServisi.BültenAboneliğiSil(abonelik);
                }

                //işlem kaydı
                _kullanıcıİşlemServisi.İşlemEkle("KullanıcıSilindi", "Kullanıcı Silindi", kullanıcı.Id);

                BaşarılıBildirimi("Kullanıcı başarıyla silindi");
                return RedirectToAction("Liste");
            }
            catch (Exception exc)
            {
                HataBildirimi(exc.Message);
                return RedirectToAction("Düzenle", new { id = kullanıcı.Id });
            }
        }
    }
}
