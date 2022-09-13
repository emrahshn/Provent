using Core.Domain.Kongre;

namespace Data.Mapping.Kongre
{
    public class GelirGiderHedefiMap : TSVarlıkTipiYapılandırması<GelirGiderHedefi>
    {
        public GelirGiderHedefiMap()
        {
            this.ToTable("GelirGiderHedefi");
            this.HasKey(t => t.Id);
        }
    }
}
