using System;
using System.Web.Mvc;
using Core;
using Core.Data;
using Core.Altyapı;
using Services;
using Services.Kullanıcılar;

namespace Web.Framework
{
    public class SiteIpAdresiAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!DataAyarlarıYardımcısı.DatabaseYüklendi())
                return;

            if (filterContext == null || filterContext.HttpContext == null || filterContext.HttpContext.Request == null)
                return;

            //Alt yöntemlere filtre uygulamayın
            if (filterContext.IsChildAction)
                return;

            //Yalnızca GET istekleri
            if (!String.Equals(filterContext.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
                return;

            var webYardımcısı = EngineContext.Current.Resolve<IWebYardımcısı>();

            //IP adresini güncelle
            string mevcutIpAdresi = webYardımcısı.MevcutIpAdresiAl();
            if (!String.IsNullOrEmpty(mevcutIpAdresi))
            {
                var workContext = EngineContext.Current.Resolve<IWorkContext>();
                var kullanıcı = workContext.MevcutKullanıcı;
                if (!mevcutIpAdresi.Equals(kullanıcı.SonIPAdresi, StringComparison.InvariantCultureIgnoreCase))
                {
                    var kullanıcıServisi = EngineContext.Current.Resolve<IKullanıcıServisi>();
                    kullanıcı.SonIPAdresi = mevcutIpAdresi;
                    kullanıcıServisi.KullanıcıGüncelle(kullanıcı);
                }
            }
        }
    }
}
