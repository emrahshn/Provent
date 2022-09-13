using System;
using System.ComponentModel;
using System.Web.Mvc;
using Web.Framework.Mvc;

namespace Web.Models.Mesajlar
{
    public partial class MesajModel : TemelTSEntityModel
    {
        [DisplayName("KullanıcıId")]
        [AllowHtml]
        public int KullanıcıId { get; set; }
        [DisplayName("KullanıcıAdı")]
        [AllowHtml]
        public string KullanıcıAdı { get; set; }
        [DisplayName("GonderenId")]
        [AllowHtml]
        public int GonderenId { get; set; }
        [DisplayName("GonderenAdı")]
        [AllowHtml]
        public string GonderenAdı { get; set; }
        [DisplayName("Baslik")]
        [AllowHtml]
        public string Baslik { get; set; }
        [DisplayName("Mesaj")]
        [AllowHtml]
        public string Msj { get; set; }
        [DisplayName("Bildirim")]
        [AllowHtml]
        public bool Bildirim { get; set; }
        [DisplayName("Okundu")]
        [AllowHtml]
        public bool Okundu { get; set; }
        [DisplayName("OlusmaTarihi")]
        [AllowHtml]
        public DateTime OlusmaTarihi { get; set; }
    }

}