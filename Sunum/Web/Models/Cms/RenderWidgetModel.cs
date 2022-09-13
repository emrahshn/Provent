using System.Web.Routing;
using Web.Framework.Mvc;

namespace Web.Models.Cms
{
    public partial class RenderWidgetModel : TemelTSModel
    {
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        public RouteValueDictionary RouteValues { get; set; }
    }
}