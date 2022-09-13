using Core.Domain.Tanımlamalar;
using System.Collections.Generic;

namespace Services.Tanımlamalar
{
    public partial interface IFirmaKategorisiServisi
    {
        void FirmaKategorisiSil(FirmaKategorisi firmaKategorisi);
        FirmaKategorisi FirmaKategorisiAlId(int firmaKategorisiId);
        IList<FirmaKategorisi> TümFirmaKategorisiAl();
        void FirmaKategorisiEkle(FirmaKategorisi firmaKategorisi);
        void FirmaKategorisiGüncelle(FirmaKategorisi firmaKategorisi);
    }
}
