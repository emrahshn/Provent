using Web.Framework.Mvc;

namespace Web.Models.Medya
{
    public partial class ResimModel : TemelTSModel
    {
        public string ResimUrl { get; set; }

        public string ThumbResimUrl { get; set; }

        public string TamBoyutResimUrl { get; set; }

        public string Başlık { get; set; }

        public string AlternatifText { get; set; }
    }
}