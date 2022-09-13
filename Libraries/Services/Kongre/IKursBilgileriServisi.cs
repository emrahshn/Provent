using Core.Domain.Kongre;
using System.Collections.Generic;

namespace Services.Kongre
{
    public partial interface IKursBilgileriServisi
    {
        void KursBilgileriSil(KursBilgileri kursBilgileri);
        KursBilgileri KursBilgileriAlId(int kursBilgileriId);
        IList<KursBilgileri> TümKursBilgileriAl();
        void KursBilgileriEkle(KursBilgileri kursBilgileri);
        void KursBilgileriGüncelle(KursBilgileri kursBilgileri);
    }

}
