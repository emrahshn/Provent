using Core.Domain.Kongre;

namespace Data.Mapping.Kongre
{
    public class KongreMap : TSVarlıkTipiYapılandırması<Kongreler>
    {
        public KongreMap()
        {
            this.ToTable("Kongre");
            this.HasKey(t => t.Id);

            this.HasMany(c => c.BankaHesapBilgileri)
                .WithMany()
                .Map(m => m.ToTable("KongreBankaHesap"));

            this.HasMany(c => c.GelirGiderHedefi)
                .WithMany()
                .Map(m => m.ToTable("KongreGelirGiderHedefi"));

            this.HasMany(c => c.KontenjanBilgileri)
                .WithMany()
                .Map(m => m.ToTable("KongreKontenjanBilgileri"));
            this.HasMany(c => c.KayıtBilgileri)
                .WithMany()
                .Map(m => m.ToTable("KongreKayıtBilgileri"));

            this.HasMany(c => c.KursBilgileri)
                .WithMany()
                .Map(m => m.ToTable("KongreKursBilgileri"));

            this.HasMany(c => c.GenelSponsorluk)
                .WithMany()
                .Map(m => m.ToTable("KongreGenelSponsorluk"));

            this.HasMany(c => c.Transfer)
                .WithMany()
                .Map(m => m.ToTable("KongreTransfer"));

            this.HasMany(c => c.FirmaBilgileri)
                .WithMany()
                .Map(m => m.ToTable("KongreFirmaBilgileri"));

            this.HasMany(c => c.SponsorlukBilgileri)
                .WithMany()
                .Map(m => m.ToTable("KongreSponsorlukBilgileri"));

            this.HasMany(c => c.TakvimBilgileri)
                .WithMany()
                .Map(m => m.ToTable("KongreTakvimi"));

            /*this.HasMany(c => c.GörüşmeRaporları)
                .WithMany()
                .Map(m => m.ToTable("KongreGörüşmeRaporları"));*/

        }
    }

}
