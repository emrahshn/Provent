using Core.Domain.CRM;
namespace Data.Mapping.Crm
{
    public class CrmFirmaYetkilisiMap : TSVarlıkTipiYapılandırması<CrmFirmaYetkilisi>
    {
        public CrmFirmaYetkilisiMap()
        {
            this.ToTable("CrmFirmaYetkilisi");
            this.HasKey(t => t.Id);
            this.HasMany(c => c.Gorusmeler)
                .WithMany()
                .Map(m => m.ToTable("CrmFirmaYetkilisiGorusme"));
        }
    }
}
