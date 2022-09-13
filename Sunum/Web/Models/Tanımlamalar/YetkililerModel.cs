using Core.Domain.EkTanımlamalar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Web.Framework.Mvc;
using Web.Models.Konum;
using Web.Models.Genel;

namespace Web.Models.Tanımlamalar
{
    public partial class YetkililerModel : TemelTSEntityModel
    {
        public YetkililerModel()
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
    public class YetkililerPagerModel : TemelTSEntityModel
    {
        public YetkililerPagerModel()
        {
            Yetkililer = new List<YetkililerModel>();
        }
        public PagerModel PagerModel { get; set; }
        public IList<YetkililerModel> Yetkililer { get; set; }
        public partial class YetkililerRouteValues : IRouteValues
        {
            public int page { get; set; }
        }
    }
}