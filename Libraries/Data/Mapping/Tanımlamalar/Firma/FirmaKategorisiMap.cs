using Core.Domain.Tanımlamalar;

namespace Data.Mapping.Tanımlamalar
{
    public class FirmaKategorisiMap : TSVarlıkTipiYapılandırması<FirmaKategorisi>
    {
        public FirmaKategorisiMap()
        {
            this.ToTable("FirmaKategorisi");
            this.HasKey(t => t.Id);
        }
    }
}
