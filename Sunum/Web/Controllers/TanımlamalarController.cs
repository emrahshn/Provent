using Services.Güvenlik;
using Services.Siteler;
using System.Linq;
using System.Web.Mvc;
using Web.Framework.Controllers;
using Web.Framework.Kendoui;
using Web.Uzantılar;
using System;
using Services.Logging;
using Services.Tanımlamalar;
using Web.Models.Tanımlamalar;
using Services.Konum;
using Services.EkTanımlamalar;
using Core.Domain.Tanımlamalar;
using System.Collections.Generic;

namespace Web.Controllers
{
    public class TanımlamalarController : TemelPublicController
    {
        private readonly IİzinServisi _izinServisi;
        private readonly IKullanıcıİşlemServisi _kullanıcıİşlemServisi;
        private readonly ISiteServisi _siteServisi;
        private readonly IKonumServisi _konumServisi;
        private readonly IMusteriSektorServisi _musteriSektorServisi;
        private readonly ITedarikciSektorServisi _tedarikciSektorServisi;
        private readonly IFirmaServisi _firmaServisi;
        private readonly IFirmaKategorisiServisi _firmaKategoriServisi;
        private readonly IYetkililerServisi _yetkiliServisi;
        private readonly IHekimlerServisi _hekimServisi;
        private readonly IHekimBranşlarıServisi _branşServisi;
        private readonly IUnvanlarServisi _unvanServisi;

        public TanımlamalarController(IİzinServisi izinServisi,
            ISiteServisi siteServisi,
            IKonumServisi konumServisi,
            IMusteriSektorServisi musteriSektorServisi,
            ITedarikciSektorServisi tedarikciSektorServisi,
            IKullanıcıİşlemServisi kullanıcıİşlemServisi,
            IFirmaServisi firmaServisi,
            IYetkililerServisi yetkiliServisi,
            IFirmaKategorisiServisi firmaKategoriServisi,
            IHekimlerServisi hekimServisi,
            IHekimBranşlarıServisi branşServisi,
            IUnvanlarServisi unvanServisi)
        {
            this._izinServisi = izinServisi;
            this._siteServisi = siteServisi;
            this._konumServisi = konumServisi;
            this._musteriSektorServisi = musteriSektorServisi;
            this._kullanıcıİşlemServisi = kullanıcıİşlemServisi;
            this._tedarikciSektorServisi = tedarikciSektorServisi;
            this._firmaServisi = firmaServisi;
            this._firmaKategoriServisi = firmaKategoriServisi;
            this._yetkiliServisi = yetkiliServisi;
            this._hekimServisi = hekimServisi;
            this._branşServisi = branşServisi;
            this._unvanServisi = unvanServisi;
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult IlceAlSehirId(string sehirId)
        {
            if (String.IsNullOrEmpty(sehirId))
            {
                throw new ArgumentNullException("sehirId");
            }
            int id = 0;
            bool isValid = Int32.TryParse(sehirId, out id);
            var ilceler = _konumServisi.IlcelerAlSehirId(id);
            var sonuc = (from s in ilceler
                         select new
                         {
                             id = s.Id,
                             name = s.Adı
                         }).ToList();
            return Json(sonuc, JsonRequestBehavior.AllowGet);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult SehirAlUlkeId(string ulkeId)
        {
            if (String.IsNullOrEmpty(ulkeId))
            {
                throw new ArgumentNullException("ulkeId");
            }
            int id = 0;
            bool isValid = Int32.TryParse(ulkeId, out id);
            var sehirler = _konumServisi.SehirlerAlUlkeId(id);
            var sonuc = (from s in sehirler
                         select new
                         {
                             id = s.Id,
                             name = s.Adı
                         }).ToList();
            return Json(sonuc, JsonRequestBehavior.AllowGet);
        }
        #region Firma
        public virtual ActionResult FirmaListe()
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KongreYönet))
                return ErişimEngellendiView();

            var model = new FirmaModel();
            return View(model);
        }

        [HttpPost]
        public virtual ActionResult FirmaListe(DataSourceİsteği command, FirmaModel model)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KongreYönet))
                return ErişimEngellendiKendoGridJson();

            var firmaModels = _firmaServisi.TümFirmaAl()
                .Select(x => x.ToModel())
                .ToList();
            var gridModel = new DataSourceSonucu
            {
                Data = firmaModels,
                Toplam = firmaModels.Count
            };

            return Json(gridModel);
        }

        public virtual ActionResult FirmaEkle()
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KongreYönet))
                ErişimEngellendiView();

            var model = new FirmaModel();
            foreach (var tumSehirler in _konumServisi.SehirlerAlUlkeId(1))
            {
                var sehirModel = tumSehirler.ToModel();
                model.Sehirler.Add(sehirModel);
            }
            foreach (var tumIlceler in _konumServisi.IlcelerAlSehirId(1))
            {
                var ilceModel = tumIlceler.ToModel();
                model.Ilceler.Add(ilceModel);
            }
            foreach (var tumYetkililer in _yetkiliServisi.TümYetkiliAl())
            {
                var yetkili = tumYetkililer.ToModel();
                model.Yetkili.Add(new SelectListItem { Text = yetkili.Adı, Value = yetkili.Id.ToString() });
            }
            foreach (var kategoriler in _firmaKategoriServisi.TümFirmaKategorisiAl())
            {
                var kategori = kategoriler.ToModel();
                model.Kategoriler.Add(new SelectListItem { Text = kategori.Adı, Value = kategori.Id.ToString() });
            }

            return View(model);
        }

        [HttpPost, FormAdıParametresi("kaydet-devam", "düzenlemeyeDevam")]
        public virtual ActionResult FirmaEkle(FirmaModel model, bool düzenlemeyeDevam)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KongreYönet))
                ErişimEngellendiView();
            if (ModelState.IsValid)
            {
                var firma = model.ToEntity();
                if (model.YetkiliIdleri.Count > 0)
                {
                    foreach (var yetkiliId in model.YetkiliIdleri)
                    {
                        var yetkili = _yetkiliServisi.YetkiliAlId(yetkiliId);
                        firma.Yetkililer.Add(yetkili);
                    }
                }
                _firmaServisi.FirmaEkle(firma);


                BaşarılıBildirimi("Kongre firması başarıyla Eklendi");
                _kullanıcıİşlemServisi.İşlemEkle("YeniFirmaEklendi", "Yeni Firma Eklendi", firma.Adı);
                if (düzenlemeyeDevam)
                {
                    SeçiliTabKaydet();
                    return RedirectToAction("FirmaDüzenle", new { id = firma.Id });
                }
                return RedirectToAction("FirmaListe");

            }
            return View(model);
        }
        public virtual ActionResult FirmaDüzenle(int id)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KongreYönet))
                ErişimEngellendiView();
            var firma = _firmaServisi.FirmaAlId(id);

            if (firma == null)
            {
                return RedirectToAction("FirmaListe");
            }
            var model = firma.ToModel();
            List<Yetkililer> l = new List<Yetkililer>();
            foreach (var tumYetkililer in _yetkiliServisi.TümYetkiliAl())
            {
                var yetkili = tumYetkililer.ToModel();
                model.Yetkili.Add(new SelectListItem { Text = yetkili.Adı, Value = yetkili.Id.ToString() });
            }

            foreach (var yetkiliId in firma.Yetkililer)
            {
                var yetkili = _yetkiliServisi.YetkiliAlId(yetkiliId.Id);
                SelectListItem a = model.Yetkili.FirstOrDefault(x => x.Value == yetkili.Id.ToString());
                a.Selected = true;
                model.YetkiliIdleri.Add(yetkili.Id);
                l.Add(yetkili);
            }
            //model.Yetkililer = l;
            foreach (var tumSehirler in _konumServisi.SehirlerAlUlkeId(1))
            {
                var sehirModel = tumSehirler.ToModel();
                model.Sehirler.Add(sehirModel);
            }
            foreach (var tumIlceler in _konumServisi.IlcelerAlSehirId(firma.SehirId))
            {
                var ilceModel = tumIlceler.ToModel();
                model.Ilceler.Add(ilceModel);
            }
            foreach (var kategoriler in _firmaKategoriServisi.TümFirmaKategorisiAl())
            {
                var kategori = kategoriler.ToModel();
                model.Kategoriler.Add(new SelectListItem { Text = kategori.Adı, Value = kategori.Id.ToString() });
            }
            return View(model);
        }
        [HttpPost, FormAdıParametresi("kaydet-devam", "düzenlemeyeDevam")]
        public virtual ActionResult FirmaDüzenle(FirmaModel model, bool düzenlemeyeDevam)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KongreYönet))
                ErişimEngellendiView();
            var firma = _firmaServisi.FirmaAlId(model.Id);
            if (firma == null)
            {
                return RedirectToAction("FirmaListe");
            }
            List<Yetkililer> l = new List<Yetkililer>();
            if (ModelState.IsValid)
            {
                firma = model.ToEntity(firma);
                firma.Yetkililer.Clear();
                if (model.YetkiliIdleri.Count > 0)
                {
                    foreach (var yetkiliId in model.YetkiliIdleri)
                    {
                        var yetkili = _yetkiliServisi.YetkiliAlId(yetkiliId);
                        firma.Yetkililer.Add(yetkili);
                        l.Add(yetkili);
                    }
                }
                _firmaServisi.FirmaGüncelle(firma);
                BaşarılıBildirimi("Kongre Firması başarıyla güncellenmiştir.");
                _kullanıcıİşlemServisi.İşlemEkle("FirmaGüncelle", "Kongre firması güncellendi", firma.Adı);
                if (düzenlemeyeDevam)
                {
                    return RedirectToAction("FirmaDüzenle", new { id = firma.Id });
                }
                return RedirectToAction("FirmaListe");
            }
            return View(model);
        }

        [HttpPost]
        public virtual ActionResult FirmaSil(FirmaModel model)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KongreYönet))
                return ErişimEngellendiView();

            var firma = _firmaServisi.FirmaAlId(model.Id);
            if (firma == null)
                return RedirectToAction("FirmaListe");
            _firmaServisi.FirmaSil(firma);
            BaşarılıBildirimi("Kongre firmalsı başarıyla silindi");
            _kullanıcıİşlemServisi.İşlemEkle("FirmaSil", "Kongre firması silindi", firma.Adı);
            return RedirectToAction("FirmaListe");
        }
        #endregion
        #region Hekimler
        public virtual ActionResult HekimListe()
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KongreYönet))
                return ErişimEngellendiView();

            var model = new HekimlerModel();
            return View(model);
        }

        [HttpPost]
        public virtual ActionResult HekimListe(DataSourceİsteği command, HekimlerModel model)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KongreYönet))
                return ErişimEngellendiKendoGridJson();

            var hekimModels = _hekimServisi.TümHekimlerAl()
                .Select(x => x.ToModel())
                .ToList();
            var gridModel = new DataSourceSonucu
            {
                Data = hekimModels,
                Toplam = hekimModels.Count
            };

            return Json(gridModel);
        }

        public virtual ActionResult HekimEkle()
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KongreYönet))
                ErişimEngellendiView();

            var model = new HekimlerModel();
            foreach (var branş in _branşServisi.TümHekimBranşlarıAl())
                model.Branşlar.Add(new SelectListItem { Text = branş.Adı, Value = branş.Id.ToString() });
            foreach (var tumUlkeler in _konumServisi.TümUlkeleriAl())
            {
                var ulkeModel = tumUlkeler.ToModel();
                model.Ulkeler.Add(ulkeModel);
            }
            foreach (var tumSehirler in _konumServisi.SehirlerAlUlkeId(1))
            {
                var sehirModel = tumSehirler.ToModel();
                model.Sehirler.Add(sehirModel);
            }
            foreach (var tumIlceler in _konumServisi.IlcelerAlSehirId(1))
            {
                var ilceModel = tumIlceler.ToModel();
                model.Ilceler.Add(ilceModel);
            }
            return View(model);
        }

        [HttpPost, FormAdıParametresi("kaydet-devam", "düzenlemeyeDevam")]
        public virtual ActionResult HekimEkle(HekimlerModel model, bool düzenlemeyeDevam)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KongreYönet))
                ErişimEngellendiView();
            if (ModelState.IsValid)
            {
                var hekim = model.ToEntity();
                _hekimServisi.HekimlerEkle(hekim);
                BaşarılıBildirimi("Müşteri sektörü başarıyla Eklendi");
                _kullanıcıİşlemServisi.İşlemEkle("YeniMüşteriEklendi", "Yeni Müşteri Eklendi", hekim.Adı);
                if (düzenlemeyeDevam)
                {
                    SeçiliTabKaydet();
                    return RedirectToAction("Düzenle", new { id = hekim.Id });
                }
                return RedirectToAction("HekimListe");
            }
            return View(model);
        }
        public virtual ActionResult HekimDüzenle(int id)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KongreYönet))
                ErişimEngellendiView();
            var hekim = _hekimServisi.HekimlerAlId(id);
            if (hekim == null)
            {
                return RedirectToAction("HekimListe");
            }
            var model = hekim.ToModel();
            foreach (var tumUlkeler in _konumServisi.TümUlkeleriAl())
            {
                var ulkeModel = tumUlkeler.ToModel();
                model.Ulkeler.Add(ulkeModel);
            }
            foreach (var tumSehirler in _konumServisi.SehirlerAlUlkeId(hekim.UlkeId))
            {
                var sehirModel = tumSehirler.ToModel();
                model.Sehirler.Add(sehirModel);
            }
            foreach (var tumIlceler in _konumServisi.IlcelerAlSehirId(hekim.SehirId))
            {
                var ilceModel = tumIlceler.ToModel();
                model.Ilceler.Add(ilceModel);
            }
            foreach (var branş in _branşServisi.TümHekimBranşlarıAl())
                model.Branşlar.Add(new SelectListItem { Text = branş.Adı, Value = branş.Id.ToString() });
            return View(model);
        }
        [HttpPost, FormAdıParametresi("kaydet-devam", "düzenlemeyeDevam")]
        public virtual ActionResult HekimDüzenle(HekimlerModel model, bool düzenlemeyeDevam)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KongreYönet))
                ErişimEngellendiView();
            var hekim = _hekimServisi.HekimlerAlId(model.Id);
            if (hekim == null)
            {
                return RedirectToAction("HekimlerListe");
            }
            if (ModelState.IsValid)
            {
                hekim = model.ToEntity(hekim);
                _hekimServisi.HekimlerGüncelle(hekim);
                BaşarılıBildirimi("Hekimler başarıyla güncellenmiştir.");
                _kullanıcıİşlemServisi.İşlemEkle("HekimlerGüncelle", "Hekimler güncellendi", hekim.Adı);
                if (düzenlemeyeDevam)
                {
                    return RedirectToAction("HekimDüzenle", new { id = hekim.Id });
                }
                return RedirectToAction("HekimListe");
            }
            return View(model);
        }

        [HttpPost]
        public virtual ActionResult HekimSil(HekimlerModel model)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KongreYönet))
                return ErişimEngellendiView();

            var hekim = _hekimServisi.HekimlerAlId(model.Id);
            if (hekim == null)
                return RedirectToAction("HekimListe");
            _hekimServisi.HekimlerSil(hekim);
            BaşarılıBildirimi("Hekimler başarıyla silindi");
            _kullanıcıİşlemServisi.İşlemEkle("HekimlerSil", "Hekimler silindi", hekim.Adı);
            return RedirectToAction("HekimListe");
        }
        #endregion
        #region KongreYetkilisi
        public virtual ActionResult YetkiliListe()
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KongreYönet))
                return ErişimEngellendiView();

            var model = new YetkililerModel();
            return View(model);
        }

        [HttpPost]
        public virtual ActionResult YetkiliListe(DataSourceİsteği command, YetkililerModel model)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KongreYönet))
                return ErişimEngellendiKendoGridJson();

            var firmaModels = _yetkiliServisi.TümYetkiliAl()
                .Select(x => x.ToModel())
                .ToList();
            var gridModel = new DataSourceSonucu
            {
                Data = firmaModels,
                Toplam = firmaModels.Count
            };

            return Json(gridModel);
        }

        public virtual ActionResult YetkiliEkle()
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KongreYönet))
                ErişimEngellendiView();

            var model = new YetkililerModel();
            foreach (var tumSehirler in _konumServisi.SehirlerAlUlkeId(1))
            {
                var sehirModel = tumSehirler.ToModel();
                model.Sehirler.Add(sehirModel);
            }
            foreach (var tumIlceler in _konumServisi.IlcelerAlSehirId(1))
            {
                var ilceModel = tumIlceler.ToModel();
                model.Ilceler.Add(ilceModel);
            }
            foreach (var unvanlar in _unvanServisi.TümUnvanlarıAl())
            {
                var unvan = unvanlar.ToModel();
                model.Görevler.Add(unvanlar);
            }
            return View(model);
        }

        [HttpPost, FormAdıParametresi("kaydet-devam", "düzenlemeyeDevam")]
        public virtual ActionResult YetkiliEkle(YetkililerModel model, bool düzenlemeyeDevam,string returnUrl)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KongreYönet))
                ErişimEngellendiView();
            if (ModelState.IsValid)
            {
                var firma = model.ToEntity();
                _yetkiliServisi.YetkiliEkle(firma);
                BaşarılıBildirimi("Kongre firması başarıyla Eklendi");
                _kullanıcıİşlemServisi.İşlemEkle("YeniMüşteriEklendi", "Yeni Müşteri Eklendi", firma.Adı);
                if (düzenlemeyeDevam)
                {
                    SeçiliTabKaydet();
                    return RedirectToAction("Düzenle", new { id = firma.Id });
                }
                if (returnUrl!=null)
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction("KongreYetkilisiListe");
                }
                
            }
            return View(model);
        }
        public virtual ActionResult YetkiliDüzenle(int id)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KongreYönet))
                ErişimEngellendiView();
            var hekim = _yetkiliServisi.YetkiliAlId(id);
            if (hekim == null)
            {
                return RedirectToAction("KongreYetkilisiListe");
            }
            var model = hekim.ToModel();
            foreach (var tumSehirler in _konumServisi.SehirlerAlUlkeId(1))
            {
                var sehirModel = tumSehirler.ToModel();
                model.Sehirler.Add(sehirModel);
            }
            foreach (var tumIlceler in _konumServisi.IlcelerAlSehirId(hekim.YSehirId))
            {
                var ilceModel = tumIlceler.ToModel();
                model.Ilceler.Add(ilceModel);
            }
            foreach (var unvanlar in _unvanServisi.TümUnvanlarıAl())
            {
                var unvan = unvanlar.ToModel();
                model.Görevler.Add(unvanlar);
            }
            return View(model);
        }
        [HttpPost, FormAdıParametresi("kaydet-devam", "düzenlemeyeDevam")]
        public virtual ActionResult YetkiliDüzenle(YetkililerModel model, bool düzenlemeyeDevam)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KongreYönet))
                ErişimEngellendiView();
            var firma = _yetkiliServisi.YetkiliAlId(model.Id);
            if (firma == null)
            {
                return RedirectToAction("KongreYetkilisiListe");
            }
            if (ModelState.IsValid)
            {
                firma = model.ToEntity(firma);
                _yetkiliServisi.YetkiliGüncelle(firma);
                BaşarılıBildirimi("KongreYetkilisi başarıyla güncellenmiştir.");
                _kullanıcıİşlemServisi.İşlemEkle("KongreYetkilisiGüncelle", "KongreYetkilisi güncellendi", firma.Adı);
                if (düzenlemeyeDevam)
                {
                    return RedirectToAction("KongreYetkilisiDüzenle", new { id = firma.Id });
                }
                return RedirectToAction("KongreYetkilisiListe");
            }
            return View(model);
        }

        [HttpPost]
        public virtual ActionResult YetkiliSil(YetkililerModel model)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KongreYönet))
                return ErişimEngellendiView();

            var firma = _yetkiliServisi.YetkiliAlId(model.Id);
            if (firma == null)
                return RedirectToAction("KongreYetkilisiListe");
            _yetkiliServisi.YetkiliSil(firma);
            BaşarılıBildirimi("KongreYetkilisi başarıyla silindi");
            _kullanıcıİşlemServisi.İşlemEkle("KongreYetkilisiSil", "KongreYetkilisi silindi", firma.Adı);
            return RedirectToAction("KongreYetkilisiListe");
        }
        #endregion
    }
}