using Autofac;
using Autofac.Core;
using Core.Önbellek;
using Core.Yapılandırma;
using Core.Altyapı;
using Core.Altyapı.BağımlılıkYönetimi;
using Eklentiler.Widgets.NivoSlider.Controllers;

namespace Eklentiler.Widgets.NivoSlider.Altyapı
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder, Config config)
        {
            //we cache presentation models between requests
            builder.RegisterType<WidgetsNivoSliderController>()
                .WithParameter(ResolvedParameter.ForNamed<IÖnbellekYönetici>("ts_cache_static"));
        }
        public int Order
        {
            get { return 2; }
        }
    }
}
