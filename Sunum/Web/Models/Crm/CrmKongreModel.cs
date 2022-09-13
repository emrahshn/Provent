using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Web.Framework.Mvc;

namespace Web.Models.Crm
{
    public partial class CrmKongreModel : TemelTSEntityModel
    {
        public CrmKongreModel()
        {
            Kisiler = new List<CrmKisiModel>();
        }
        [DisplayName("Adı")]
        [AllowHtml]
        public string Adı { get; set; }
        [DisplayName("Yer")]
        [AllowHtml]
        public string Yer { get; set; }
        [UIHint("Date")]
        [DisplayName("Tarih")]
        [AllowHtml]
        public DateTime Tarih { get; set; }
        [DisplayName("Acenta")]
        [AllowHtml]
        public string Acenta { get; set; }
        [DisplayName("Web")]
        [AllowHtml]
        public string Web { get; set; }
        [DisplayName("KatılımcıSayısı")]
        [AllowHtml]
        public int KatılımcıSayısı { get; set; }
        [DisplayName("StandSayısı")]
        [AllowHtml]
        public int StandSayısı { get; set; }
        [DisplayName("KatılımUcreti")]
        [AllowHtml]
        public string KatılımUcreti { get; set; }
        [DisplayName("IhaleYeri")]
        [AllowHtml]
        public string IhaleYeri { get; set; }
        [UIHint("Date")]
        [DisplayName("IhaleTarihi")]
        [AllowHtml]
        public DateTime IhaleTarihi { get; set; }
        [DisplayName("Ilgili")]
        [AllowHtml]
        public int Ilgili { get; set; }
        [UIHint("RichEditor")]
        [DisplayName("Notlar")]
        [AllowHtml]
        public string Notlar { get; set; }
        public IList<CrmKisiModel> Kisiler { get; set; }
    }

}