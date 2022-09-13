using Core.Domain.Kongre;

namespace Data.Mapping.Kongre
{
    public class GenelSponsorlukMap : TSVarlıkTipiYapılandırması<GenelSponsorluk>
    {
        public GenelSponsorlukMap()
        {
            this.ToTable("GenelSponsorluk");
            this.HasKey(t => t.Id);
        }
    }

}
