using System.ComponentModel;
using System.Web.Mvc;
using Web.Framework.Mvc;

namespace Web.Models.Notlar
{
    public partial class NotModel : TemelTSEntityModel
    {
        [DisplayName("KullanıcıId")]
        [AllowHtml]
        public int KullanıcıId { get; set; }
        [DisplayName("GrupId")]
        [AllowHtml]
        public int GrupId { get; set; }
        [DisplayName("Grup")]
        [AllowHtml]
        public string Grup { get; set; }
        [DisplayName("Icerik")]
        [AllowHtml]
        public string Icerik { get; set; }
    }

}