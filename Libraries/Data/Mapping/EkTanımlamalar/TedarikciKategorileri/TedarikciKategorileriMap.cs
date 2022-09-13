using Core.Domain.EkTanımlamalar;

namespace Data.Mapping.EkTanımlamalar
{
    public class TedarikciKategorileriMap : TSVarlıkTipiYapılandırması<TedarikciKategorileri>
    {
        public TedarikciKategorileriMap()
        {
            this.ToTable("TedarikciKategorileri");
            this.HasKey(t => t.Id);
        }
    }

}
