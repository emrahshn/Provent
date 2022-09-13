using Core.Domain.Tanımlamalar;
using System.Collections.Generic;

namespace Services.Tanımlamalar
{
    public partial interface IFirmaServisi
    {
        void FirmaSil(Firma firma);
        Firma FirmaAlId(int firmaId);
        IList<Firma> FirmaAlKategoriId(int kategoriId);
        IList<Firma> TümFirmaAl();
        void FirmaEkle(Firma firma);
        void FirmaGüncelle(Firma firma);
    }
}
