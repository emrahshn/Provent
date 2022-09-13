using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Routing;

namespace Web.Framework.Menu
{
    public class SiteHaritasıNode
    {
        public SiteHaritasıNode()
        {
            RouteValues = new RouteValueDictionary();
            ChildNodes = new List<SiteHaritasıNode>();
        }
        public string SistemAdı { get; set; }
        public string Başlık { get; set; }
        public string ControllerAdı { get; set; }
        public string ActionAdı { get; set; }
        public RouteValueDictionary RouteValues { get; set; }
        public List<SiteHaritasıNode> ChildNodes { get; set; }
        public string Url { get; set; }
        public string IconClass { get; set; }
        public bool Visible { get; set; }
        public bool YeniSekmedeAç { get; set; }
    }
}
