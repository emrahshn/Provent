using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Web.Framework.Mvc;
using Web.Models.Konum;

namespace Web.Models.Crm
{
    public partial class CrmFirmaModel : TemelTSEntityModel
    {
        public CrmFirmaModel()
        {
            Sehirler = new List<KonumModel.SehirModel>();
            Ilceler = new List<KonumModel.IlceModel>();
            Yetkili = new List<SelectListItem>();
            YetkiliIdleri = new List<int>();
            Görüsmeler = new List<CrmFirmaGorusmeModel>();
        }
        [DisplayName("Adı")]
        [AllowHtml]
        public string Adı { get; set; }
        [DisplayName("Ticari Ünvanı")]
        [AllowHtml]
        public string TicariUnvan { get; set; }
        [DisplayName("Telefon")]
        [AllowHtml]
        public string Tel { get; set; }
        [DisplayName("Cep Telefonu")]
        [AllowHtml]
        public string CepTel { get; set; }
        [DisplayName("Faks")]
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
        [DisplayName("E-Mail")]
        [AllowHtml]
        public string Email { get; set; }
        [DisplayName("Vergi Dairesi")]
        [AllowHtml]
        public string VergiDairesi { get; set; }
        [DisplayName("Vergi Numarası")]
        [AllowHtml]
        public string VergiNo { get; set; }
        [AllowHtml]
        public DateTime? OlusturulmaTarihi { get; set; }
        public IList<KonumModel.SehirModel> Sehirler { get; set; }
        public IList<KonumModel.IlceModel> Ilceler { get; set; }
        public IList<CrmFirmaGorusmeModel> Görüsmeler { get; set; }
        [UIHint("MultiSelect")]
        [DisplayName("Yetkililer")]
        public List<int> YetkiliIdleri { get; set; }
        public IList<SelectListItem> Yetkili { get; set; }
        public string SehirAdı { get; set; }
        public string IlceAdı { get; set; }

    }

}