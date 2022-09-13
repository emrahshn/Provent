using Core.Domain.Kongre;
using Core.Domain.Tanımlamalar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Web.Framework.Mvc;
using Web.Models.Konum;
using Web.Models.Notlar;
using Web.Models.Tanımlamalar;

namespace Web.Models.Kongre
{
    public partial class KatilimciModel : TemelTSEntityModel
    {
        public KatilimciModel()
        {
            Sehirler = new List<KonumModel.SehirModel>();
            Ilceler = new List<KonumModel.IlceModel>();
            Kongre = new List<KongreModel>();
            this.KongreAra = new List<int>();
            this.KullanılabilirKongreler = new List<SelectListItem>();
            Kurs = new List<KursModel>();
            Transfer = new List<TransferModel>();
            Konaklama = new List<KonaklamaModel>();
            Kayıt = new List<KayitModel>();
            MusterilerKayıt = new List<SelectListItem>();
            MusterilerKonaklama = new List<SelectListItem>();
            MusterilerTransfer = new List<SelectListItem>();
            MusterilerKurs = new List<SelectListItem>();
            KayıtSponsorIdleri = new List<int>();
            KursSponsorIdleri = new List<int>();
            KonaklamaSponsorIdleri = new List<int>();
            TransferSponsorIdleri = new List<int>();
            Notlar = new List<NotModel>();
            this.Page = 1;
            this.PageSize = 10;
        }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int KongreIdAra { get; set; }
        [UIHint("MultiSelect")]
        [DisplayName("Kongre ara")]
        public IList<int> KongreAra { get; set; }
        public IList<SelectListItem> KullanılabilirKongreler { get; set; }
        [DisplayName("Adı")]
        public string AdAra { get; set; }
        [DisplayName("Soyadı")]
        public string SoyadAra { get; set; }
        [DisplayName("Kongre")]
        [AllowHtml]
        public int KongreId { get; set; }
        [DisplayName("Adı")]
        [AllowHtml]
        public string Adı { get; set; }
        [DisplayName("Soyadı")]
        [AllowHtml]
        public string Soyadı { get; set; }
        [DisplayName("TCKN")]
        [AllowHtml]
        public string TCKN { get; set; }
        [DisplayName("Ülke")]
        [AllowHtml]
        public int UlkeId { get; set; }
        [DisplayName("Şehir")]
        [AllowHtml]
        public int SehirId { get; set; }
        [DisplayName("İlçe")]
        [AllowHtml]
        public int IlceId { get; set; }
        [DisplayName("Tel")]
        [AllowHtml]
        public string Tel { get; set; }
        [DisplayName("Email")]
        [AllowHtml]
        public string Email { get; set; }
        [DisplayName("Refakatçi")]
        [AllowHtml]
        public int Refakatci { get; set; }
        public bool KayıtOldu { get; set; }
        [DisplayName("Kayıt Tipi")]
        [AllowHtml]
        public int KayıtId { get; set; }
        [DisplayName("Kayıt Sponsoru")]
        [AllowHtml]
        public string KayıtSponsorAdı { get; set; }
        [DisplayName("Kayıt Sponsoru")]
        [UIHint("MultiSelect")]
        public int KayıtSponsorId { get; set; }
        [DisplayName("Kayıt Sponsoru")]
        [UIHint("SingleMultiSelect")]
        public IList<int> KayıtSponsorIdleri { get; set; }
        [DisplayName("Kurs Tipi")]
        [AllowHtml]
        public int KursId { get; set; }
        [DisplayName("Kurs Sponsoru")]
        [AllowHtml]
        public int KursSponsorId { get; set; }
        [DisplayName("Kurs Sponsoru")]
        [UIHint("MultiSelect")]
        public IList<int> KursSponsorIdleri { get; set; }
        [DisplayName("Konaklama Tipi")]
        [AllowHtml]
        public int KonaklamaId { get; set; }
        [DisplayName("Konaklama Sponsoru")]
        [AllowHtml]
        public int KonaklamaSponsorId { get; set; }
        [DisplayName("Konaklama Sponsoru")]
        [UIHint("SingleMultiSelect")]
        public IList<int> KonaklamaSponsorIdleri { get; set; }
        [DisplayName("Otel Girişi")]
        [AllowHtml]
        public DateTime OtelGiris { get; set; }
        [DisplayName("Otel Çıkışı")]
        [AllowHtml]
        public DateTime OtelCikis { get; set; }
        [DisplayName("Transfer Tipi")]
        [AllowHtml]
        public int TransferId { get; set; }
        [DisplayName("Transfer Sponsoru")]
        [AllowHtml]
        public int TransferSponsorId { get; set; }
        [DisplayName("Transfer Sponsoru")]
        [UIHint("MultiSelect")]
        public IList<int> TransferSponsorIdleri { get; set; }
        [DisplayName("Ulaşım Ücreti")]
        [AllowHtml]
        public string UlasimUcreti { get; set; }
        [DisplayName("Ulaşım Ücreti Döviz")]
        [AllowHtml]
        public int UlasimUcretiDoviz { get; set; }
        [DisplayName("Kalkış Parkuru")]
        [AllowHtml]
        public string UlasimKalkisParkur { get; set; }
        [DisplayName("Varış Parkuru")]
        [AllowHtml]
        public string UlasimVarisParkur { get; set; }
        [DisplayName("Kalkış Uçuşu")]
        [AllowHtml]
        public string UlasimKalkisUcus { get; set; }
        [DisplayName("Varış Uçuşu")]
        [AllowHtml]
        public string UlasimVarisUcus { get; set; }
        [DisplayName("Varış Tarihi")]
        [AllowHtml]
        public DateTime UlasimVarisTarihi { get; set; }
        [DisplayName("Kalkış Tarihi")]
        [AllowHtml]
        public DateTime UlasimKalkisTarihi { get; set; }
        [DisplayName("Transfer Tarihi")]
        [AllowHtml]
        public DateTime TransferTarihi { get; set; }
        public List<KongreModel> Kongre { get; set; }
        public List<KayitModel> Kayıt { get; set; }
        public List<KonaklamaModel> Konaklama { get; set; }
        public List<KursModel> Kurs { get; set; }
        public List<TransferModel> Transfer { get; set; }
        public IList<KonumModel.SehirModel> Sehirler { get; set; }
        public IList<KonumModel.IlceModel> Ilceler { get; set; }
        public IList<SelectListItem> MusterilerKayıt { get; set; }
        public IList<SelectListItem> MusterilerKonaklama { get; set; }
        public IList<SelectListItem> MusterilerTransfer { get; set; }
        public IList<SelectListItem> MusterilerKurs { get; set; }
        public List<NotModel> Notlar { get; set; }
        public bool Iptal { get; set; }
        

    }
}