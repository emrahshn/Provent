using Core.Domain.Tanımlamalar;

namespace Data.Mapping.Tanımlamalar
{
    public class FirmaMap : TSVarlıkTipiYapılandırması<Firma>
    {
        public FirmaMap()
        {
            this.ToTable("Firma");
            this.HasKey(t => t.Id);
            this.HasMany(c => c.Yetkililer)
                .WithMany()
                .Map(m => m.ToTable("FirmaYetkililer"));
        }
    }
}
