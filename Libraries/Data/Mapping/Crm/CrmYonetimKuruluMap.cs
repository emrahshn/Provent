using Core.Domain.CRM;
namespace Data.Mapping.Crm
{
    public class CrmYonetimKuruluMap : TSVarlıkTipiYapılandırması<CrmYonetimKurulu>
    {
        public CrmYonetimKuruluMap()
        {
            this.ToTable("CrmYonetimKurulu");
            this.HasKey(t => t.Id);
        }
    }
}
