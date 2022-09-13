using Core.Domain.EkTanımlamalar;

namespace Data.Mapping.EkTanımlamalar.GelirGider
{
    public class GelirGiderTanımlamaMap : TSVarlıkTipiYapılandırması<GelirGiderTanımlama>
    {
        public GelirGiderTanımlamaMap()
        {
            this.ToTable("GelirGiderKalemleri");
            this.HasKey(t => t.Id);
        }
    }
}
