using Core.Domain.KongreTanımlama;
using System.Collections.Generic;

namespace Services.KongreTanımlama
{
    public partial interface IKongreTedarikçiServisi
    {
        void KongreTedarikçiSil(KongreTedarikçi kongreTedarikçi);
        KongreTedarikçi KongreTedarikçiAlId(int kongreTedarikçiId);
        IList<KongreTedarikçi> TümKongreTedarikçiAl();
        void KongreTedarikçiEkle(KongreTedarikçi kongreTedarikçi);
        void KongreTedarikçiGüncelle(KongreTedarikçi kongreTedarikçi);
    }
}
