using Core.Yapılandırma;

namespace Web.Framework.Güvenlik.Captcha
{
    public class CaptchaAyarları : IAyarlar
    {
        public bool Etkin { get; set; }
        public bool GirişSayfasındaGöster { get; set; }
        public bool KayıtSayfasındaGöster { get; set; }
        public bool İletişimeGeçinSayfasındaGöster { get; set; }
        public bool BlogYorumlarındaGöster { get; set; }
        public bool HaberYorumlarındaGöster { get; set; }
        public string ReCaptchaPublicKey { get; set; }
        public string ReCaptchaPrivateKey { get; set; }
        public ReCaptchaSürümü ReCaptchaSürümü { get; set; }
        public string ReCaptchaTeması { get; set; }
    }
}