using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;
using Web.Framework.Mvc;

namespace Web.Models.Kongre
{
    public partial class KayitModel : TemelTSEntityModel
    {
        public KayitModel()
        {
            Kongre = new List<KongreModel>();
        }
        public int KongreId { get; set; }
        [DisplayName("Kongre Adı")]
        [AllowHtml]
        public string KongreAdı { get; set; }
        [DisplayName("Kayıt Tipi")]
        [AllowHtml]
        public string KayıtTipi { get; set; }
        [DisplayName("Kayıt Ücreti")]
        [AllowHtml]
        public string KayıtUcreti { get; set; }
        [DisplayName("Döviz")]
        [AllowHtml]
        public int KayıtUcretiDoviz { get; set; }
        [DisplayName("Dış Katılımcı Farkı")]
        [AllowHtml]
        public string DisKatilimciFarkı { get; set; }
        [DisplayName("Kayıt Notu")]
        [AllowHtml]
        public string KayıtNotu { get; set; }
        public List<KongreModel> Kongre { get; set; }
    }

}