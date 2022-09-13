using Core.Domain.Kongre;

namespace Data.Mapping.Kongre
{
    public class KursBilgileriMap : TSVarlıkTipiYapılandırması<KursBilgileri>
    {
        public KursBilgileriMap()
        {
            this.ToTable("KursBilgileri");
            this.HasKey(t => t.Id);
        }
    }
}
