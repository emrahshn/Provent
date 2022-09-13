using Web.Framework.Mvc;

namespace Web.Models.Genel
{
    public partial class HeaderElemanlarıModel : TemelTSModel
    {
        public bool GirişYapıldı { get; set; }
        public string KullanıcıAdı { get; set; }
        public bool ÖzelMesajlarİzinli { get; set; }
        public string OkunmamışÖzelMesajlar { get; set; }
        public string UyarıMesajları { get; set; }
    }
}