using Core.Domain.KongreTanımlama;

namespace Data.Mapping.KongreTanımlamaları
{
    public class KongreTedarikçiMap : TSVarlıkTipiYapılandırması<KongreTedarikçi>
    {
        public KongreTedarikçiMap()
        {
            this.ToTable("KongreTedarikçi");
            this.HasKey(t => t.Id);
        }
    }
}
