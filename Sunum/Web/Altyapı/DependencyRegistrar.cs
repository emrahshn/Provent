using Autofac;
using Core.Yapılandırma;
using Core.Domain;
using Core.Altyapı;
using Core.Altyapı.BağımlılıkYönetimi;
using Web.Framework.Mvc.Routes;
using Services.Medya;
using Web.Fabrika;
using Web.Hubs;
using Microsoft.AspNet.SignalR;
using Autofac.Integration.SignalR;
using System.Reflection;
using Microsoft.AspNet.SignalR.Hubs;
using Web.Controllers;
using Autofac.Core;
using SignalR.Extras.Autofac;
using Services.Kongre;

namespace Web.Altyapı
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder, TSConfig config)
        {
            //installation localization service
            builder.RegisterType<RotaYayınlayıcı>().As<IRotaYayınlayıcı>().SingleInstance();
            builder.RegisterType<SiteBilgiAyarları>().As<IAyarlar>();
            builder.RegisterType<ResimServisi>().As<IResimServisi>();
            builder.RegisterType<KullanıcıModelFabrikası>().As<IKullanıcıModelFabrikası>()
               .InstancePerLifetimeScope();
            builder.RegisterType<YetkiliModelFabrikası>().As<IYetkiliModelFabrikası>()
               .InstancePerLifetimeScope();
            
            builder.RegisterLifetimeHubManager();
            builder.RegisterHubs(Assembly.GetExecutingAssembly()); 
            builder.RegisterType<KatılımcıHub>().ExternallyOwned();
        }
       
        public int Order
        {
            get { return 2; }
        }
      
    }
}