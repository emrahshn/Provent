using Web.Framework.Mvc;

namespace Web.Models.Genel
{
    public partial class AdminHeaderModel : TemelTSModel
    {
        public string KimliğineBürünülenKullanıcı { get; set; }
        public bool KullanıcıKimliğineBürünüldü { get; set; }
        public bool AdminLinkiGörüntüle { get; set; }
        public string SayfayıDüzenle { get; set; }
    }
}