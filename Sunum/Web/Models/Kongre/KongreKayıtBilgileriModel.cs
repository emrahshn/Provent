using Web.Framework.Mvc;

namespace Web.Models.Kongre
{
    public partial class KongreKayıtBilgileriModel : TemelTSModel
    {
        public int KongreId { get; set; }
        public KayıtBilgileriModel KayıtBilgileriModel { get; set; }
    }
}