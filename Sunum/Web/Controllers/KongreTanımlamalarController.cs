using Services.EkTanımlamalar;
using Services.Güvenlik;
using Services.Kongre;
using Services.KongreTanımlama;
using Services.Konum;
using Services.Logging;
using System.Linq;
using System.Web.Mvc;
using Web.Framework.Controllers;
using Web.Framework.Kendoui;
using Web.Models.KongreTanımlamaları;
using Web.Models.Kongre;
using Web.Uzantılar;
using Core.Domain.KongreTanımlama;
using System.Collections.Generic;
using Services.Tanımlamalar;
using Core.Domain.Tanımlamalar;
using Web.Models.Tanımlamalar;

namespace Web.Controllers
{
    public class KongreTanımlamalarController : TemelPublicController
    {
        private readonly IİzinServisi _izinServisi;
        private readonly IHekimlerServisi _hekimServisi;
        private readonly IKullanıcıİşlemServisi _kullanıcıİşlemServisi;
        private readonly IHekimBranşlarıServisi _branşServisi;
        private readonly IKonumServisi _konumServisi;
        private readonly IYetkililerServisi _yetkiliServisi;
        private readonly IKongreTedarikçiServisi _tedarikçiServisi;
        private readonly IFirmaServisi _firmaServisi;
        private readonly IUnvanlarServisi _unvanServisi;
        public KongreTanımlamalarController(IİzinServisi izinServisi,
            IHekimlerServisi hekimServisi,
            IKullanıcıİşlemServisi kullanıcıİşlemServisi,
            IHekimBranşlarıServisi branşServisi,
            IKonumServisi konumServisi,
            IYetkililerServisi yetkiliServisi,
            IUnvanlarServisi unvanServisi,
            IKongreTedarikçiServisi tedarikçiServisi,
            IFirmaServisi firmaServisi)
        {
            this._izinServisi = izinServisi;
            this._hekimServisi = hekimServisi;
            this._kullanıcıİşlemServisi = kullanıcıİşlemServisi;
            this._branşServisi = branşServisi;
            this._konumServisi = konumServisi;
            this._firmaServisi=firmaServisi;
            this._unvanServisi = unvanServisi;
            this._tedarikçiServisi = tedarikçiServisi;
            this._yetkiliServisi = yetkiliServisi;
        }
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
        public virtual ActionResult KongreYetkilisiListe()
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KongreYönet))
                return ErişimEngellendiView();

            var model = new YetkililerModel();
            return View(model);
        }

        [HttpPost]
        public virtual ActionResult KongreYetkilisiListe(DataSourceİsteği command, YetkililerModel model)
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

        public virtual ActionResult KongreYetkilisiEkle()
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
        public virtual ActionResult KongreYetkilisiEkle(YetkililerModel model, bool düzenlemeyeDevam)
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
                return RedirectToAction("KongreYetkilisiListe");
            }
            return View(model);
        }
        public virtual ActionResult KongreYetkilisiDüzenle(int id)
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
        public virtual ActionResult KongreYetkilisiDüzenle(YetkililerModel model, bool düzenlemeyeDevam)
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
        public virtual ActionResult KongreYetkilisiSil(YetkililerModel model)
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
        #region KongreTedarikçi
        public virtual ActionResult KongreTedarikciListe()
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KongreYönet))
                return ErişimEngellendiView();

            var model = new KongreTedarikçiModel();
            return View(model);
        }

        [HttpPost]
        public virtual ActionResult KongreTedarikciListe(DataSourceİsteği command, KongreTedarikçiModel model)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KongreYönet))
                return ErişimEngellendiKendoGridJson();

            var tedarikçiModels = _tedarikçiServisi.TümKongreTedarikçiAl()
                .Select(x => x.ToModel())
                .ToList();
            var gridModel = new DataSourceSonucu
            {
                Data = tedarikçiModels,
                Toplam = tedarikçiModels.Count
            };

            return Json(gridModel);
        }

        public virtual ActionResult KongreTedarikciEkle()
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KongreYönet))
                ErişimEngellendiView();

            var model = new KongreTedarikçiModel();
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
        public virtual ActionResult KongreTedarikciEkle(KongreTedarikçiModel model, bool düzenlemeyeDevam)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KongreYönet))
                ErişimEngellendiView();
            if (ModelState.IsValid)
            {
                var tedarikçi = model.ToEntity();
                _tedarikçiServisi.KongreTedarikçiEkle(tedarikçi);
                BaşarılıBildirimi("Kongre tedarikçisı başarıyla Eklendi");
                _kullanıcıİşlemServisi.İşlemEkle("YeniMüşteriEklendi", "Yeni Müşteri Eklendi", tedarikçi.Adı);
                if (düzenlemeyeDevam)
                {
                    SeçiliTabKaydet();
                    return RedirectToAction("Düzenle", new { id = tedarikçi.Id });
                }
                return RedirectToAction("KongreTedarikciListe");
            }
            return View(model);
        }
        public virtual ActionResult KongreTedarikciDüzenle(int id)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KongreYönet))
                ErişimEngellendiView();
            var hekim = _tedarikçiServisi.KongreTedarikçiAlId(id);
            if (hekim == null)
            {
                return RedirectToAction("KongreTedarikciListe");
            }
            var model = hekim.ToModel();
            foreach (var tumSehirler in _konumServisi.SehirlerAlUlkeId(1))
            {
                var sehirModel = tumSehirler.ToModel();
                model.Sehirler.Add(sehirModel);
            }
            foreach (var tumIlceler in _konumServisi.IlcelerAlSehirId(hekim.SehirId))
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
        public virtual ActionResult KongreTedarikciDüzenle(KongreTedarikçiModel model, bool düzenlemeyeDevam)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KongreYönet))
                ErişimEngellendiView();
            var tedarikçi = _tedarikçiServisi.KongreTedarikçiAlId(model.Id);
            if (tedarikçi == null)
            {
                return RedirectToAction("KongreTedarikciListe");
            }
            if (ModelState.IsValid)
            {
                tedarikçi = model.ToEntity(tedarikçi);
                _tedarikçiServisi.KongreTedarikçiGüncelle(tedarikçi);
                BaşarılıBildirimi("KongreTedarikçiler başarıyla güncellenmiştir.");
                _kullanıcıİşlemServisi.İşlemEkle("KongreTedarikçilerGüncelle", "KongreTedarikçiler güncellendi", tedarikçi.Adı);
                if (düzenlemeyeDevam)
                {
                    return RedirectToAction("KongreTedarikçiDüzenle", new { id = tedarikçi.Id });
                }
                return RedirectToAction("KongreTedarikciListe");
            }
            return View(model);
        }

        [HttpPost]
        public virtual ActionResult KongreTedarikciSil(KongreTedarikçiModel model)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KongreYönet))
                return ErişimEngellendiView();

            var tedarikçi = _tedarikçiServisi.KongreTedarikçiAlId(model.Id);
            if (tedarikçi == null)
                return RedirectToAction("KongreTedarikciListe");
            _tedarikçiServisi.KongreTedarikçiSil(tedarikçi);
            BaşarılıBildirimi("KongreTedarikçiler başarıyla silindi");
            _kullanıcıİşlemServisi.İşlemEkle("KongreTedarikciSil", "KongreTedarikçiler silindi", tedarikçi.Adı);
            return RedirectToAction("KongreTedarikciListe");
        }
        #endregion
    }
}