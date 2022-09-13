using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;
using Web.Framework.Mvc;

namespace Web.Models.Kongre
{
    public partial class SponsorlukSatışıModel : TemelTSEntityModel
    {
        public SponsorlukSatışıModel()
        {
            KullanılabilirSponsorlar = new List<SelectListItem>();
        }
        [DisplayName("Adı")]
        [AllowHtml]
        public string Adı { get; set; }
        [DisplayName("BirimFiyat")]
        [AllowHtml]
        public decimal BirimFiyat { get; set; }
        [DisplayName("Adet")]
        [AllowHtml]
        public int Adet { get; set; }
        [DisplayName("Gün")]
        [AllowHtml]
        public int Gün { get; set; }
        [DisplayName("Tutar")]
        [AllowHtml]
        public decimal Tutar { get; set; }
        [DisplayName("Döviz")]
        [AllowHtml]
        public int Döviz { get; set; }
        [DisplayName("SponsorlukKalemiId")]
        [AllowHtml]
        public int SponsorlukKalemiId { get; set; }
        public IList<SelectListItem> KullanılabilirSponsorlar { get; set; }
        public int KongreId { get; set; }
        public int SponsorId { get; set; }
        public bool Locked { get; set; }
        public int Tipi { get; set; }
        public int OgeId { get; set; }
    }
}