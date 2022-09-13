using System;
using Core.Yapılandırma;
using Core.Altyapı.BağımlılıkYönetimi;

namespace Core.Altyapı
{
    public interface IEngine
    {
        ContainerManager ContainerManager { get; }
        void Initialize(TSConfig config);
        T Resolve<T>() where T : class;
        T ResolveUnregistered<T>() where T : class;
        object Resolve(Type type);
        T[] ResolveAll<T>();
    }
}
