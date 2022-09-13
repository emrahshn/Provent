using System;
using System.Web.Mvc;
using Core;
using Core.Data;
using Core.Domain.Güvenlik;
using Core.Altyapı;

namespace Web.Framework.Güvenlik
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class HttpsGerekliAttribute : FilterAttribute, IAuthorizationFilter
    {
        public HttpsGerekliAttribute(SslGerekli sslGerekli)
        {
            this.SslGerekli = sslGerekli;
        }
        public virtual void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
                throw new ArgumentNullException("filterContext");

            //child metod ise gerekli değil
            if (filterContext.IsChildAction)
                return;

            // sadece GET isteklerinde yönlendir, 
            // diğer türlü çalışmayacaktır.
            if (!String.Equals(filterContext.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
                return;

            if (!DataAyarlarıYardımcısı.DatabaseYüklendi())
                return;
            var güvenlikAyarları = EngineContext.Current.Resolve<GüvenlikAyarları>();
            if (güvenlikAyarları.TümSayfalarıSslİçinZorla)
                //belirtilen değerden bağımsız olarak tüm sayfalar SSL olmaya zorlanır
                this.SslGerekli = SslGerekli.Evet;

            switch (this.SslGerekli)
            {
                case SslGerekli.Evet:
                    {
                        var webYardımcısı = EngineContext.Current.Resolve<IWebYardımcısı>();
                        var mevcutBağlantıGüvenli = webYardımcısı.MevcutBağlantıGüvenli();
                        if (!mevcutBağlantıGüvenli)
                        {
                            var siteContext = EngineContext.Current.Resolve<ISiteContext>();
                            if (siteContext.MevcutSite.SslEtkin)
                            {
                                //sayfayı HTTPS sürümüne yönlendir
                                string url = webYardımcısı.SayfanınUrlsiniAl(true, true);

                                //301 (permanent) yönlendirme
                                filterContext.Result = new RedirectResult(url, true);
                            }
                        }
                    }
                    break;
                case SslGerekli.Hayır:
                    {
                        var webYardımcısı = EngineContext.Current.Resolve<IWebYardımcısı>();
                        var mevcutBağlantıGüvenli = webYardımcısı.MevcutBağlantıGüvenli();
                        if (mevcutBağlantıGüvenli)
                        {
                            //sayfayı HTTPS sürümüne yönlendir
                            string url = webYardımcısı.SayfanınUrlsiniAl(true, false);
                            //301 (permanent) yönlendirme
                            filterContext.Result = new RedirectResult(url, true);
                        }
                    }
                    break;
                case SslGerekli.ÖnemliDeğil:
                    {
                        //bişey yapma
                    }
                    break;
                default:
                    throw new TSHata("Desteklenmeyen Ssl Koruma parametresi");
            }
        }

        public SslGerekli SslGerekli { get; set; }
    }
}
