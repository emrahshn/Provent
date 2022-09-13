using Core.Domain.Kongre;
using System.Collections.Generic;

namespace Services.Kongre
{
    public partial interface ISponsorlukSatışıServisi
    {
        void SponsorlukSatışıSil(SponsorlukSatışı sponsorlukSatışı);
        SponsorlukSatışı SponsorlukSatışıAlId(int sponsorlukSatışıId);
        IList<SponsorlukSatışı> SponsorlukSatışıAlKongreId(int kongreId,int sponsorId);
        IList<SponsorlukSatışı> TümSponsorlukSatışıAl();
        void SponsorlukSatışıEkle(SponsorlukSatışı sponsorlukSatışı);
        void SponsorlukSatışıGüncelle(SponsorlukSatışı sponsorlukSatışı);
    }

}
