using Core.Domain.Tanımlamalar;

namespace Data.Mapping.Tanımlamalar
{
    public class HekimlerMap : TSVarlıkTipiYapılandırması<Hekimler>
    {
        public HekimlerMap()
        {
            this.ToTable("Hekimler");
            this.HasKey(t => t.Id);
        }
    }
}
