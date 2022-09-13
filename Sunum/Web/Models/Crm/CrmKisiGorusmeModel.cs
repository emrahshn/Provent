using Web.Framework.Mvc;

namespace Web.Models.Crm
{
    public partial class CrmKisiGorusmeModel : TemelTSModel
    {
        public int KisiId { get; set; }
        public CrmGorusmeModel Gorusmeler { get; set; }
    }
}