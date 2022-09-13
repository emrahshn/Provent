using System;
using System.Web.Mvc;
using Core;
using Core.Data;
using Core.Domain.Kullanıcılar;
using Core.Altyapı;
using Services.Genel;

namespace Web.Framework
{
    public class SiteSonZiyaretEdilenSayfaAttribute : ActionFilterAttribute
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

            var kullanıcıAyarları = EngineContext.Current.Resolve<KullanıcıAyarları>();
            if (!kullanıcıAyarları.SiteSonZiyaretSayfası)
                return;

            var webYardımcısı = EngineContext.Current.Resolve<IWebYardımcısı>();
            var sayfaUrl = webYardımcısı.SayfanınUrlsiniAl(true);
            if (!String.IsNullOrEmpty(sayfaUrl))
            {
                var workContext = EngineContext.Current.Resolve<IWorkContext>();
                var genelÖznitelikServisi = EngineContext.Current.Resolve<IGenelÖznitelikServisi>();

                var öncekiSayfaUrl = workContext.MevcutKullanıcı.ÖznitelikAl<string>(SistemKullanıcıÖznitelikAdları.SonZiyaretEdilenSayfa);
                if (!sayfaUrl.Equals(öncekiSayfaUrl))
                {
                    genelÖznitelikServisi.ÖznitelikKaydet(workContext.MevcutKullanıcı, SistemKullanıcıÖznitelikAdları.SonZiyaretEdilenSayfa, sayfaUrl);
                }
            }
        }
    }
}
