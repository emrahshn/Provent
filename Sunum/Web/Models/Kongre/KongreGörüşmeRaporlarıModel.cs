using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Web.Framework.Mvc;

namespace Web.Models.Kongre
{
    public partial class KongreGörüşmeRaporlarıModel : TemelTSEntityModel
    {
        public int KongreId { get; set; }
        public int MusteriId { get; set; }
        [DisplayName("YetkiliId")]
        [AllowHtml]
        public int YetkiliId { get; set; }
        [DisplayName("GörüsmeTarihi")]
        [AllowHtml]
        public DateTime GörüsmeTarihi { get; set; }
        [DisplayName("OlusturulmaTarihi")]
        [AllowHtml]
        public DateTime OlusturulmaTarihi { get; set; }
        [DisplayName("Görüsen")]
        [AllowHtml]
        public int Görüsen { get; set; }
        [DisplayName("Durumu")]
        [AllowHtml]
        public string Durumu { get; set; }
        [UIHint("RichEditor")]
        [DisplayName("Durumu")]
        [AllowHtml]
        public string Rapor { get; set; }
        public string MusteriAdı { get; set; }
        public string YetkiliAdı { get; set; }
    }

}