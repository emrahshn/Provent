using System.Collections.Generic;
using System.Web.Routing;
using Core.Eklentiler;

namespace Services.Cms
{
    public partial interface IWidgetEklenti : IEklenti
    {
        IList<string> WidgetBölgeleriniAl();
        void YapılandırmaRotasınıAl(out string actionName, out string controllerName, out RouteValueDictionary routeValues);
        void GörüntülemeRotasınıAl(string widgetZone, out string actionName, out string controllerName, out RouteValueDictionary routeValues);
    }
}
