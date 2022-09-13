using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;
using Web.Framework.Mvc;

namespace Web.Models.Kongre
{
    public partial class BankaHesapBilgileriModel: TemelTSEntityModel
    {
        public BankaHesapBilgileriModel()
        {
            KullanılabilirBankalar = new List<SelectListItem>();
        }
        [DisplayName("Hesap Adı")]
        [AllowHtml]
        public string HesapAdı { get; set; }
        [DisplayName("Vergi Dairesi")]
        [AllowHtml]
        public string VergiDairesi { get; set; }
        [DisplayName("Vergi No")]
        [AllowHtml]
        public string VergiNo { get; set; }
        [DisplayName("Türk Lirası Hesabı")]
        [AllowHtml]
        public string TlHesabı { get; set; }
        [DisplayName("Amerikan Doları Hesabı")]
        [AllowHtml]
        public string DolarHesabı { get; set; }
        [DisplayName("Euro Hesabı")]
        [AllowHtml]
        public string EuroHesabı { get; set; }
        [DisplayName("Banka Adı")]
        [AllowHtml]
        public int BankaId { get; set; }
        [DisplayName("Swift")]
        [AllowHtml]
        public string Swift { get; set; }
        public IList<SelectListItem> KullanılabilirBankalar { get; set; }
    }
}