using Core.Domain.EkTanımlamalar;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;
using Web.Framework.Mvc;

namespace Web.Models.EkTanımlamalar
{
    public partial class GelirGiderTanımlamaModel : TemelTSEntityModel
    {
        public GelirGiderTanımlamaModel()
        {
            AnaTeklifKalemi = new List<GelirGiderTanımlama>();
        }
        [DisplayName("Adı")]
        [AllowHtml]
        public string Adı { get; set; }
        [DisplayName("Gelir")]
        [AllowHtml]
        public bool Gelir { get; set; }
        [DisplayName("Ana Başlık")]
        [AllowHtml]
        public bool Anabaşlık { get; set; }
        [DisplayName("Ana Başlık")]
        [AllowHtml]
        public int? NodeId { get; set; }
        public List<GelirGiderTanımlama> AnaTeklifKalemi { get; set; }
    }

}