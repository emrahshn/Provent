using Web;
using Microsoft.Owin;
using Owin;
using Microsoft.AspNet.SignalR;
using Web.Hubs;
using Services.Kongre;
using Microsoft.AspNet.SignalR.Hubs;
using Autofac.Integration.Mvc;

[assembly: OwinStartup(typeof(Startup))]

namespace Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var hubConfiguration = new HubConfiguration();
            hubConfiguration.EnableJSONP = true;
            hubConfiguration.EnableDetailedErrors = true;
            app.MapSignalR(hubConfiguration);
        }
    }
}