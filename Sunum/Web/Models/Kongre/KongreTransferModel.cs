using Web.Framework.Mvc;

namespace Web.Models.Kongre
{
    public partial class KongreTransferModel : TemelTSModel
    {
        public int KongreId { get; set; }
        public TransferModel TransferModel { get; set; }
    }
}