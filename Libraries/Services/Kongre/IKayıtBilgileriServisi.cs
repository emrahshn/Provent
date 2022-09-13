using Core.Domain.Kongre;
using System.Collections.Generic;

namespace Services.Kongre
{
    public partial interface IKayıtBilgileriServisi
    {
        void KayıtBilgileriSil(KayıtBilgileri kayıtBilgileri);
        KayıtBilgileri KayıtBilgileriAlId(int kayıtBilgileriId);
        IList<KayıtBilgileri> TümKayıtBilgileriAl();
        void KayıtBilgileriEkle(KayıtBilgileri kayıtBilgileri);
        void KayıtBilgileriGüncelle(KayıtBilgileri kayıtBilgileri);
    }

}
