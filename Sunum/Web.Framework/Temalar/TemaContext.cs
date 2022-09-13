using System;
using System.Linq;
using Core;
using Core.Domain;
using Core.Domain.Kullanıcılar;
using Services.Genel;

namespace Web.Framework.Temalar
{
    public partial class TemaContext : ITemaContext
    {
        private readonly IWorkContext _workContext;
        private readonly ISiteContext _siteContext;
        private readonly IGenelÖznitelikServisi _genelÖznitelikServisi;
        private readonly SiteBilgiAyarları _siteBilgiAyarları;
        private readonly ITemaSağlayıcı _temaSağlayıcı;

        private bool _temaÖnbellekte;
        private string _önbelleklenmişTemaAdı;

        public TemaContext(IWorkContext workContext,
            ISiteContext siteContext,
            IGenelÖznitelikServisi genelÖznitelikServisi,
            SiteBilgiAyarları siteBilgiAyarları,
            ITemaSağlayıcı temaSağlayıcı)
        {
            this._workContext = workContext;
            this._siteContext = siteContext;
            this._genelÖznitelikServisi = genelÖznitelikServisi;
            this._siteBilgiAyarları = siteBilgiAyarları;
            this._temaSağlayıcı = temaSağlayıcı;
        }
        public string ÇalışanTemaAdı
        {
            get
            {
                if (_temaÖnbellekte)
                    return _önbelleklenmişTemaAdı;

                string tema = "DefaultClean";
               /* if (_siteBilgiAyarları.KullanıcılarTemaSeçebilsin)
                {
                    if (_workContext.MevcutKullanıcı != null)
                        tema = _workContext.MevcutKullanıcı.ÖznitelikAl<string>(SistemKullanıcıÖznitelikAdları.MevcutTemaAdı, _genelÖznitelikServisi, _siteContext.MevcutSite.Id);
                }

                //varsayılan site adı
                if (string.IsNullOrEmpty(tema))
                    tema = _siteBilgiAyarları.MevcutSiteTeması;

                //temanın mevcut olduğunu doğrula
                if (!_temaSağlayıcı.TemaAyarlarıMevcut(tema))
                {
                    var temaModeli = _temaSağlayıcı.TemaAyarlarıAl()
                        .FirstOrDefault();
                    if (temaModeli == null)
                        throw new Exception("Tema yüklenemedi");
                    tema = temaModeli.TemaAdı;
                }

                //tema önbellek
                this._önbelleklenmişTemaAdı = tema;
                this._temaÖnbellekte = true;*/
                return tema;
            }
            set
            {
                if (!_siteBilgiAyarları.KullanıcılarTemaSeçebilsin)
                    return;

                if (_workContext.MevcutKullanıcı == null)
                    return;

                _genelÖznitelikServisi.ÖznitelikKaydet(_workContext.MevcutKullanıcı, SistemKullanıcıÖznitelikAdları.MevcutTemaAdı, value, _siteContext.MevcutSite.Id);

                //önbellek temizle
                this._temaÖnbellekte = false;
            }
        }
    }
}
