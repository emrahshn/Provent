using Web.Framework.Mvc;

namespace Web.Models.Kongre
{
    public partial class KongreGenelSponsorlukModel : TemelTSModel
    {
        public int KongreId { get; set; }
        public GenelSponsorlukModel GenelSponsorlukModel { get; set; }
    }
}