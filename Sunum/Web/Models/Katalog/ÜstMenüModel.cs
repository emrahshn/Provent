using System.Collections.Generic;
using System.Linq;
using Web.Framework.Mvc;

namespace Web.Models.Katalog
{
    public partial class ÜstMenüModel:TemelTSModel
    {
        public ÜstMenüModel()
        {
            Kategoriler = new List<KategoriBasitModel>();
            Başlıklar = new List<ÜstMenüBaşlıkModeli>();
        }

        public IList<ÜstMenüBaşlıkModeli> Başlıklar { get; set; }
        public IList<KategoriBasitModel> Kategoriler { get; set; }
        public bool BlogEtkin { get; set; }
        public bool ForumEtkin { get; set; }
        public bool AnasayfaÖğesiGörüntüle { get; set; }
        public bool KullanıcıHesabıÖğesiGörüntüle { get; set; }
        public bool BlogÖğesiGörüntüle { get; set; }
        public bool ForumÖğesiGörüntüle { get; set; }
        public bool İletişimeGeçinÖğesiGörüntüle { get; set; }
        public bool SadeceKategoriler
        {
            get
            {
                return Kategoriler.Any()
                    //&& !Başlıklar.Any()
                    && !AnasayfaÖğesiGörüntüle
                    && !KullanıcıHesabıÖğesiGörüntüle
                    && !(BlogÖğesiGörüntüle && BlogEtkin)
                    && !(ForumEtkin && ForumÖğesiGörüntüle)
                    && !İletişimeGeçinÖğesiGörüntüle;
            }
        }

        public class ÜstMenüBaşlıkModeli : TemelTSEntityModel
        {
            public string Adı { get; set; }
            public string SeAdı { get; set; }
        }
    }
}