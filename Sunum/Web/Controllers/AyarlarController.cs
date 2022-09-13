using Web.Models.Ayarlar;
using Core;
using Core.Domain;
using Core.Domain.Blogs;
using Core.Domain.Genel;
using Core.Domain.Güvenlik;
using Core.Domain.Kullanıcılar;
using Core.Domain.Seo;
using Services;
using Services.Genel;
using Services.Güvenlik;
using Services.Logging;
using Services.Siteler;
using Services.Yapılandırma;
using Services.Yardımcılar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Web.Controllers;
using Web.Framework.Controllers;
using Web.Framework.Güvenlik.Captcha;
using Web.Framework.Temalar;
using Web.Uzantılar;
using Core.Domain.Medya;
using Services.Medya;

namespace Admin.Controllers
{
    public partial class AyarlarController : TemelPublicController
    {
        private readonly IWorkContext _workContext;
        private readonly IİzinServisi _izinServisi;
        private readonly ISiteServisi _siteServisi;
        private readonly IAyarlarServisi _ayarlarServisi;
        private readonly ITemaSağlayıcı _temaSağlayıcı;
        private readonly GenelAyarlar _genelAyarlar;
        private readonly IKullanıcıİşlemServisi _kulllanıcıİşlemServisi;
        private readonly ITamMetinServisi _tamMetinServisi;
        private readonly ITarihYardımcısı _tarihYardımcısı;
        private readonly IResimServisi _resimServisi;
        public AyarlarController(IWorkContext workContext,
            IİzinServisi izinServisi,
            ISiteServisi siteServisi,
            IAyarlarServisi ayarlarServisi,
            ITemaSağlayıcı temaSağlayıcı,
            GenelAyarlar genelAyarlar,
            IKullanıcıİşlemServisi kulllanıcıİşlemServisi,
            ITamMetinServisi tamMetinServisi,
            ITarihYardımcısı tarihYardımcısı,
            IResimServisi resimServisi)
        {
            this._workContext = workContext;
            this._izinServisi = izinServisi;
            this._siteServisi = siteServisi;
            this._ayarlarServisi = ayarlarServisi;
            this._temaSağlayıcı = temaSağlayıcı;
            this._genelAyarlar = genelAyarlar;
            this._kulllanıcıİşlemServisi = kulllanıcıİşlemServisi;
            this._tamMetinServisi = tamMetinServisi;
            this._tarihYardımcısı = tarihYardımcısı;
            this._resimServisi = resimServisi;
        }
        [ChildActionOnly]
        public virtual ActionResult Mod(string modAdı = "ayarlar-gelişmiş-mod")
        {
            var model = new ModModel()
            {
                ModAdı = modAdı,
                Etkin = _workContext.MevcutKullanıcı.ÖznitelikAl<bool>(modAdı)
            };
            return PartialView(model);
        }

        public virtual ActionResult GenelAyarlar()
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.AyarlarıYönet))
                return ErişimEngellendiView();
            this.Server.ScriptTimeout = 300;

            var model = new GenelAyarlarModel();
            var siteScope = this.AktifSiteKapsamYapılandırmaAl(_siteServisi, _workContext);
            //model.ActiveStoreScopeConfiguration = storeScope;
            //site bilgisi
            var siteBilgiAyarları = _ayarlarServisi.AyarYükle<SiteBilgiAyarları>(siteScope);
            var genelAyarlar = _ayarlarServisi.AyarYükle<GenelAyarlar>(siteScope);
            var menuÖğesiAyarları = _ayarlarServisi.AyarYükle<MenuÖğesiAyarları>(siteScope);
            var seoAyarları = _ayarlarServisi.AyarYükle<SeoAyarları>(siteScope);
            var güvenlikAyarları = _ayarlarServisi.AyarYükle<GüvenlikAyarları>(siteScope);

            model.SiteBilgiAyarları.SiteKapalı = siteBilgiAyarları.SiteKapalı;
            //temalar
            model.SiteBilgiAyarları.VarsayılanSiteTeması = siteBilgiAyarları.MevcutSiteTeması;
            model.SiteBilgiAyarları.MevcutSiteTemaları = _temaSağlayıcı
                .TemaAyarlarıAl()
                .Select(x => new GenelAyarlarModel.SiteBilgiAyarlarıModel.TemaYapılandırmaModeli
                {
                    TemaBaşlığı = x.TemaBaşlığı,
                    TemaAdı = x.TemaAdı,
                    ÖnizlemeResimUrl = x.ResimURLÖnizleme,
                    ÖnizlemeYazısı = x.TextÖnizleme,
                    Seçili = x.TemaAdı.Equals(siteBilgiAyarları.MevcutSiteTeması, StringComparison.InvariantCultureIgnoreCase)
                })
                .ToList();
            model.SiteBilgiAyarları.KullanıcılarınTemaSeçmesiEtkin = siteBilgiAyarları.KullanıcılarTemaSeçebilsin;
            model.SiteBilgiAyarları.LogoResimId = siteBilgiAyarları.LogoResimId;
            //EU Çerez yasası
            //model.SiteBilgiAyarları.EuÇerezHukukuUyarısınıGörüntüle = siteBilgiAyarları.;
            //sosyal ağ
            model.SiteBilgiAyarları.FacebookLink = siteBilgiAyarları.FacebookLink;
            model.SiteBilgiAyarları.TwitterLink = siteBilgiAyarları.TwitterLink;
            model.SiteBilgiAyarları.YoutubeLink = siteBilgiAyarları.YoutubeLink;
            model.SiteBilgiAyarları.GooglePlusLink = siteBilgiAyarları.GooglePlusLink;
            //iletişime geçin
            model.SiteBilgiAyarları.İletişimeGeçinFormundaKonuAlanı = _genelAyarlar.İletişimFormuKonuBaşlığı;
            model.SiteBilgiAyarları.İletişimFormuİçinSistemEMailiniKullan = _genelAyarlar.İletişimFormuSistemMaili;
            //siteharitası
            model.SiteBilgiAyarları.SiteHaritasıEtkin = _genelAyarlar.SiteHaritasıEtkin;
            model.SiteBilgiAyarları.SiteHaritasıKategorileriİçerir = _genelAyarlar.SiteHaritasındaKategoriler;

            model.VarsayılanMenuÖğeleri.AnasayfaMenuÖğesi = menuÖğesiAyarları.AnasayfaMenuÖğesi;
            model.VarsayılanMenuÖğeleri.KullanıcıBilgisiMenuÖğesi = menuÖğesiAyarları.KullanıcıBilgisiMenuÖğesi;
            model.VarsayılanMenuÖğeleri.BlogMenuÖğesi = menuÖğesiAyarları.BlogMenuÖğesi;
            model.VarsayılanMenuÖğeleri.ForumMenuÖğesi = menuÖğesiAyarları.ForumMenuÖğesi;
            model.VarsayılanMenuÖğeleri.İletişimMenuÖğesi = menuÖğesiAyarları.İletişimMenuÖğesi;
            
            //seo ayarları
            model.SeoAyarları.SayfaBaşlığıAyırıcı = seoAyarları.SayfaBaşlığıAyırıcısı;
            model.SeoAyarları.SayfaBaşlığıSeoAyarları = (int)seoAyarları.SayfaBaşlığıSeoAyarı;
            model.SeoAyarları.SayfaBaşlığıSeoAyarlarıDeğeri = seoAyarları.SayfaBaşlığıSeoAyarı.ToSelectList();
            model.SeoAyarları.VarsayılanBaşlık = seoAyarları.VarsayılanBaşlık;
            model.SeoAyarları.VarsayılanMetaKeywords = seoAyarları.VarsayılanMetaKeywordler;
            model.SeoAyarları.VarsayılanMetaDescription = seoAyarları.VarsayılanMetaDescription;
            model.SeoAyarları.BatılıOlmayanKarakterleriDönüşütür = seoAyarları.BatıOlmayanKarakterleriDönüştür;
            model.SeoAyarları.CanonicalUrlEtkin = seoAyarları.CanonicalUrlIzinVer;
            model.SeoAyarları.WwwGerekliliği = (int)seoAyarları.WwwGerekliliği;
            model.SeoAyarları.WwwGerekliliğiDeğeri = seoAyarları.WwwGerekliliği.ToSelectList();
            model.SeoAyarları.JsBundlingEtkin = seoAyarları.JSPaketlemeyeIzinVer;
            model.SeoAyarları.CssBundlingEtkin = seoAyarları.CssPaketlemeyeIzinVer;
            model.SeoAyarları.TwitterMetaTags = seoAyarları.TwitterMetaTagları;
            model.SeoAyarları.OpenGraphMetaTags = seoAyarları.OpenGraphMetaTagları;
            model.SeoAyarları.ÖzelHeadTags = seoAyarları.ÖzelHeadTagları;
           
            
            //güvenlik ayarları
            model.GüvenlikAyarları.EncryptionKey = güvenlikAyarları.ŞifrelemeAnahtarı;
            if (güvenlikAyarları.YöneticiAlanıİzinVerilenIPAdresleri != null)
                for (int i = 0; i < güvenlikAyarları.YöneticiAlanıİzinVerilenIPAdresleri.Count; i++)
                {
                    model.GüvenlikAyarları.AdminAlanıİzinliIpAdresleri += güvenlikAyarları.YöneticiAlanıİzinVerilenIPAdresleri[i];
                    if (i != güvenlikAyarları.YöneticiAlanıİzinVerilenIPAdresleri.Count - 1)
                        model.GüvenlikAyarları.AdminAlanıİzinliIpAdresleri += ",";
                }
            model.GüvenlikAyarları.TümSayfalardaSSLKullan = güvenlikAyarları.TümSayfalarıSslİçinZorla;
            model.GüvenlikAyarları.AdminAlanındaXSRFKorumasıEtkin = güvenlikAyarları.YöneticiAlanıiçinXsrfKorumasınıEtkinleştir;
            model.GüvenlikAyarları.SitedeXSRFKorumasıEtkin = güvenlikAyarları.GenelAlaniçinXsrfKorumasınıEtkinleştir;
            model.GüvenlikAyarları.HoneypotEtkin = güvenlikAyarları.HoneypotEtkin;

            var captchaAyarları = _ayarlarServisi.AyarYükle<CaptchaAyarları>(siteScope);
            model.CaptchaAyarları = captchaAyarları.ToModel();
            model.CaptchaAyarları.MevcutCaptchaSürümleri = ReCaptchaSürümü.Sürüm1.ToSelectList(false).ToList();

            //PDF settings
            var pdfAyarları = _ayarlarServisi.AyarYükle<PdfAyarları>(siteScope);
            model.PdfAyarları.HarfSayfaBüyüklüğüEtkin = pdfAyarları.HarfSayfaBüyüklüğüEtkin;
            model.PdfAyarları.LogoResimId = pdfAyarları.LogoResimId;

            model.TamMetinAyarları.Destekli = _tamMetinServisi.TamMetinDestekli();
            model.TamMetinAyarları.Etkin = genelAyarlar.TamMetinAramayıKullan;
            model.TamMetinAyarları.AramaModu = (int)genelAyarlar.TamMetinModu;
            model.TamMetinAyarları.AramaModuDeğerleri = genelAyarlar.TamMetinModu.ToSelectList();


            /*
                        //localization
                        var localizationSettings = _ayarlarServisi.LoadSetting<LocalizationSettings>(storeScope);
                        model.LocalizationSettings.UseImagesForLanguageSelection = localizationSettings.UseImagesForLanguageSelection;
                        model.LocalizationSettings.SeoFriendlyUrlsForLanguagesEnabled = localizationSettings.SeoFriendlyUrlsForLanguagesEnabled;
                        model.LocalizationSettings.AutomaticallyDetectLanguage = localizationSettings.AutomaticallyDetectLanguage;
                        model.LocalizationSettings.LoadAllLocaleRecordsOnStartup = localizationSettings.LoadAllLocaleRecordsOnStartup;
                        model.LocalizationSettings.LoadAllLocalizedPropertiesOnStartup = localizationSettings.LoadAllLocalizedPropertiesOnStartup;
                        model.LocalizationSettings.LoadAllUrlRecordsOnStartup = localizationSettings.LoadAllUrlRecordsOnStartup;

                        //full-text support
                        model.FullTextSettings.Supported = _fulltextService.IsFullTextSupported();
                        model.FullTextSettings.Enabled = _genelAyarlar.UseFullTextSearch;
                        model.FullTextSettings.SearchMode = (int)_genelAyarlar.FullTextMode;
                        model.FullTextSettings.SearchModeValues = _genelAyarlar.FullTextMode.ToSelectList();

                        //display default menu item
                        var displayDefaultMenuItemSettings = _ayarlarServisi.LoadSetting<DisplayDefaultMenuItemSettings>(storeScope);
                        model.DisplayDefaultMenuItemSettings.DisplayHomePageMenuItem = displayDefaultMenuItemSettings.DisplayHomePageMenuItem;
                        model.DisplayDefaultMenuItemSettings.DisplayNewProductsMenuItem = displayDefaultMenuItemSettings.DisplayNewProductsMenuItem;
                        model.DisplayDefaultMenuItemSettings.DisplayProductSearchMenuItem = displayDefaultMenuItemSettings.DisplayProductSearchMenuItem;
                        model.DisplayDefaultMenuItemSettings.DisplayCustomerInfoMenuItem = displayDefaultMenuItemSettings.DisplayCustomerInfoMenuItem;
                        model.DisplayDefaultMenuItemSettings.DisplayBlogMenuItem = displayDefaultMenuItemSettings.DisplayBlogMenuItem;
                        model.DisplayDefaultMenuItemSettings.DisplayForumsMenuItem = displayDefaultMenuItemSettings.DisplayForumsMenuItem;
                        model.DisplayDefaultMenuItemSettings.DisplayContactUsMenuItem = displayDefaultMenuItemSettings.DisplayContactUsMenuItem;

                        model.DisplayDefaultMenuItemSettings.DisplayHomePageMenuItem_OverrideForStore = _ayarlarServisi.AyarlarMevcut(displayDefaultMenuItemSettings, x => x.DisplayHomePageMenuItem, storeScope);
                        model.DisplayDefaultMenuItemSettings.DisplayNewProductsMenuItem_OverrideForStore = _ayarlarServisi.AyarlarMevcut(displayDefaultMenuItemSettings, x => x.DisplayNewProductsMenuItem, storeScope);
                        model.DisplayDefaultMenuItemSettings.DisplayProductSearchMenuItem_OverrideForStore = _ayarlarServisi.AyarlarMevcut(displayDefaultMenuItemSettings, x => x.DisplayProductSearchMenuItem, storeScope);
                        model.DisplayDefaultMenuItemSettings.DisplayCustomerInfoMenuItem_OverrideForStore = _ayarlarServisi.AyarlarMevcut(displayDefaultMenuItemSettings, x => x.DisplayCustomerInfoMenuItem, storeScope);
                        model.DisplayDefaultMenuItemSettings.DisplayBlogMenuItem_OverrideForStore = _ayarlarServisi.AyarlarMevcut(displayDefaultMenuItemSettings, x => x.DisplayBlogMenuItem, storeScope);
                        model.DisplayDefaultMenuItemSettings.DisplayForumsMenuItem_OverrideForStore = _ayarlarServisi.AyarlarMevcut(displayDefaultMenuItemSettings, x => x.DisplayForumsMenuItem, storeScope);
                        model.DisplayDefaultMenuItemSettings.DisplayContactUsMenuItem_OverrideForStore = _ayarlarServisi.AyarlarMevcut(displayDefaultMenuItemSettings, x => x.DisplayContactUsMenuItem, storeScope);
                        */

            return View(model);
        }
        [HttpPost]
        [FormDeğeriGerekli("kaydet")]
        public virtual ActionResult GenelAyarlar(GenelAyarlarModel model)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.AyarlarıYönet))
                return ErişimEngellendiView();

            var siteScope = this.AktifSiteKapsamYapılandırmaAl(_siteServisi, _workContext);
            var siteBilgiAyarları = _ayarlarServisi.AyarYükle<SiteBilgiAyarları>(siteScope);
            var genelAyarlar = _ayarlarServisi.AyarYükle<GenelAyarlar>(siteScope);
            var menuÖğesiAyarlar = _ayarlarServisi.AyarYükle<MenuÖğesiAyarları>(siteScope);
            var seoAyarları = _ayarlarServisi.AyarYükle<SeoAyarları>(siteScope);
            var güvenlikAyarları = _ayarlarServisi.AyarYükle<GüvenlikAyarları>(siteScope);
            var pdfAyarları = _ayarlarServisi.AyarYükle<PdfAyarları>(siteScope);
            siteBilgiAyarları.SiteKapalı=model.SiteBilgiAyarları.SiteKapalı ;
            //temalar
            siteBilgiAyarları.MevcutSiteTeması= model.SiteBilgiAyarları.VarsayılanSiteTeması;
            siteBilgiAyarları.KullanıcılarTemaSeçebilsin= model.SiteBilgiAyarları.KullanıcılarınTemaSeçmesiEtkin  ;
            siteBilgiAyarları.LogoResimId =model.SiteBilgiAyarları.LogoResimId  ;
            //EU Çerez yasası
            //model.SiteBilgiAyarları.EuÇerezHukukuUyarısınıGörüntüle = siteBilgiAyarları.;
            //sosyal ağ
            siteBilgiAyarları.FacebookLink=model.SiteBilgiAyarları.FacebookLink  ;
            siteBilgiAyarları.TwitterLink=model.SiteBilgiAyarları.TwitterLink  ;
            siteBilgiAyarları.YoutubeLink=model.SiteBilgiAyarları.YoutubeLink  ;
            siteBilgiAyarları.GooglePlusLink=model.SiteBilgiAyarları.GooglePlusLink  ;
            //iletişime geçin
            genelAyarlar.İletişimFormuKonuBaşlığı=model.SiteBilgiAyarları.İletişimeGeçinFormundaKonuAlanı  ;
            genelAyarlar.İletişimFormuSistemMaili=model.SiteBilgiAyarları.İletişimFormuİçinSistemEMailiniKullan  ;
            //siteharitası
            genelAyarlar.SiteHaritasıEtkin=model.SiteBilgiAyarları.SiteHaritasıEtkin  ;
            genelAyarlar.SiteHaritasındaKategoriler=model.SiteBilgiAyarları.SiteHaritasıKategorileriİçerir  ;

            menuÖğesiAyarlar.AnasayfaMenuÖğesi = model.VarsayılanMenuÖğeleri.AnasayfaMenuÖğesi;
            menuÖğesiAyarlar.KullanıcıBilgisiMenuÖğesi = model.VarsayılanMenuÖğeleri.KullanıcıBilgisiMenuÖğesi;
            menuÖğesiAyarlar.BlogMenuÖğesi = model.VarsayılanMenuÖğeleri.BlogMenuÖğesi;
            menuÖğesiAyarlar.ForumMenuÖğesi = model.VarsayılanMenuÖğeleri.ForumMenuÖğesi;
            menuÖğesiAyarlar.İletişimMenuÖğesi = model.VarsayılanMenuÖğeleri.İletişimMenuÖğesi;

            //seo ayarları
            seoAyarları.SayfaBaşlığıAyırıcısı = model.SeoAyarları.SayfaBaşlığıAyırıcı;
            seoAyarları.SayfaBaşlığıSeoAyarı = (SayfaBaşlığıSeoAyarı)model.SeoAyarları.SayfaBaşlığıSeoAyarları;
            seoAyarları.VarsayılanBaşlık = model.SeoAyarları.VarsayılanBaşlık;
            seoAyarları.VarsayılanMetaKeywordler = model.SeoAyarları.VarsayılanMetaKeywords;
            seoAyarları.VarsayılanMetaDescription = model.SeoAyarları.VarsayılanMetaDescription;
            seoAyarları.BatıOlmayanKarakterleriDönüştür = model.SeoAyarları.BatılıOlmayanKarakterleriDönüşütür;
            seoAyarları.CanonicalUrlIzinVer = model.SeoAyarları.CanonicalUrlEtkin;
            seoAyarları.WwwGerekliliği = (WwwGerekliliği)model.SeoAyarları.WwwGerekliliği;
            seoAyarları.JSPaketlemeyeIzinVer = model.SeoAyarları.JsBundlingEtkin;
            seoAyarları.CssPaketlemeyeIzinVer = model.SeoAyarları.CssBundlingEtkin;
            seoAyarları.TwitterMetaTagları = model.SeoAyarları.TwitterMetaTags;
            seoAyarları.OpenGraphMetaTagları = model.SeoAyarları.OpenGraphMetaTags;
            seoAyarları.ÖzelHeadTagları = model.SeoAyarları.ÖzelHeadTags;

            //Güvenlik ayarları
            if (güvenlikAyarları.YöneticiAlanıİzinVerilenIPAdresleri == null)
                güvenlikAyarları.YöneticiAlanıİzinVerilenIPAdresleri = new List<string>();
            güvenlikAyarları.YöneticiAlanıİzinVerilenIPAdresleri.Clear();
            if (!String.IsNullOrEmpty(model.GüvenlikAyarları.AdminAlanıİzinliIpAdresleri))
                foreach (string s in model.GüvenlikAyarları.AdminAlanıİzinliIpAdresleri.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    if (!String.IsNullOrWhiteSpace(s))
                        güvenlikAyarları.YöneticiAlanıİzinVerilenIPAdresleri.Add(s.Trim());
            güvenlikAyarları.TümSayfalarıSslİçinZorla = model.GüvenlikAyarları.TümSayfalardaSSLKullan;
            güvenlikAyarları.YöneticiAlanıiçinXsrfKorumasınıEtkinleştir = model.GüvenlikAyarları.AdminAlanındaXSRFKorumasıEtkin;
            güvenlikAyarları.GenelAlaniçinXsrfKorumasınıEtkinleştir = model.GüvenlikAyarları.SitedeXSRFKorumasıEtkin;
            güvenlikAyarları.HoneypotEtkin = model.GüvenlikAyarları.HoneypotEtkin;

            //captcha settings
            var captchaAyarları = _ayarlarServisi.AyarYükle<CaptchaAyarları>(siteScope);
            captchaAyarları = model.CaptchaAyarları.ToEntity(captchaAyarları);
            if (captchaAyarları.Etkin &&
                (String.IsNullOrWhiteSpace(captchaAyarları.ReCaptchaPublicKey) || String.IsNullOrWhiteSpace(captchaAyarları.ReCaptchaPrivateKey)))
            {
                HataBildirimi("Doğrulama kodu girilmedi");
            }

            //pdf ayarları
            pdfAyarları.HarfSayfaBüyüklüğüEtkin = model.PdfAyarları.HarfSayfaBüyüklüğüEtkin;
            pdfAyarları.LogoResimId = model.PdfAyarları.LogoResimId;

            //Tam metin ayarları
            genelAyarlar.TamMetinModu = (TamMetinAramaModu)model.TamMetinAyarları.AramaModu;

            //_ayarlarServisi.ÖnbelleğiTemizle();
            _ayarlarServisi.AyarKaydet(genelAyarlar);
            _ayarlarServisi.AyarKaydet(siteBilgiAyarları);
            _ayarlarServisi.AyarKaydet(menuÖğesiAyarlar);
            _ayarlarServisi.AyarKaydet(seoAyarları); 
            _ayarlarServisi.AyarKaydet(güvenlikAyarları);
            _ayarlarServisi.AyarKaydet(captchaAyarları);
            _ayarlarServisi.AyarKaydet(pdfAyarları);


            //activity log
            _kulllanıcıİşlemServisi.İşlemEkle("AyarlarGüncellendi", "Ayarlar Güncellendi");

            BaşarılıBildirimi("Ayarlar Güncellendi");

            return RedirectToAction("GenelAyarlar");
        }
        /*
        [HttpPost, ActionName("GenelAyarlar")]
        [FormDeğeriGerekli("encryptionkeydeğiştir")]
        public virtual ActionResult EncryptionKeyDeğiştir(GenelAyarlarModel model)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.AyarlarıYönet))
                return ErişimEngellendiView();

            this.Server.ScriptTimeout = 300;

            var siteScope = this.AktifSiteKapsamYapılandırmaAl(_siteServisi, _workContext);
            var güvenlikAyarları = _ayarlarServisi.AyarYükle<GüvenlikAyarları>(siteScope);

            try
            {
                if (model.GüvenlikAyarları.EncryptionKey == null)
                    model.GüvenlikAyarları.EncryptionKey = "";

                model.GüvenlikAyarları.EncryptionKey = model.GüvenlikAyarları.EncryptionKey.Trim();

                var newEncryptionPrivateKey = model.GüvenlikAyarları.EncryptionKey;
                if (String.IsNullOrEmpty(newEncryptionPrivateKey) || newEncryptionPrivateKey.Length != 16)
                    throw new Exception("çok kısa");

                string oldEncryptionPrivateKey = güvenlikAyarları.EncryptionKey;
                if (oldEncryptionPrivateKey == newEncryptionPrivateKey)
                    throw new NopException(_localizationService.GetResource("Admin.Configuration.Settings.GeneralCommon.EncryptionKey.TheSame"));

                //update encrypted order info
                var orders = _orderService.SearchOrders();
                foreach (var order in orders)
                {
                    string decryptedCardType = _encryptionService.DecryptText(order.CardType, oldEncryptionPrivateKey);
                    string decryptedCardName = _encryptionService.DecryptText(order.CardName, oldEncryptionPrivateKey);
                    string decryptedCardNumber = _encryptionService.DecryptText(order.CardNumber, oldEncryptionPrivateKey);
                    string decryptedMaskedCreditCardNumber = _encryptionService.DecryptText(order.MaskedCreditCardNumber, oldEncryptionPrivateKey);
                    string decryptedCardCvv2 = _encryptionService.DecryptText(order.CardCvv2, oldEncryptionPrivateKey);
                    string decryptedCardExpirationMonth = _encryptionService.DecryptText(order.CardExpirationMonth, oldEncryptionPrivateKey);
                    string decryptedCardExpirationYear = _encryptionService.DecryptText(order.CardExpirationYear, oldEncryptionPrivateKey);

                    string encryptedCardType = _encryptionService.EncryptText(decryptedCardType, newEncryptionPrivateKey);
                    string encryptedCardName = _encryptionService.EncryptText(decryptedCardName, newEncryptionPrivateKey);
                    string encryptedCardNumber = _encryptionService.EncryptText(decryptedCardNumber, newEncryptionPrivateKey);
                    string encryptedMaskedCreditCardNumber = _encryptionService.EncryptText(decryptedMaskedCreditCardNumber, newEncryptionPrivateKey);
                    string encryptedCardCvv2 = _encryptionService.EncryptText(decryptedCardCvv2, newEncryptionPrivateKey);
                    string encryptedCardExpirationMonth = _encryptionService.EncryptText(decryptedCardExpirationMonth, newEncryptionPrivateKey);
                    string encryptedCardExpirationYear = _encryptionService.EncryptText(decryptedCardExpirationYear, newEncryptionPrivateKey);

                    order.CardType = encryptedCardType;
                    order.CardName = encryptedCardName;
                    order.CardNumber = encryptedCardNumber;
                    order.MaskedCreditCardNumber = encryptedMaskedCreditCardNumber;
                    order.CardCvv2 = encryptedCardCvv2;
                    order.CardExpirationMonth = encryptedCardExpirationMonth;
                    order.CardExpirationYear = encryptedCardExpirationYear;
                    _orderService.UpdateOrder(order);
                }
                //update password information
                //optimization - load only passwords with PasswordFormat.Encrypted
                var customerPasswords = _customerService.GetCustomerPasswords(passwordFormat: PasswordFormat.Encrypted);
                foreach (var customerPassword in customerPasswords)
                {
                    var decryptedPassword = _encryptionService.DecryptText(customerPassword.Password, oldEncryptionPrivateKey);
                    var encryptedPassword = _encryptionService.EncryptText(decryptedPassword, newEncryptionPrivateKey);

                    customerPassword.Password = encryptedPassword;
                    _customerService.UpdateCustomerPassword(customerPassword);
                }

                securitySettings.EncryptionKey = newEncryptionPrivateKey;
                _ayarlarServisi.SaveSetting(securitySettings);
                SuccessNotification(_localizationService.GetResource("Admin.Configuration.Settings.GeneralCommon.EncryptionKey.Changed"));
            }
            catch (Exception exc)
            {
                ErrorNotification(exc);
            }
            return RedirectToAction("GeneralCommon");
        }
        */

        public virtual ActionResult Kullanıcı()
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.AyarlarıYönet))
                return ErişimEngellendiView();

            var siteScope = this.AktifSiteKapsamYapılandırmaAl(_siteServisi, _workContext);
            var kullanıcıAyarları = _ayarlarServisi.AyarYükle<KullanıcıAyarları>(siteScope);
            var adresAyarları = _ayarlarServisi.AyarYükle<AdresAyarları>(siteScope);
            var tarihAyarları = _ayarlarServisi.AyarYükle<TarihAyarları>(siteScope);
            var hariciYetkilendirmeAyarları = _ayarlarServisi.AyarYükle<HariciYetkilendirmeAyarları>(siteScope);

            //ayarları birleştir
            var model = new KullanıcıAyarlarModel();
            model.KullanıcıAyarları = kullanıcıAyarları.ToModel();
            model.AdresAyarlari = adresAyarları.ToModel();

            model.TarihAyarları.KullanıcılarZamanDilimiAyarlayabilir = tarihAyarları.KullanıcıZamanDilimiAyarlamasıİzinli;
            model.TarihAyarları.VarsayılanSiteZamanDilimiId = _tarihYardımcısı.SiteVarsayılanZamanDilimi.Id;
            foreach (TimeZoneInfo timeZone in _tarihYardımcısı.SistemZamanDilimiAl())
            {
                model.TarihAyarları.MevcutZamanDilimleri.Add(new SelectListItem
                {
                    Text = timeZone.DisplayName,
                    Value = timeZone.Id,
                    Selected = timeZone.Id.Equals(_tarihYardımcısı.SiteVarsayılanZamanDilimi.Id, StringComparison.InvariantCultureIgnoreCase)
                });
            }

            model.HariciKimlikDoğrulamaAyarları.OtomatikKayıtEtkin = hariciYetkilendirmeAyarları.OtoKayıtEtkin;

            return View(model);
        }
        [HttpPost]
        public virtual ActionResult Kullanıcı(KullanıcıAyarlarModel model)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.AyarlarıYönet))
                return ErişimEngellendiView();

            var siteScope = this.AktifSiteKapsamYapılandırmaAl(_siteServisi, _workContext);
            var kullanıcıAyarları = _ayarlarServisi.AyarYükle<KullanıcıAyarları>(siteScope);
            var adresAyarları = _ayarlarServisi.AyarYükle<AdresAyarları>(siteScope);
            var tarihAyarları = _ayarlarServisi.AyarYükle<TarihAyarları>(siteScope);
            var hariciYetkilendirmeAyarları = _ayarlarServisi.AyarYükle<HariciYetkilendirmeAyarları>(siteScope);


            kullanıcıAyarları = model.KullanıcıAyarları.ToEntity(kullanıcıAyarları);
            _ayarlarServisi.AyarKaydet(kullanıcıAyarları);

            adresAyarları = model.AdresAyarlari.ToEntity(adresAyarları);
            _ayarlarServisi.AyarKaydet(adresAyarları);

            tarihAyarları.SiteVarsayılanZamanDilimiId = model.TarihAyarları.VarsayılanSiteZamanDilimiId;
            tarihAyarları.KullanıcıZamanDilimiAyarlamasıİzinli = model.TarihAyarları.KullanıcılarZamanDilimiAyarlayabilir;
            _ayarlarServisi.AyarKaydet(tarihAyarları);

            hariciYetkilendirmeAyarları.OtoKayıtEtkin = model.HariciKimlikDoğrulamaAyarları.OtomatikKayıtEtkin;
            _ayarlarServisi.AyarKaydet(hariciYetkilendirmeAyarları);

            //işlemKaydı
            _kulllanıcıİşlemServisi.İşlemEkle("AyarlarDüzenlendi", "Ayarlar Düzenlendi");

            BaşarılıBildirimi("Ayarlar güncellendi");

            //seçili tab
            SeçiliTabKaydet();

            return RedirectToAction("Kullanıcı");
        }

        public virtual ActionResult Blog()
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.AyarlarıYönet))
                return ErişimEngellendiView();

            var siteScope = this.AktifSiteKapsamYapılandırmaAl(_siteServisi, _workContext);
            var blogAyarları = _ayarlarServisi.AyarYükle<BlogAyarları>(siteScope);
            var model = blogAyarları.ToModel();
            model.ActiveStoreScopeConfiguration = siteScope;
            return View(model);
        }
        [HttpPost]
        public virtual ActionResult Blog(BlogAyarlarModel model)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.AyarlarıYönet))
                return ErişimEngellendiView();

            var siteScope = this.AktifSiteKapsamYapılandırmaAl(_siteServisi, _workContext);
            var blogAyarları = _ayarlarServisi.AyarYükle<BlogAyarları>(siteScope);
            blogAyarları = model.ToEntity(blogAyarları);
            _ayarlarServisi.AyarKaydet(blogAyarları);
            //_ayarlarServisi.AyarKaydet(blogAyarları, x => x.BlogYorumlarıTümSitelerdeGöster, önbelleğiTemizle: false);
            //önbelleği temizle
            _ayarlarServisi.ÖnbelleğiTemizle();

            //işlem kaydı
            _kulllanıcıİşlemServisi.İşlemEkle("AyarlarDüzenle", "Ayarlar Düzenle");
            BaşarılıBildirimi("Güncellendi");
            return RedirectToAction("Blog");
        }

        public virtual ActionResult Medya()
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.AyarlarıYönet))
                return ErişimEngellendiView();
            
            var storeScope = this.AktifSiteKapsamYapılandırmaAl(_siteServisi, _workContext);
            var mediaSettings = _ayarlarServisi.AyarYükle<MedyaAyarları>(storeScope);
            var model = mediaSettings.ToModel();
            model.ResimVeritabanındaDepola = _resimServisi.VeritabanındaDepola;
            return View(model);
        }
        [HttpPost]
        [FormDeğeriGerekli("kaydet")]
        public virtual ActionResult Medya(MedyaAyarlarıModel model)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.AyarlarıYönet))
                return ErişimEngellendiView();

            //load settings for a chosen store scope
            var storeScope = this.AktifSiteKapsamYapılandırmaAl(_siteServisi, _workContext);
            var mediaSettings = _ayarlarServisi.AyarYükle<MedyaAyarları>(storeScope);
            mediaSettings = model.ToEntity(mediaSettings);
            
            _ayarlarServisi.ÖnbelleğiTemizle();
            
            _kulllanıcıİşlemServisi.İşlemEkle("AyarlarDüzenlendi", "AyarlarDüzenlendi");

            BaşarılıBildirimi("Güncellendi");
            return RedirectToAction("Medya");
        }
    }
}