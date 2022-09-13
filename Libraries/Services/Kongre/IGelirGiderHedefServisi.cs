using Core.Domain.Kongre;
using System.Collections.Generic;

namespace Services.Kongre
{
    public partial interface IGelirGiderHedefiServisi
    {
        void GelirGiderHedefiSil(GelirGiderHedefi gelirGiderHedefi);
        GelirGiderHedefi GelirGiderHedefiAlId(int gelirGiderHedefiId);
        IList<GelirGiderHedefi> TümGelirGiderHedefiAl();
        void GelirGiderHedefiEkle(GelirGiderHedefi gelirGiderHedefi);
        void GelirGiderHedefiGüncelle(GelirGiderHedefi gelirGiderHedefi);
    }

}
