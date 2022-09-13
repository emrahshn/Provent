using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using Core;

namespace Web.Framework.Temalar
{
    public partial class TemaSağlayıcı : ITemaSağlayıcı
    {
        #region Fields

        private readonly IList<TemaAyarları> _temaAyarları = new List<TemaAyarları>();
        private readonly string _temelYol = string.Empty;

        #endregion

        #region Constructors

        public TemaSağlayıcı()
        {
            _temelYol = GenelYardımcı.MapPath("~/Temalar/");
            AyarlarıYükle();
        }

        #endregion

        #region IThemeProvider

        public TemaAyarları TemaAyarıAl(string temaAdı)
        {
            return _temaAyarları
                .SingleOrDefault(x => x.TemaAdı.Equals(temaAdı, StringComparison.InvariantCultureIgnoreCase));
        }

        public IList<TemaAyarları> TemaAyarlarıAl()
        {
            return _temaAyarları;
        }

        public bool TemaAyarlarıMevcut(string temaAdı)
        {
            return TemaAyarlarıAl().Any(ayar => ayar.TemaAdı.Equals(temaAdı, StringComparison.InvariantCultureIgnoreCase));
        }

        #endregion

        #region Utility

        private void AyarlarıYükle()
        {
            foreach (string temaAdı in Directory.GetDirectories(_temelYol))
            {
                var ayar = TemaAyarlarıOluştur(temaAdı);
                if (ayar != null)
                {
                    _temaAyarları.Add(ayar);
                }
            }
        }

        private TemaAyarları TemaAyarlarıOluştur(string temaYolu)
        {
            var temaKlasörü = new DirectoryInfo(temaYolu);
            var temaAyarDosyası = new FileInfo(Path.Combine(temaKlasörü.FullName, "tema.ayar"));

            if (temaAyarDosyası.Exists)
            {
                var doc = new XmlDocument();
                doc.Load(temaAyarDosyası.FullName);
                return new TemaAyarları(temaKlasörü.Name, temaKlasörü.FullName, doc);
            }

            return null;
        }

        #endregion
    }
}
