using System.ComponentModel;
using System.Web.Mvc;
using Web.Framework.Mvc;

namespace Web.Models.Konum
{
    public partial class KonumModel : TemelTSEntityModel
    {
        public KonumModel()
        {
            Ulke = new UlkeModel();
            Sehir = new SehirModel();
            Ilce = new IlceModel();
        }
        public UlkeModel Ulke { get; set; }
        public SehirModel Sehir { get; set; }
        public IlceModel Ilce { get; set; }

        public partial class IlceModel: TemelTSEntityModel
        {
            [DisplayName("Adı")]
            [AllowHtml]
            public string Adı { get; set; }
            [DisplayName("Şehir Adı")]
            [AllowHtml]
            public string SehirAdı { get; set; }
            public int SehirId { get; set; }
            [DisplayName("Ülke Adı")]
            [AllowHtml]
            public string UlkeAdı { get; set; }
            public int UlkeId { get; set; }
        }

        public partial class SehirModel: TemelTSEntityModel
        {
            [DisplayName("Adı")]
            [AllowHtml]
            public string Adı { get; set; }
            [DisplayName("Ülke Adı")]
            [AllowHtml]
            public string UlkeAdı { get; set; }
            public int UlkeId { get; set; }
        }

        public partial class UlkeModel: TemelTSEntityModel
        {
            [DisplayName("Adı")]
            [AllowHtml]
            public string Adı { get; set; }
        }
    }
}