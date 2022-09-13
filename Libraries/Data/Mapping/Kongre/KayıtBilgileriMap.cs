using Core.Domain.Kongre;

namespace Data.Mapping.Kongre
{
    public class KayıtBilgileriMap : TSVarlıkTipiYapılandırması<KayıtBilgileri>
    {
        public KayıtBilgileriMap()
        {
            this.ToTable("KayıtBilgileri");
            this.HasKey(t => t.Id);
        }
    }
}
