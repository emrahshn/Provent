using System;
using System.Web.Mvc;
using Core;
using Core.Data;
using Core.Altyapı;
using Services.Kullanıcılar;


namespace Web.Framework
{
    public class ŞifreDoğrulaAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext == null || filterContext.HttpContext == null || filterContext.HttpContext.Request == null)
                return;

            //Alt yöntemlere filtre uygulamayın
            if (filterContext.IsChildAction)
                return;

            var actionName = filterContext.ActionDescriptor.ActionName;
            if (string.IsNullOrEmpty(actionName) || actionName.Equals("ŞifreDeğiştir", StringComparison.InvariantCultureIgnoreCase))
                return;

            var controllerName = filterContext.Controller.ToString();
            if (string.IsNullOrEmpty(controllerName) || controllerName.Equals("Kullanıcı", StringComparison.InvariantCultureIgnoreCase))
                return;

            if (!DataAyarlarıYardımcısı.DatabaseYüklendi())
                return;

            //Mevcut kullanıcı al
            var kullanıcı = EngineContext.Current.Resolve<IWorkContext>().MevcutKullanıcı;
            //Şifre geçerliliğini kontrol et
            if (kullanıcı.ParolaSüresiDoldu())
            {
                var şifreDeğiştimeUrlsi = new UrlHelper(filterContext.RequestContext).RouteUrl("KullanıcıŞifreDeğiştirme");
                filterContext.Result = new RedirectResult(şifreDeğiştimeUrlsi);
            }
        }
    }
}
