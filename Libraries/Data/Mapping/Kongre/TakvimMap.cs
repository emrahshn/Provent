using Core.Domain.Kongre;

namespace Data.Mapping.Kongre
{
    public class TakvimMap : TSVarlıkTipiYapılandırması<Takvim>
    {
        public TakvimMap()
        {
            this.ToTable("Takvim");
            this.HasKey(t => t.Id);

        }
    }
}
