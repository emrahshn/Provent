using Core;
using Core.Altyapı;
using Core.Domain.Güvenlik;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Framework.Güvenlik
{
    public class IpAdresiDoğrulaAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext == null || filterContext.HttpContext == null)
                return;
            HttpRequestBase request = filterContext.HttpContext.Request;
            if (request == null)
                return;
            if (filterContext.IsChildAction)
                return;
            bool ok = false;
            var ipAdresi = EngineContext.Current.Resolve<GüvenlikAyarları>().YöneticiAlanıİzinVerilenIPAdresleri;
            if(ipAdresi !=null && ipAdresi.Any())
            {
                var webYardımcısı = EngineContext.Current.Resolve<IWebYardımcısı>();
                foreach(string ip in ipAdresi)
                    if(ip.Equals(webYardımcısı.MevcutIpAdresiAl()))
                    {
                        ok = true;
                        break;
                    }
            }
            else
            {
                ok = true;
            }
            if (!ok)
            {
                var webYardımcısı = EngineContext.Current.Resolve<IWebYardımcısı>();
                var buSayfaUrlsi = webYardımcısı.SayfanınUrlsiniAl(false);
                if (!buSayfaUrlsi.StartsWith(string.Format("{0}Güvenlik/ErişimEngellendi", webYardımcısı.SiteKonumuAl())))
                {
                    filterContext.Result = new RedirectResult(webYardımcısı.SiteKonumuAl() + "Güvenlik/ErişimEngellendi");
                }
            }
        }
    }
}
