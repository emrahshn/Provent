using Core.Domain.EkTanımlamalar;
using System.Collections.Generic;

namespace Services.EkTanımlamalar
{
    public partial interface ITedarikciKategorileriServisi
    {
        void TedarikciKategorileriSil(TedarikciKategorileri tedarikciKategorileri);
        TedarikciKategorileri TedarikciKategorileriAlId(int tedarikciKategorileriId);
        IList<TedarikciKategorileri> TümTedarikciKategorileriAl();
        void TedarikciKategorileriEkle(TedarikciKategorileri tedarikciKategorileri);
        void TedarikciKategorileriGüncelle(TedarikciKategorileri tedarikciKategorileri);
    }

}
