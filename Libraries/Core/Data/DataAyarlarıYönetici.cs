using System;
using System.Collections.Generic;
using System.IO;

namespace Core.Data
{
    public partial class DataAyarlarıYönetici
    {
        protected const char ayırıcı = ':';
        protected const string dosyaadı = "Ayarlar.txt";
        protected virtual DataAyarları AyarlarıAyrıştır(string text)
        {
            var kabukAyarları = new DataAyarları();
            if (String.IsNullOrEmpty(text))
                return kabukAyarları;
            var ayarlar = new List<string>();
            using (var reader = new StringReader(text))
            {
                string str;
                while ((str = reader.ReadLine()) != null)
                    ayarlar.Add(str);
            }

            foreach (var ayar in ayarlar)
            {
                var ayırıcıIndeksi = ayar.IndexOf(ayırıcı);
                if (ayırıcıIndeksi == -1)
                {
                    continue;
                }
                string key = ayar.Substring(0, ayırıcıIndeksi).Trim();
                string value = ayar.Substring(ayırıcıIndeksi + 1).Trim();

                switch (key)
                {
                    case "DataProvider":
                        kabukAyarları.DataSağlayıcı = value;
                        break;
                    case "DataConnectionString":
                        kabukAyarları.DataConnectionString = value;
                        break;
                    default:
                        kabukAyarları.HamDataAyarları.Add(key, value);
                        break;
                }
            }

            return kabukAyarları;
        }

        protected virtual string AyarlarıOluştur(DataAyarları ayarlar)
        {
            if (ayarlar == null)
                return "";

            return string.Format("DataSağlayıcı: {0}{2}DataConnectionString: {1}{2}",
                                 ayarlar.DataSağlayıcı,
                                 ayarlar.DataConnectionString,
                                 Environment.NewLine
                );
        }
        public virtual DataAyarları AyarlarıYükle(string dosyaYolu = null)
        {
            if (String.IsNullOrEmpty(dosyaYolu))
            {
                dosyaYolu = Path.Combine(GenelYardımcı.MapPath("~/App_Data/"), dosyaadı);
            }
            if (File.Exists(dosyaYolu))
            {
                string text = File.ReadAllText(dosyaYolu);
                return AyarlarıAyrıştır(text);
            }

            return new DataAyarları();
        }
        public virtual void AyarlarıKaydet(DataAyarları ayarlar)
        {
            if (ayarlar == null)
                throw new ArgumentNullException("ayarlar");

            string dosyaYolu = Path.Combine(GenelYardımcı.MapPath("~/App_Data/"), dosyaadı);
            if (!File.Exists(dosyaYolu))
            {
                using (File.Create(dosyaYolu))
                {
                }
            }
            var text = AyarlarıOluştur(ayarlar);
            File.WriteAllText(dosyaYolu, text);
        }
    }
}
