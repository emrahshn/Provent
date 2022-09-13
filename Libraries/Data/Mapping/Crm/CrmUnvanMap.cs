using Core.Domain.CRM;

namespace Data.Mapping.Crm
{
    public class CrmUnvanMap : TSVarlıkTipiYapılandırması<CrmUnvan>
    {
        public CrmUnvanMap()
        {
            this.ToTable("CrmUnvan");
            this.HasKey(t => t.Id);
        }
    }
}
