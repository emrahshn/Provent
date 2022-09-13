using System.Collections.Generic;
using System.Web.Mvc;

namespace Web.Framework.Mvc
{
    [ModelBinder(typeof(TSModelBinder))]
    public partial class TemelTSModel
    {
        public TemelTSModel()
        {
            this.CustomProperties = new Dictionary<string, object>();
            PostInitialize();
        }

        public virtual void BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
        }
        protected virtual void PostInitialize()
        {
            
        }
        public Dictionary<string, object> CustomProperties { get; set; }
    }
    public partial class TemelTSEntityModel : TemelTSModel
    {
        public virtual int Id { get; set; }
    }
}