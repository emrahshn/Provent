using System.ComponentModel;
using System.Web.Mvc;
using Web.Framework.Mvc;

namespace Web.Models.Crm
{
    public partial class CrmUnvanModel : TemelTSEntityModel
    {
        [DisplayName("Adı")]
        [AllowHtml]
        public string Adı { get; set; }
    }

}