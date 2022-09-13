
using Web.Framework.Mvc;

namespace Web.Models.Kongre
{
    public partial class KongreBankaBilgileriModel : TemelTSModel
    {
        public int KongreId { get; set; }
        public BankaHesapBilgileriModel BankaHesapBilgileri { get; set; }
    }
}