using Web.Framework.Mvc;

namespace Web.Models.Kongre
{
    public partial class KongreGelirGiderHedefModel : TemelTSModel
    {
        public int KongreId { get; set; }
        public GelirGiderHedefiModel GelirGiderHedefiModel { get; set; }
    }
}