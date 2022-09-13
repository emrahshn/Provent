using Core.Domain.Kongre;
using System.Collections.Generic;

namespace Services.Kongre
{
    public partial interface IKongreGörüşmeRaporlarıServisi
    {
        void KongreGörüşmeRaporlarıSil(KongreGörüşmeRaporları kongreGörüşmeRaporları);
        KongreGörüşmeRaporları KongreGörüşmeRaporlarıAlId(int kongreGörüşmeRaporlarıId);
        KongreGörüşmeRaporları KongreGörüşmeRaporlarıKongreId(int kongreId,int sponsorId);
        IList<KongreGörüşmeRaporları> TümKongreGörüşmeRaporlarıAl();
        void KongreGörüşmeRaporlarıEkle(KongreGörüşmeRaporları kongreGörüşmeRaporları);
        void KongreGörüşmeRaporlarıGüncelle(KongreGörüşmeRaporları kongreGörüşmeRaporları);
    }

}
