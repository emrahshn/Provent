using Core.Domain.CRM;

namespace Data.Mapping.Crm
{
    public class CrmKurumMap : TSVarlıkTipiYapılandırması<CrmKurum>
    {
        public CrmKurumMap()
        {
            this.ToTable("CrmKurum");
            this.HasKey(t => t.Id);
            this.HasMany(c => c.Kongreler)
                .WithMany()
                .Map(m => m.ToTable("CrmKurumKongre"));
        }
    }
}
