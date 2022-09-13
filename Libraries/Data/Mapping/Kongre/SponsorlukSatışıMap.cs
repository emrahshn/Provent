using Core.Domain.Kongre;

namespace Data.Mapping.Kongre
{
    public class SponsorlukSatışıMap : TSVarlıkTipiYapılandırması<SponsorlukSatışı>
    {
        public SponsorlukSatışıMap()
        {
            this.ToTable("SponsorlukSatışı");
            this.HasKey(t => t.Id);
        }
    }

}
