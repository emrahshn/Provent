using System.ComponentModel.DataAnnotations;
using Web.Framework.Mvc;

namespace Web.Models.Xls
{
    public partial class XlsReaderModel : TemelTSEntityModel
    {
        [UIHint("Xls")]
        public int xls { get; set; }
        public string SheetName { get; set; }
    }
}