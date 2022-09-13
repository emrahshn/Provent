using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Web.Framework.Mvc;

namespace Web.Models.Kongre
{
    public partial class KontenjanBilgileriModel: TemelTSEntityModel
    {
        public KontenjanBilgileriModel()
        {
            Oteller = new List<SelectListItem>();
        }
        [DisplayName("Kongre")]
        [AllowHtml]
        public int KongreId { get; set; }
        [DisplayName("Otel")]
        [AllowHtml]
        public int OtelId { get; set; }

        [DisplayName("Adı")]
        [AllowHtml]
        public string Adı { get; set; }
        [DisplayName("Oda kişi sayısı")]
        [AllowHtml]
        public int OdaKisiSayısı { get; set; }
        [DisplayName("Tutar")]
        [AllowHtml]
        public decimal Tutar { get; set; }
        [DisplayName("Para birimi")]
        [AllowHtml]
        public int Döviz { get; set; }
        [UIHint("Date")]
        [DisplayName("Geçerlilik başlama tarihi")]
        [AllowHtml]
        public DateTime Tarihinden { get; set; }
        [UIHint("Date")]
        [DisplayName("Geçerlilik bitiş tarihi")]
        [AllowHtml]
        public DateTime Tarihine { get; set; }
        public IList<SelectListItem> Oteller { get; set; }
    }
}