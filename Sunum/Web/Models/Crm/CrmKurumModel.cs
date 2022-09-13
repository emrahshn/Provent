using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;
using Web.Framework.Mvc;
using Web.Models.Konum;

namespace Web.Models.Crm
{
    public partial class CrmKurumModel : TemelTSEntityModel
    {
        public CrmKurumModel()
        {
            Sehirler = new List<KonumModel.SehirModel>();
            Ilceler = new List<KonumModel.IlceModel>();
        }
        [DisplayName("Adı")]
        [AllowHtml]
        public string Adı { get; set; }
        [DisplayName("Kurum Tipi")]
        [AllowHtml]
        public int Tipi { get; set; }
        [DisplayName("Şehir")]
        [AllowHtml]
        public int SehirId { get; set; }
        [DisplayName("İlçe")]
        [AllowHtml]
        public int IlceId { get; set; }
        [DisplayName("Adres")]
        [AllowHtml]
        public string Adres { get; set; }
        [DisplayName("Web")]
        [AllowHtml]
        public string Web { get; set; }
        [DisplayName("E-Mail")]
        [AllowHtml]
        public string Email { get; set; }
        [DisplayName("Telefon")]
        [AllowHtml]
        public string Tel { get; set; }
        [DisplayName("Faks")]
        [AllowHtml]
        public string Faks { get; set; }
        public IList<KonumModel.SehirModel> Sehirler { get; set; }
        public IList<KonumModel.IlceModel> Ilceler { get; set; }
    }
}