using Core.Domain.EkTanımlamalar;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;
using Web.Framework.Mvc;

namespace Web.Models.EkTanımlamalar
{
    public partial class TeklifKalemiModel : TemelTSEntityModel
    {
        public TeklifKalemiModel()
        {
            AnaTeklifKalemi = new List<TeklifKalemi>();
        }
        [DisplayName("Ana Teklif Kalemi")]
        public bool AnaTeklifKalemicb { get; set; }

        [DisplayName("Bağlı olduğu teklif kalemi")]
        [AllowHtml]
        public int? NodeId { get; set; }
        public string NodeAdı { get; set; }
        [DisplayName("Adı")]
        [AllowHtml]
        public string Adı { get; set; }
        [DisplayName("Sıra numarası")]
        [AllowHtml]
        public int SıraNo { get; set; }
        [DisplayName("KDV")]
        [AllowHtml]
        public int Kdv { get; set; }
        public int AnaBaslik { get; set; }

        public List<TeklifKalemi> AnaTeklifKalemi { get; set; }
    }
}