using System;
using System.Linq;
using System.Web.Mvc;

namespace Web.Framework.Controllers
{
    [AttributeUsage(AttributeTargets.Method,AllowMultiple =true)]
    public class FormAdıParametresiAttribute : FilterAttribute, IActionFilter
    {
        private readonly string _adı;
        private readonly string _actionParametreAdı;
        public FormAdıParametresiAttribute(string adı,string actionParametreAdı)
        {
            this._adı = adı;
            this._actionParametreAdı = actionParametreAdı;
        }
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.ActionParameters[_actionParametreAdı] = filterContext.RequestContext
                .HttpContext.Request.Form.AllKeys.Any(x => x.Equals(_adı));
        }
    }
}
