using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;
using Web.Framework.Mvc;

namespace Web.Models.Kongre
{
    public partial class KursModel : TemelTSEntityModel
    {
        public KursModel()
        {
            Kongre = new List<KongreModel>();
        } 
        [DisplayName("Kongre Adı")]
        [AllowHtml]
        public string KongreAdı { get; set; }
        public int KongreId { get; set; }
        [DisplayName("Kurs Adı")]
        [AllowHtml]
        public string KursAdı { get; set; }
        [DisplayName("Kurs Ücreti")]
        [AllowHtml]
        public string KursUcreti { get; set; }
        [DisplayName("Kurs Başlama Tarihi")]
        [AllowHtml]
        public DateTime KursBaslamaTarihi { get; set; }
        [DisplayName("Kurs Bitiş Tarihi")]
        [AllowHtml]
        public DateTime KursBitisTarihi { get; set; }
        [DisplayName("Kurs Ücreti Döviz")]
        [AllowHtml]
        public string KursUcretiDoviz { get; set; }
        [DisplayName("Kurs Notu")]
        [AllowHtml]
        public string KursNotu { get; set; }
        public List<KongreModel> Kongre { get; set; }
    }

}