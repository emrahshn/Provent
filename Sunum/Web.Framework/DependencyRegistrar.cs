using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using Autofac;
using Autofac.Builder;
using Autofac.Core;
using Autofac.Integration.Mvc;
using Core;
using Core.Önbellek;
using Core.Yapılandırma;
using Core.Data;
using Core.Fakes;
using Core.Altyapı;
using Core.Altyapı.BağımlılıkYönetimi;
using Web.Framework.Mvc.Routes;
//using Services.Tasks;
using Data;
using Services.Siteler;
using Services.Olaylar;
using Services.Yardımcılar;
using Services.Kullanıcılar;
using Services.Genel;
using Services.Yapılandırma;
using Services.Cms;
using Web.Framework.Temalar;
using Core.Eklentiler;
using Web.Framework.UI;
using Services.Medya;
using Services.Logging;
using Services.KimlikDoğrulama;
using Services.Güvenlik;
using Services.Katalog;
using Services.Sayfalar;
using Services.Mesajlar;
using Services.Klasör;
using Services.Yetkilendirme.Harici;
using Services.Forumlar;
using Services.Seo;
using Services.Haberler;
using Services.Blog;
using Services.Anketler;
using Services.EkTanımlamalar;
using Services.Konum;
using Services.Tanımlamalar;
using Services.Teklifler;
using Services.Görüşmeler;
using Services.Finans;
using Services.Hint;
using Services.Kongre;
using Services.KongreTanımlama;
using Services.Notlar;
using Services.DovizServisi;
using Services.Testler;
using Microsoft.AspNet.SignalR;
using Services.Crm;

namespace Web.Framework
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder, TSConfig config)
        {

            //HTTP context and other related stuff
            builder.Register(c =>
                //register FakeHttpContext when HttpContext is not available
                HttpContext.Current != null ?
                (new HttpContextWrapper(HttpContext.Current) as HttpContextBase) :
                (new FakeHttpContext("~/") as HttpContextBase))
                .As<HttpContextBase>()
                .InstancePerLifetimeScope();
            builder.Register(c => c.Resolve<HttpContextBase>().Request)
                .As<HttpRequestBase>()
                .InstancePerLifetimeScope();
            builder.Register(c => c.Resolve<HttpContextBase>().Response)
                .As<HttpResponseBase>()
                .InstancePerLifetimeScope();
            builder.Register(c => c.Resolve<HttpContextBase>().Server)
                .As<HttpServerUtilityBase>()
                .InstancePerLifetimeScope();
            builder.Register(c => c.Resolve<HttpContextBase>().Session)
                .As<HttpSessionStateBase>()
                .InstancePerLifetimeScope();

            //web helper
            builder.RegisterType<WebYardımcısı>().As<IWebYardımcısı>().InstancePerLifetimeScope();
            //user agent helper
            builder.RegisterType<KullanıcıAracıYardımcısı>().As<IKullanıcıAracıYardımcısı>().InstancePerLifetimeScope();


            //controllers
            builder.RegisterControllers(typeFinder.GetAssemblies().ToArray());

            //data layer
            var dataSettingsManager = new DataAyarlarıYönetici();
            var dataProviderSettings = dataSettingsManager.AyarlarıYükle();
            builder.Register(c => dataSettingsManager.AyarlarıYükle()).As<DataAyarları>();
            builder.Register(x => new EfDataSağlayıcıYöneticisi(x.Resolve<DataAyarları>())).As<TemelVeriSağlayıcıYöneticisi>().InstancePerDependency();


            builder.Register(x => x.Resolve<TemelVeriSağlayıcıYöneticisi>().DataSağlayıcıYükle()).As<IDataSağlayıcı>().InstancePerDependency();

            if (dataProviderSettings != null && dataProviderSettings.Geçerli())
            {
                var efDataProviderManager = new EfDataSağlayıcıYöneticisi(dataSettingsManager.AyarlarıYükle());
                var dataProvider = efDataProviderManager.DataSağlayıcıYükle();
                dataProvider.BağlantıFabrikasıBaşlat();

                builder.Register<IDbContext>(c => new TSObjectContext(dataProviderSettings.DataConnectionString)).InstancePerLifetimeScope();
            }
            else
            {
                builder.Register<IDbContext>(c => new TSObjectContext(dataSettingsManager.AyarlarıYükle().DataConnectionString)).InstancePerLifetimeScope();
            }
            builder.RegisterGeneric(typeof(EfDepo<>)).As(typeof(IDepo<>)).InstancePerLifetimeScope();

            //plugins
            builder.RegisterType<EklentiBulucu>().As<IEklentiBulucu>().InstancePerLifetimeScope();
            

            //cache managers
            if (config.RedisCachingEnabled)
            {
                builder.RegisterType<RedisConnectionWrapper>().As<IRedisConnectionWrapper>().SingleInstance();
                builder.RegisterType<RedisÖnbellekYönetici>().As<IÖnbellekYönetici>().Named<IÖnbellekYönetici>("ts_cache_static").InstancePerLifetimeScope();
            }
            else
            {
                builder.RegisterType<BellekÖnbellekYönetici>().As<IÖnbellekYönetici>().Named<IÖnbellekYönetici>("ts_cache_static").SingleInstance();
            }
            builder.RegisterType<İstekBaşınaÖnbellekYöneticisi>().As<IÖnbellekYönetici>().Named<IÖnbellekYönetici>("ts_cache_per_request").InstancePerLifetimeScope();

            if (config.RunOnAzureWebApps)
            {
                //builder.RegisterType<AzureWebAppsMachineNameProvider>().As<IMachineNameProvider>().SingleInstance();
            }
            else
            {
                //builder.RegisterType<DefaultMachineNameProvider>().As<IMachineNameProvider>().SingleInstance();
            }

            //work context
            builder.RegisterType<WebWorkContext>().As<IWorkContext>().InstancePerLifetimeScope();
            //store context
            builder.RegisterType<WebSiteContext>().As<ISiteContext>().InstancePerLifetimeScope();
            builder.RegisterType<GenelÖznitelikServisi>().As<IGenelÖznitelikServisi>().InstancePerLifetimeScope();
            builder.RegisterType<KullanıcıServisi>().As<IKullanıcıServisi>().InstancePerLifetimeScope();
            builder.RegisterType<SiteServisi>().As<ISiteServisi>().InstancePerLifetimeScope();

            //use static cache (between HTTP requests)
            builder.RegisterType<AyarlarServisi>().As<IAyarlarServisi>()
                .WithParameter(ResolvedParameter.ForNamed<IÖnbellekYönetici>("ts_cache_static"))
                .InstancePerLifetimeScope();
            builder.RegisterSource(new SettingsSource());
            builder.RegisterType<ResimServisi>().As<IResimServisi>().InstancePerLifetimeScope();
            
            builder.RegisterType<VarsayılanLogger>().As<ILogger>().InstancePerLifetimeScope();
            
            //use static cache (between HTTP requests)
            
            builder.RegisterType<WidgetServisi>().As<IWidgetServisi>().InstancePerLifetimeScope();
            builder.RegisterType<SayfaHeadOluşturucu>().As<ISayfaHeadOluşturucu>().InstancePerLifetimeScope();
            builder.RegisterType<TemaSağlayıcı>().As<ITemaSağlayıcı>().InstancePerLifetimeScope();
            builder.RegisterType<TemaContext>().As<ITemaContext>().InstancePerLifetimeScope();
            builder.RegisterType<RotaYayınlayıcı>().As<IRotaYayınlayıcı>().SingleInstance();
            //builder.RegisterType<KullanıcıServisi>().As<IKullanıcıServisi>().InstancePerLifetimeScope();
            builder.RegisterType<KullanıcıKayıtServisi>().As<IKullanıcıKayıtServisi>().InstancePerLifetimeScope();
            builder.RegisterType<FormKimlikDoğrulamaServisi>().As<IKimlikDoğrulamaServisi>().InstancePerLifetimeScope();
            builder.RegisterType<ŞifrelemeServisi>().As<IŞifrelemeServisi>().InstancePerLifetimeScope();
            //Register event consumers
            var consumers = typeFinder.FindClassesOfType(typeof(IMüşteri<>)).ToList();
            foreach (var consumer in consumers)
            {
                builder.RegisterType(consumer)
                    .As(consumer.FindInterfaces((type, criteria) =>
                    {
                        var isMatch = type.IsGenericType && ((Type)criteria).IsAssignableFrom(type.GetGenericTypeDefinition());
                        return isMatch;
                    }, typeof(IMüşteri<>)))
                    .InstancePerLifetimeScope();
            }
            builder.RegisterType<OlayYayınlayıcı>().As<IOlayYayınlayıcı>().SingleInstance();
            builder.RegisterType<AbonelikServisi>().As<IAbonelikServisi>().SingleInstance();
            builder.RegisterType<İzinServisi>().As<IİzinServisi>()
                .WithParameter(ResolvedParameter.ForNamed<IÖnbellekYönetici>("cache_static"))
                .InstancePerLifetimeScope();
            builder.RegisterType<KategoriServisi>().As<IKategoriServisi>().InstancePerLifetimeScope();
            builder.RegisterType<SayfalarServisi>().As<ISayfalarServisi>().InstancePerLifetimeScope();
            builder.RegisterType<BankalarServisi>().As<IBankalarServisi>().InstancePerLifetimeScope();
            builder.RegisterType<MusteriSektorServisi>().As<IMusteriSektorServisi>().InstancePerLifetimeScope();
            builder.RegisterType<TedarikciSektorServisi>().As<ITedarikciSektorServisi>().InstancePerLifetimeScope();
            builder.RegisterType<HariciSektorServisi>().As<IHariciSektorServisi>().InstancePerLifetimeScope();
            builder.RegisterType<TeklifKalemiServisi>().As<ITeklifKalemiServisi>().InstancePerLifetimeScope();
            builder.RegisterType<UnvanlarServisi>().As<IUnvanlarServisi>().InstancePerLifetimeScope();
            builder.RegisterType<KonumServisi>().As<IKonumServisi>().InstancePerLifetimeScope();
            builder.RegisterType<TeklifServisi>().As<ITeklifServisi>().InstancePerLifetimeScope();
            builder.RegisterType<Teklif2Servisi>().As<ITeklif2Servisi>().InstancePerLifetimeScope();
            builder.RegisterType<BagliTeklifOgesiServisi>().As<IBagliTeklifOgesiServisi>().InstancePerLifetimeScope();
            builder.RegisterType<BagliTeklifOgesi2Servisi>().As<IBagliTeklifOgesi2Servisi>().InstancePerLifetimeScope();
            builder.RegisterType<TeklifHariciServisi>().As<ITeklifHariciServisi>().InstancePerLifetimeScope();
            builder.RegisterType<BagliTeklifOgesiHariciServisi>().As<IBagliTeklifOgesiHariciServisi>().InstancePerLifetimeScope();
            builder.RegisterType<GorusmeRaporlariServisi>().As<IGorusmeRaporlariServisi>().InstancePerLifetimeScope();
            builder.RegisterType<OdemeFormuServisi>().As<IOdemeFormuServisi>().InstancePerLifetimeScope();
            builder.RegisterType<HintServisi>().As<IHintServisi>().InstancePerLifetimeScope();
            builder.RegisterType<PdfServisi>().As<IPdfServisi>().InstancePerLifetimeScope();
            builder.RegisterType<KatilimciServisi>().As<IKatilimciServisi>().InstancePerLifetimeScope();
            builder.RegisterType<RefakatciServisi>().As<IRefakatciServisi>().InstancePerLifetimeScope();
            builder.RegisterType<KayitServisi>().As<IKayitServisi>().InstancePerLifetimeScope();
            builder.RegisterType<KonaklamaServisi>().As<IKonaklamaServisi>().InstancePerLifetimeScope();
            builder.RegisterType<KursServisi>().As<IKursServisi>().InstancePerLifetimeScope();
            builder.RegisterType<TransferServisi>().As<ITransferServisi>().InstancePerLifetimeScope();
            builder.RegisterType<KongreServisi>().As<IKongreServisi>().InstancePerLifetimeScope();
            builder.RegisterType<NotServisi>().As<INotServisi>().InstancePerLifetimeScope();
            builder.RegisterType<DovizServisi>().As<IDovizServisi>().InstancePerLifetimeScope();
            builder.RegisterType<MesajServisi>().As<IMesajServisi>().InstancePerLifetimeScope();
            builder.RegisterType<MesajlarServisi>().As<IMesajlarServisi>().InstancePerLifetimeScope();
            builder.RegisterType<TestServisi>().As<ITestServisi>().InstancePerLifetimeScope();
            builder.RegisterType<KontenjanServisi>().As<IKontenjanServisi>().InstancePerLifetimeScope();

            builder.RegisterType<CrmGorevServisi>().As<ICrmGorevServisi>().InstancePerLifetimeScope();
            builder.RegisterType<CrmUnvanServisi>().As<ICrmUnvanServisi>().InstancePerLifetimeScope();
            builder.RegisterType<CrmKisiServisi>().As<ICrmKisiServisi>().InstancePerLifetimeScope();
            builder.RegisterType<CrmKurumServisi>().As<ICrmKurumServisi>().InstancePerLifetimeScope();
            builder.RegisterType<CrmKongreServisi>().As<ICrmKongreServisi>().InstancePerLifetimeScope();
            builder.RegisterType<CrmGorusmeServisi>().As<ICrmGorusmeServisi>().InstancePerLifetimeScope();
            builder.RegisterType<CrmFirmaGorusmeServisi>().As<ICrmFirmaGorusmeServisi>().InstancePerLifetimeScope();
            builder.RegisterType<CrmYonetimKuruluServisi>().As<ICrmYonetimKuruluServisi>().InstancePerLifetimeScope();
            builder.RegisterType<CrmFirmaServisi>().As<ICrmFirmaServisi>().InstancePerLifetimeScope();
            builder.RegisterType<CrmFirmaYetkilisiServisi>().As<ICrmFirmaYetkilisiServisi>().InstancePerLifetimeScope();


            builder.RegisterType<BültenAbonelikServisi>().As<IBültenAbonelikServisi>().InstancePerLifetimeScope();
            builder.RegisterType<ÜlkeServisi>().As<IÜlkeServisi>().InstancePerLifetimeScope();
            builder.RegisterType<AçıkYetkilendirmeServisi>().As<IAçıkYetkilendirmeServisi>().InstancePerLifetimeScope();
            builder.RegisterType<TarihYardımcısı>().As<ITarihYardımcısı>().InstancePerLifetimeScope();
            builder.RegisterType<KullanıcıİşlemServisi>().As<IKullanıcıİşlemServisi>()
                .WithParameter(ResolvedParameter.ForNamed<IÖnbellekYönetici>("cache_static"))
                .InstancePerLifetimeScope();
            builder.RegisterType<MesajTemasıServisi>().As<IMesajTemasıServisi>().InstancePerLifetimeScope();
            builder.RegisterType<MesajServisi>().As<IMesajServisi>().InstancePerLifetimeScope();
            builder.RegisterType<EmailHesapServisi>().As<IEmailHesapServisi>().InstancePerLifetimeScope();
            builder.RegisterType<BekleyenMailServisi>().As<IBekleyenMailServisi>().InstancePerLifetimeScope();
            builder.RegisterType<ForumServisi>().As<IForumServisi>().InstancePerLifetimeScope();
            builder.RegisterType<UrlKayıtServisi>().As<IUrlKayıtServisi>()
               .WithParameter(ResolvedParameter.ForNamed<IÖnbellekYönetici>("cache_static"))
               .InstancePerLifetimeScope();
            builder.RegisterType<AclServisi>().As<IAclServisi>()
                .WithParameter(ResolvedParameter.ForNamed<IÖnbellekYönetici>("cache_static"))
                .InstancePerLifetimeScope();
            builder.RegisterType<SiteMappingServisi>().As<ISiteMappingServisi>()
                .WithParameter(ResolvedParameter.ForNamed<IÖnbellekYönetici>("cache_static"))
                .InstancePerLifetimeScope();
            builder.RegisterType<SayfaTemaServisi>().As<ISayfaTemaServisi>().InstancePerLifetimeScope();
            builder.RegisterType<HaberServisi>().As<IHaberServisi>().InstancePerLifetimeScope();
            builder.RegisterType<BlogServisi>().As<IBlogServisi>().InstancePerLifetimeScope();
            builder.RegisterType<AnketServisi>().As<IAnketServisi>().InstancePerLifetimeScope();
            builder.RegisterType<TamMetinServisi>().As<ITamMetinServisi>().InstancePerLifetimeScope();
            builder.RegisterType<EmailGönderici>().As<IEmailGönderici>().InstancePerLifetimeScope();
            builder.RegisterType<DownloadServisi>().As<IDownloadServisi>().InstancePerLifetimeScope();
            builder.RegisterType<XlsDosyaServisi>().As<IXlsDosyaServisi>().InstancePerLifetimeScope();
            builder.RegisterType<XlsServisi>().As<IXlsServisi>().InstancePerLifetimeScope();
            builder.RegisterType<XlsUploadServisi>().As<IXlsUploadServisi>().InstancePerLifetimeScope();
            builder.RegisterType<BankaBilgileriServisi>().As<IBankaBilgileriServisi>().InstancePerLifetimeScope();
            builder.RegisterType<GelirGiderHedefiServisi>().As<IGelirGiderHedefiServisi>().InstancePerLifetimeScope();
            builder.RegisterType<KontenjanBilgileriServisi>().As<IKontenjanBilgileriServisi>().InstancePerLifetimeScope();
            builder.RegisterType<TakvimServisi>().As<ITakvimServisi>().InstancePerLifetimeScope();
            builder.RegisterType<GelirGiderTanımlamaServisi>().As<IGelirGiderTanımlamaServisi>().InstancePerLifetimeScope();
            builder.RegisterType<SponsorlukKalemleriServisi>().As<ISponsorlukKalemleriServisi>().InstancePerLifetimeScope();
            builder.RegisterType<HekimBranşlarıServisi>().As<IHekimBranşlarıServisi>().InstancePerLifetimeScope();
            builder.RegisterType<HekimlerServisi>().As<IHekimlerServisi>().InstancePerLifetimeScope();
            builder.RegisterType<TedarikciKategorileriServisi>().As<ITedarikciKategorileriServisi>().InstancePerLifetimeScope();
            builder.RegisterType<YetkililerServisi>().As<IYetkililerServisi>().InstancePerLifetimeScope();
            builder.RegisterType<FirmaServisi>().As<IFirmaServisi>().InstancePerLifetimeScope();
            builder.RegisterType<FirmaKategorisiServisi>().As<IFirmaKategorisiServisi>().InstancePerLifetimeScope();
            builder.RegisterType<KongreTedarikçiServisi>().As<IKongreTedarikçiServisi>().InstancePerLifetimeScope();
            builder.RegisterType<KongreGörüşmeRaporlarıServisi>().As<IKongreGörüşmeRaporlarıServisi>().InstancePerLifetimeScope();
            builder.RegisterType<SponsorlukSatışıServisi>().As<ISponsorlukSatışıServisi>().InstancePerLifetimeScope();
            builder.RegisterType<KayıtTipiServisi>().As<IKayıtTipiServisi>().InstancePerLifetimeScope();
            builder.RegisterType<KayıtBilgileriServisi>().As<IKayıtBilgileriServisi>().InstancePerLifetimeScope();
            builder.RegisterType<KursBilgileriServisi>().As<IKursBilgileriServisi>().InstancePerLifetimeScope();
            builder.RegisterType<GenelSponsorlukServisi>().As<IGenelSponsorlukServisi>().InstancePerLifetimeScope();
            builder.RegisterType<TransferServisi>().As<ITransferServisi>().InstancePerLifetimeScope();
            

        }

        public int Order
        {
            get { return 0; }
        }
        

    }


    public class SettingsSource : IRegistrationSource
    {
        static readonly MethodInfo BuildMethod = typeof(SettingsSource).GetMethod(
            "BuildRegistration",
            BindingFlags.Static | BindingFlags.NonPublic);

        public IEnumerable<IComponentRegistration> RegistrationsFor(
                Service service,
                Func<Service, IEnumerable<IComponentRegistration>> registrations)
        {
            var ts = service as TypedService;
            if (ts != null && typeof(IAyarlar).IsAssignableFrom(ts.ServiceType))
            {
                var buildMethod = BuildMethod.MakeGenericMethod(ts.ServiceType);
                yield return (IComponentRegistration)buildMethod.Invoke(null, null);
            }
        }
        
        static IComponentRegistration BuildRegistration<TSettings>() where TSettings : IAyarlar, new()
        {
            return RegistrationBuilder
                .ForDelegate((c, p) =>
                {
                    var mevcutSiteId = c.Resolve<ISiteContext>().MevcutSite.Id;
                    //uncomment the code below if you want load settings per store only when you have two stores installed.
                    //var currentStoreId = c.Resolve<IStoreService>().GetAllStores().Count > 1
                    //    c.Resolve<IStoreContext>().CurrentStore.Id : 0;

                    //although it's better to connect to your database and execute the following SQL:
                    //DELETE FROM [Setting] WHERE [StoreId] > 0
                    return c.Resolve<IAyarlarServisi>().AyarYükle<TSettings>(mevcutSiteId);
                })
                .InstancePerLifetimeScope()
                .CreateRegistration();
        }
        
        public bool IsAdapterForIndividualComponents { get { return false; } }
    }
}
