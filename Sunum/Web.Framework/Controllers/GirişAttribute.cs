using Core.Altyapı;
using Services.Güvenlik;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Web.Framework.Controllers
{
    [AttributeUsage(AttributeTargets.Method|AttributeTargets.Class,Inherited =true,AllowMultiple =true)]
    public class GirişAttribute : FilterAttribute, IAuthorizationFilter
    {
        private readonly bool _doğrulama;
        public GirişAttribute(): this(false) { }

        public GirişAttribute(bool doğrulama)
        {
            this._doğrulama = doğrulama;
        }
        private void GirişYapılmamışİsteğiEleAl(AuthorizationContext filterContext)
        {
            filterContext.Result = new HttpUnauthorizedResult();
        }
        private IEnumerable<GirişAttribute> AdminGirişAttributeAl(ActionDescriptor descriptor)
        {
            return descriptor.GetCustomAttributes(typeof(GirişAttribute), true)
                .Concat(descriptor.ControllerDescriptor.GetCustomAttributes(typeof(GirişAttribute), true))
                .OfType<GirişAttribute>();
        }
        private bool AdminSayfasıİstendi(AuthorizationContext filterContext)
        {
            var adminAttributes = AdminGirişAttributeAl(filterContext.ActionDescriptor);
            if(adminAttributes !=null && adminAttributes.Any())
            {
                return true;
            }
            return false;
        }
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (_doğrulama)
                return;
            if (filterContext == null)
                throw new ArgumentNullException("filterContext");
            if (OutputCacheAttribute.IsChildActionCacheActive(filterContext))
                throw new InvalidOperationException("Bir alt eylem önbelleği etkin olduğunda [AdminGiriş] özniteliğini kullanamazsınız.");
            if(AdminSayfasıİstendi(filterContext))
            {
                if (!this.AdminErişimi())
                    this.GirişYapılmamışİsteğiEleAl(filterContext);
            }

        }
        public virtual bool AdminErişimi()
        {
            var izinServisi = EngineContext.Current.Resolve<IİzinServisi>();
            bool sonuç = izinServisi.YetkiVer(StandartİzinSağlayıcı.YöneticiBölgesiErişimi);
            return sonuç;
        }
    }
}
