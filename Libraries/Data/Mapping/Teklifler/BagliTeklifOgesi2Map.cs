using Core.Domain.Teklif;

namespace Data.Mapping.Teklifler
{
    public class BagliTeklifOgesi2Map : TSVarlıkTipiYapılandırması<BagliTeklifOgesi2>
    {
        public BagliTeklifOgesi2Map()
        {
            this.ToTable("BagliTeklifOgesi2");
            this.HasKey(t => t.Id);
        }
    }
}
