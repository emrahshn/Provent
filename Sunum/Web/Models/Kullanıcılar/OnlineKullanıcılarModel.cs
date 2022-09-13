using System;
using System.ComponentModel;
using Web.Framework.Mvc;

namespace Web.Models.Kullanıcılar
{
    public partial class OnlineKullanıcılarModel : TemelTSEntityModel
    {
        [DisplayName("Kullanıcı Bilgisi")]
        public string KullanıcıBilgisi { get; set; }

        [DisplayName("IP Adresi")]
        public string SonIPAdresi { get; set; }

        [DisplayName("Konum")]
        public string Konum { get; set; }

        [DisplayName("Son İşlem Tarihi")]
        public DateTime SonİşlemTarihi { get; set; }

        [DisplayName("Son Ziyaret Edilen Sayfa")]
        public string SonZiyaretEdilenSayfa { get; set; }
    }
}