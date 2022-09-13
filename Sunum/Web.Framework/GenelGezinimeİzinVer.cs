using System;
using System.Web;
using System.Web.Mvc;
using Core.Data;
using Core.Altyapı;
using Services.Güvenlik;

namespace Web.Framework
{
    public class GenelGezinimeİzinVer : ActionFilterAttribute
    {
        private readonly bool _yoksay;
        public GenelGezinimeİzinVer(bool yoksay = false)
        {
            this._yoksay = yoksay;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext == null || filterContext.HttpContext == null)
                return;

            if (_yoksay)
                return;

            HttpRequestBase istek = filterContext.HttpContext.Request;
            if (istek == null)
                return;

            string actionName = filterContext.ActionDescriptor.ActionName;
            if (String.IsNullOrEmpty(actionName))
                return;

            string controllerName = filterContext.Controller.ToString();
            if (String.IsNullOrEmpty(controllerName))
                return;

            //child metod ise uygulama
            if (filterContext.IsChildAction)
                return;

            if (!DataAyarlarıYardımcısı.DatabaseYüklendi())
                return;

            var izinServisi = EngineContext.Current.Resolve<IİzinServisi>();
            var publicStoreAllowNavigation = izinServisi.YetkiVer(StandartİzinSağlayıcı.Navigasyonİzinli);
            if (publicStoreAllowNavigation)
                return;

            filterContext.Result = new HttpUnauthorizedResult();
        }
    }
}
