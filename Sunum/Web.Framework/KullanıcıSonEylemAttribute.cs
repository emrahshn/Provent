using System;
using System.Web.Mvc;
using Core;
using Core.Data;
using Core.Altyapı;
using Services.Kullanıcılar;

namespace Web.Framework
{
    public class KullanıcıSonEylemAttribute : ActionFilterAttribute
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

            var workContext = EngineContext.Current.Resolve<IWorkContext>();
            var kullanıcı = workContext.MevcutKullanıcı;

            //update last activity date
            if (kullanıcı.SonİşlemTarihi.AddMinutes(1.0) < DateTime.UtcNow)
            {
                var kullanıcıServisi = EngineContext.Current.Resolve<IKullanıcıServisi>();
                kullanıcı.SonİşlemTarihi = DateTime.UtcNow;
                kullanıcıServisi.KullanıcıGüncelle(kullanıcı);
            }
        }
    }
}
