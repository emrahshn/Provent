using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Web.Framework.Mvc;
using Web.Models.Konum;

namespace Web.Models.Kongre
{
    public partial class RefakatciModel : TemelTSEntityModel
    {
        public RefakatciModel()
        {
            Sehirler = new List<KonumModel.SehirModel>();
            Ilceler = new List<KonumModel.IlceModel>();
            Kongre = new List<KongreModel>();
            Kurs = new List<KursModel>();
            Transfer = new List<TransferModel>();
            Konaklama = new List<KonaklamaModel>();
            Kayıt = new List<KayitModel>();
            Musteriler = new List<SelectListItem>();
            Katılımcılar = new List<SelectListItem>();
            KatilimciIdleri = new List<int>();
            KayıtSponsorIdleri = new List<int>();
        }
        [DisplayName("KongreId")]
        [AllowHtml]
        public int KongreId { get; set; }
        [DisplayName("Katılımcı")]
        [AllowHtml]
        [UIHint("MultiSelect")]
        public IList<int> KatilimciIdleri { get; set; }
        public int KatilimciId { get; set; }
        [DisplayName("Adı")]
        [AllowHtml]
        public string Adı { get; set; }
        [DisplayName("Soyadı")]
        [AllowHtml]
        public string Soyadı { get; set; }
        [DisplayName("TCKN")]
        [AllowHtml]
        public string TCKN { get; set; }
        [DisplayName("Ulke")]
        [AllowHtml]
        public int UlkeId { get; set; }
        [DisplayName("Sehir")]
        [AllowHtml]
        public int SehirId { get; set; }
        [DisplayName("Ilce")]
        [AllowHtml]
        public int IlceId { get; set; }
        [DisplayName("Tel")]
        [AllowHtml]
        public string Tel { get; set; }
        [DisplayName("Email")]
        [AllowHtml]
        public string Email { get; set; }

        [DisplayName("KayıtId")]
        [AllowHtml]
        public int KayıtId { get; set; }
        [DisplayName("Kayıt Sponsoru")]
        [AllowHtml]
        public string KayıtSponsorAdı { get; set; }
        [UIHint("SingleMultiSelect")]
        public int KayıtSponsorId { get; set; }
        [UIHint("MultiSelect")]
        public IList<int> KayıtSponsorIdleri { get; set; }
        [DisplayName("KursId")]
        [AllowHtml]
        public int KursId { get; set; }
        [DisplayName("KursSponsorId")]
        [AllowHtml]
        public int KursSponsorId { get; set; }
        [DisplayName("KonaklamaId")]
        [AllowHtml]
        public int KonaklamaId { get; set; }
        [DisplayName("KonaklamaSponsorId")]
        [AllowHtml]
        public int KonaklamaSponsorId { get; set; }
        [DisplayName("OtelGiris")]
        [AllowHtml]
        public DateTime OtelGiris { get; set; }
        [DisplayName("OtelCikis")]
        [AllowHtml]
        public DateTime OtelCikis { get; set; }
        [DisplayName("TransferId")]
        [AllowHtml]
        public int TransferId { get; set; }
        [DisplayName("TransferSponsorId")]
        [AllowHtml]
        public int TransferSponsorId { get; set; }
        [DisplayName("UlasimUcreti")]
        [AllowHtml]
        public string UlasimUcreti { get; set; }
        [DisplayName("UlasimUcretiDoviz")]
        [AllowHtml]
        public int UlasimUcretiDoviz { get; set; }
        [DisplayName("UlasimKalkisParkur")]
        [AllowHtml]
        public string UlasimKalkisParkur { get; set; }
        [DisplayName("UlasimVarisParkur")]
        [AllowHtml]
        public string UlasimVarisParkur { get; set; }
        [DisplayName("UlasimKalkisUcus")]
        [AllowHtml]
        public string UlasimKalkisUcus { get; set; }
        [DisplayName("UlasimVarisUcus")]
        [AllowHtml]
        public string UlasimVarisUcus { get; set; }
        [DisplayName("UlasimVarisTarihi")]
        [AllowHtml]
        public DateTime UlasimVarisTarihi { get; set; }
        [DisplayName("UlasimKalkisTarihi")]
        [AllowHtml]
        public DateTime UlasimKalkisTarihi { get; set; }
        [DisplayName("TransferTarihi")]
        [AllowHtml]
        public DateTime TransferTarihi { get; set; }
        public List<KongreModel> Kongre { get; set; }
        public List<KayitModel> Kayıt { get; set; }
        public List<KonaklamaModel> Konaklama { get; set; }
        public List<KursModel> Kurs { get; set; }
        public List<TransferModel> Transfer { get; set; }
        public IList<KonumModel.SehirModel> Sehirler { get; set; }
        public IList<KonumModel.IlceModel> Ilceler { get; set; }
        public IList<SelectListItem> Musteriler { get; set; }
        public IList<SelectListItem> Katılımcılar { get; set; }
    }

}