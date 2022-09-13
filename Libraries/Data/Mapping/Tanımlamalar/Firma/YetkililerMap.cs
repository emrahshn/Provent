using Core.Domain.Tanımlamalar;

namespace Data.Mapping.Tanımlamalar
{
    public class YetkililerMap : TSVarlıkTipiYapılandırması<Yetkililer>
    {
        public YetkililerMap()
        {
            this.ToTable("Yetkililer");
            this.HasKey(t => t.Id);
        }
    }
}
