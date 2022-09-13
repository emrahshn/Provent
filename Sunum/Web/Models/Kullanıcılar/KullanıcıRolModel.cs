using FluentValidation.Attributes;
using System.ComponentModel;
using System.Web.Mvc;
using Web.Framework.Mvc;

namespace Web.Models.Kullanıcılar
{
    //[Validator(typeof(CustomerRoleValidator))]
    public partial class KullanıcıRolModel : TemelTSEntityModel
    {
        [DisplayName("Adı")]
        [AllowHtml]
        public string Adı { get; set; }
        [DisplayName("Aktif")]
        public bool Aktif { get; set; }

        [DisplayName("Sistem Rolü")]
        public bool SistemRolü { get; set; }

        [DisplayName("Sistem Adı")]
        public string SistemAdı { get; set; }

        [DisplayName("ParolaÖmrünüEtkinleştir")]
        public bool ParolaÖmrünüEtkinleştir { get; set; }
        
    }
}