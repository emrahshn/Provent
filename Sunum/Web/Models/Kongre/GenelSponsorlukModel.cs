using Core.Domain.EkTanımlamalar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Web.Framework.Mvc;

namespace Web.Models.Kongre
{
    public partial class GenelSponsorlukModel : TemelTSEntityModel
    {
        public GenelSponsorlukModel()
        {

        }
        [DisplayName("KongreId")]
        [AllowHtml]
        public int KongreId { get; set; }
        [DisplayName("Adı")]
        [AllowHtml]
        public string Adı { get; set; }
        [DisplayName("Tutar")]
        [AllowHtml]
        public decimal Tutar { get; set; }
        [DisplayName("Döviz")]
        [AllowHtml]
        public int Döviz { get; set; }
        [DisplayName("Tarih")]
        [AllowHtml]
        public DateTime Tarih { get; set; }
    }

}