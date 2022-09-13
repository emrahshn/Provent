using System;
using System.Globalization;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using FluentValidation.Mvc;
using Core;
using Core.Data;
using Core.Domain;
using Core.Domain.Genel;
using Core.Altyapı;
//using Services.Tasks;
using Web.Controllers;
using Web.Framework;
using Web.Framework.Mvc;
using Web.Framework.Mvc.Routes;
using Web.Framework.Temalar;
using StackExchange.Profiling;
using StackExchange.Profiling.Mvc;
using Autofac;
using System.Reflection;
using Autofac.Integration.SignalR;
using Microsoft.AspNet.SignalR;
using SignalR.Extras.Autofac;
using Web.Hubs;

namespace Web
{
    public class MvcApplication : HttpApplication
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("favicon.ico");
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //register custom routes (plugins, etc)
            var routePublisher = EngineContext.Current.Resolve<IRotaYayınlayıcı>();
            routePublisher.RotaKaydet(routes);

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                new[] { "Web.Controllers" }
            );

        }

        protected void Application_Start()
        {
            //Database.SetInitializer<NameOfDbContext>(new DropCreateDatabaseIfModelChanges<NameOfDbContext>());
            //most of API providers require TLS 1.2 nowadays
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            //disable "X-AspNetMvc-Version" header name
            MvcHandler.DisableMvcResponseHeader = true;

            //initialize engine context
            EngineContext.Initialize(false);

            bool databaseInstalled = DataAyarlarıYardımcısı.DatabaseYüklendi();
            if (databaseInstalled)
            {
                //remove all view engines
                ViewEngines.Engines.Clear();
                //except the themeable razor view engine we use
                ViewEngines.Engines.Add(new TemalanabilirRazorGörünümMotoru());
            }

            //Add some functionality on top of the default ModelMetadataProvider
            //ModelMetadataProviders.Current = new NopMetadataProvider();

            //Registering some regular mvc stuff
            AreaRegistration.RegisterAllAreas();
            RegisterRoutes(RouteTable.Routes);

            //fluent validation
            DataAnnotationsModelValidatorProvider.AddImplicitRequiredAttributeForValueTypes = false;
            //ModelValidatorProviders.Providers.Add(new FluentValidationModelValidatorProvider(new NopValidatorFactory()));

            if (databaseInstalled)
            {
                //start scheduled tasks
                /*
                TaskManager.Instance.Initialize();
                TaskManager.Instance.Start();
                
                //miniprofiler
                if (EngineContext.Current.Resolve<StoreInformationSettings>().DisplayMiniProfilerInPublicStore)
                {
                    GlobalFilters.Filters.Add(new ProfilingActionFilter());
                }
                */
                //log application start
                try
                {
                    //log
                    //var logger = EngineContext.Current.Resolve<ILogger>();
                    //logger.Information("Application started", null, null);

                   

                }
                catch (Exception)
                {
                    //don't throw new exception if occurs
                }
            }
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            //ignore static resources
            var webHelper = EngineContext.Current.Resolve<IWebYardımcısı>();
            if (webHelper.SabitKaynak(this.Request))
                return;

            //keep alive page requested (we ignore it to prevent creating a guest customer records)
            string keepAliveUrl = string.Format("{0}keepalive/index", webHelper.SiteKonumuAl());
            if (webHelper.SayfanınUrlsiniAl(false).StartsWith(keepAliveUrl, StringComparison.InvariantCultureIgnoreCase))
                return;

            //ensure database is installed
            if (!DataAyarlarıYardımcısı.DatabaseYüklendi())
            {
                string installUrl = string.Format("{0}install", webHelper.SiteKonumuAl());
                if (!webHelper.SayfanınUrlsiniAl(false).StartsWith(installUrl, StringComparison.InvariantCultureIgnoreCase))
                {
                    this.Response.Redirect(installUrl);
                }
            }

            if (!DataAyarlarıYardımcısı.DatabaseYüklendi())
                return;
            /*
            //miniprofiler
            if (EngineContext.Current.Resolve<SiteBilgiAyarları>().DisplayMiniProfilerInPublicStore)
            {
                MiniProfiler.Start();
                //store a value indicating whether profiler was started
                HttpContext.Current.Items["nop.MiniProfilerStarted"] = true;
            }
            */
        }

        protected void Application_EndRequest(object sender, EventArgs e)
        {
            //miniprofiler
            var miniProfilerStarted = HttpContext.Current.Items.Contains("TS.MiniProfilerStarted") &&
                 Convert.ToBoolean(HttpContext.Current.Items["TS.MiniProfilerStarted"]);
            if (miniProfilerStarted)
            {
                MiniProfiler.Stop();
            }
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            //we don't do it in Application_BeginRequest because a user is not authenticated yet
            SetWorkingCulture();
        }

        protected void Application_Error(Object sender, EventArgs e)
        {
            var exception = Server.GetLastError();

            //log error
            LogException(exception);

            //process 404 HTTP errors
            var httpException = exception as HttpException;
            if (httpException != null && httpException.GetHttpCode() == 404)
            {
                var webHelper = EngineContext.Current.Resolve<IWebYardımcısı>();
                if (!webHelper.SabitKaynak(this.Request))
                {
                    Response.Clear();
                    Server.ClearError();
                    Response.TrySkipIisCustomErrors = true;

                    // Call target Controller and pass the routeData.
                    //IController errorController = EngineContext.Current.Resolve<CommonController>();

                    var routeData = new RouteData();
                    routeData.Values.Add("controller", "Genel");
                    routeData.Values.Add("action", "SayfaBulunamadı");

                    //errorController.Execute(new RequestContext(new HttpContextWrapper(Context), routeData));
                }
            }
        }

        protected void SetWorkingCulture()
        {
            if (!DataAyarlarıYardımcısı.DatabaseYüklendi())
                return;

            //ignore static resources
            var webHelper = EngineContext.Current.Resolve<IWebYardımcısı>();
            if (webHelper.SabitKaynak(this.Request))
                return;

            //keep alive page requested (we ignore it to prevent creation of guest customer records)
            string keepAliveUrl = string.Format("{0}keepalive/index", webHelper.SiteKonumuAl());
            if (webHelper.SayfanınUrlsiniAl(false).StartsWith(keepAliveUrl, StringComparison.InvariantCultureIgnoreCase))
                return;


            if (webHelper.SayfanınUrlsiniAl(false).StartsWith(string.Format("{0}Yönetim", webHelper.SiteKonumuAl()),
                StringComparison.InvariantCultureIgnoreCase))
            {
                //admin area


                //always set culture to 'en-US'
                //we set culture of admin area to 'en-US' because current implementation of Telerik grid 
                //doesn't work well in other cultures
                //e.g., editing decimal value in russian culture
                GenelYardımcı.TelerikKültürAyarla();
            }
            else
            {
                //public store
                /*
                var workContext = EngineContext.Current.Resolve<IWorkContext>();
                var culture = new CultureInfo(workContext.WorkingLanguage.LanguageCulture);
                Thread.CurrentThread.CurrentCulture = culture;
                Thread.CurrentThread.CurrentUICulture = culture;
                */
            }
        }

        protected void LogException(Exception exc)
        {
            if (exc == null)
                return;

            if (!DataAyarlarıYardımcısı.DatabaseYüklendi())
                return;

            //ignore 404 HTTP errors
            var httpException = exc as HttpException;
            if (httpException != null && httpException.GetHttpCode() == 404 &&
                !EngineContext.Current.Resolve<GenelAyarlar>().Günlük404Hataları)
                return;

            try
            {
                //log
                /*
                var logger = EngineContext.Current.Resolve<ILogger>();
                var workContext = EngineContext.Current.Resolve<IWorkContext>();
                logger.Error(exc.Message, exc, workContext.CurrentCustomer);
                */
            }
            catch (Exception)
            {
                //don't throw new exception if occurs
            }
        }
    }
}
