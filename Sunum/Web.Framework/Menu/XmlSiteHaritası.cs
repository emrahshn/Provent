using Core;
using Core.Altyapı;
using Services.Güvenlik;
using System;
using System.IO;
using System.Linq;
using System.Web.Routing;
using System.Xml;

namespace Web.Framework.Menu
{
    public class XmlSiteHaritası
    {
        public XmlSiteHaritası()
        {
            RootNode = new SiteHaritasıNode();
        }
        public SiteHaritasıNode RootNode { get; set; }
        public virtual void Yükle(string fizikselYol)
        {
            string dosyaYolu = GenelYardımcı.MapPath(fizikselYol);
            string içerik = File.ReadAllText(dosyaYolu);
            if (!String.IsNullOrEmpty(içerik))
            {
                using (var sr = new StringReader(içerik))
                {
                    using (var xr = XmlReader.Create(sr,
                        new XmlReaderSettings {
                            CloseInput = true,
                            IgnoreWhitespace = true,
                            IgnoreComments = true,
                            IgnoreProcessingInstructions = true

                        }))
                    {
                        var doc = new XmlDocument();
                        doc.Load(xr);
                        if ((doc.DocumentElement != null) && doc.HasChildNodes)
                        {
                            XmlNode xmlRootNode = doc.DocumentElement.FirstChild;
                            Iterate(RootNode, xmlRootNode);
                        }
                    }
                }
            }
        }

        private static void Iterate(SiteHaritasıNode siteHaritasıNode, XmlNode xmlNode)
        {
            NodeÇoğalt(siteHaritasıNode, xmlNode);

            foreach (XmlNode xmlChildNode in xmlNode.ChildNodes)
            {
                if (xmlChildNode.LocalName.Equals("siteHaritasıNode", StringComparison.InvariantCultureIgnoreCase))
                {
                    var siteHaritasıChildNode = new SiteHaritasıNode();
                    siteHaritasıNode.ChildNodes.Add(siteHaritasıChildNode);
                    Iterate(siteHaritasıChildNode, xmlChildNode);
                }
            }
        }

        private static void NodeÇoğalt(SiteHaritasıNode siteHaritasıNode, XmlNode xmlNode)
        {
            //sistem adı
            siteHaritasıNode.SistemAdı = GetStringValueFromAttribute(xmlNode, "SistemAdı");

            //başlık
            siteHaritasıNode.Başlık = GetStringValueFromAttribute(xmlNode, "Kaynak");

            //routes, url
            string controllerName = GetStringValueFromAttribute(xmlNode, "controller");
            string actionName = GetStringValueFromAttribute(xmlNode, "action");
            string url = GetStringValueFromAttribute(xmlNode, "url");
            if (!string.IsNullOrEmpty(controllerName) && !string.IsNullOrEmpty(actionName))
            {
                siteHaritasıNode.ControllerAdı = controllerName;
                siteHaritasıNode.ActionAdı = actionName;
            }
            else if (!string.IsNullOrEmpty(url))
            {
                siteHaritasıNode.Url = url;
            }

            //resim URL
            siteHaritasıNode.IconClass = GetStringValueFromAttribute(xmlNode, "IconClass");

            //izin adı
            var izinAdları = GetStringValueFromAttribute(xmlNode, "İzinAdları");
            if (!string.IsNullOrEmpty(izinAdları))
            {
                var izinServisi = EngineContext.Current.Resolve<IİzinServisi>();
                siteHaritasıNode.Visible = izinAdları.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                   .Any(izinAdı => izinServisi.YetkiVer(izinAdı.Trim()));
            }
            else
            {
                siteHaritasıNode.Visible = true;
            }

            // yeni sekmede aç
            var yeniSekmedeAç = GetStringValueFromAttribute(xmlNode, "YeniSekmedeAç");
            bool sonuç;
            if (!string.IsNullOrWhiteSpace(yeniSekmedeAç) && bool.TryParse(yeniSekmedeAç, out sonuç))
            {
                siteHaritasıNode.YeniSekmedeAç = sonuç;
            }
        }

        private static string GetStringValueFromAttribute(XmlNode node, string attributeName)
        {
            string value = null;

            if (node.Attributes != null && node.Attributes.Count > 0)
            {
                XmlAttribute attribute = node.Attributes[attributeName];

                if (attribute != null)
                {
                    value = attribute.Value;
                }
            }

            return value;
        }
    }
}

