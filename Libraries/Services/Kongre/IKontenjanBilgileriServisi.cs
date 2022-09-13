using Core.Domain.Kongre;
using System.Collections.Generic;

namespace Services.Kongre
{
    public partial interface IKontenjanBilgileriServisi
    {
        void KontenjanBilgileriSil(KontenjanBilgileri kontenjanBilgileri);
        KontenjanBilgileri KontenjanBilgileriAlId(int bankaId);
        void KontenjanBilgileriEkle(KontenjanBilgileri kontenjanBilgileri);
        void KontenjanBilgileriGüncelle(KontenjanBilgileri kontenjanBilgileri);
    }
}
