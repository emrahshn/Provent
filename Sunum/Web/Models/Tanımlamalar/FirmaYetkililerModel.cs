using Web.Framework.Mvc;
using Web.Models.KongreTanımlamaları;

namespace Web.Models.Tanımlamalar
{
    public partial class FirmaYetkililerModel : TemelTSModel
    {
        public int FirmaId { get; set; }
        public YetkililerModel YetkililerModel { get; set; }
    }
}