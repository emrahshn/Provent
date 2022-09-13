using System.Xml;

namespace Web.Framework.Temalar
{
    public class TemaAyarları
    {
        public TemaAyarları(string temaAdı, string yol, XmlDocument doc)
        {
            TemaAdı = temaAdı;
            Yol = yol;
            var node = doc.SelectSingleNode("Tema");
            if (node != null)
            {
                YapılandırmaNode = node;
                var öznitelik = node.Attributes["Başlık"];
                TemaBaşlığı = öznitelik == null ? string.Empty : öznitelik.Value;
                öznitelik = node.Attributes["ResimURLÖnizleme"];
                ResimURLÖnizleme = öznitelik == null ? string.Empty : öznitelik.Value;
                öznitelik = node.Attributes["TextÖnizleme"];
                TextÖnizleme = öznitelik == null ? string.Empty : öznitelik.Value;
            }
        }

        public XmlNode YapılandırmaNode { get; protected set; }

        public string Yol { get; protected set; }

        public string ResimURLÖnizleme { get; protected set; }

        public string TextÖnizleme { get; protected set; }

        public string TemaAdı { get; protected set; }

        public string TemaBaşlığı { get; protected set; }

    }
}