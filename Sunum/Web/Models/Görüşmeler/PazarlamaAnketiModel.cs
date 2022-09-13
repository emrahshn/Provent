using System;
using System.ComponentModel;
using System.Web.Mvc;
using Web.Framework.Mvc;

namespace Web.Models.Görüşmeler
{
    public partial class PazarlamaAnketiModel : TemelTSEntityModel
    {
        [DisplayName("Değerlendiren")]
        [AllowHtml]
        public string Degerlendiren { get; set; }
        [DisplayName("Firma Adı")]
        [AllowHtml]
        public string FirmaAdı { get; set; }
        [DisplayName("Yetkili")]
        [AllowHtml]
        public string Yetkili { get; set; }
        [DisplayName("Organizasyon")]
        [AllowHtml]
        public string Organizasyon { get; set; }
        [DisplayName("Mekan")]
        [AllowHtml]
        public string Mekan { get; set; }
        [DisplayName("Kişi Sayısı")]
        [AllowHtml]
        public string Kisi { get; set; }
        [DisplayName("Başlama Tarihi")]
        [AllowHtml]
        public DateTime BaslangicTarihi { get; set; }
        [DisplayName("Bitiş Tarihi")]
        [AllowHtml]
        public DateTime BitisTarihi { get; set; }
        
    }

}