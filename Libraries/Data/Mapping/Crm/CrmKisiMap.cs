using Core.Domain.CRM;
namespace Data.Mapping.Crm
{
    public class CrmKisiMap : TSVarlıkTipiYapılandırması<CrmKisi>
    {
        public CrmKisiMap()
        {
            this.ToTable("CrmKisi");
            this.HasKey(t => t.Id);
            this.HasMany(c => c.Gorusmeler)
                .WithMany()
                .Map(m => m.ToTable("CrmKisiGorusme"));
        }
    }
}
