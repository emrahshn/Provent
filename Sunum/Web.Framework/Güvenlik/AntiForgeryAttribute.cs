using Core.Altyapı;
using Core.Data;
using Core.Domain.Güvenlik;
using System;
using System.Web.Mvc;

namespace Web.Framework.Güvenlik
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class AntiForgeryAttribute : FilterAttribute, IAuthorizationFilter
    {
        private readonly bool _görmezdenGel;
        public AntiForgeryAttribute(bool görmezdenGel = false)
        {
            this._görmezdenGel = görmezdenGel;
        }
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
                throw new ArgumentNullException("filterContext");
            if (_görmezdenGel)
                return;
            if (filterContext.IsChildAction)
                return;
            if (!String.Equals(filterContext.HttpContext.Request.HttpMethod, "POST", StringComparison.OrdinalIgnoreCase))
                return;
            if (!DataAyarlarıYardımcısı.DatabaseYüklendi())
                return;
            var güvenlikAyarları = EngineContext.Current.Resolve<GüvenlikAyarları>();
            if (!güvenlikAyarları.YöneticiAlanıiçinXsrfKorumasınıEtkinleştir)
                return;
            var doğrulayıcı = new ValidateAntiForgeryTokenAttribute();
            doğrulayıcı.OnAuthorization(filterContext);
        }
    }
}
