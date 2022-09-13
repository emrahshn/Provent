using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using FluentValidation.Attributes;
using Web.Framework;
using Web.Framework.Mvc;
using Web.Doğrulayıcılar.Kullanıcılar;

namespace Web.Models.Kullanıcılar
{
    [Validator(typeof(KayıtDoğrulayıcı))]
    public partial class KayıtModel : TemelTSModel
    {
        public KayıtModel()
        {
            //this.AvailableTimeZones = new List<SelectListItem>();
            this.KullanılabilirÜlkeler = new List<SelectListItem>();
            //this.CustomerAttributes = new List<CustomerAttributeModel>();
        }

        [AllowHtml]
        public string Email { get; set; }

        public bool EmailİkiDefa { get; set; }
        [AllowHtml]
        public string EmailOnayla { get; set; }

        public bool KullanıcıAdlarıEtkin { get; set; }
        [AllowHtml]
        public string KullanıcıAdı { get; set; }

        public bool KullanıcıAdıUygunluğu { get; set; }

        [DataType(DataType.Password)]
        [NoTrim]
        [AllowHtml]
        public string Şifre { get; set; }

        [DataType(DataType.Password)]
        [NoTrim]
        [AllowHtml]
        public string ŞifreOnayla { get; set; }

        //form fields & properties
        public bool CinsiyetEtkin { get; set; }
        public string Cinsiyet { get; set; }

        [AllowHtml]
        public string Adı { get; set; }
        [AllowHtml]
        public string Soyadı { get; set; }


        public bool DoğumTarihiEtkin { get; set; }
        public int? DoğumGünü { get; set; }
        public int? DoğumAyı { get; set; }
        public int? DoğumYılı { get; set; }
        public bool DoğumTarihiGerekli { get; set; }
        public DateTime? DoğumTarihiniParçala()
        {
            if (!DoğumYılı.HasValue || !DoğumAyı.HasValue || !DoğumGünü.HasValue)
                return null;

            DateTime? doğumTarihi = null;
            try
            {
                doğumTarihi = new DateTime(DoğumYılı.Value, DoğumAyı.Value, DoğumGünü.Value);
            }
            catch { }
            return doğumTarihi;
        }

        public bool ŞirketEtkin { get; set; }
        public bool ŞirketGerekli { get; set; }
        [AllowHtml]
        public string Şirket { get; set; }

        public bool SokakAdresiEtkin { get; set; }
        public bool SokakAdresiGerekli { get; set; }
        [AllowHtml]
        public string SokakAdresi { get; set; }
        public bool SokakAdresiEtkin2 { get; set; }
        public bool SokakAdresiGerekli2 { get; set; }
        [AllowHtml]
        public string SokakAdresi2 { get; set; }

        public bool PostaKoduEtkin { get; set; }
        public bool PostaKoduGerekli { get; set; }
        [AllowHtml]
        public string PostaKodu { get; set; }

        public bool ŞehirEtkin { get; set; }
        public bool ŞehirGerekli { get; set; }
        [AllowHtml]
        public string Şehir { get; set; }

        public bool ÜlkeEtkin { get; set; }
        public bool ÜlkeGerekli { get; set; }
        public int ÜlkeId { get; set; }
        public IList<SelectListItem> KullanılabilirÜlkeler { get; set; }

        public bool TelEtkin { get; set; }
        public bool TelGerekli { get; set; }
        [AllowHtml]
        public string Tel { get; set; }

        public bool FaksEtkin { get; set; }
        public bool FaksGerekli { get; set; }
        [AllowHtml]
        public string Faks { get; set; }
        public bool BültenEtkin { get; set; }
        public bool Bülten { get; set; }
        public bool AbonelikEtkin { get; set; }
        public bool Abonelik { get; set; }

        public bool GizlilikSözleşmesiEtkin { get; set; }

        public bool HoneypotEtkin { get; set; }
        public bool CaptchaGörüntüle { get; set; }

        //public IList<CustomerAttributeModel> CustomerAttributes { get; set; }
    }
}