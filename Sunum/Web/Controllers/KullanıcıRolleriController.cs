using Core;
using Core.Domain.Kullanıcılar;
using Services.Güvenlik;
using Services.Kullanıcılar;
using Services.Logging;
using System;
using System.Linq;
using System.Web.Mvc;
using Web.Framework.Controllers;
using Web.Framework.Kendoui;
using Web.Controllers;
using Web.Models.Kullanıcılar;
using Web.Uzantılar;

namespace Web.Controllers
{
    public class KullanıcıRolleriController : TemelPublicController
    {
        private readonly IİzinServisi _izinServisi;
        private readonly IKullanıcıServisi _kullanıcıServisi;
        private readonly IKullanıcıİşlemServisi _işlemServisi;
        public KullanıcıRolleriController(IİzinServisi izinServisi,
            IKullanıcıServisi kullanıcıServisi,
            IKullanıcıİşlemServisi işlemServisi)
        {
            this._izinServisi = izinServisi;
            this._kullanıcıServisi = kullanıcıServisi;
            this._işlemServisi = işlemServisi;
        }

        [NonAction]
        protected virtual KullanıcıRolModel KullanıcıRolModelHazırla(KullanıcıRolü kullanıcıRolü)
        {
            var model = kullanıcıRolü.ToModel();
            return model;
        }
        public virtual ActionResult Index()
        {
            return RedirectToAction("Liste");
        }
        public virtual ActionResult Liste()
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KullanıcılarıYönet))
                return ErişimEngellendiView();

            return View();
        }

        [HttpPost]
        public virtual ActionResult Liste(DataSourceİsteği command)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KullanıcılarıYönet))
                return ErişimEngellendiKendoGridJson();

            var kullanıcıRolleri = _kullanıcıServisi.TümKullanıcıRolleriniAl(true);
            var gridModel = new DataSourceSonucu
            {
                Data = kullanıcıRolleri.Select(KullanıcıRolModelHazırla),
                Toplam = kullanıcıRolleri.Count()
            };

            return Json(gridModel);
        }
        public virtual ActionResult Ekle()
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KullanıcılarıYönet))
                return ErişimEngellendiView();

            var model = new KullanıcıRolModel();
            model.Aktif = true;
            return View(model);
        }
        [HttpPost, FormAdıParametresi("kaydet-devam", "düzenlemeyeDevam")]
        public virtual ActionResult Ekle(KullanıcıRolModel model, bool düzenlemeyeDevam)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KullanıcılarıYönet))
                return ErişimEngellendiView();
            if (ModelState.IsValid)
            {
                var kullanıcıRolü = model.ToEntity();
                _kullanıcıServisi.KullanıcıRolüEkle(kullanıcıRolü);
                _işlemServisi.İşlemEkle("YeniKullanıcıRolü", "Yeni kullanıcı rolü başarıyla eklendi", kullanıcıRolü.Adı);
                BaşarılıBildirimi("Kullanıcı rolü başarıyla eklendi");
                return düzenlemeyeDevam ? RedirectToAction("Düzenle", new { id = kullanıcıRolü.Id }) : RedirectToAction("Liste");
            }
            return View(model);
        }
        public virtual ActionResult Düzenle(int id)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KullanıcılarıYönet))
                return ErişimEngellendiView();
            var kullanıcıRolü = _kullanıcıServisi.KullanıcıRolüAlId(id);
            if (kullanıcıRolü == null)
            {
                return RedirectToAction("Liste");
            }
            var model = KullanıcıRolModelHazırla(kullanıcıRolü);
            return View(model);
        }
        [HttpPost, FormAdıParametresi("kaydet-devam", "düzenlemeyeDevam")]
        public virtual ActionResult Düzenle(KullanıcıRolModel model, bool düzenlemeyeDevam)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KullanıcılarıYönet))
                return ErişimEngellendiView();
            var kullanıcıRolü = _kullanıcıServisi.KullanıcıRolüAlId(model.Id);
            if (kullanıcıRolü == null)
            {
                return RedirectToAction("Liste");
            }
            try
            {
                if (ModelState.IsValid)
                {
                    if (kullanıcıRolü.SistemRolü && !model.Aktif)
                        throw new TSHata("Sistem rolünü düzenleyemezsiniz.");
                    if (kullanıcıRolü.SistemRolü && !kullanıcıRolü.SistemAdı.Equals(model.SistemAdı, StringComparison.InvariantCultureIgnoreCase))
                        throw new TSHata("Sistem rolünü düzenleyemezsiniz.");
                    kullanıcıRolü = model.ToEntity(kullanıcıRolü);
                    _kullanıcıServisi.KullanıcıRolüGüncelle(kullanıcıRolü);
                    _işlemServisi.İşlemEkle("KullanıcıRolü", "Kullanıcı rolü başarıyla güncellendi", kullanıcıRolü.Adı);
                    BaşarılıBildirimi("Kullanıcı rolü başarıyla güncellendi");
                    return düzenlemeyeDevam ? RedirectToAction("Düzenle", new { id = kullanıcıRolü.Id }) : RedirectToAction("Liste");
                }
                return View(model);
            }
            catch (Exception ex)
            {
                HataBildirimi(ex);
                return RedirectToAction("Düzenle", new { id = kullanıcıRolü.Id });
            }
        }
        [HttpPost]
        public virtual ActionResult Sil(KullanıcıRolModel model)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KullanıcılarıYönet))
                return ErişimEngellendiView();
            var kullanıcıRolü = _kullanıcıServisi.KullanıcıRolüAlId(model.Id);
            if (kullanıcıRolü == null)
                return RedirectToAction("Liste");

            try
            {
                _kullanıcıServisi.KullanıcıRolüSil(kullanıcıRolü);
                _işlemServisi.İşlemEkle("KullanıcıRolü", "Kullanıcı rolü başarıyla silindi");
                BaşarılıBildirimi("Kullanıcı rolü başarıyla silindi");
                return RedirectToAction("Liste");
            }
            catch (Exception ex)
            {
                HataBildirimi(ex);
                return RedirectToAction("Düzenle", new { id = kullanıcıRolü.Id });
            }
        }
    }
}