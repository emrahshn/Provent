using System.Collections.Generic;
using Web.Framework.Mvc;

namespace Web.Models.Katalog
{
    public class KategoriBasitModel : TemelTSEntityModel
    {
        public KategoriBasitModel()
        {
            AltKategoriler = new List<KategoriBasitModel>();
        }
        public List<KategoriBasitModel> AltKategoriler { get; set; }
        public string Adı { get; set; }
        public string SeAdı { get; set; }
        public bool ÜstMenüyeDahil { get; set; }

    }
}