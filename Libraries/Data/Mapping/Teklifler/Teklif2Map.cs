using Core.Domain.Teklif;

namespace Data.Mapping.Teklifler
{
    public class Teklif2Map : TSVarlıkTipiYapılandırması<Teklif2>
    {
        public Teklif2Map()
        {
            this.ToTable("Teklif2");
            this.HasKey(t => t.Id);
            this.HasMany(c => c.BagliTeklifOgesi2)
                .WithMany()
                .Map(m => m.ToTable("TeklifBagliOgeler"));
        }
    }
}
