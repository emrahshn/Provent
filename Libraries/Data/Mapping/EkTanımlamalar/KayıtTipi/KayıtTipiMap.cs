using Core.Domain.EkTanımlamalar;

namespace Data.Mapping.EkTanımlamalar.KayıtTipleri
{
    public class KayıtTipiMap : TSVarlıkTipiYapılandırması<KayıtTipi>
    {
        public KayıtTipiMap()
        {
            this.ToTable("KayıtTipi");
            this.HasKey(t => t.Id);
        }
    }
}
