using Core.Domain.CRM;

namespace Data.Mapping.Crm
{
    public class CrmFirmaMap : TSVarlıkTipiYapılandırması<CrmFirma>
    {
        public CrmFirmaMap()
        {
            this.ToTable("CrmFirma");
            this.HasKey(t => t.Id);
            this.HasMany(c => c.Gorusmeler)
                .WithMany()
                .Map(m => m.ToTable("CrmFirmaGorusmeGorusme"));
            this.HasMany(c => c.Yetkililer)
                .WithMany()
                .Map(m => m.ToTable("CrmFirmaFirmaYetkilisi"));
        }
    }
}
