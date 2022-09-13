using Core.Domain.EkTanımlamalar;

namespace Data.Mapping.EkTanımlamalar.SponsorlukKalemi
{
    public class SponsorlukKalemleriMap : TSVarlıkTipiYapılandırması<SponsorlukKalemleri>
    {
        public SponsorlukKalemleriMap()
        {
            this.ToTable("SponsorlukKalemleri");
            this.HasKey(t => t.Id);
        }
    }
}
