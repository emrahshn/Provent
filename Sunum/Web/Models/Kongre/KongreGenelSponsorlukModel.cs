using Web.Framework.Mvc;

namespace Web.Models.Kongre
{
    public partial class KongreKursBilgileriModel : TemelTSModel
    {
        public int KongreId { get; set; }
        public KursBilgileriModel KursBilgileriModel { get; set; }
    }
}