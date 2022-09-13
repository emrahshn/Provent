using Core.Domain.EkTanımlamalar;
using System.Collections.Generic;

namespace Services.EkTanımlamalar
{
    public partial interface IHekimBranşlarıServisi
    {
        void HekimBranşlarıSil(HekimBranşları hekimBranşları);
        HekimBranşları HekimBranşlarıAlId(int hekimBranşlarıId);
        IList<HekimBranşları> TümHekimBranşlarıAl();
        void HekimBranşlarıEkle(HekimBranşları hekimBranşları);
        void HekimBranşlarıGüncelle(HekimBranşları hekimBranşları);
    }

}
