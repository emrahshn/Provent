using Core.Domain.Tanımlamalar;
using System.Collections.Generic;
using Web.Models.Tanımlamalar;

namespace Web.Fabrika
{
    public partial interface IYetkiliModelFabrikası
    {
        IEnumerable<YetkililerModel> YetkiliModeliHazırla(IEnumerable<Yetkililer> yetkililer);
    }
}