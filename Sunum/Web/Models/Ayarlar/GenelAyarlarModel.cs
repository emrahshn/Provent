using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Web.Framework.Güvenlik.Captcha;
using Web.Framework.Mvc;

namespace Web.Models.Ayarlar
{
    public partial class GenelAyarlarModel : TemelTSModel
    {
        public GenelAyarlarModel()
        {
            SiteBilgiAyarları = new SiteBilgiAyarlarıModel();
            VarsayılanMenuÖğeleri = new VarsayılanMenuÖğeleriModel();
            SeoAyarları = new SeoAyarlarıModel();
            GüvenlikAyarları = new GüvenlikAyarlarıModel();
            CaptchaAyarları = new CaptchaAyarlarıModel();
            PdfAyarları = new PdfAyarlarModel();
            TamMetinAyarları = new TamMetinAyarlarıModel();
        }
        public SiteBilgiAyarlarıModel SiteBilgiAyarları { get; set; }
        public VarsayılanMenuÖğeleriModel VarsayılanMenuÖğeleri { get; set; }
        public SeoAyarlarıModel SeoAyarları { get; set; }
        public GüvenlikAyarlarıModel GüvenlikAyarları { get; set; }
        public CaptchaAyarlarıModel CaptchaAyarları { get; set; }
        public PdfAyarlarModel PdfAyarları { get; set; }
        public TamMetinAyarlarıModel TamMetinAyarları { get; set; }

        #region SiteBilgiAyarları
        public partial class SiteBilgiAyarlarıModel : TemelTSModel
        {
            public SiteBilgiAyarlarıModel()
            {
                this.MevcutSiteTemaları = new List<TemaYapılandırmaModeli>();
            }
            [DisplayName("Site Kapalı")]
            public bool SiteKapalı { get; set; }

            [DisplayName("Varsayılan Site Teması")]
            [AllowHtml]
            public string VarsayılanSiteTeması { get; set; }
            public IList<TemaYapılandırmaModeli> MevcutSiteTemaları { get; set; }

            [DisplayName("Kullanıcıların Tema Seçmesi Etkin")]
            public bool KullanıcılarınTemaSeçmesiEtkin { get; set; }

            [UIHint("Picture")]
            [DisplayName("Logo")]
            public int LogoResimId { get; set; }

            [DisplayName("Eu Çerez Hukuku Uyarısını Görüntüle")]
            public bool EuÇerezHukukuUyarısınıGörüntüle { get; set; }

            [DisplayName("FacebookLink")]
            public string FacebookLink { get; set; }

            [DisplayName("TwitterLink")]
            public string TwitterLink { get; set; }

            [DisplayName("YoutubeLink")]
            public string YoutubeLink { get; set; }

            [DisplayName("GooglePlusLink")]
            public string GooglePlusLink { get; set; }

            [DisplayName("İletişime Geçin Formunda Konu Alanı Açık")]
            public bool İletişimeGeçinFormundaKonuAlanı { get; set; }

            [DisplayName("İletişim Formu İçin Sistem E-Mailini Kullan")]
            public bool İletişimFormuİçinSistemEMailiniKullan { get; set; }

            [DisplayName("Site Haritası Etkin")]
            public bool SiteHaritasıEtkin { get; set; }

            [DisplayName("Site Haritası Kategorileri İçerir")]
            public bool SiteHaritasıKategorileriİçerir { get; set; }

            #region TemaYarılandırmaModeli

            public partial class TemaYapılandırmaModeli
            {
                public string TemaAdı { get; set; }
                public string TemaBaşlığı { get; set; }
                public string ÖnizlemeResimUrl { get; set; }
                public string ÖnizlemeYazısı { get; set; }
                public bool Seçili { get; set; }
            }

            #endregion
        }
        #endregion
        #region MenuÖğesiAyarlarıModel
        public partial class VarsayılanMenuÖğeleriModel : TemelTSModel
        {
            [DisplayName("Anasayfa görüntüle")]
            public bool AnasayfaMenuÖğesi { get; set; }
            [DisplayName("Kullanıcı bilgisi görüntüle")]
            public bool KullanıcıBilgisiMenuÖğesi { get; set; }
            [DisplayName("Blog görüntüle")]
            public bool BlogMenuÖğesi { get; set; }
            [DisplayName("Forum görüntüle")]
            public bool ForumMenuÖğesi { get; set; }
            [DisplayName("İletişime geçin görüntüle")]
            public bool İletişimMenuÖğesi { get; set; }
        }
        #endregion
        #region SeoAyarlarıModel
        public partial class SeoAyarlarıModel : TemelTSModel
        {
            [DisplayName("Sayfa başlığı ayırıcı")]
            [AllowHtml]
            [NoTrim]
            public string SayfaBaşlığıAyırıcı { get; set; }

            [DisplayName("Sayfa başlığı Seo ayarları")]
            public int SayfaBaşlığıSeoAyarları { get; set; }
            public SelectList SayfaBaşlığıSeoAyarlarıDeğeri { get; set; }

            [DisplayName("Varsayılan başlık")]
            [AllowHtml]
            public string VarsayılanBaşlık { get; set; }

            [DisplayName("Varsayılan MetaKeywords")]
            [AllowHtml]
            public string VarsayılanMetaKeywords { get; set; }

            [DisplayName("Varsayılan MetaDescription")]
            [AllowHtml]
            public string VarsayılanMetaDescription { get; set; }

            [DisplayName("Batılı Olmayan Karakterleri Dönüşütür")]
            public bool BatılıOlmayanKarakterleriDönüşütür { get; set; }

            [DisplayName("Canonical Url Etkin")]
            public bool CanonicalUrlEtkin { get; set; }

            [DisplayName("Www Gerekliliği")]
            public int WwwGerekliliği { get; set; }
            public SelectList WwwGerekliliğiDeğeri { get; set; }

            [DisplayName("JsBundling Etkin")]
            public bool JsBundlingEtkin { get; set; }

            [DisplayName("CssBundling Etkin")]
            public bool CssBundlingEtkin { get; set; }

            [DisplayName("Twitter MetaTags")]
            public bool TwitterMetaTags { get; set; }

            [DisplayName("OpenGraph MetaTags")]
            public bool OpenGraphMetaTags { get; set; }

            [DisplayName("Özel HeadTags")]
            [AllowHtml]
            public string ÖzelHeadTags { get; set; }
        }
        #endregion
        #region GüvenlikAyarlarıModel
        public partial class GüvenlikAyarlarıModel : TemelTSModel
        {
            [DisplayName("EncryptionKey")]
            [AllowHtml]
            public string EncryptionKey { get; set; }

            [DisplayName("Admin alanı izinli IP adresleri")]
            [AllowHtml]
            public string AdminAlanıİzinliIpAdresleri { get; set; }

            [DisplayName("Tüm Sayfalarda SSL Kullan")]
            public bool TümSayfalardaSSLKullan { get; set; }

            [DisplayName("Admin alanında XSRF koruması etkin")]
            public bool AdminAlanındaXSRFKorumasıEtkin { get; set; }
            [DisplayName("Sitede XSRF koruması etkin")]
            public bool SitedeXSRFKorumasıEtkin { get; set; }

            [DisplayName("Honeypot etkin")]
            public bool HoneypotEtkin { get; set; }
        }
        #endregion
        #region CaptchaAyarlarıModel
        public partial class CaptchaAyarlarıModel : TemelTSModel
        {
            public CaptchaAyarlarıModel()
            {
                this.MevcutCaptchaSürümleri = new List<SelectListItem>();
            }

            [DisplayName("Captcha Etkin")]
            public bool Etkin { get; set; }

            [DisplayName("Giriş sayfasında göster")]
            public bool GirişSayfasındaGöster { get; set; }

            [DisplayName("Kayıt sayfasında göster")]
            public bool KayıtSayfasındaGöster { get; set; }

            [DisplayName("İletişime geçin sayfasında göster")]
            public bool İletişimeGeçinSayfasındaGöster { get; set; }

            [DisplayName("Blog yorumlarında göster")]
            public bool BlogYorumlarındaGöster { get; set; }

            [DisplayName("Haber yorumlarında göster")]
            public bool HaberYorumlarındaGöster { get; set; }

            [DisplayName("reCaptcha PublicKey")]
            [AllowHtml]
            public string ReCaptchaPublicKey { get; set; }

            [DisplayName("reCaptcha PrivateKey")]
            [AllowHtml]
            public string ReCaptchaPrivateKey { get; set; }

            [DisplayName("reCaptcha sürümü")]
            public ReCaptchaSürümü ReCaptchaSürümü { get; set; }

            public IList<SelectListItem> MevcutCaptchaSürümleri { get; set; }
        }
        #endregion
        #region PdfAyarlarModel
        public partial class PdfAyarlarModel : TemelTSModel
        {
            [DisplayName("Pdf harf sayfa büyüklüğü etkin")]
            public bool HarfSayfaBüyüklüğüEtkin { get; set; }

            [DisplayName("Pdf logosu")]
            [UIHint("Picture")]
            public int LogoResimId { get; set; }
        }
        #endregion
        #region TamMetinAyarlarıModel
        public partial class TamMetinAyarlarıModel : TemelTSModel
        {
            public bool Destekli { get; set; }

            public bool Etkin { get; set; }

            [DisplayName("Arama modu")]
            public int AramaModu { get; set; }
            public SelectList AramaModuDeğerleri { get; set; }
        }
        #endregion
    }
}