using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;
using Web.Framework.Mvc;

namespace Web.Models.Kongre
{
    public partial class TransferModel : TemelTSEntityModel
    {
        public TransferModel()
        {
            Kongre = new List<KongreModel>();
        }
        [DisplayName("KongreAdı")]
        [AllowHtml]
        public string KongreAdı { get; set; }
        public int KongreId { get; set; }
        [DisplayName("Adı")]
        [AllowHtml]
        public string Adı { get; set; }
        [DisplayName("Tutar")]
        [AllowHtml]
        public string Tutar { get; set; }
        [DisplayName("Döviz")]
        [AllowHtml]
        public int Döviz { get; set; }
        [DisplayName("Transfer Aracı")]
        [AllowHtml]
        public string TransferAracı { get; set; }
        [DisplayName("Transfer Notu")]
        [AllowHtml]
        public string TransferNotu { get; set; }
        [DisplayName("Tarih")]
        [AllowHtml]
        public DateTime Tarih { get; set; }
        public List<KongreModel> Kongre { get; set; }
    }
}