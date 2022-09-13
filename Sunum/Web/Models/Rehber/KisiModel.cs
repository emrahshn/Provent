using Core.Domain.EkTanımlamalar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Web.Framework.Mvc;
using Web.Models.Konum;
using Web.Models.Genel;
using Core.Domain.Tanımlamalar;

namespace Web.Models.Rehber
{
    public partial class KisiModel : TemelTSEntityModel
    {
        public KisiModel()
        {
            Görevler = new List<Unvanlar>();
            Sehirler = new List<KonumModel.SehirModel>();
            Ilceler = new List<KonumModel.IlceModel>();
        }
        [DisplayName("Adı")]
        [AllowHtml]
        public string Adı { get; set; }
        [DisplayName("Soyadı")]
        [AllowHtml]
        public string Soyadı { get; set; }
        [UIHint("Date")]
        [DisplayName("Doğum Tarihi")]
        [AllowHtml]
        public DateTime DoğumTarihi { get; set; }
        [DisplayName("Cep Tel 1")]
        [UIHint("Tel")]
        [AllowHtml]
        public string CepTel1 { get; set; }
        [DisplayName("Cep Tel 2")]
        [UIHint("Tel")]
        [AllowHtml]
        public string CepTel2 { get; set; }
        [DisplayName("E-mail 1")]
        [AllowHtml]
        public string Email1 { get; set; }
        [DisplayName("E-mail 2")]
        [AllowHtml]
        public string Email2 { get; set; }
        [DisplayName("Görev/Ünvan")]
        [AllowHtml]
        public int UnvanId { get; set; }
        [DisplayName("Adres")]
        [AllowHtml]
        public string Adres { get; set; }
        [DisplayName("Şehir")]
        [AllowHtml]
        public int YSehirId { get; set; }
        [DisplayName("İlçe")]
        [AllowHtml]
        public int YIlceId { get; set; }
        [DisplayName("Posta Kodu")]
        [AllowHtml]
        public int PostaKodu { get; set; }
        public int KategoriId { get; set; }
        [DisplayName("Kategoriler")]
        [AllowHtml]
        public IList<Unvanlar> Görevler { get; set; }
        public IList<KonumModel.SehirModel> Sehirler { get; set; }
        public IList<KonumModel.IlceModel> Ilceler { get; set; }
    }
    public class KisilerModel : TemelTSEntityModel
    {
        public KisilerModel()
        {
            Kisiler = new List<KisiModel>();
            FirmaKategorileri = new List<FirmaKategorisi>();
            HekimBranslari = new List<HekimBranşları>();
        }
        public PagerModel PagerModel { get; set; }
        public IList<KisiModel> Kisiler { get; set; }
        public partial class KisilerRouteValues : IRouteValues
        {
            public int page { get; set; }
        }
        [DisplayName("Ad Ara")]
        public string AdAra { get; set; }
        [DisplayName("Soyadı Ara")]
        public string SoyadAra { get; set; }
        [DisplayName("TC Kimlik Numarası Ara")]
        public string TCKNAra { get; set; }
        [DisplayName("E-Mail Ara")]
        public string EmailAra { get; set; }
        [DisplayName("Firma Ara")]
        public int FirmaAra { get; set; }
        [DisplayName("Branş Ara")]
        public int BransAra { get; set; }
        public IList<FirmaKategorisi> FirmaKategorileri { get; set; }
        public IList<HekimBranşları> HekimBranslari { get; set; }
    }
}