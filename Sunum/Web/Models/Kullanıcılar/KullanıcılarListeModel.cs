using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Web.Framework.Mvc;

namespace Web.Models.Kullanıcılar
{
    public partial class KullanıcılarListeModel : TemelTSModel
    {
        public KullanıcılarListeModel()
        {
            KullaniciRolleriAra = new List<int>();
            KullanılabilirKullanıcıRolleri = new List<SelectListItem>();
        }
        [UIHint("MultiSelect")]
        [DisplayName("Kullanıcı rolleri ara")]
        public IList<int> KullaniciRolleriAra { get; set; }
        public IList<SelectListItem> KullanılabilirKullanıcıRolleri { get; set; }
        [DisplayName("E-mail ara")]
        [AllowHtml]
        public string EmailAra { get; set; }
        [DisplayName("Kullanıcı adı ara")]
        [AllowHtml]
        public string KullanıcıAdıAra { get; set; }
        public bool KullanıcıAdlarıEtkin { get; set; }
        [DisplayName("Ad ara")]
        [AllowHtml]
        public string AdAra { get; set; }
        [DisplayName("Soyad ara")]
        [AllowHtml]
        public string SoyadAra { get; set; }
        [DisplayName("Doğum günü ara")]
        [AllowHtml]
        public string DoğumGünüAra { get; set; }
        [DisplayName("Doğum ayı ara")]
        [AllowHtml]
        public string DoğumAyıAra { get; set; }
        public bool DoğumGünüEtkin { get; set; }
        [DisplayName("Şirket ara")]
        [AllowHtml]
        public string SirketAra { get; set; }
        public bool SirketEtkin { get; set; }
        [DisplayName("Telefon ara")]
        [AllowHtml]
        public string TelAra { get; set; }
        public bool TelEtkin { get; set; }
        [DisplayName("Posta kodu ara")]
        [AllowHtml]
        public string PostaKoduAra { get; set; }
        public bool PostaKoduEtkin { get; set; }
        [DisplayName("IP adresi ara")]
        public string IpAdresiAra { get; set; }
    }
}