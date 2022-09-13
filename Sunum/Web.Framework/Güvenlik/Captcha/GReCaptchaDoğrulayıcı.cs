using System;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace Web.Framework.Güvenlik.Captcha
{
    public class GReCaptchaDoğrulayıcı
    {
        private const string RECAPTCHA_DOĞRULAMA_URL_SURUM1 = "https://www.google.com/recaptcha/api/verify?privatekey={0}&response={1}&remoteip={2}&challenge={3}";
        private const string RECAPTCHA_DOĞRULAMA_URL_SURUM2 = "https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}&remoteip={2}";

        public string SecretKey { get; set; }
        public string RemoteIp { get; set; }
        public string Response { get; set; }
        public string Challenge { get; set; }

        private readonly ReCaptchaSürümü _sürüm;

        public GReCaptchaDoğrulayıcı(ReCaptchaSürümü sürüm = ReCaptchaSürümü.Sürüm1)
        {
            _sürüm = sürüm;
        }

        public GReCaptchaYanıtı Doğrula()
        {
            GReCaptchaYanıtı sonuç = null;
            var httpClient = new HttpClient();
            var requestUri = string.Empty;

            switch (_sürüm)
            {
                case ReCaptchaSürümü.Sürüm2:
                    requestUri = string.Format(RECAPTCHA_DOĞRULAMA_URL_SURUM2, SecretKey, Response, RemoteIp);
                    break;
                default:
                    requestUri = string.Format(RECAPTCHA_DOĞRULAMA_URL_SURUM1, SecretKey, Response, RemoteIp, Challenge);
                    break;
            }

            try
            {
                var görevSonucu = httpClient.GetAsync(requestUri);
                görevSonucu.Wait();
                var response = görevSonucu.Result;
                response.EnsureSuccessStatusCode();
                var taskString = response.Content.ReadAsStringAsync();
                taskString.Wait();
                sonuç = YanıtSonucunuParçala(taskString.Result);
            }
            catch
            {
                sonuç = new GReCaptchaYanıtı { Uygun = false };
                sonuç.HataKodları.Add("Bilinmeyen hata");
            }
            finally
            {
                httpClient.Dispose();
            }

            return sonuç;
        }

        private GReCaptchaYanıtı YanıtSonucunuParçala(string yanıtString)
        {
            var sonuç = new GReCaptchaYanıtı();

            if (_sürüm == ReCaptchaSürümü.Sürüm1)
            {
                var sonuçParçası = yanıtString.Split('\n');
                sonuç.Uygun = sonuçParçası.Contains("true");
                if (!sonuç.Uygun)
                    sonuç.HataKodları.AddRange(sonuçParçası.Where(r => !r.Equals("false", StringComparison.InvariantCultureIgnoreCase)));
            }
            else if (_sürüm == ReCaptchaSürümü.Sürüm2)
            {
                var sonuçParçası = JObject.Parse(yanıtString);
                sonuç.Uygun = sonuçParçası.Value<bool>("success");
                if (sonuçParçası.Value<JToken>("error-codes") != null &&
                        sonuçParçası.Value<JToken>("error-codes").Values<string>().Any())
                    sonuç.HataKodları = sonuçParçası.Value<JToken>("error-codes").Values<string>().ToList();
            }

            return sonuç;
        }
    }
}