using Core.Domain.EkTanımlamalar;
using System.Collections.Generic;

namespace Services.EkTanımlamalar
{
    public partial interface IKayıtTipiServisi
    {
        void KayıtTipiSil(KayıtTipi kayıtTipi);
        KayıtTipi KayıtTipiAlId(int kayıtTipiId);
        IList<KayıtTipi> TümKayıtTipiAl();
        void KayıtTipiEkle(KayıtTipi kayıtTipi);
        void KayıtTipiGüncelle(KayıtTipi kayıtTipi);
    }

}
