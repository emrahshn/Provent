using Core.Altyapı;
using System.Web.Mvc;

namespace Web.Framework.Güvenlik.Captcha
{
    public class CaptchaDoğrulayıcı : ActionFilterAttribute
    {
        private const string ZORUNLU_ALAN_KEY = "recaptcha_challenge_field";
        private const string YANIT_ALAN_KEY = "recaptcha_response_field";
        private const string G_YANIT_ALAN_KEY = "g-recaptcha-response";

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            bool doğrulandı = false;
            var captchaZorunluDeğeri = filterContext.HttpContext.Request.Form[ZORUNLU_ALAN_KEY];
            var captchaYanıtDeğeri = filterContext.HttpContext.Request.Form[YANIT_ALAN_KEY];
            var gCaptchaYanıtDeğeri = filterContext.HttpContext.Request.Form[G_YANIT_ALAN_KEY];
            if ((!string.IsNullOrEmpty(captchaZorunluDeğeri) && !string.IsNullOrEmpty(captchaYanıtDeğeri)) || !string.IsNullOrEmpty(gCaptchaYanıtDeğeri))
            {
                var captchaAyarları = EngineContext.Current.Resolve<CaptchaAyarları>();
                if (captchaAyarları.Etkin)
                {
                    var captchaDoğrulayıcı = new GReCaptchaDoğrulayıcı(captchaAyarları.ReCaptchaSürümü)
                    {
                        SecretKey = captchaAyarları.ReCaptchaPrivateKey,
                        RemoteIp = filterContext.HttpContext.Request.UserHostAddress,
                        Response = captchaYanıtDeğeri ?? gCaptchaYanıtDeğeri,
                        Challenge = captchaZorunluDeğeri
                    };

                    var recaptchaYanıtı = captchaDoğrulayıcı.Doğrula();
                    doğrulandı = recaptchaYanıtı.Uygun;
                }
            }

            //this will push the result value into a parameter in our Action  
            filterContext.ActionParameters["captchaDoğrulandı"] = doğrulandı;

            base.OnActionExecuting(filterContext);
        }
    }
}
