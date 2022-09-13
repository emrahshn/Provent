using Core.Domain.Katalog;
using FluentValidation.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Web.Doğrulayıcılar.Kullanıcılar;
using Web.Framework.Mvc;

namespace Web.Models.Kullanıcılar
{
    [Validator(typeof(KullanıcıDoğrulayıcı))]
    public partial class KullanıcıModel : TemelTSEntityModel
    {
        public KullanıcıModel()
        {
            this.EmailGönder = new EmailGöndermeModeli() { HemenGönder = true };
            this.ÖzelMesajGönder = new ÖzelMesajGönderModeli();
            this.SeçiliKullanıcıRolIdleri = new List<int>();
            this.KullanılabilirKullanıcıRolleri = new List<SelectListItem>();
            this.İlişkilendirilmişHariciKimlikDoğrulamaKayıtları = new List<İlişkilendirilmişHariciKimlikDoğrulamaModeli>();
            this.KullanılabilirÜlkeler = new List<SelectListItem>();
            this.KullanıcıÖznitelikleri = new List<KullanıcıÖznitelikModeli>();
            this.AbonelikKayıtlarıKullanılabilirSiteler = new List<SiteModel>();
            this.ÖdülPuanıKullanılabilirSiteler = new List<SelectListItem>();
        }
        public bool KullanıcıAdlarıEtkin { get; set; }
        [AllowHtml]
        public string KullanıcıAdı { get; set; }
        [AllowHtml]
        public string Email { get; set; }
        [AllowHtml]
        [DataType(DataType.Password)]
        [NoTrim]
        public string Şifre { get; set; }

        //form alanı & seçenekler
        public bool CinsiyetEtkin { get; set; }
        public string Cinsiyet { get; set; }
        [AllowHtml]
        public string Adı { get; set; }
        [AllowHtml]
        public string Soyadı { get; set; }
        public string TamAd { get; set; }

        public bool DoğumGünüEtkin { get; set; }
        [UIHint("DateNullable")]
        public DateTime? DoğumGünü { get; set; }

        public bool ŞirketEtkin { get; set; }
        [AllowHtml]
        public string Şirket { get; set; }

        public bool SokakAdresiEtkin { get; set; }
        [AllowHtml]
        public string SokakAdresi { get; set; }
        public bool SokakAdresi2Etkin { get; set; }
        [AllowHtml]
        public string SokakAdresi2 { get; set; }
        public bool PostaKoduEtkin { get; set; }
        [AllowHtml]
        public string PostaKodu { get; set; }
        public bool ŞehirEtkin { get; set; }
        [AllowHtml]
        public string Şehir { get; set; }
        public bool ÜlkeEtkin { get; set; }
        public int ÜlkeId { get; set; }
        public IList<SelectListItem> KullanılabilirÜlkeler { get; set; }
        public bool TelEtkin { get; set; }
        [AllowHtml]
        public string Tel { get; set; }
        public bool FaksEtkin { get; set; }
        [AllowHtml]
        public string Faks { get; set; }
        public List<KullanıcıÖznitelikModeli> KullanıcıÖznitelikleri { get; set; }

        public string SitedeKayıtlı { get; set; }

        [AllowHtml]
        public string AdminYorumu { get; set; }
        public bool Aktif { get; set; }

        //Kayıt tarihi
        public DateTime OluşturulmaTarihi { get; set; }
        public DateTime SonİşlemTarihi { get; set; }

        //IP adresi
        public string SonIpAdresi { get; set; }
        public string SonZiyaretEdilenSayfa { get; set; }

        //kullanıcı rolü
        public string KullanıcıRolAdları { get; set; }
        public List<SelectListItem> KullanılabilirKullanıcıRolleri { get; set; }
        [UIHint("MultiSelect")]
        public IList<int> SeçiliKullanıcıRolIdleri { get; set; }

        //abonelik kayıtları
        public List<SiteModel> AbonelikKayıtlarıKullanılabilirSiteler { get; set; }
        public int[] SeçiliAbonleikKayıtları { get; set; }

        //reward points history
        public bool ÖdülPuanıGeçmişiniGörüntüle { get; set; }
        public int ÖdülPuanıDeğeriEkle { get; set; }
        [AllowHtml]
        public string ÖdülPuanıMesajıEkle { get; set; }
        public int ÖdülPuanıSiteIdEkle { get; set; }
        public IList<SelectListItem> ÖdülPuanıKullanılabilirSiteler { get; set; }

        //email gönder modeli
        public EmailGöndermeModeli EmailGönder { get; set; }
        //PM gönder modeli
        public ÖzelMesajGönderModeli ÖzelMesajGönder { get; set; }
        //hoşgeldiniz mesajı gönder
        public bool HoşgeldinizMesajıGönderimiEtkin { get; set; }
        //yeniden aktivasyon mesajı gönderimi
        public bool AktivasyonMesajıGönderimiEtkin { get; set; }
        public IList<İlişkilendirilmişHariciKimlikDoğrulamaModeli> İlişkilendirilmişHariciKimlikDoğrulamaKayıtları { get; set; }


        #region Nested classes

        public partial class SiteModel : TemelTSEntityModel
        {
            public string Adı { get; set; }
        }

        public partial class İlişkilendirilmişHariciKimlikDoğrulamaModeli : TemelTSEntityModel
        {
            public string Email { get; set; }
            public string HariciDoğrulayıcı { get; set; }
            public string HariciMetodAdı { get; set; }
        }

        public partial class ÖdülPuanıGeçmişiModeli : TemelTSEntityModel
        {
            public string SiteAdı { get; set; }
            public int Puanlar { get; set; }
            public string PuanDengesi { get; set; }
            [AllowHtml]
            public string Mesaj { get; set; }
            public DateTime OluşturulmaTarihi { get; set; }
        }

        public partial class EmailGöndermeModeli : TemelTSModel
        {
            [AllowHtml]
            public string Konu { get; set; }
            [AllowHtml]
            public string Gövde { get; set; }
            public bool HemenGönder { get; set; }
            [UIHint("DateTimeNullable")]
            public DateTime? ŞuTarihtenÖnceGönderme { get; set; }
        }

        public partial class ÖzelMesajGönderModeli : TemelTSModel
        {
            public string Konu { get; set; }
            public string Mesaj { get; set; }
        }

        public partial class İşlemGeçmişiModeli : TemelTSEntityModel
        {
            public string İşlemGeçmişiTipiAdı { get; set; }
            public string Yorum { get; set; }
            public DateTime OluşturulmaTarihi { get; set; }
            public string IpAdresi { get; set; }
        }

        public partial class KullanıcıÖznitelikModeli : TemelTSEntityModel
        {
            public KullanıcıÖznitelikModeli()
            {
                Değerler = new List<KullanıcıÖznitelikDeğerModeli>();
            }
            public string Adı { get; set; }
            public bool Gerekli { get; set; }
            public string VarsayılanDeğer { get; set; }
            public ÖznitelikKontrolTipi ÖznitelikKontrolTipi { get; set; }
            public IList<KullanıcıÖznitelikDeğerModeli> Değerler { get; set; }

        }

        public partial class KullanıcıÖznitelikDeğerModeli : TemelTSEntityModel
        {
            public string Adı { get; set; }
            public bool ÖnSeçildi { get; set; }
        }

        #endregion
    }
}