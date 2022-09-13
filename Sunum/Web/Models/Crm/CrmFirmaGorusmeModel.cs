using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Web.Framework.Mvc;

namespace Web.Models.Crm
{
    public partial class CrmFirmaGorusmeModel : TemelTSEntityModel
    {
        public CrmFirmaGorusmeModel()
        {
            Yetkililer = new List<SelectListItem>();
        }
        [DisplayName("Yetkili")]
        [AllowHtml]
        public int YetkiliId { get; set; }
        [UIHint("DateTime")]
        [DisplayName("Görüşme Tarihi")]
        [AllowHtml]
        public DateTime GorusmeTarihi { get; set; }
        [DisplayName("Görüşme Şekli")]
        [AllowHtml]
        public int GorusmeSekli { get; set; }
        [DisplayName("Görüşme Sebebi")]
        [AllowHtml]
        public int GorusmeSebebi { get; set; }
        [DisplayName("Görüşen")]
        [AllowHtml]
        public int Gorusen { get; set; }
        [UIHint("RichEditor")]
        [DisplayName("Notlar")]
        [AllowHtml]
        public string Notlar { get; set; }
        [DisplayName("3 Günlük Hatırlatma")]
        [AllowHtml]
        public bool UcGun { get; set; }
        [DisplayName("3 Haftalık Hatırlatma")]
        [AllowHtml]
        public bool UcHafta { get; set; }
        [DisplayName("3 Aylık Hatırlatma")]
        [AllowHtml]
        public bool UcAy { get; set; }
        public string GorusenAdı { get; set; }
        public string GorusulenAdı { get; set; }
        public IList<SelectListItem> Yetkililer { get; set; }
        public string FirmaAdı { get; set; }
        public string YetkiliAdı { get; set; }
        

    }

}