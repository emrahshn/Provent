using Core.Domain.KongreTanımlama;
using FluentValidation.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Web.Framework.Mvc;
using Web.Models.EkTanımlamalar;
using Web.Models.Konum;

namespace Web.Models.Tanımlamalar
{
    //[Validator(typeof())]
    public partial class FirmaModel : TemelTSEntityModel
    {
        public FirmaModel()
        {
            Sehirler = new List<KonumModel.SehirModel>();
            Ilceler = new List<KonumModel.IlceModel>();
            Yetkili = new List<SelectListItem>();
            YetkiliIdleri = new List<int>();
            Kategoriler = new List<SelectListItem>();
        }
        [DisplayName("Adı")]
        [AllowHtml]
        public string Adı { get; set; }
        [DisplayName("Kategorisi")]
        [AllowHtml]
        public int KategoriId { get; set; }
        [DisplayName("Telefon")]
        [UIHint("Tel")]
        [AllowHtml]
        public string Tel { get; set; }
        [DisplayName("Cep Telefonu")]
        [UIHint("Tel")]
        [AllowHtml]
        public string CepTel { get; set; }
        [DisplayName("Faks")]
        [UIHint("Tel")]
        [AllowHtml]
        public string Faks { get; set; }
        [DisplayName("Şehir")]
        [AllowHtml]
        public int SehirId { get; set; }
        [DisplayName("İlçe")]
        [AllowHtml]
        public int IlceId { get; set; }
        [DisplayName("Adres")]
        [AllowHtml]
        public string Adres { get; set; }
        [DisplayName("Web")]
        [AllowHtml]
        public string Web { get; set; }
        [DisplayName("Email")]
        [AllowHtml]
        public string Email { get; set; }
        [DisplayName("Vergi Dairesi")]
        [AllowHtml]
        public string VergiDairesi { get; set; }
        [DisplayName("Vergi No")]
        [AllowHtml]
        public string VergiNo { get; set; }
        public DateTime? OlusturulmaTarihi { get; set; }
        public IList<KonumModel.SehirModel> Sehirler { get; set; }
        public IList<KonumModel.IlceModel> Ilceler { get; set; }
        public IList<SelectListItem> Yetkili { get; set; }
        [UIHint("MultiSelect")]
        [DisplayName("Yetkililer")]
        public List<int> YetkiliIdleri { get; set; }
        public IList<SelectListItem> Kategoriler { get; set; }
        
    }

}