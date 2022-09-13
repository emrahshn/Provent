using Core.Domain.EkTanımlamalar;
using System.Collections.Generic;

namespace Services.EkTanımlamalar
{
    public partial interface ISponsorlukKalemleriServisi
    {
        void SponsorlukKalemleriSil(SponsorlukKalemleri sponsorlukKalemleri);
        SponsorlukKalemleri SponsorlukKalemleriAlId(int sponsorlukKalemleriId);
        IList<SponsorlukKalemleri> TümSponsorlukKalemleriAl();
        void SponsorlukKalemleriEkle(SponsorlukKalemleri sponsorlukKalemleri);
        void SponsorlukKalemleriGüncelle(SponsorlukKalemleri sponsorlukKalemleri);
    }

}
