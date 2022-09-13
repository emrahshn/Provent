using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;
using Web.Framework.Mvc;

namespace Web.Models.Kongre
{
    public partial class GelirGiderHedefiModel : TemelTSEntityModel
    {
        public GelirGiderHedefiModel()
        {
            KullanılabilirKalemler = new List<SelectListItem>();
        }
        [DisplayName("Adı")]
        [AllowHtml]
        public string Adı { get; set; }
        [DisplayName("Birim Fiyat")]
        [AllowHtml]
        public decimal BirimFiyat { get; set; }
        [DisplayName("Gerçekleşen Birim Fiyat")]
        [AllowHtml]
        public decimal GerçekleşenBirimFiyat { get; set; }
        [DisplayName("Kişi/Adet")]
        [AllowHtml]
        public int Adet { get; set; }
        [DisplayName("Gün")]
        [AllowHtml]
        public int Gün { get; set; }
        [DisplayName("Gerçekleşen Kişi/Adet")]
        [AllowHtml]
        public int GerçekleşenAdet { get; set; }
        [DisplayName("Tutar")]
        [AllowHtml]
        public decimal Tutar { get; set; }
        [DisplayName("Gelir (Kazanç)")]
        [AllowHtml]
        public decimal GelirYüzde { get; set; }
        [DisplayName("Gerçekleşen Tutar")]
        [AllowHtml]
        public decimal GerçekleşenTutar { get; set; }
        [DisplayName("Döviz")]
        [AllowHtml]
        public int Döviz { get; set; }
        public bool Gelir { get; set; }
        [DisplayName("Fark")]
        [AllowHtml]
        public decimal Fark { get; set; }
        public int GelirKalemiId { get; set; }
        public IList<SelectListItem> KullanılabilirKalemler { get; set; }

    }

}