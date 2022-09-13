using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Web.Framework.Mvc;
using Web.Models.Konum;

namespace Web.Models.Crm
{
    public partial class CrmKisiModel : TemelTSEntityModel
    {
        public CrmKisiModel()
        {
            Sehirler = new List<KonumModel.SehirModel>();
            Ilceler = new List<KonumModel.IlceModel>();
            Unvanlar = new List<CrmUnvanModel>();
        }
        [DisplayName("Adı")]
        [AllowHtml]
        public string Adı { get; set; }
        [DisplayName("Soyadı")]
        [AllowHtml]
        public string Soyadı { get; set; }
        [DisplayName("Ünvan")]
        [AllowHtml]
        public int Unvan { get; set; }
        [DisplayName("Cep Telefonu")]
        [AllowHtml]
        public string CepTel { get; set; }
        [DisplayName("Telefon")]
        [AllowHtml]
        public string Tel { get; set; }
        [DisplayName("Faks")]
        [AllowHtml]
        public string Fax { get; set; }
        [DisplayName("E-Mail")]
        [AllowHtml]
        public string Email { get; set; }
        [DisplayName("Adres")]
        [AllowHtml]
        public string Adres { get; set; }
        [DisplayName("Şehir")]
        [AllowHtml]
        public int SehirId { get; set; }
        [DisplayName("İlçe")]
        [AllowHtml]
        public int IlceId { get; set; }
        [DisplayName("Cinsiyet")]
        [AllowHtml]
        public int Cinsiyet { get; set; }
        [DisplayName("Kurum")]
        [AllowHtml]
        public int KurumId { get; set; }
        [DisplayName("Bölüm")]
        [AllowHtml]
        public string Bölüm { get; set; }
        [UIHint("Date")]
        [DisplayName("Doğum Tarihi")]
        [AllowHtml]
        public DateTime DoğumTarihi { get; set; }
        public string SehirAdı { get; set; }
        public string IlceAdı { get; set; }
        public string KurumAdı { get; set; }
        public string KisiTamAd { get; set; }
        public IList<KonumModel.SehirModel> Sehirler { get; set; }
        public IList<KonumModel.IlceModel> Ilceler { get; set; }
        public IList<CrmUnvanModel> Unvanlar { get; set; }
        public CrmGorusmeModel Görüsmeler { get; set; }
        
    }

}