using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Web.Framework.Mvc;

namespace Web.Models.Crm
{
    public partial class CrmYonetimKuruluModel : TemelTSEntityModel
    {
        public CrmYonetimKuruluModel()
        {
            Gorevler = new List<CrmGorevModel>();
            Kurumlar = new List<CrmKurumModel>();
        }
        [DisplayName("Kurum")]
        [AllowHtml]
        public int KurumId { get; set; }
        [DisplayName("Kişi")]
        [AllowHtml]
        public int KisiId { get; set; }
        [DisplayName("Görevi")]
        [AllowHtml]
        public int Gorevi { get; set; }
        [UIHint("DateNullable")]
        [DisplayName("Başlangıç Tarihi")]
        [AllowHtml]
        public DateTime? BaslamaTarihi { get; set; }
        [UIHint("DateNullable")]
        [DisplayName("Bitiş Tarihi")]
        [AllowHtml]
        public DateTime? BitisTarihi { get; set; }
        [DisplayName("Görevini Tamamladı")]
        [AllowHtml]
        public bool Onceki { get; set; }
        public string GorevAdı { get; set; }
        public string KurumAdı { get; set; }
        public string KisiAdı { get; set; }
        public IList<CrmGorevModel> Gorevler { get; set; }
        public IList<CrmKurumModel> Kurumlar { get; set; }
    }
}