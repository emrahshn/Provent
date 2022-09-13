using Core.Domain.Kongre;

namespace Data.Mapping.Kongre
{
    public class KongreGörüşmeRaporlarıMap : TSVarlıkTipiYapılandırması<KongreGörüşmeRaporları>
    {
        public KongreGörüşmeRaporlarıMap()
        {
            this.ToTable("KongreGörüşmeRaporları");
            this.HasKey(t => t.Id);
        }
    }
}
