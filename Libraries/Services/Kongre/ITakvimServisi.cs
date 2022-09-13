using Core.Domain.Kongre;
using System.Collections.Generic;

namespace Services.Kongre
{
    public partial interface ITakvimServisi
    {
        void TakvimSil(Takvim takvim);
        Takvim TakvimAlId(int takvimId);
        IList<Takvim> TümTakvimAl();
        void TakvimEkle(Takvim takvim);
        void TakvimGüncelle(Takvim takvim);
    }
}
