using System;
using System.ComponentModel;
using System.Web.Mvc;
using Web.Framework.Mvc;

namespace Web.Models.Görüşmeler
{
    public partial class GorusmeRaporlariModel : TemelTSEntityModel
    {
        [DisplayName("Firma Adı")]
        [AllowHtml]
        public string FirmaAdı { get; set; }
        [DisplayName("Görüşülen Adı")]
        [AllowHtml]
        public string GorusulenAdı { get; set; }
        [DisplayName("Görüşülen Soyadı")]
        [AllowHtml]
        public string GorusulenSoyadı { get; set; }
        public string Gorusulen { get; set; }
        [DisplayName("E-mail")]
        [AllowHtml]
        public string Email { get; set; }
        [DisplayName("Tel")]
        [AllowHtml]
        public string Tel { get; set; }
        [DisplayName("Konu")]
        [AllowHtml]
        public string Konu { get; set; }
        [DisplayName("Tarih")]
        [AllowHtml]
        public DateTime Tarih { get; set; }
        [DisplayName("Deadline")]
        [AllowHtml]
        public DateTime Deadline { get; set; }
        [DisplayName("Durumu")]
        [AllowHtml]
        public bool Durumu { get; set; }
        [DisplayName("Sorumlu")]
        [AllowHtml]
        public string Sorumlu { get; set; }
        [DisplayName("Beklemede")]
        [AllowHtml]
        public int Beklemede { get; set; }
        [DisplayName("Olumsuz")]
        [AllowHtml]
        public int Olumsuz { get; set; }
        [DisplayName("Takipte")]
        [AllowHtml]
        public int Takipte { get; set; }
        [DisplayName("Teklif")]
        [AllowHtml]
        public int Teklif { get; set; }
        [DisplayName("Grup")]
        [AllowHtml]
        public int Grup { get; set; }
        [DisplayName("Grup Id")]
        [AllowHtml]
        public int GrupId { get; set; }
        [DisplayName("Olusturulma Tarihi")]
        [AllowHtml]
        public DateTime OlusturulmaTarihi { get; set; }
    }

}