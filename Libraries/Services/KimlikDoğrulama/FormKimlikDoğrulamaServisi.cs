using System;
using System.Web;
using System.Web.Security;
using Core.Domain.Kullanıcılar;
using Services.Kullanıcılar;

namespace Services.KimlikDoğrulama
{
    public partial class FormKimlikDoğrulamaServisi : IKimlikDoğrulamaServisi
    {
        #region Fields

        private readonly HttpContextBase _httpContext;
        private readonly IKullanıcıServisi _kullanıcıServisi;
        private readonly KullanıcıAyarları _kulanıcıAyarları;
        private readonly TimeSpan _sonkullanmaTarihi;

        private Kullanıcı _önbelleklenmişKullanıcı;

        #endregion

        #region Ctor
        public FormKimlikDoğrulamaServisi(
            HttpContextBase httpContext,
            IKullanıcıServisi kullanıcıServisi,
            KullanıcıAyarları kulanıcıAyarları)
        {
            this._httpContext = httpContext;
            this._kullanıcıServisi = kullanıcıServisi;
            this._kulanıcıAyarları = kulanıcıAyarları;
            this._sonkullanmaTarihi = FormsAuthentication.Timeout;
        }

        #endregion

        #region Utilities
        protected virtual Kullanıcı KimliğiDoğrulananKullanıcıAlBilet(FormsAuthenticationTicket bilet)
        {
            if (bilet == null)
                throw new ArgumentNullException("ticket");

            var kullanıcıAdıVeyaEmail = bilet.UserData;

            if (String.IsNullOrWhiteSpace(kullanıcıAdıVeyaEmail))
                return null;
            var kullanıcı = _kulanıcıAyarları.KullanıcıAdlarıEtkin
                ? _kullanıcıServisi.KullanıcıAlKullanıcıAdı(kullanıcıAdıVeyaEmail)
                : _kullanıcıServisi.KullanıcıAlEmail(kullanıcıAdıVeyaEmail);
            return kullanıcı;
        }

        #endregion

        #region Methods
        public virtual void Giriş(Kullanıcı kullanıcı, bool kalıcıÇerezOluştur)
        {
            var şuan = DateTime.UtcNow.ToLocalTime();

            var bilet = new FormsAuthenticationTicket(
                1 /*version*/,
                _kulanıcıAyarları.KullanıcıAdlarıEtkin ? kullanıcı.KullanıcıAdı : kullanıcı.Email,
                şuan,
                şuan.Add(_sonkullanmaTarihi),
                kalıcıÇerezOluştur,
                _kulanıcıAyarları.KullanıcıAdlarıEtkin ? kullanıcı.KullanıcıAdı : kullanıcı.Email,
                FormsAuthentication.FormsCookiePath);

            var şifrelenmişBilet = FormsAuthentication.Encrypt(bilet);

            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, şifrelenmişBilet);
            cookie.HttpOnly = true;
            if (bilet.IsPersistent)
            {
                cookie.Expires = bilet.Expiration;
            }
            cookie.Secure = FormsAuthentication.RequireSSL;
            cookie.Path = FormsAuthentication.FormsCookiePath;
            if (FormsAuthentication.CookieDomain != null)
            {
                cookie.Domain = FormsAuthentication.CookieDomain;
            }

            _httpContext.Response.Cookies.Add(cookie);
            _önbelleklenmişKullanıcı = kullanıcı;
        }
        public virtual void Çıkış()
        {
            _önbelleklenmişKullanıcı = null;
            FormsAuthentication.SignOut();
        }
        public virtual Kullanıcı KimliğiDoğrulananKullanıcı()
        {
            if (_önbelleklenmişKullanıcı != null)
                return _önbelleklenmişKullanıcı;

            if (_httpContext == null ||
                _httpContext.Request == null ||
                !_httpContext.Request.IsAuthenticated ||
                !(_httpContext.User.Identity is FormsIdentity))
            {
                return null;
            }

            var formKimliği = (FormsIdentity)_httpContext.User.Identity;
            var kullanıcı = KimliğiDoğrulananKullanıcıAlBilet(formKimliği.Ticket);
            if (kullanıcı != null && kullanıcı.Aktif && !kullanıcı.GirişGerekli && !kullanıcı.Silindi && kullanıcı.IsRegistered())
                _önbelleklenmişKullanıcı = kullanıcı;
            return _önbelleklenmişKullanıcı;
        }

        #endregion

    }
}
