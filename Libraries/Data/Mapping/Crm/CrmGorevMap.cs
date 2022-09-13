using Core.Domain.CRM;

namespace Data.Mapping.Crm
{
    public class CrmGorevMap : TSVarlıkTipiYapılandırması<CrmGorev>
    {
        public CrmGorevMap()
        {
            this.ToTable("CrmGorev");
            this.HasKey(t => t.Id);
        }
    }
}
