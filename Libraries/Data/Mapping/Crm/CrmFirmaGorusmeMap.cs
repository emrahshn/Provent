using Core.Domain.CRM;

namespace Data.Mapping.Crm
{
    public class CrmFirmaGorusmeMap : TSVarlıkTipiYapılandırması<CrmFirmaGorusme>
    {
        public CrmFirmaGorusmeMap()
        {
            this.ToTable("CrmFirmaGorusme");
            this.HasKey(t => t.Id);
        }
    }

}
