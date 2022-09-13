using System.ComponentModel;
using Web.Framework.Mvc;

namespace Web.Models.Ayarlar
{
    public partial class FinansAyarlarModel : TemelTSModel
    {

        [DisplayName("Finans Bildirimi Rolleri")]
        public bool Etkin { get; set; }
    }
}