using System.Collections.Generic;
using System.Web.Mvc;
using Web.Framework.Mvc;

namespace Web.Models.EkTanımlamalar
{
    public partial class BankaListeModel : TemelTSModel
    {
        public BankaListeModel()
        {
            MevcutSiteler = new List<SelectListItem>();
        }
        public int SiteIdAra { get; set; }
        public IList<SelectListItem> MevcutSiteler { get; set; }
    }
}