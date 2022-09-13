using Web.Framework.Mvc;

namespace Web.Models.Ayarlar
{
    public partial class ModModel : TemelTSModel
    {
        public string ModAdı { get; set; }
        public bool Etkin { get; set; }
    }
}