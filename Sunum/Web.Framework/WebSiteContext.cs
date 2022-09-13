using System;
using System.Linq;
using Core;
using Core.Domain.Siteler;
using Services.Siteler;

namespace Web.Framework
{
    public partial class WebSiteContext : ISiteContext
    {
        private readonly ISiteServisi _siteServisi;
        private readonly IWebYardımcısı _webYardımcısı;

        private Site _cachedStore;

        public WebSiteContext(ISiteServisi siteServisi, IWebYardımcısı webYardımcısı)
        {
            this._siteServisi = siteServisi;
            this._webYardımcısı = webYardımcısı;
        }
        public virtual Site MevcutSite
        {
            get
            {
                if (_cachedStore != null)
                    return _cachedStore;

                var host = _webYardımcısı.ServerDeğişkenleri("HTTP_HOST");
                var allSites = _siteServisi.TümSiteler();
                var site = allSites.FirstOrDefault(s => s.HostDeğeriİçerir(host));

                if (site == null)
                {
                    //ilk bulunan siteyi yükle
                    site = allSites.FirstOrDefault();
                }
                if (site == null)
                    throw new Exception("Hiçbir site yüklenemedi");

                _cachedStore = site;
                return _cachedStore;
            }
        }
    }
}
