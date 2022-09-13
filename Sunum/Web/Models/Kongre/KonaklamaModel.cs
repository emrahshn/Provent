using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;
using Web.Framework.Mvc;

namespace Web.Models.Kongre
{
    public partial class KonaklamaModel : TemelTSEntityModel
    {
        public KonaklamaModel()
        {
            Kongre = new List<KongreModel>();
            KullanılabilirOteller = new List<SelectListItem>();
        }
        [DisplayName("Kongre Adı")]
        [AllowHtml]
        public string KongreAdı { get; set; }
        [DisplayName("Kongre")]
        [AllowHtml]
        public int KongreId { get; set; }
        [DisplayName("Konaklama Adı")]
        [AllowHtml]
        public string KonaklamaAdı { get; set; }
        [DisplayName("Otel Adı")]
        [AllowHtml]
        public string OtelAdi { get; set; }
        [DisplayName("Otel Adı")]
        [AllowHtml]
        public int OtelId { get; set; }
        public IList<SelectListItem> KullanılabilirOteller { get; set; }
        [DisplayName("Otel Kontenjanı")]
        [AllowHtml]
        public int OtelKontenjanı { get; set; }
        [DisplayName("Otel Ücreti")]
        [AllowHtml]
        public string OtelUcreti { get; set; }
        [DisplayName("Döviz")]
        [AllowHtml]
        public string OtelUcretiDoviz { get; set; }
        [DisplayName("Otel Giriş Tarihi")]
        [AllowHtml]
        public DateTime? OtelGiris { get; set; }
        [DisplayName("Otel Çıkış")]
        [AllowHtml]
        public DateTime? OtelCikis { get; set; }
        [DisplayName("Oda Tipi")]
        [AllowHtml]
        public string OdaTipi { get; set; }
        [DisplayName("Gecelik Fark")]
        [AllowHtml]
        public string GecelikFark { get; set; }
        [DisplayName("Otel Notu")]
        [AllowHtml]
        public string OtelNotu { get; set; }
        public List<KongreModel> Kongre { get; set; }
    }

}