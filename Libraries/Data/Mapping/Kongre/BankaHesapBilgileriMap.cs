using Core.Domain.Kongre;

namespace Data.Mapping.Kongre
{
    public class BankaHesapBilgileriMap : TSVarlıkTipiYapılandırması<BankaHesapBilgileri>
    {
        public BankaHesapBilgileriMap()
        {
            this.ToTable("BankaHesapBilgileri");
            this.HasKey(t => t.Id);

        }
    }
}
