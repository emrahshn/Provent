using System.Collections.Generic;
using Core.Altyapı;

namespace Services.Olaylar
{
    public class AbonelikServisi : IAbonelikServisi
    {
        public IList<IMüşteri<T>> AbonelikleriAl<T>()
        {
            return EngineContext.Current.ResolveAll<IMüşteri<T>>();
        }
    }
}
