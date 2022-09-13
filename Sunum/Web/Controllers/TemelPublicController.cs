using Core;
using Core.Altyapı;
using Core.Domain.Genel;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Web.Framework;
using Web.Framework.Controllers;
using Web.Framework.Güvenlik;
using Web.Framework.Mvc;

namespace Web.Controllers
{
    [HttpsGerekli(SslGerekli.Evet)]
    [IpAdresiDoğrula]
    [AntiForgery]
    [GenelGezinimeİzinVer(false)]
    public abstract partial class TemelPublicController : TemelController
    {
        protected virtual ActionResult Http404Çağır()
        {
            //hedef controller çağır ve routeData'yı geç.
            IController hataController = EngineContext.Current.Resolve<TemelController>();

            var routeData = new RouteData();
            routeData.Values.Add("controller", "Common");
            routeData.Values.Add("action", "PageNotFound");

            hataController.Execute(new RequestContext(this.HttpContext, routeData));

            return new EmptyResult();
        }
        protected override void Initialize(RequestContext requestContext)
        {
            //EngineContext.Current.Resolve<IWorkContext>().Yönetici = true;
            base.Initialize(requestContext);
        }
        protected override void OnException(ExceptionContext filterContext)
        {
            if (filterContext.Exception != null)
                LogHatası(filterContext.Exception);
            base.OnException(filterContext);
        }
        protected virtual ActionResult ErişimEngellendiView()
        {
            return RedirectToAction("ErişimEngellendi", "Güvenlik", new { pageUrl = this.Request.RawUrl });
        }
        protected virtual ActionResult ErişimEngellendiKendoGridJson()
        {
            return KendoGridJsonHatası("Erişim Engellendi");
        }
        protected virtual void SeçiliTabKaydet(string tabAdı = "", bool sonrakiİstekİçinKalıcı = true)
        {
            if (String.IsNullOrEmpty(tabAdı))
            {
                tabAdı = this.Request.Form["selected-tab"];
            }
            if (!String.IsNullOrEmpty(tabAdı))
            {
                const string dataKey = "cms.selected-tab";
                if (sonrakiİstekİçinKalıcı)
                    TempData[dataKey] = tabAdı;
                else
                    ViewData[dataKey] = tabAdı;
            }
        }

        protected virtual void SeçiliSayfaKaydet(string sayfa = "", bool sonrakiİstekİçinKalıcı = true)
        {
            if (String.IsNullOrEmpty(sayfa))
            {
                sayfa = this.Request.Form["page"];
            }
            if (!String.IsNullOrEmpty(sayfa))
            {
                const string dataKey = "selected-page";
                if (sonrakiİstekİçinKalıcı)
                    TempData[dataKey] = sayfa;
                else
                    ViewData[dataKey] = sayfa;
            }
        }
        protected override JsonResult Json(object data, string contentType, Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            var result = EngineContext.Current.Resolve<AdminAyarları>().JsondaIsoTarihdönüşümüKullan ? new JsonSonuçDönüştürücü(new IsoDateTimeConverter()) : new JsonResult();
            result.Data = data;
            result.ContentType = contentType;
            result.ContentEncoding = contentEncoding;
            result.JsonRequestBehavior = behavior;
            result.MaxJsonLength = int.MaxValue;
            return result;
        }
    }
}