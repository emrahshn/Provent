using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Web.Framework.Mvc;
using Web.Models.Genel;
using Web.Models.Konum;

namespace Web.Models.Kongre
{
    public partial class KongreModel : TemelTSEntityModel
    {
        public KongreModel()
        {
            Sehirler = new List<SelectListItem>();
            Ilceler = new List<SelectListItem>();
            KullanılabilirMusteriler = new List<SelectListItem>();
            KullanılabilirKontenjanlar = new List<SelectListItem>();
        }
        [DisplayName("Kongre Adı")]
        [AllowHtml]
        public string Adı { get; set; }
        [DisplayName("Kongre Kodu")]
        [AllowHtml]
        public string Kodu { get; set; }
        [DisplayName("Şehir")]
        [AllowHtml]
        public int SehirId { get; set; }
        [DisplayName("İlçe")]
        [AllowHtml]
        public int IlceId { get; set; }
        [UIHint("Date")]
        [DisplayName("Başlama Tarihi")]
        [AllowHtml]
        public DateTime BaslamaTarihi { get; set; }
        [UIHint("Date")]
        [DisplayName("Bitiş Tarihi")]
        [AllowHtml]
        public DateTime BitisTarihi { get; set; }
        [DisplayName("Dolar Kuru")]
        [AllowHtml]
        public decimal KurDolar { get; set; }
        [DisplayName("Euro Kuru")]
        [AllowHtml]
        public decimal KurEuro { get; set; }
        [UIHint("RichEditor")]
        [DisplayName("Açıklama")]
        [AllowHtml]
        public string Açıklama { get; set; }
        [UIHint("RichEditor")]
        [DisplayName("Kongre İptal Şartları")]
        [AllowHtml]
        public string KongreIptalSartları { get; set; }
        public IList<SelectListItem> KullanılabilirMusteriler { get; set; }
        public IList<SelectListItem> KullanılabilirKontenjanlar { get; set; }
        public IList<SelectListItem> Sehirler { get; set; }
        public IList<SelectListItem> Ilceler { get; set; }
        public int MevcutMusteri { get; set; }
        public int HedefId { get; set; }
    }
    public class KongrelerModel:TemelTSEntityModel
    {
        public KongrelerModel()
        {
            Kongreler = new List<KongreModel>();
        }
        public PagerModel PagerModel { get; set; }
        public IList<KongreModel> Kongreler { get; set; }
        public partial class KongrelerRouteValues : IRouteValues
        {
            public int page { get; set; }
        }
    }
   
}