using Core.Eklentiler;
using System.Web.Routing;

namespace Services.Yetkilendirme.Harici
{
    public partial interface IHariciYetkilendirmeMetodu : IEklenti
    {
        void YapılandırmaRotasınıAl(out string actionName, out string controllerName, out RouteValueDictionary routeValues);
        void HalkaAçıkRotayıAl(out string actionName, out string controllerName, out RouteValueDictionary routeValues);
    }
}
