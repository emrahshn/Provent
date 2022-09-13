using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Web.Framework.Mvc;
using Web.Models.Konum;

namespace Web.Models.Tanımlamalar
{
    public partial class HekimlerModel : TemelTSEntityModel
    {
        public HekimlerModel()
        {
            Branşlar = new List<SelectListItem>();
            Ulkeler = new List<KonumModel.UlkeModel>();
            Sehirler = new List<KonumModel.SehirModel>();
            Ilceler = new List<KonumModel.IlceModel>();
        }
        [DisplayName("Branş")]
        [AllowHtml]
        public int BranşId { get; set; }
        [DisplayName("TC Kimlik")]
        [AllowHtml]
        public string TCKN { get; set; }
        [DisplayName("Adı")]
        [AllowHtml]
        public string Adı { get; set; }
        [DisplayName("Soyadı")]
        [AllowHtml]
        public string Soyadı { get; set; }
        [DisplayName("Cep Tel 1")]
        [UIHint("Tel")]
        [AllowHtml]
        public string CepTel1 { get; set; }
        [DisplayName("Cep Tel 2")]
        [UIHint("Tel")]
        [AllowHtml]
        public string CepTel2 { get; set; }
        [DisplayName("İş Tel")]
        [AllowHtml]
        [UIHint("Tel")]
        public string İşTel { get; set; }
        [DisplayName("Fax")]
        [UIHint("Tel")]
        [AllowHtml]
        public string Fax { get; set; }
        [DisplayName("E-mail 1")]
        [AllowHtml]
        public string Email1 { get; set; }
        [DisplayName("E-mail 2")]
        [AllowHtml]
        public string Email2 { get; set; }
        [DisplayName("Kurum Adresi")]
        [AllowHtml]
        public string KurumAdresi { get; set; }
        [DisplayName("Ev Adresi")]
        [AllowHtml]
        public string EvAdresi { get; set; }
        [DisplayName("Ülke")]
        [AllowHtml]
        public int UlkeId { get; set; }
        [DisplayName("Şehir")]
        [AllowHtml]
        public int SehirId { get; set; }
        [DisplayName("İlçe")]
        [AllowHtml]
        public int IlceId { get; set; }
        [DisplayName("Posta Kodu")]
        [AllowHtml]
        public int PostaKodu { get; set; }
        [UIHint("Date")]
        [DisplayName("Doğum Tarihi")]
        [AllowHtml]
        public DateTime DoğumTarihi { get; set; }
        [DisplayName("Miles Smiles No")]
        [AllowHtml]
        public string MilesSmilesNo { get; set; }
        [DisplayName("Pasaport No")]
        [AllowHtml]
        public string PasaportNo { get; set; }
        [UIHint("Date")]
        [DisplayName("Pasaport Geçerlilik Tarihi")]
        [AllowHtml]
        public DateTime PasaportGeçerlilikTarihi { get; set; }
        [DisplayName("İlgi Alanları")]
        [AllowHtml]
        public string İlgiAlanları { get; set; }
        [UIHint("Picture")]
        [DisplayName("Resim")]
        [AllowHtml]
        public int Resim { get; set; }
        public IList<SelectListItem> Branşlar { get; set; }
        public IList<KonumModel.UlkeModel> Ulkeler { get; set; }
        public IList<KonumModel.SehirModel> Sehirler { get; set; }
        public IList<KonumModel.IlceModel> Ilceler { get; set; }
    }

}