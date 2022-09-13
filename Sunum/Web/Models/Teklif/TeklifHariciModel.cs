using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Web.Framework.Mvc;
using Web.Models.EkTanımlamalar;
using Web.Models.Konum;
using Web.Models.Notlar;

namespace Web.Models.Teklif
{
    public partial class TeklifHariciModel : TemelTSEntityModel
    {
        public TeklifHariciModel()
        {
            Sehirler = new List<KonumModel.SehirModel>();
            Ulkeler = new List<KonumModel.UlkeModel>();
            Notlar = new List<NotModel>();
            KullanılabilirHariciSektorler = new List<HariciSektorModel>();
        }
        [DisplayName("Adı")]
        [AllowHtml]
        public string Adı { get; set; }
        [DisplayName("PO")]
        [AllowHtml]
        public string PO { get; set; }
        [DisplayName("Belge Türü")]
        [AllowHtml]
        public int BelgeTuru { get; set; }
        public string BelgeTuruAdı { get; set; }
        [DisplayName("Hazırlayan")]
        [AllowHtml]
        public int HazırlayanId { get; set; }
        
        [DisplayName("Kongre Tarihi")]
        [AllowHtml]
        public DateTime Tarih { get; set; }
        [DisplayName("Fatura Edilecek Tarihi")]
        [AllowHtml]
        public DateTime TeslimTarihi { get; set; }
        public int UlkeAdı { get; set; }
        [DisplayName("Ülke")]
        [AllowHtml]
        public int UlkeId { get; set; }
        public int SehirAdı { get; set; }
        [DisplayName("Şehir")]
        [AllowHtml]
        public int SehirId { get; set; }
        [DisplayName("Acenta")]
        [AllowHtml]
        public string Acenta { get; set; }
        [DisplayName("Talep No")]
        [AllowHtml]
        public string TalepNo { get; set; }
        [DisplayName("Hizmet Bedeli")]
        [AllowHtml]
        public decimal HizmetBedeli { get; set; }
        [DisplayName("Parabirimi")]
        [AllowHtml]
        public int Parabirimi { get; set; }
        [DisplayName("Fatura")]
        [AllowHtml]
        public int Fatura { get; set; }
        public string FaturaNo { get; set; }
        [UIHint("Picture")]
        [DisplayName("Fatura Resmi")]
        [AllowHtml]
        public int FaturaResim { get; set; }
        public string FaturaResimUrl { get; set; }
        [DisplayName("Onay")]
        [AllowHtml]
        public bool Onay { get; set; }
        public IList<KonumModel.SehirModel> Sehirler { get; set; }
        public IList<KonumModel.UlkeModel> Ulkeler { get; set; }
        public List<NotModel> Notlar { get; set; }
        public List<HariciSektorModel> KullanılabilirHariciSektorler { get; set; }

        [DisplayName("Adı")]
        [AllowHtml]
        public string AdAra { get; set; }
        [DisplayName("Acenta")]
        [AllowHtml]
        public string AcentaAra { get; set; }
        [DisplayName("PO")]
        [AllowHtml]
        public string POAra { get; set; }
        [DisplayName("Talep No")]
        [AllowHtml]
        public string TalepNoAra { get; set; }
        public int BelgeAra { get; set; }
        [DisplayName("Kongre Tarihi")]
        [AllowHtml]
        public DateTime TarihRapor1 { get; set; }
        public partial class KongreKapama : TemelTSModel
        {
            public string KongreAdı { get; set; }
        }
    }
}