using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Web.Framework.Mvc;
using Web.Models.Konum;

namespace Web.Models.Teklif
{
    public partial class TeklifModel : TemelTSEntityModel
    {
        public TeklifModel()
        {
            Sehirler = new List<KonumModel.SehirModel>();
            Ulkeler = new List<KonumModel.UlkeModel>();
        }
        [DisplayName("Adı")]
        [AllowHtml]
        public string Adı { get; set; }
        [DisplayName("Hazırlayan")]
        [AllowHtml]
        public int HazırlayanId { get; set; }
        public string Hazırlayan { get; set; }
        [DisplayName("Sunan")]
        [AllowHtml]
        public int SunanId { get; set; }
        public int Operasyon { get; set; }
        public int Konfirme { get; set; }
        public int Biten { get; set; }
        public int Iptal { get; set; }
        [DisplayName("Açıklama")]
        [AllowHtml]
        public string Aciklama { get; set; }
        [DisplayName("Başlama Tarihi")]
        [AllowHtml]
        public DateTime BaslamaTarihi { get; set; }
        [DisplayName("Bitiş Tarihi")]
        [AllowHtml]
        public DateTime BitisTarihi { get; set; }
        [DisplayName("Ülke")]
        [AllowHtml]
        public int UlkeId { get; set; }
        [DisplayName("Şehir")]
        [AllowHtml]
        public int SehirId { get; set; }
        [DisplayName("Hizmet Bedeli")]
        [AllowHtml]
        public int HizmetBedeli { get; set; }
        [DisplayName("Firma")]
        [AllowHtml]
        public int FirmaId { get; set; }
        [DisplayName("Yetkili")]
        [AllowHtml]
        public int YetkiliId { get; set; }
        [DisplayName("Konum")]
        [AllowHtml]
        public string Konum { get; set; }
        [DisplayName("Kod")]
        [AllowHtml]
        public string Kod { get; set; }
        [DisplayName("Kur(Dolar)")]
        [AllowHtml]
        public decimal KurDolar { get; set; }
        [DisplayName("Kur(Euro)")]
        [AllowHtml]
        public decimal KurEuro { get; set; }
        [DisplayName("Mekan Adı")]
        [AllowHtml]
        public string MekanAdı { get; set; }
        [DisplayName("Toplantı Adı")]
        [AllowHtml]
        public string ToplantıAdı { get; set; }
        [DisplayName("Opsiyon Tarihi")]
        [AllowHtml]
        public DateTime OpsiyonTarihi { get; set; }
        public int OrijinalTeklifId { get; set; }
        public string Durumu { get; set; }
        public DateTime OlusturulmaTarihi { get; set; }
        public IList<KonumModel.SehirModel> Sehirler { get; set; }
        public IList<KonumModel.UlkeModel> Ulkeler { get; set; }
        
    }
}