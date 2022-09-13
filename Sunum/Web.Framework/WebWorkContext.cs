using Core;
using System;
using System.Linq;
using System.Web;
using Core.Domain.Kullanıcılar;
//using Core.Domain.Directory;
//using Core.Domain.Localization;
//using Core.Domain.Tax;
//using Core.Domain.Vendors;
using Core.Fakes;
//using Services.Authentication;
using Services.Genel;
using Services.Kullanıcılar;
//using Services.Directory;
using Services.Yardımcılar;
using Services.KimlikDoğrulama;
//using Services.Localization;
//using Services.Stores;
//using Services.Vendors;
//using Web.Framework.Localization;
namespace Web.Framework
{
    public partial class WebWorkContext : IWorkContext
    {
        #region Const

        private const string KullanıcıÇerezAdı = "TS.Kullanici";

        #endregion

        #region Fields

        private readonly HttpContextBase _httpContext;
        private readonly IKullanıcıServisi _KullanıcıService;
        private readonly ISiteContext _storeContext;
        private readonly IGenelÖznitelikServisi _genericAttributeService;
        private readonly IKullanıcıAracıYardımcısı _userAgentHelper;
        private readonly IKimlikDoğrulamaServisi _kimlikDoğrulamaServisi;

        private Kullanıcı _önbelleklenmişKullanıcı;
        private Kullanıcı _originalKullanıcıIfImpersonated;

        #endregion

        #region Ctor

        public WebWorkContext(HttpContextBase httpContext,
            IKullanıcıServisi KullanıcıService,
            ISiteContext storeContext,
            IGenelÖznitelikServisi genericAttributeService,
            IKullanıcıAracıYardımcısı userAgentHelper,
            IKimlikDoğrulamaServisi kimlikDoğrulamaServisi)
        {
            this._httpContext = httpContext;
            this._KullanıcıService = KullanıcıService;
            this._storeContext = storeContext;
            this._genericAttributeService = genericAttributeService;
            this._userAgentHelper = userAgentHelper;
            this._kimlikDoğrulamaServisi = kimlikDoğrulamaServisi;
        }

        #endregion

        #region Utilities

        protected virtual HttpCookie KullanıcıÇereziniAl()
        {
            if (_httpContext == null || _httpContext.Request == null)
                return null;

            return _httpContext.Request.Cookies[KullanıcıÇerezAdı];
        }

        protected virtual void KullanıcıÇereziniAyarla(Guid KullanıcıGuid)
        {
            if (_httpContext != null && _httpContext.Response != null)
            {
                var çerez = new HttpCookie(KullanıcıÇerezAdı);
                çerez.HttpOnly = true;
                çerez.Value = KullanıcıGuid.ToString();
                if (KullanıcıGuid == Guid.Empty)
                {
                    çerez.Expires = DateTime.Now.AddMonths(-1);
                }
                else
                {
                    int cookieExpires = 24 * 365; //Yapılandırılabilir
                    çerez.Expires = DateTime.Now.AddHours(cookieExpires);
                }

                _httpContext.Response.Cookies.Remove(KullanıcıÇerezAdı);
                _httpContext.Response.Cookies.Add(çerez);
            }
        }

        public virtual Kullanıcı MevcutKullanıcı
        {
            get
            {
                if (_önbelleklenmişKullanıcı != null)
                    return _önbelleklenmişKullanıcı;

                Kullanıcı Kullanıcı = null;
                if (_httpContext == null || _httpContext is FakeHttpContext)
                {
                    //Isteğin bir arka plan görevi tarafından yapıp yapmadığını kontrol et
                    //Bu durumda arka plan görevi için yerleşik kullanıcı kaydı getir
                    Kullanıcı = _KullanıcıService.KullanıcıAlSistemAdı(SistemKullanıcıAdları.ArkaPlanGörevi);
                }

                //Isteğin bir arama motoru tarafından yapıldığını kontrol et
                //Bu durumda arama motorlarına yerleşik kullanıcı kaydı getirin
                if (Kullanıcı == null || Kullanıcı.Silindi || !Kullanıcı.Aktif || Kullanıcı.GirişGerekli)
                {
                    if (_userAgentHelper.AramaMotoru())
                    {
                        Kullanıcı = _KullanıcıService.KullanıcıAlSistemAdı(SistemKullanıcıAdları.AramaMotoru);
                    }
                }
                
                //registered user
                if (Kullanıcı == null || Kullanıcı.Silindi || !Kullanıcı.Aktif || Kullanıcı.GirişGerekli)
                {
                    Kullanıcı = _kimlikDoğrulamaServisi.KimliğiDoğrulananKullanıcı();
                }
                var kullanıcı = Kullanıcı;
                
                //impersonate user if required (currently used for 'phone order' support)
                if (Kullanıcı != null && !Kullanıcı.Silindi && Kullanıcı.Aktif && !Kullanıcı.GirişGerekli)
                {
                    var KimliğeBürünmüşKullanıcıId = Kullanıcı.ÖznitelikAl<int?>(SistemKullanıcıÖznitelikAdları.KimliğineBürünülenKullanıcıId);
                    if (KimliğeBürünmüşKullanıcıId.HasValue && KimliğeBürünmüşKullanıcıId.Value > 0)
                    {
                        var KimliğeBürünmüşKullanıcı = _KullanıcıService.KullanıcıAlId(KimliğeBürünmüşKullanıcıId.Value);
                        if (KimliğeBürünmüşKullanıcı != null && !KimliğeBürünmüşKullanıcı.Silindi && KimliğeBürünmüşKullanıcı.Aktif && !KimliğeBürünmüşKullanıcı.GirişGerekli)
                        {
                            //Kimliğe bürünmüş kullanıcı ayarla
                            _originalKullanıcıIfImpersonated = Kullanıcı;
                            Kullanıcı = KimliğeBürünmüşKullanıcı;
                        }
                    }
                }

                //Ziyaret kullanıcı yükle
                if (Kullanıcı == null || Kullanıcı.Silindi || !Kullanıcı.Aktif || Kullanıcı.GirişGerekli)
                {
                    var KullanıcıÇerezi = KullanıcıÇereziniAl();
                    if (KullanıcıÇerezi != null && !String.IsNullOrEmpty(KullanıcıÇerezi.Value))
                    {
                        Guid KullanıcıGuid;
                        if (Guid.TryParse(KullanıcıÇerezi.Value, out KullanıcıGuid))
                        {
                            var KullanıcıÇerezden = _KullanıcıService.KullanıcıAlGuid(KullanıcıGuid);
                            if (KullanıcıÇerezden != null &&
                                //this Kullanıcı (from cookie) should not be registered
                                !KullanıcıÇerezden.IsRegistered())
                                Kullanıcı = KullanıcıÇerezden;
                        }
                    }
                }
                
                //Mevcut değilse ziyaretçi oluştur
                if (Kullanıcı == null || Kullanıcı.Silindi || !Kullanıcı.Aktif || Kullanıcı.GirişGerekli)
                {
                    Kullanıcı = _KullanıcıService.ZiyaretciKullanıcıEkle();
                }

    
                //doğrulama
                if (!Kullanıcı.Silindi && Kullanıcı.Aktif && !Kullanıcı.GirişGerekli)
                {
                    KullanıcıÇereziniAyarla(Kullanıcı.KullanıcıGuid);
                    _önbelleklenmişKullanıcı = Kullanıcı;
                }
                
                return _önbelleklenmişKullanıcı;
            }
            set
            {
                KullanıcıÇereziniAyarla(value.KullanıcıGuid);
                _önbelleklenmişKullanıcı = value;
            }
        }
        public virtual Kullanıcı OrijinalKullanıcıyıTaklitEt
        {
            get
            {
                return _originalKullanıcıIfImpersonated;
            }
        }
        public virtual bool Yönetici { get; set; }

        #endregion
    }
}