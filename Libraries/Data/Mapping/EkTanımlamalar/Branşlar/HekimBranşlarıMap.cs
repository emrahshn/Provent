using Core.Domain.EkTanımlamalar;

namespace Data.Mapping.EkTanımlamalar.Branşlar
{
    public class HekimBranşlarıMap : TSVarlıkTipiYapılandırması<HekimBranşları>
    {
        public HekimBranşlarıMap()
        {
            this.ToTable("HekimBranşları");
            this.HasKey(t => t.Id);
        }
    }
}
