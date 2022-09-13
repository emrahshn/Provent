using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using Core.Data;
using Core.Altyapı;

namespace Core
{
    public partial class WebYardımcısı : IWebYardımcısı
    {
        #region Fields 

        private readonly HttpContextBase _httpContext;
        private readonly string[] _sabitDosyaUzantıları;

        #endregion

        #region Constructor
        public WebYardımcısı(HttpContextBase httpContext)
        {
            this._httpContext = httpContext;
            this._sabitDosyaUzantıları = new[] { ".axd", ".ashx", ".bmp", ".css", ".gif", ".htm", ".html", ".ico", ".jpeg", ".jpg", ".js", ".png", ".rar", ".zip" };
        }

        #endregion

        #region Utilities

        protected virtual Boolean İstekErişilebilir(HttpContextBase httpContext)
        {
            if (httpContext == null)
                return false;

            try
            {
                if (httpContext.Request == null)
                    return false;
            }
            catch (HttpException)
            {
                return false;
            }

            return true;
        }
        protected virtual bool WebConfigYazmayıDene()
        {
            try
            {
                // Orta güvenlikte, "UnloadAppDomain" desteklenmez.
                // AppDomaini yeniden başlatmayı deneyin.
                File.SetLastWriteTimeUtc(GenelYardımcı.MapPath("~/web.config"), DateTime.UtcNow);
                return true;
            }
            catch
            {
                return false;
            }
        }

        protected virtual bool GlobalAsaxYazmayıDene()
        {
            try
            {
                File.SetLastWriteTimeUtc(GenelYardımcı.MapPath("~/global.asax"), DateTime.UtcNow);
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region Methods
        public virtual bool İstekYönlendirildi
        {
            get
            {
                var cevap = _httpContext.Response;
                return cevap.IsRequestBeingRedirected;
            }
        }

        public bool PostTamamlandı
        {
            get
            {
                if (_httpContext.Items["TS.PostTamamlandı"] == null)
                    return false;
                return Convert.ToBoolean(_httpContext.Items["TS.PostTamamlandı"]);
            }
            set
            {
                _httpContext.Items["TS.PostTamamlandı"] = value;
            }
        }

        public void AppDomainYenidenBaşlat(bool yönlendir = false, string yönlendirmeUrlsi = "")
        {
            if (GenelYardımcı.GüvenSeviyesiAl() > AspNetHostingPermissionLevel.Medium)
            {
                //Tam güvenlik
                HttpRuntime.UnloadAppDomain();

                GlobalAsaxYazmayıDene();
            }
            else
            {
                //Orta sevşye güvenlik
                bool başarılı = WebConfigYazmayıDene();
                if (!başarılı)
                {
                    throw new TSHata("Yapılandırma değişikliği yüzünden yeniden başlatılmalı ancak bunu yapamadı." + Environment.NewLine +
                        "Gelecekte bu sorunu önlemek için web sunucusu yapılandırmasında bir değişiklik yapılması gerekiyor:" + Environment.NewLine +
                        "- Uygulamayı tam güven ortamında çalıştırın veya" + Environment.NewLine +
                        "- 'web.config' dosyasına uygulama yazma erişimi verin.");
                }
                başarılı = GlobalAsaxYazmayıDene();

                if (!başarılı)
                {
                    throw new TSHata("Yapılandırma değişikliği yüzünden yeniden başlatılmalı ancak bunu yapamadı." + Environment.NewLine +
                        "Gelecekte bu sorunu önlemek için web sunucusu yapılandırmasında bir değişiklik yapılması gerekiyor:" + Environment.NewLine +
                        "- Uygulamayı tam güven ortamında çalıştırın veya" + Environment.NewLine +
                        "- 'Global.asax' dosyasına uygulama yazma erişimi verin.");
                }
            }

            // If setting up extensions/modules requires an AppDomain restart, it's very unlikely the
            // current request can be processed correctly.  So, we redirect to the same URL, so that the
            // new request will come to the newly started AppDomain.
            if (_httpContext != null && yönlendir)
            {
                if (String.IsNullOrEmpty(yönlendirmeUrlsi))
                    yönlendirmeUrlsi = SayfanınUrlsiniAl(true);
                _httpContext.Response.Redirect(yönlendirmeUrlsi, true /*endResponse*/);
            }
        }

        public bool MevcutBağlantıGüvenli()
        {
            bool sslKullan = false;
            if (İstekErişilebilir(_httpContext))
            {
                //Barındırma sunucunuzda bir yük dengeleyici kullanıyorken, Request.IsSecureConnection asla true olarak ayarlanmaz

                //1.HTTP_CLUSTER_HTTPS kullan?
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["HTTP_CLUSTER_HTTPS_Kullan"]) &&
                   Convert.ToBoolean(ConfigurationManager.AppSettings["HTTP_CLUSTER_HTTPS_Kullan"]))
                {
                    sslKullan = ServerDeğişkenleri("HTTP_CLUSTER_HTTPS") == "on";
                }
                //2.HTTP_X_FORWARDED_PROTO kullan?
                else if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["HTTP_X_FORWARDED_PROTO_Kullan"]) &&
                   Convert.ToBoolean(ConfigurationManager.AppSettings["HTTP_X_FORWARDED_PROTO_Kullan"]))
                {
                    sslKullan = string.Equals(ServerDeğişkenleri("HTTP_X_FORWARDED_PROTO"), "https", StringComparison.OrdinalIgnoreCase);
                }
                else
                {
                    sslKullan = _httpContext.Request.IsSecureConnection;
                }
            }

            return sslKullan;
        }

        public string MevcutIpAdresiAl()
        {
            if (!İstekErişilebilir(_httpContext))
                return string.Empty;

            var sonuç = "";
            try
            {
                if (_httpContext.Request.Headers != null)
                {
                    // Bir istemcinin kaynak IP adresini tanımlamak için
                    // Bir HTTP proxy veya yük dengeleyici aracılığıyla bir web sunucusuna bağlanmak gerekir.
                    var yönlendirilmişHttpHeader = "X-FORWARDED-FOR";
                    if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["YönlendirilmişHttpHeader"]))
                    {
                        //Ancak bazı durumlarda sunucu, diğer HTTP header kullanır
                        //Bu durumlarda yönetici özel bir Yönlendirilmiş HTTP header belirleyebilir
                        //CF-Connecting-IP, X-FORWARDED-PROTO
                        yönlendirilmişHttpHeader = ConfigurationManager.AppSettings["YönlendirilmişHttpHeader"];
                    }

                    //Bir web sunucusuna bağlanan istemcinin, kaynak IP adresini tanımlamak için kullanılır
                    //Bir HTTP proxy veya yük dengeleyici aracılığıyla.
                    string xff = _httpContext.Request.Headers.AllKeys
                        .Where(x => yönlendirilmişHttpHeader.Equals(x, StringComparison.InvariantCultureIgnoreCase))
                        .Select(k => _httpContext.Request.Headers[k])
                        .FirstOrDefault();

                    if (!String.IsNullOrEmpty(xff))
                    {
                        string lastIp = xff.Split(new[] { ',' }).FirstOrDefault();
                        sonuç = lastIp;
                    }
                }

                if (String.IsNullOrEmpty(sonuç) && _httpContext.Request.UserHostAddress != null)
                {
                    sonuç = _httpContext.Request.UserHostAddress;
                }
            }
            catch
            {
                return sonuç;
            }

            //Bazı doğrulamalar
            if (sonuç == "::1")
                sonuç = "127.0.0.1";
            //portu sil
            if (!String.IsNullOrEmpty(sonuç))
            {
                int index = sonuç.IndexOf(":", StringComparison.InvariantCultureIgnoreCase);
                if (index > 0)
                    sonuç = sonuç.Substring(0, index);
            }
            return sonuç;
        }

        public bool SabitKaynak(HttpRequest istek)
        {
            if (istek == null)
                throw new ArgumentNullException("istek");

            string yol = istek.Path;
            string uzantı = VirtualPathUtility.GetExtension(yol);

            if (uzantı == null) return false;

            return _sabitDosyaUzantıları.Contains(uzantı);
        }

        public string SayfanınUrlsiniAl(bool sorguİçerir)
        {
            bool useSsl = MevcutBağlantıGüvenli();
            return SayfanınUrlsiniAl(sorguİçerir, useSsl);
        }

        public string SayfanınUrlsiniAl(bool sorguİçerir, bool SslKullan)
        {
            if (!İstekErişilebilir(_httpContext))
                return string.Empty;

            //Ana bilgisayarın SSL kullanmayı düşünmesini sağlayın
            var url = SiteHostAl(SslKullan).TrimEnd('/');

            //Tam Urlyi al sorgu ile veya sorgusuz
            url += sorguİçerir ? _httpContext.Request.RawUrl : _httpContext.Request.Path;

            return url.ToLowerInvariant();
        }

        public string ServerDeğişkenleri(string ad)
        {
            string sonuç = string.Empty;

            try
            {
                if (!İstekErişilebilir(_httpContext))
                    return sonuç;

                if (_httpContext.Request.ServerVariables[ad] != null)
                {
                    sonuç = _httpContext.Request.ServerVariables[ad];
                }
            }
            catch
            {
                sonuç = string.Empty;
            }
            return sonuç;
        }

        public string SiteHostAl(bool SslKullan)
        {
            var sonuç = "";
            var httpHost = ServerDeğişkenleri("HTTP_HOST");
            if (!String.IsNullOrEmpty(httpHost))
            {
                sonuç = "http://" + httpHost;
                if (!sonuç.EndsWith("/"))
                    sonuç += "/";
            }

            if (DataAyarlarıYardımcısı.DatabaseYüklendi())
            {
                #region Database yüklendi

                //let's resolve IWorkContext  here.
                //Do not inject it via constructor  because it'll cause circular references
                var siteContext = EngineContext.Current.Resolve<ISiteContext>();
                var mevcutSite = siteContext.MevcutSite;
                if (mevcutSite == null)
                    throw new Exception("Mevcut site yüklenemedi");

                if (String.IsNullOrWhiteSpace(httpHost))
                {
                    //HTTP_HOST değişkeni erişilemez durumda
                    //Bu senaryo, yalnızca HttpContext kullanılabilir olmadığında (örneğin, bir zamanlama görevinde çalışırken) mümkündür.
                    //Bu durumda yönetici alanında yapılandırılmış bir site öğesinin URL'sini kullanın
                    sonuç = mevcutSite.Url;
                    if (!sonuç.EndsWith("/"))
                        sonuç += "/";
                }

                if (SslKullan)
                {
                    sonuç = !String.IsNullOrWhiteSpace(mevcutSite.GüvenliUrl) ?
                        mevcutSite.GüvenliUrl :
                        sonuç.Replace("http:/", "https:/");
                }
                else
                {
                    if (mevcutSite.SslEtkin && !String.IsNullOrWhiteSpace(mevcutSite.GüvenliUrl))
                    {
                        sonuç = mevcutSite.Url;
                    }
                }
                #endregion
            }
            else
            {
                #region Database yüklenmedi
                if (SslKullan)
                {
                    sonuç = sonuç.Replace("http:/", "https:/");
                }
                #endregion
            }


            if (!sonuç.EndsWith("/"))
                sonuç += "/";
            return sonuç.ToLowerInvariant();
        }

        public string SiteKonumuAl()
        {
            bool sslKullan = MevcutBağlantıGüvenli();
            return SiteKonumuAl(sslKullan);
        }

        public string SiteKonumuAl(bool SslKullan)
        {
            string sonuç = SiteHostAl(SslKullan);
            if (sonuç.EndsWith("/"))
                sonuç = sonuç.Substring(0, sonuç.Length - 1);
            if (İstekErişilebilir(_httpContext))
                sonuç = sonuç + _httpContext.Request.ApplicationPath;
            if (!sonuç.EndsWith("/"))
                sonuç += "/";

            return sonuç.ToLowerInvariant();
        }

        public T Sorgu<T>(string ad)
        {
            string sorguDeğeri = null;
            if (İstekErişilebilir(_httpContext) && _httpContext.Request.QueryString[ad] != null)
                sorguDeğeri = _httpContext.Request.QueryString[ad];

            if (!String.IsNullOrEmpty(sorguDeğeri))
                return GenelYardımcı.To<T>(sorguDeğeri);

            return default(T);
        }

        public string SorguDeğiştir(string url, string sorguDeğiştirme, string anchor)
        {
            if (url == null)
                url = string.Empty;
            url = url.ToLowerInvariant();

            if (sorguDeğiştirme == null)
                sorguDeğiştirme = string.Empty;
            sorguDeğiştirme = sorguDeğiştirme.ToLowerInvariant();

            if (anchor == null)
                anchor = string.Empty;
            anchor = anchor.ToLowerInvariant();


            string str = string.Empty;
            string str2 = string.Empty;
            if (url.Contains("#"))
            {
                str2 = url.Substring(url.IndexOf("#") + 1);
                url = url.Substring(0, url.IndexOf("#"));
            }
            if (url.Contains("?"))
            {
                str = url.Substring(url.IndexOf("?") + 1);
                url = url.Substring(0, url.IndexOf("?"));
            }
            if (!string.IsNullOrEmpty(sorguDeğiştirme))
            {
                if (!string.IsNullOrEmpty(str))
                {
                    var dictionary = new Dictionary<string, string>();
                    foreach (string str3 in str.Split(new[] { '&' }))
                    {
                        if (!string.IsNullOrEmpty(str3))
                        {
                            string[] strArray = str3.Split(new[] { '=' });
                            if (strArray.Length == 2)
                            {
                                if (!dictionary.ContainsKey(strArray[0]))
                                {
                                    dictionary[strArray[0]] = strArray[1];
                                }
                            }
                            else
                            {
                                dictionary[str3] = null;
                            }
                        }
                    }
                    foreach (string str4 in sorguDeğiştirme.Split(new[] { '&' }))
                    {
                        if (!string.IsNullOrEmpty(str4))
                        {
                            string[] strArray2 = str4.Split(new[] { '=' });
                            if (strArray2.Length == 2)
                            {
                                dictionary[strArray2[0]] = strArray2[1];
                            }
                            else
                            {
                                dictionary[str4] = null;
                            }
                        }
                    }
                    var builder = new StringBuilder();
                    foreach (string str5 in dictionary.Keys)
                    {
                        if (builder.Length > 0)
                        {
                            builder.Append("&");
                        }
                        builder.Append(str5);
                        if (dictionary[str5] != null)
                        {
                            builder.Append("=");
                            builder.Append(dictionary[str5]);
                        }
                    }
                    str = builder.ToString();
                }
                else
                {
                    str = sorguDeğiştirme;
                }
            }
            if (!string.IsNullOrEmpty(anchor))
            {
                str2 = anchor;
            }
            return (url + (string.IsNullOrEmpty(str) ? "" : ("?" + str)) + (string.IsNullOrEmpty(str2) ? "" : ("#" + str2))).ToLowerInvariant();
        }

        public string SorguSil(string url, string sorgu)
        {
            if (url == null)
                url = string.Empty;
            url = url.ToLowerInvariant();

            if (sorgu == null)
                sorgu = string.Empty;
            sorgu = sorgu.ToLowerInvariant();


            string str = string.Empty;
            if (url.Contains("?"))
            {
                str = url.Substring(url.IndexOf("?") + 1);
                url = url.Substring(0, url.IndexOf("?"));
            }
            if (!string.IsNullOrEmpty(sorgu))
            {
                if (!string.IsNullOrEmpty(str))
                {
                    var dictionary = new Dictionary<string, string>();
                    foreach (string str3 in str.Split(new[] { '&' }))
                    {
                        if (!string.IsNullOrEmpty(str3))
                        {
                            string[] strArray = str3.Split(new[] { '=' });
                            if (strArray.Length == 2)
                            {
                                dictionary[strArray[0]] = strArray[1];
                            }
                            else
                            {
                                dictionary[str3] = null;
                            }
                        }
                    }
                    dictionary.Remove(sorgu);

                    var builder = new StringBuilder();
                    foreach (string str5 in dictionary.Keys)
                    {
                        if (builder.Length > 0)
                        {
                            builder.Append("&");
                        }
                        builder.Append(str5);
                        if (dictionary[str5] != null)
                        {
                            builder.Append("=");
                            builder.Append(dictionary[str5]);
                        }
                    }
                    str = builder.ToString();
                }
            }
            return (url + (string.IsNullOrEmpty(str) ? "" : ("?" + str)));
        }

        public string UrlYönlendiriciAl()
        {
            string yönlendiriUrl = string.Empty;

            //URL yönlendirici bir şekilde null dönebilir
            if (İstekErişilebilir(_httpContext) && _httpContext.Request.UrlReferrer != null)
                yönlendiriUrl = _httpContext.Request.UrlReferrer.PathAndQuery;

            return yönlendiriUrl;
        }
        #endregion
    }
}
