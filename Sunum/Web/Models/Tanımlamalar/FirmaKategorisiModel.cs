using System.ComponentModel;
using System.Web.Mvc;
using Web.Framework.Mvc;

namespace Web.Models.Tanımlamalar
{
    public partial class FirmaKategorisiModel : TemelTSEntityModel
    {
        [DisplayName("Adı")]
        [AllowHtml]
        public string Adı { get; set; }
    }
}