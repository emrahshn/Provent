using Core.Domain.Kongre;

namespace Data.Mapping.Kongre
{
    public class KontenjanBilgileriMap : TSVarlıkTipiYapılandırması<KontenjanBilgileri>
    {
        public KontenjanBilgileriMap()
        {
            this.ToTable("KontenjanBilgileri");
            this.HasKey(t => t.Id);
        }
    }
}
