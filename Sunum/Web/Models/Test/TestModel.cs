using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Web.Framework.Mvc;

namespace Web.Models.Test
{
    public partial class TestModel : TemelTSEntityModel
    {
        public TestModel()
        {
        }
        [DisplayName("Sayi")]
        [AllowHtml]
        public decimal Sayi { get; set; }
        [UIHint("Xls")]
        public int xls { get; set; }

    }
}