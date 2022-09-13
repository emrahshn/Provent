using Core.Domain.CRM;

namespace Data.Mapping.Crm
{
    public class CrmKongreMap : TSVarlıkTipiYapılandırması<CrmKongre>
    {
        public CrmKongreMap()
        {
            this.ToTable("CrmKongre");
            this.HasKey(t => t.Id);
        }
    }

}
