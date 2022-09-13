using Core.Domain.EkTanımlamalar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Web.Framework.Mvc;

namespace Web.Models.Kongre
{
    public partial class KayıtBilgileriModel : TemelTSEntityModel
    {
        public KayıtBilgileriModel()
        {
            KayıtTipleri =new  List<SelectListItem>();
        }
        [DisplayName("KongreId")]
        [AllowHtml]
        public int KongreId { get; set; }
        [DisplayName("Kayıt Tipi")]
        [AllowHtml]
        public int KayıtTipiId { get; set; }
        [DisplayName("Adı")]
        [AllowHtml]
        public string Adı { get; set; }
        [DisplayName("Tutar")]
        [AllowHtml]
        public decimal Tutar { get; set; }
        [DisplayName("Döviz")]
        [AllowHtml]
        public int Döviz { get; set; }
        [DisplayName("Tarihinden")]
        [AllowHtml]
        public DateTime Tarihinden { get; set; }
        [DisplayName("Tarihine")]
        [AllowHtml]
        public DateTime Tarihine { get; set; }
        public IList<SelectListItem> KayıtTipleri { get; set; }
    }

}