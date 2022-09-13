using Core.Domain.CRM;

namespace Data.Mapping.Crm
{
    public class CrmGorusmeMap : TSVarlıkTipiYapılandırması<CrmGorusme>
    {
        public CrmGorusmeMap()
        {
            this.ToTable("CrmGorusme");
            this.HasKey(t => t.Id);
        }
    }
}
