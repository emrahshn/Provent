using Web.Framework.Mvc;

namespace Web.Models.Crm
{
    public partial class CrmKurumKongreModel : TemelTSModel
    {
        public int KurumId { get; set; }
        public CrmKongreModel Kongreler { get; set; }
    }
}