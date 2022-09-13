using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using FluentValidation.Attributes;
using Web.Framework;
using Web.Framework.Mvc;
using Web.Doğrulayıcılar.Kullanıcılar;

namespace Web.Models.Kullanıcılar
{
    [Validator(typeof(GirişDoğrulayıcı))]
    public partial class GirişModel : TemelTSModel
    {
        //public bool MisafirOlarakÖdeme { get; set; }

        [AllowHtml]
        public string Email { get; set; }

        public bool KullanıcıAdlarıEtkin { get; set; }
        [AllowHtml]
        public string KullanıcıAdı { get; set; }

        [DataType(DataType.Password)]
        [NoTrim]
        [AllowHtml]
        public string Şifre { get; set; }

        public bool BeniHatırla { get; set; }

        public bool CaptchaGörüntüle { get; set; }
    }
}