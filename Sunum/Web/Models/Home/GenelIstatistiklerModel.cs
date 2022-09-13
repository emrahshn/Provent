using Web.Framework.Mvc;

namespace Web.Models.Home
{
    public partial class GenelIstatistiklerModel : TemelTSModel
    {
        public int TeklifSayısı { get; set; }

        public int KongreSayısı { get; set; }

        public int OdemeFormuSayısı { get; set; }

        public int KullanıcıSayısı { get; set; }
    }
}