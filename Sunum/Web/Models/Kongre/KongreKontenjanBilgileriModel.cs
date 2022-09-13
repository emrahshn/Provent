using Web.Framework.Mvc;

namespace Web.Models.Kongre
{
    public partial class KongreKontenjanBilgileriModel : TemelTSModel
    {
        public int KongreId { get; set; }
        public KontenjanBilgileriModel KontenjanBilgileriModel { get; set; }
    }
}