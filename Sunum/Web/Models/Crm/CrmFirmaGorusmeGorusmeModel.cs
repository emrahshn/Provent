using Web.Framework.Mvc;

namespace Web.Models.Crm
{
    public partial class CrmFirmaGorusmeGorusmeModel : TemelTSModel
    {
        public int FirmaId { get; set; }
        public CrmFirmaGorusmeModel Gorusmeler { get; set; }
    }
}