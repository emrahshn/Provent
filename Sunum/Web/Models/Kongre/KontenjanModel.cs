
using Web.Framework.Mvc;

namespace Web.Models.Kongre
{
    public partial class KontenjanModel : TemelTSEntityModel
    {
        public int KongreId { get; set; }
        public int OtelId { get; set; }
        public string OtelKonaklamaTipi { get; set; }
        public int OdaKisiSayısı { get; set; }
        public int OtelKontenjanı { get; set; }
    }
}