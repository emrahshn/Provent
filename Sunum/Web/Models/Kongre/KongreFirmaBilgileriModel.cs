using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Web.Framework.Mvc;
using Web.Models.Tanımlamalar;

namespace Web.Models.Kongre
{
    public partial class KongreFirmaBilgileriModel : TemelTSModel
    {
        public KongreFirmaBilgileriModel()
        {
            this.FirmaBilgileriModel = new List<FirmaModel>();
            FirmaMusterileri = new List<SelectListItem>();
            MusteriIdleri = new List<int>();
        }
        public int KongreId { get; set; }
        [UIHint("MultiSelect")]
        public List<int> MusteriIdleri { get; set; }
        public IList<SelectListItem> FirmaMusterileri { get; set; }
        public List<FirmaModel> FirmaBilgileriModel { get; set; }

    }
}