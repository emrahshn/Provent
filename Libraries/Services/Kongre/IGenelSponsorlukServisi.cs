using Core.Domain.Kongre;
using System.Collections.Generic;

namespace Services.Kongre
{
    public partial interface IGenelSponsorlukServisi
    {
        void GenelSponsorlukSil(GenelSponsorluk genelSponsorluk);
        GenelSponsorluk GenelSponsorlukAlId(int genelSponsorlukId);
        IList<GenelSponsorluk> TümGenelSponsorlukAl();
        void GenelSponsorlukEkle(GenelSponsorluk genelSponsorluk);
        void GenelSponsorlukGüncelle(GenelSponsorluk genelSponsorluk);
    }

}
