using Core.Domain.EkTanımlamalar;
using System.Collections.Generic;
namespace Services.EkTanımlamalar
{
    public partial interface IGelirGiderTanımlamaServisi
    {
        void GelirGiderTanımlamaSil(GelirGiderTanımlama gelirGiderTanımlama);
        GelirGiderTanımlama GelirGiderTanımlamaAlId(int gelirGiderTanımlamaId);
        IList<GelirGiderTanımlama> TümGelirGiderTanımlamaAl();
        void GelirGiderTanımlamaEkle(GelirGiderTanımlama gelirGiderTanımlama);
        void GelirGiderTanımlamaGüncelle(GelirGiderTanımlama gelirGiderTanımlama);
        IList<GelirGiderTanımlama> AnaTeklifKalemleriAl(bool gelir);
    }
}
