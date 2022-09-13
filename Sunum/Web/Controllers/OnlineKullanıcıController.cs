using Web.Models.Kullanıcılar;
using Core.Domain.Kullanıcılar;
using Services.Genel;
using Services.Güvenlik;
using Services.Kullanıcılar;
using Services.Yardımcılar;
using System;
using System.Linq;
using System.Web.Mvc;
using Web.Controllers;
using Web.Framework.Kendoui;
using Web.Models.Kullanıcılar;

namespace Web.Controllers
{
    public partial class OnlineKullanıcıController : TemelPublicController
    {
        private readonly IKullanıcıServisi _kullanıcıServisi;
        private readonly ITarihYardımcısı _tarihYardımcısı;
        private readonly KullanıcıAyarları _kullanıcıAyarları;
        private readonly IİzinServisi _izinServisi;
        public OnlineKullanıcıController(IKullanıcıServisi kullanıcıServisi,
            ITarihYardımcısı tarihYardımcısı,
            KullanıcıAyarları kullanıcıAyarları,
            IİzinServisi izinServisi)
        {
            this._kullanıcıAyarları = kullanıcıAyarları;
            this._kullanıcıServisi = kullanıcıServisi;
            this._tarihYardımcısı = tarihYardımcısı;
            this._izinServisi = izinServisi;
        }
        public virtual ActionResult Liste()
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KullanıcılarıYönet))
                return ErişimEngellendiView();
            return View();
        }
        [HttpPost]
        public virtual ActionResult Liste(DataSourceİsteği istek)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KullanıcılarıYönet))
                return ErişimEngellendiView();
            var kullanıcılar = _kullanıcıServisi.OnlineKullanıcılarıAl(DateTime.UtcNow.AddMinutes(-_kullanıcıAyarları.OnlineKullanıcıDakikaları), null, istek.Page - 1, istek.PageSize);
            var gridModel = new DataSourceSonucu
            {
                Data = kullanıcılar.Select(x => new OnlineKullanıcılarModel
                {
                    Id = x.Id,
                    KullanıcıBilgisi = x.IsRegistered() ? x.Email : "Ziyaretçi",
                    SonIPAdresi = x.SonIPAdresi,
                    //Konum=_geoLookupService.LookupCountryName(x.SonIPAdresi),
                    Konum = "Türkiye",
                    SonZiyaretEdilenSayfa = _kullanıcıAyarları.SiteSonZiyaretSayfası ? x.ÖznitelikAl<string>(SistemKullanıcıÖznitelikAdları.SonZiyaretEdilenSayfa) : "Son ziyaret edilan sayfa özelliği kapalı",
                    SonİşlemTarihi = _tarihYardımcısı.KullanıcıZamanınaDönüştür(x.SonİşlemTarihi, DateTimeKind.Utc)
                }),
                Toplam = kullanıcılar.TotalCount
            };
            return Json(gridModel);
        }
        
    }
}