using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Web.Framework.Mvc;
using Web.Models.KongreTanımlamaları;
using Web.Models.Tanımlamalar;

namespace Web.Models.Kongre
{
    public partial class KongreSponsorlukBilgileriModel : TemelTSModel
    {
        public KongreSponsorlukBilgileriModel()
        {
            this.SponsorlukBilgileriModel = new List<FirmaModel>();
            this.GörüsmeRaporlarıModel = new List<KongreGörüşmeRaporlarıModel>();
            SponsorlukMusterileri = new List<SelectListItem>();
            MusteriYetkilileri = new List<SelectListItem>();
            //MusteriIdleri = new List<int>();
        }
        public int KongreId { get; set; }
        [DisplayName("Sponsor Firma")]
        public int MusteriId { get; set; }
        [DisplayName("Firma Yetkilisi")]
        public int YetkiliId { get; set; }
        [UIHint("RichEditor")]
        public string Rapor { get; set; }
        [DisplayName("Görüşme Tarihi")]
        public DateTime GörüsmeTarihi { get; set; }
        public IList<SelectListItem> SponsorlukMusterileri { get; set; }
        public IList<SelectListItem> MusteriYetkilileri { get; set; }
        public List<FirmaModel> SponsorlukBilgileriModel { get; set; }
        public List<KongreGörüşmeRaporlarıModel> GörüsmeRaporlarıModel { get; set; }
        //public decimal Toplam { get; set; }
    }
}