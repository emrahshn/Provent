using Web.Framework.Mvc;
using Web.Models.Notlar;

namespace Web.Models.Teklif
{
    public partial class TeklifHariciNotModel : TemelTSModel
    {
        public int TeklifHariciId { get; set; }
        public NotModel NotModel { get; set; }
    }
}