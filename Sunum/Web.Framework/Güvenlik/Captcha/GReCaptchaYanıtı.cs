using System.Collections.Generic;

namespace Web.Framework.Güvenlik.Captcha
{
    public class GReCaptchaYanıtı
    {
        public bool Uygun { get; set; }
        public List<string> HataKodları { get; set; }

        public GReCaptchaYanıtı()
        {
            HataKodları = new List<string>();
        }
    }
}