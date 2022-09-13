using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using Core;
using Core.Altyapı;
using Services.Genel;
using Services.Logging;
using Services.Siteler;
using Web.Framework.Kendoui;
using Web.Framework.UI;
using Core.Domain.Kullanıcılar;

namespace Web.Framework.Controllers
{
    [SiteIpAdresi]
    [KullanıcıSonEylem]
    [SiteSonZiyaretEdilenSayfa]
    [ŞifreDoğrula]
    public abstract class TemelController:Controller
    {
        public virtual string RenderPartialViewToString()
        {
            return RenderPartialViewToString(null, null);
        }
        public virtual string RenderPartialViewToString(string viewName)
        {
            return RenderPartialViewToString(viewName, null);
        }
        public virtual string RenderPartialViewToString(object model)
        {
            return RenderPartialViewToString(null, model);
        }
        public virtual string RenderPartialViewToString(string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = this.ControllerContext.RouteData.GetRequiredString("action");

            this.ViewData.Model = model;

            using (var sw = new StringWriter())
            {
                ViewEngineResult viewResult = System.Web.Mvc.ViewEngines.Engines.FindPartialView(this.ControllerContext, viewName);
                var viewContext = new ViewContext(this.ControllerContext, viewResult.View, this.ViewData, this.TempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }
        public virtual int AktifSiteKapsamYapılandırmaAl(ISiteServisi siteServisi, IWorkContext workContext)
        {
            //Birden fazla site varmı kontrol et
            if (siteServisi.TümSiteler().Count < 2)
                return 0;


            var siteId = workContext.MevcutKullanıcı.ÖznitelikAl<int>(SistemKullanıcıÖznitelikAdları.YöneticiAlanıSiteKapsamAyarı);
            var site = siteServisi.SiteAlId(siteId);
            return site != null ? site.Id : 0;
        }
        protected void LogHatası(Exception exc)
        {
            var workContext = EngineContext.Current.Resolve<IWorkContext>();
            var logger = EngineContext.Current.Resolve<ILogger>();

            var kullanıcı = workContext.MevcutKullanıcı;
            logger.Hata(exc.Message, exc, kullanıcı);
        }
        protected virtual void BaşarılıBildirimi(string mesaj, bool sonrakiİstekİçinSürdür = true)
        {
            BildirimEkle(BildirimTipi.Başarılı, mesaj, sonrakiİstekİçinSürdür);
        }
        protected virtual void HataBildirimi(string mesaj, bool sonrakiİstekİçinSürdür = true)
        {
            BildirimEkle(BildirimTipi.Hata, mesaj, sonrakiİstekİçinSürdür);
        }
        protected virtual void HataBildirimi(Exception hata, bool sonrakiİstekİçinSürdür = true, bool logHatası = true)
        {
            if (logHatası)
                LogHatası(hata);
            BildirimEkle(BildirimTipi.Hata, hata.Message, sonrakiİstekİçinSürdür);
        }
        protected virtual void UyarıBildirimi(string mesaj, bool sonrakiİstekİçinSürdür = true)
        {
            BildirimEkle(BildirimTipi.Uyarı, mesaj, sonrakiİstekİçinSürdür);
        }
        protected virtual void BildirimEkle(BildirimTipi tip, string mesaj, bool sonrakiİstekİçinSürdür)
        {
            string dataKey = string.Format("TS.bildirimleri.{0}", tip);
            if (sonrakiİstekİçinSürdür)
            {
                if (TempData[dataKey] == null)
                    TempData[dataKey] = new List<string>();
                ((List<string>)TempData[dataKey]).Add(mesaj);
            }
            else
            {
                if (ViewData[dataKey] == null)
                    ViewData[dataKey] = new List<string>();
                ((List<string>)ViewData[dataKey]).Add(mesaj);
            }
        }
        protected JsonResult KendoGridJsonHatası(string hataMesajı)
        {
            var gridModel = new DataSourceSonucu
            {
                Hatalar = hataMesajı
            };

            return Json(gridModel);
        }
        protected virtual void DüzenleLinkiniGörüntüle(string düzenleSayfaUrlsi)
        {
            var sayfaHeadOluşturucu = EngineContext.Current.Resolve<ISayfaHeadOluşturucu>();
            sayfaHeadOluşturucu.DüzenleSayfaURLsiEkle(düzenleSayfaUrlsi);
        }
    }
}
