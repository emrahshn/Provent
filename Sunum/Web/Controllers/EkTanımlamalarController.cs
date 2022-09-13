using Services.Güvenlik;
using Services.Sayfalar;
using Services.Seo;
using Services.Siteler;
using System.Linq;
using System.Web.Mvc;
using Web.Framework.Controllers;
using Web.Framework.Kendoui;
using Core.Domain.Sayfalar;
using System;
using Services.Kullanıcılar;
using Services.Logging;
using Services.EkTanımlamalar;
using Web.Uzantılar;
using Web.Models.EkTanımlamalar;

namespace Web.Controllers
{
    public class EkTanımlamalarController : TemelPublicController
    {
        private readonly IİzinServisi _izinServisi;
        private readonly ISiteServisi _siteServisi;
        private readonly IBankalarServisi _bankaServisi;
        private readonly ITedarikciSektorServisi _tedarikciServisi;
        private readonly IUnvanlarServisi _unvanServisi;
        private readonly IMusteriSektorServisi _musteriServisi;
        private readonly IHariciSektorServisi _hariciServisi;
        private readonly ITeklifKalemiServisi _teklifKalemiServisi;
        private readonly IUrlKayıtServisi _urlKayıtServisi;
        private readonly IAclServisi _aclServisi;
        private readonly IKullanıcıServisi _kullanıcıServisi;
        private readonly ISiteMappingServisi _siteMappingServisi;
        private readonly IKullanıcıİşlemServisi _kullanıcıİşlemServisi;
        private readonly ISayfaTemaServisi _sayfaTemaServisi;
        private readonly IGelirGiderTanımlamaServisi _gelirGiderServisi;
        private readonly ISponsorlukKalemleriServisi _sponsorlukServisi;
        private readonly IHekimBranşlarıServisi _branşServisi;
        private readonly ITedarikciKategorileriServisi _tKategoriServisi;
        private readonly IKayıtTipiServisi _kayıtTipiServisi;
        public EkTanımlamalarController(IİzinServisi izinServisi,
            ISiteServisi siteServisi,
            IBankalarServisi bankaServisi,
            IMusteriSektorServisi musteriServisi,
            ITedarikciSektorServisi tedarikciServisi,
            IHariciSektorServisi hariciServisi,
            ITeklifKalemiServisi teklifKalemiServisi,
            IUnvanlarServisi unvanServisi,
            IUrlKayıtServisi urlKayıtServisi,
            IAclServisi aclServisi,
            IKullanıcıServisi kullanıcıServisi,
            ISiteMappingServisi siteMappingServisi, 
            IKullanıcıİşlemServisi kullanıcıİşlemServisi,
            ISayfaTemaServisi sayfaTemaServisi,
            IGelirGiderTanımlamaServisi gelirGiderServisi,
            ISponsorlukKalemleriServisi sponsorlukServisi,
            IHekimBranşlarıServisi branşServisi,
            ITedarikciKategorileriServisi tKategoriServisi,
            IKayıtTipiServisi kayıtTipiServisi)
        {
            this._izinServisi = izinServisi;
            this._siteServisi = siteServisi;
            this._bankaServisi = bankaServisi;
            this._musteriServisi = musteriServisi;
            this._unvanServisi = unvanServisi;
            this._tedarikciServisi = tedarikciServisi;
            this._hariciServisi = hariciServisi;
            this._teklifKalemiServisi = teklifKalemiServisi;
            this._urlKayıtServisi = urlKayıtServisi;
            this._aclServisi = aclServisi;
            this._kullanıcıServisi = kullanıcıServisi;
            this._siteMappingServisi = siteMappingServisi;
            this._kullanıcıİşlemServisi = kullanıcıİşlemServisi;
            this._sayfaTemaServisi = sayfaTemaServisi;
            this._gelirGiderServisi = gelirGiderServisi;
            this._sponsorlukServisi = sponsorlukServisi;
            this._branşServisi = branşServisi;
            this._tKategoriServisi = tKategoriServisi;
            this._kayıtTipiServisi = kayıtTipiServisi;
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GelirKalemleriAl(bool gelir)
        {
            var kalemler = _gelirGiderServisi.AnaTeklifKalemleriAl(gelir);
            var sonuc = (from s in kalemler
                         select new
                         {
                             id = s.Id,
                             name = s.Adı
                         }).ToList();
            return Json(sonuc, JsonRequestBehavior.AllowGet);
        }

        #region Banka
        public virtual ActionResult BankaListe()
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.BankaYönet))
                return ErişimEngellendiView();

            var model = new BankaListeModel();
            //siteler
            model.MevcutSiteler.Add(new SelectListItem { Text = "Tümü", Value = "0" });
            foreach (var s in _siteServisi.TümSiteler())
                model.MevcutSiteler.Add(new SelectListItem { Text = s.Adı, Value = s.Id.ToString() });

            return View(model);
        }

        [HttpPost]
        public virtual ActionResult BankaListe(DataSourceİsteği command, BankaListeModel model)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.BankaYönet))
                return ErişimEngellendiKendoGridJson();

            var bankaModels = _bankaServisi.TümBankalarıAl()
                .Select(x => x.ToModel())
                .ToList();
            var gridModel = new DataSourceSonucu
            {
                Data = bankaModels,
                Toplam = bankaModels.Count
            };

            return Json(gridModel);
        }
        
        public virtual ActionResult BankaEkle()
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.BankaYönet))
                ErişimEngellendiView();

            var model = new BankaModel();
            return View(model);
        }
        
        [HttpPost,FormAdıParametresi("kaydet-devam","düzenlemeyeDevam")]
        public virtual ActionResult BankaEkle(BankaModel model,bool düzenlemeyeDevam)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.BankaYönet))
                ErişimEngellendiView();
            if (ModelState.IsValid)
            {
                var banka = model.ToEntity();
                _bankaServisi.BankaEkle(banka);
                BaşarılıBildirimi("Banka başarıyla Eklendi");
                _kullanıcıİşlemServisi.İşlemEkle("YeniBankaEklendi", "Yeni Banka Eklendi", banka.Adı);
                if (düzenlemeyeDevam)
                {
                    SeçiliTabKaydet();
                    return RedirectToAction("BankaDüzenle", new { id = banka.Id });
                }
                return RedirectToAction("BankaListe");
            }
            return View(model);
        }
        public virtual ActionResult BankaDüzenle(int id)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.BankaYönet))
                ErişimEngellendiView();
            var banka = _bankaServisi.BankaAlId(id);
            if (banka == null)
            {
                return RedirectToAction("BankaListe");
            }
            var model = banka.ToModel();
            return View(model);
        }
        [HttpPost,FormAdıParametresi("kaydet-devam","düzenlemeyeDevam")]
        public virtual ActionResult BankaDüzenle(BankaModel model,bool düzenlemeyeDevam)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.BankaYönet))
                ErişimEngellendiView();
            var banka = _bankaServisi.BankaAlId(model.Id);
            if (banka == null)
            {
                return RedirectToAction("BankaListe");
            }
            if (ModelState.IsValid)
            {
                banka = model.ToEntity(banka);
                _bankaServisi.BankaGüncelle(banka);
                BaşarılıBildirimi("Banka başarıyla güncellenmiştir.");
                _kullanıcıİşlemServisi.İşlemEkle("BankaGüncelle", "Banka güncellendi", banka.Adı);
                if(düzenlemeyeDevam)
                {
                    return RedirectToAction("Düzenle", new { id = banka.Id });
                }
                return RedirectToAction("BankaListe");
            }
            return View(model);
        }
        
        [HttpPost]
        public virtual ActionResult BankaSil(BankaModel model)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.BankaYönet))
                return ErişimEngellendiView();

            var banka = _bankaServisi.BankaAlId(model.Id);
            if (banka == null)
                return RedirectToAction("BankaListe");
            _bankaServisi.BankaSil(banka);
            BaşarılıBildirimi("Banka başarıyla silindi");
            _kullanıcıİşlemServisi.İşlemEkle("BankaSil", "Banka silindi", banka.Adı);
            return RedirectToAction("BankaListe");
        }
        #endregion
        #region Musteri
        public virtual ActionResult MusteriListe()
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.MusteriYönet))
                return ErişimEngellendiView();

            var model = new MusteriSektorModel();
            return View(model);
        }

        [HttpPost]
        public virtual ActionResult MusteriListe(DataSourceİsteği command, MusteriSektorModel model)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.MusteriYönet))
                return ErişimEngellendiKendoGridJson();

            var musteriModels = _musteriServisi.TümMusterilarıAl(true, true)
                .Select(x => x.ToModel())
                .ToList();
            var gridModel = new DataSourceSonucu
            {
                Data = musteriModels,
                Toplam = musteriModels.Count
            };

            return Json(gridModel);
        }

        public virtual ActionResult MusteriEkle()
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.MusteriYönet))
                ErişimEngellendiView();

            var model = new MusteriSektorModel();
            return View(model);
        }

        [HttpPost, FormAdıParametresi("kaydet-devam", "düzenlemeyeDevam")]
        public virtual ActionResult MusteriEkle(MusteriSektorModel model, bool düzenlemeyeDevam)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.MusteriYönet))
                ErişimEngellendiView();
            if (ModelState.IsValid)
            {
                var musteri = model.ToEntity();
                _musteriServisi.MusteriEkle(musteri);
                BaşarılıBildirimi("Müşteri sektörü başarıyla Eklendi");
                _kullanıcıİşlemServisi.İşlemEkle("YeniMüşteriEklendi", "Yeni Müşteri Eklendi", musteri.Adı);
                if (düzenlemeyeDevam)
                {
                    SeçiliTabKaydet();
                    return RedirectToAction("Düzenle", new { id = musteri.Id });
                }
                return RedirectToAction("MusteriListe");
            }
            return View(model);
        }
        public virtual ActionResult MusteriDüzenle(int id)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.MusteriYönet))
                ErişimEngellendiView();
            var musteri = _musteriServisi.MusteriAlId(id);
            if (musteri == null)
            {
                return RedirectToAction("MusteriListe");
            }
            var model = musteri.ToModel();
            return View(model);
        }
        [HttpPost, FormAdıParametresi("kaydet-devam", "düzenlemeyeDevam")]
        public virtual ActionResult MusteriDüzenle(MusteriSektorModel model, bool düzenlemeyeDevam)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.MusteriYönet))
                ErişimEngellendiView();
            var musteri = _musteriServisi.MusteriAlId(model.Id);
            if (musteri == null)
            {
                return RedirectToAction("MusteriListe");
            }
            if (ModelState.IsValid)
            {
                musteri = model.ToEntity(musteri);
                _musteriServisi.MusteriGüncelle(musteri);
                BaşarılıBildirimi("Musteri başarıyla güncellenmiştir.");
                _kullanıcıİşlemServisi.İşlemEkle("MusteriGüncelle", "Musteri güncellendi", musteri.Adı);
                if (düzenlemeyeDevam)
                {
                    return RedirectToAction("MusteriDüzenle", new { id = musteri.Id });
                }
                return RedirectToAction("MusteriListe");
            }
            return View(model);
        }

        [HttpPost]
        public virtual ActionResult MusteriSil(MusteriSektorModel model)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.MusteriYönet))
                return ErişimEngellendiView();

            var musteri = _musteriServisi.MusteriAlId(model.Id);
            if (musteri == null)
                return RedirectToAction("MusteriListe");
            _musteriServisi.MusteriSil(musteri);
            BaşarılıBildirimi("Musteri başarıyla silindi");
            _kullanıcıİşlemServisi.İşlemEkle("MusteriSil", "Musteri silindi", musteri.Adı);
            return RedirectToAction("MusteriListe");
        }
        #endregion
        #region tedarikci
        public virtual ActionResult TedarikciListe()
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.TedarikciYönet))
                return ErişimEngellendiView();

            var model = new TedarikciSektorModel();
            return View(model);
        }

        [HttpPost]
        public virtual ActionResult TedarikciListe(DataSourceİsteği command, TedarikciSektorModel model)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.MusteriYönet))
                return ErişimEngellendiKendoGridJson();

            var tedarikciModels = _tedarikciServisi.TümTedarikciSektorleriAl(true, true)
                .Select(x => x.ToModel())
                .ToList();
            var gridModel = new DataSourceSonucu
            {
                Data = tedarikciModels,
                Toplam = tedarikciModels.Count
            };

            return Json(gridModel);
        }

        public virtual ActionResult TedarikciEkle()
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.TedarikciYönet))
                ErişimEngellendiView();

            var model = new TedarikciSektorModel();
            return View(model);
        }

        [HttpPost, FormAdıParametresi("kaydet-devam", "düzenlemeyeDevam")]
        public virtual ActionResult TedarikciEkle(TedarikciSektorModel model, bool düzenlemeyeDevam)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.TedarikciYönet))
                ErişimEngellendiView();
            if (ModelState.IsValid)
            {
                var tedarikci = model.ToEntity();
                _tedarikciServisi.TedarikciSektorEkle(tedarikci);
                BaşarılıBildirimi("Tedarikci sektörü başarıyla Eklendi");
                _kullanıcıİşlemServisi.İşlemEkle("YeniTedarikciSektörüEklendi", "Yeni Tedarikçi Sektörü Eklendi", tedarikci.Adı);
                if (düzenlemeyeDevam)
                {
                    SeçiliTabKaydet();
                    return RedirectToAction("TedarikciDüzenle", new { id = tedarikci.Id });
                }
                return RedirectToAction("TedarikciListe");
            }
            return View(model);
        }
        public virtual ActionResult TedarikciDüzenle(int id)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.TedarikciYönet))
                ErişimEngellendiView();
            var tedarikci = _tedarikciServisi.TedarikciSektorAlId(id);
            if (tedarikci == null)
            {
                return RedirectToAction("TedarikciListe");
            }
            var model = tedarikci.ToModel();
            return View(model);
        }
        [HttpPost, FormAdıParametresi("kaydet-devam", "düzenlemeyeDevam")]
        public virtual ActionResult TedarikciDüzenle(TedarikciSektorModel model, bool düzenlemeyeDevam)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.MusteriYönet))
                ErişimEngellendiView();
            var tedarikci = _tedarikciServisi.TedarikciSektorAlId(model.Id);
            if (tedarikci == null)
            {
                return RedirectToAction("TedarikciListe");
            }
            if (ModelState.IsValid)
            {
                tedarikci = model.ToEntity(tedarikci);
                _tedarikciServisi.TedarikciSektorGüncelle(tedarikci);
                BaşarılıBildirimi("Tedarikci başarıyla güncellenmiştir.");
                _kullanıcıİşlemServisi.İşlemEkle("TedarikciSektorGüncelle", "Tedarikci Sektor güncellendi", tedarikci.Adı);
                if (düzenlemeyeDevam)
                {
                    return RedirectToAction("TedarikciDüzenle", new { id = tedarikci.Id });
                }
                return RedirectToAction("TedarikciListe");
            }
            return View(model);
        }

        [HttpPost]
        public virtual ActionResult TedarikciSil(TedarikciSektorModel model)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.MusteriYönet))
                return ErişimEngellendiView();

            var tedarikci = _tedarikciServisi.TedarikciSektorAlId(model.Id);
            if (tedarikci == null)
                return RedirectToAction("TedarikciListe");
            _tedarikciServisi.TedarikciSektorSil(tedarikci);
            BaşarılıBildirimi("Tedarikci sektörü başarıyla silindi");
            _kullanıcıİşlemServisi.İşlemEkle("TedarikciSektorSil", "Tedarikci Sektörü silindi", tedarikci.Adı);
            return RedirectToAction("TedarikciListe");
        }
        #endregion
        #region harici
        public virtual ActionResult HariciListe()
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.HariciYönet))
                return ErişimEngellendiView();

            var model = new HariciSektorModel();
            return View(model);
        }

        [HttpPost]
        public virtual ActionResult HariciListe(DataSourceİsteği command, HariciSektorModel model)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.HariciYönet))
                return ErişimEngellendiKendoGridJson();

            var hariciModels = _hariciServisi.TümHariciSektorleriAl(true, true)
                .Select(x => x.ToModel())
                .ToList();
            var gridModel = new DataSourceSonucu
            {
                Data = hariciModels,
                Toplam = hariciModels.Count
            };

            return Json(gridModel);
        }

        public virtual ActionResult HariciEkle()
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.HariciYönet))
                ErişimEngellendiView();

            var model = new HariciSektorModel();
            return View(model);
        }

        [HttpPost, FormAdıParametresi("kaydet-devam", "düzenlemeyeDevam")]
        public virtual ActionResult HariciEkle(HariciSektorModel model, bool düzenlemeyeDevam)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.HariciYönet))
                ErişimEngellendiView();
            if (ModelState.IsValid)
            {
                var harici = model.ToEntity();
                _hariciServisi.HariciSektorEkle(harici);
                BaşarılıBildirimi("Harici sektörü başarıyla Eklendi");
                _kullanıcıİşlemServisi.İşlemEkle("YeniHariciSektörüEklendi", "Yeni Tedarikçi Sektörü Eklendi", harici.Adı);
                if (düzenlemeyeDevam)
                {
                    SeçiliTabKaydet();
                    return RedirectToAction("HariciDüzenle", new { id = harici.Id });
                }
                return RedirectToAction("HariciListe");
            }
            return View(model);
        }
        public virtual ActionResult HariciDüzenle(int id)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.HariciYönet))
                ErişimEngellendiView();
            var harici = _hariciServisi.HariciSektorAlId(id);
            if (harici == null)
            {
                return RedirectToAction("HariciListe");
            }
            var model = harici.ToModel();
            return View(model);
        }
        [HttpPost, FormAdıParametresi("kaydet-devam", "düzenlemeyeDevam")]
        public virtual ActionResult HariciDüzenle(HariciSektorModel model, bool düzenlemeyeDevam)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.MusteriYönet))
                ErişimEngellendiView();
            var harici = _hariciServisi.HariciSektorAlId(model.Id);
            if (harici == null)
            {
                return RedirectToAction("HariciListe");
            }
            if (ModelState.IsValid)
            {
                harici = model.ToEntity(harici);
                _hariciServisi.HariciSektorGüncelle(harici);
                BaşarılıBildirimi("Harici başarıyla güncellenmiştir.");
                _kullanıcıİşlemServisi.İşlemEkle("HariciSektorGüncelle", "Harici Sektor güncellendi", harici.Adı);
                if (düzenlemeyeDevam)
                {
                    return RedirectToAction("HariciDüzenle", new { id = harici.Id });
                }
                return RedirectToAction("HariciListe");
            }
            return View(model);
        }

        [HttpPost]
        public virtual ActionResult HariciSil(HariciSektorModel model)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.MusteriYönet))
                return ErişimEngellendiView();

            var harici = _hariciServisi.HariciSektorAlId(model.Id);
            if (harici == null)
                return RedirectToAction("HariciListe");
            _hariciServisi.HariciSektorSil(harici);
            BaşarılıBildirimi("Harici sektörü başarıyla silindi");
            _kullanıcıİşlemServisi.İşlemEkle("HariciSektorSil", "Harici Sektörü silindi", harici.Adı);
            return RedirectToAction("HariciListe");
        }
        #endregion
        #region teklif kalemleri
        public virtual ActionResult TeklifKalemiListe()
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.TeklifKalemiYönet))
                return ErişimEngellendiView();

            var model = new TeklifKalemiModel();
            return View(model);
        }

        [HttpPost]
        public virtual ActionResult TeklifKalemiListe(DataSourceİsteği command, TeklifKalemiModel model)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.TeklifKalemiYönet))
                return ErişimEngellendiKendoGridJson();

            var teklifKalemiModels = _teklifKalemiServisi.TümTeklifKalemleriAl(true, true)
                .Select(x => 
                {
                    var m = x.ToModel();
                    int nodeid = 0;
                    if (m.NodeId != null)
                        nodeid = Convert.ToInt32(m.NodeId);
                    m.NodeAdı = nodeid == 0 ? "" : _teklifKalemiServisi.TeklifKalemiAlId(nodeid).Adı;
                    return m;
                })
                .ToList();
            var gridModel = new DataSourceSonucu
            {
                Data = teklifKalemiModels,
                Toplam = teklifKalemiModels.Count()
            };
            return Json(gridModel);
        }

        public virtual ActionResult TeklifKalemiEkle()
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.TeklifKalemiYönet))
                ErişimEngellendiView();
            var model = new TeklifKalemiModel();
            foreach (var anaTeklifKalemi in _teklifKalemiServisi.AnaTeklifKalemleriAl())
            {
                var AnaTeklifKalemi = anaTeklifKalemi.ToModel();
                model.AnaTeklifKalemi.Add(anaTeklifKalemi);
            }
            return View(model);
        }

        [HttpPost, FormAdıParametresi("kaydet-devam", "düzenlemeyeDevam")]
        public virtual ActionResult TeklifKalemiEkle(TeklifKalemiModel model, bool düzenlemeyeDevam)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.TeklifKalemiYönet))
                ErişimEngellendiView();
            if (ModelState.IsValid)
            {
                var teklifKalemi = model.ToEntity();
                if (model.AnaTeklifKalemicb)
                    teklifKalemi.NodeId = null;
                _teklifKalemiServisi.TeklifKalemiEkle(teklifKalemi);
                BaşarılıBildirimi("Teklif Kalemi sektörü başarıyla Eklendi");
                _kullanıcıİşlemServisi.İşlemEkle("YeniTeklifKalemiSektörüEklendi", "Yeni Teklif Kalemi Eklendi", teklifKalemi.Adı);
                if (düzenlemeyeDevam)
                {
                    SeçiliTabKaydet();
                    return RedirectToAction("TeklifKalemiDüzenle", new { id = teklifKalemi.Id });
                }
                
                return RedirectToAction("TeklifKalemiListe");
            }
            return View(model);
        }
        public virtual ActionResult TeklifKalemiDüzenle(int id)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.TeklifKalemiYönet))
                ErişimEngellendiView();
            var teklifKalemi = _teklifKalemiServisi.TeklifKalemiAlId(id);
            if (teklifKalemi == null)
            {
                return RedirectToAction("TeklifKalemiListe");
            }
            var model = teklifKalemi.ToModel();
            foreach (var anaTeklifKalemi in _teklifKalemiServisi.AnaTeklifKalemleriAl())
            {
                var AnaTeklifKalemi = anaTeklifKalemi.ToModel();
                model.AnaTeklifKalemi.Add(anaTeklifKalemi);
            }
            if (model.NodeId == null || model.NodeId == 0)
                model.AnaTeklifKalemicb = true;
            return View(model);
        }
        [HttpPost, FormAdıParametresi("kaydet-devam", "düzenlemeyeDevam")]
        public virtual ActionResult TeklifKalemiDüzenle(TeklifKalemiModel model, bool düzenlemeyeDevam)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.MusteriYönet))
                ErişimEngellendiView();
            var teklifKalemi = _teklifKalemiServisi.TeklifKalemiAlId(model.Id);
            if (teklifKalemi == null)
            {
                return RedirectToAction("TeklifKalemiListe");
            }
            if (ModelState.IsValid)
            {
                teklifKalemi = model.ToEntity(teklifKalemi);
                if (model.AnaTeklifKalemicb)
                    teklifKalemi.NodeId = null;
                _teklifKalemiServisi.TeklifKalemiGüncelle(teklifKalemi);
                BaşarılıBildirimi("Teklif Kalemi başarıyla güncellenmiştir.");
                _kullanıcıİşlemServisi.İşlemEkle("TeklifKalemiSektorGüncelle", "Teklif Kalemi güncellendi", teklifKalemi.Adı);
                if (düzenlemeyeDevam)
                {
                    return RedirectToAction("TeklifKalemiDüzenle", new { id = teklifKalemi.Id });
                }
                return RedirectToAction("TeklifKalemiListe");
            }
            return View(model);
        }

        [HttpPost]
        public virtual ActionResult TeklifKalemiSil(TeklifKalemiModel model)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.MusteriYönet))
                return ErişimEngellendiView();

            var teklifKalemi = _teklifKalemiServisi.TeklifKalemiAlId(model.Id);
            if (teklifKalemi == null)
                return RedirectToAction("TeklifKalemiListe");
            _teklifKalemiServisi.TeklifKalemiSil(teklifKalemi);
            BaşarılıBildirimi("Teklif Kalemi başarıyla silindi");
            _kullanıcıİşlemServisi.İşlemEkle("TeklifKalemiSektorSil", "Teklif Kalemi silindi", teklifKalemi.Adı);
            return RedirectToAction("TeklifKalemiListe");
        }
        #endregion
        #region Unvanlar
        public virtual ActionResult UnvanListe()
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.UnvanYönet))
                return ErişimEngellendiView();

            var model = new UnvanlarModel();
            return View(model);
        }

        [HttpPost]
        public virtual ActionResult UnvanListe(DataSourceİsteği command, UnvanlarModel model)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.UnvanYönet))
                return ErişimEngellendiKendoGridJson();

            var unvanModels = _unvanServisi.TümUnvanlarıAl(true, true)
                .Select(x => x.ToModel())
                .ToList();
            var gridModel = new DataSourceSonucu
            {
                Data = unvanModels,
                Toplam = unvanModels.Count
            };

            return Json(gridModel);
        }

        public virtual ActionResult UnvanEkle()
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.UnvanYönet))
                ErişimEngellendiView();

            var model = new UnvanlarModel();
            return View(model);
        }

        [HttpPost, FormAdıParametresi("kaydet-devam", "düzenlemeyeDevam")]
        public virtual ActionResult UnvanEkle(UnvanlarModel model, bool düzenlemeyeDevam)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.UnvanYönet))
                ErişimEngellendiView();
            if (ModelState.IsValid)
            {
                var unvan = model.ToEntity();
                _unvanServisi.UnvanlarEkle(unvan);
                BaşarılıBildirimi("Müşteri sektörü başarıyla Eklendi");
                _kullanıcıİşlemServisi.İşlemEkle("YeniMüşteriEklendi", "Yeni Müşteri Eklendi", unvan.Adı);
                if (düzenlemeyeDevam)
                {
                    SeçiliTabKaydet();
                    return RedirectToAction("Düzenle", new { id = unvan.Id });
                }
                return RedirectToAction("UnvanListe");
            }
            return View(model);
        }
        public virtual ActionResult UnvanDüzenle(int id)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.UnvanYönet))
                ErişimEngellendiView();
            var unvan = _unvanServisi.UnvanlarAlId(id);
            if (unvan == null)
            {
                return RedirectToAction("UnvanListe");
            }
            var model = unvan.ToModel();
            return View(model);
        }
        [HttpPost, FormAdıParametresi("kaydet-devam", "düzenlemeyeDevam")]
        public virtual ActionResult UnvanDüzenle(UnvanlarModel model, bool düzenlemeyeDevam)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.UnvanYönet))
                ErişimEngellendiView();
            var unvan = _unvanServisi.UnvanlarAlId(model.Id);
            if (unvan == null)
            {
                return RedirectToAction("UnvanListe");
            }
            if (ModelState.IsValid)
            {
                unvan = model.ToEntity(unvan);
                _unvanServisi.UnvanlarGüncelle(unvan);
                BaşarılıBildirimi("Unvan başarıyla güncellenmiştir.");
                _kullanıcıİşlemServisi.İşlemEkle("UnvanGüncelle", "Unvan güncellendi", unvan.Adı);
                if (düzenlemeyeDevam)
                {
                    return RedirectToAction("UnvanDüzenle", new { id = unvan.Id });
                }
                return RedirectToAction("UnvanListe");
            }
            return View(model);
        }

        [HttpPost]
        public virtual ActionResult UnvanSil(UnvanlarModel model)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.UnvanYönet))
                return ErişimEngellendiView();

            var unvan = _unvanServisi.UnvanlarAlId(model.Id);
            if (unvan == null)
                return RedirectToAction("UnvanListe");
            _unvanServisi.UnvanlarSil(unvan);
            BaşarılıBildirimi("Unvan başarıyla silindi");
            _kullanıcıİşlemServisi.İşlemEkle("UnvanSil", "Unvan silindi", unvan.Adı);
            return RedirectToAction("UnvanListe");
        }
        #endregion
        #region GelirGider
        public virtual ActionResult GelirGiderListe()
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.OtelYönet))
                return ErişimEngellendiView();

            var model = new GelirGiderTanımlamaModel();
            return View(model);
        }

        [HttpPost]
        public virtual ActionResult GelirGiderListe(DataSourceİsteği command, GelirGiderTanımlamaModel model)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.OtelYönet))
                return ErişimEngellendiKendoGridJson();

            var gelir = _gelirGiderServisi.TümGelirGiderTanımlamaAl()
                .Select(x => x.ToModel())
                .ToList();
            var gridModel = new DataSourceSonucu
            {
                Data = gelir,
                Toplam = gelir.Count
            };

            return Json(gridModel);
        }

        public virtual ActionResult GelirGiderEkle()
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.OtelYönet))
                ErişimEngellendiView();

            var model = new GelirGiderTanımlamaModel();
            foreach (var anaTeklifKalemi in _gelirGiderServisi.AnaTeklifKalemleriAl(false))
            {
                var AnaTeklifKalemi = anaTeklifKalemi.ToModel();
                model.AnaTeklifKalemi.Add(anaTeklifKalemi);
            }
            return View(model);
        }

        [HttpPost, FormAdıParametresi("kaydet-devam", "düzenlemeyeDevam")]
        public virtual ActionResult GelirGiderEkle(GelirGiderTanımlamaModel model, bool düzenlemeyeDevam)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.OtelYönet))
                ErişimEngellendiView();
            if (ModelState.IsValid)
            {
                var gelir = model.ToEntity();
                _gelirGiderServisi.GelirGiderTanımlamaEkle(gelir);
                BaşarılıBildirimi("Gelir gider kalemi sektörü başarıyla Eklendi");
                _kullanıcıİşlemServisi.İşlemEkle("GelirGiderKalemiEklendi", "Yeni Kalem Eklendi", gelir.Adı);
                if (düzenlemeyeDevam)
                {
                    SeçiliTabKaydet();
                    return RedirectToAction("Düzenle", new { id = gelir.Id });
                }
                return RedirectToAction("GelirGiderListe");
            }
            return View(model);
        }
        public virtual ActionResult GelirGiderDüzenle(int id)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.OtelYönet))
                ErişimEngellendiView();
            var gelir = _gelirGiderServisi.GelirGiderTanımlamaAlId(id);
            if (gelir == null)
            {
                return RedirectToAction("GelirGiderListe");
            }
            var model = gelir.ToModel();
            foreach (var anaTeklifKalemi in _gelirGiderServisi.AnaTeklifKalemleriAl(false))
            {
                var AnaTeklifKalemi = anaTeklifKalemi.ToModel();
                model.AnaTeklifKalemi.Add(anaTeklifKalemi);
            }
            return View(model);
        }
        [HttpPost, FormAdıParametresi("kaydet-devam", "düzenlemeyeDevam")]
        public virtual ActionResult GelirGiderDüzenle(GelirGiderTanımlamaModel model, bool düzenlemeyeDevam)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.OtelYönet))
                ErişimEngellendiView();
            var gelir = _gelirGiderServisi.GelirGiderTanımlamaAlId(model.Id);
            if (gelir == null)
            {
                return RedirectToAction("GelirGiderDüzenle");
            }
            if (ModelState.IsValid)
            {
                gelir = model.ToEntity(gelir);
                _gelirGiderServisi.GelirGiderTanımlamaGüncelle(gelir);
                BaşarılıBildirimi("Gelir gider kalemi sektörü başarıyla güncellenmiştir.");
                _kullanıcıİşlemServisi.İşlemEkle("GelirGiderGüncelle", "GelirGider güncellendi", gelir.Adı);
                if (düzenlemeyeDevam)
                {
                    return RedirectToAction("GelirGiderDüzenle", new { id = gelir.Id });
                }
                return RedirectToAction("GelirGiderListe");
            }
            return View(model);
        }

        [HttpPost]
        public virtual ActionResult GelirGiderSil(GelirGiderTanımlamaModel model)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.OtelYönet))
                return ErişimEngellendiView();

            var gelir = _gelirGiderServisi.GelirGiderTanımlamaAlId(model.Id);
            if (gelir == null)
                return RedirectToAction("GelirGiderListe");
            _gelirGiderServisi.GelirGiderTanımlamaSil(gelir);
            BaşarılıBildirimi("Unvan başarıyla silindi");
            _kullanıcıİşlemServisi.İşlemEkle("UnvanSil", "Unvan silindi", gelir.Adı);
            return RedirectToAction("GelirGiderListe");
        }
        #endregion
        #region Sponsorluklar
        public virtual ActionResult SponsorlukListe()
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.OtelYönet))
                return ErişimEngellendiView();

            var model = new SponsorlukKalemleriModel();
            return View(model);
        }

        [HttpPost]
        public virtual ActionResult SponsorlukListe(DataSourceİsteği command, SponsorlukKalemleriModel model)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.OtelYönet))
                return ErişimEngellendiKendoGridJson();

            var gelir = _sponsorlukServisi.TümSponsorlukKalemleriAl()
                .Select(x => x.ToModel())
                .ToList();
            var gridModel = new DataSourceSonucu
            {
                Data = gelir,
                Toplam = gelir.Count
            };

            return Json(gridModel);
        }

        public virtual ActionResult SponsorlukEkle()
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.OtelYönet))
                ErişimEngellendiView();

            var model = new SponsorlukKalemleriModel();
            return View(model);
        }

        [HttpPost, FormAdıParametresi("kaydet-devam", "düzenlemeyeDevam")]
        public virtual ActionResult SponsorlukEkle(SponsorlukKalemleriModel model, bool düzenlemeyeDevam)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.OtelYönet))
                ErişimEngellendiView();
            if (ModelState.IsValid)
            {
                var spo = model.ToEntity();
                _sponsorlukServisi.SponsorlukKalemleriEkle(spo);
                BaşarılıBildirimi("Sponsorluk kalemi sektörü başarıyla Eklendi");
                _kullanıcıİşlemServisi.İşlemEkle("SponsorlukKalemiEklendi", "Yeni Kalem Eklendi", spo.Adı);
                if (düzenlemeyeDevam)
                {
                    SeçiliTabKaydet();
                    return RedirectToAction("SponsorlukDüzenle", new { id = spo.Id });
                }
                return RedirectToAction("SponsorlukListe");
            }
            return View(model);
        }
        public virtual ActionResult SponsorlukDüzenle(int id)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.OtelYönet))
                ErişimEngellendiView();
            var spo = _sponsorlukServisi.SponsorlukKalemleriAlId(id);
            if (spo == null)
            {
                return RedirectToAction("SponsorlukListe");
            }
            var model = spo.ToModel();
            return View(model);
        }
        [HttpPost, FormAdıParametresi("kaydet-devam", "düzenlemeyeDevam")]
        public virtual ActionResult SponsorlukDüzenle(SponsorlukKalemleriModel model, bool düzenlemeyeDevam)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.OtelYönet))
                ErişimEngellendiView();
            var spo = _sponsorlukServisi.SponsorlukKalemleriAlId(model.Id);
            if (spo == null)
            {
                return RedirectToAction("SponsorlukListe");
            }
            if (ModelState.IsValid)
            {
                spo = model.ToEntity(spo);
                _sponsorlukServisi.SponsorlukKalemleriGüncelle(spo);
                BaşarılıBildirimi("Sponsorluk kalemi sektörü başarıyla güncellenmiştir.");
                _kullanıcıİşlemServisi.İşlemEkle("GSponsorlukGüncelle", "Sponsorluk güncellendi", spo.Adı);
                if (düzenlemeyeDevam)
                {
                    return RedirectToAction("SponsorlukDüzenle", new { id = spo.Id });
                }
                return RedirectToAction("SponsorlukListe");
            }
            return View(model);
        }

        [HttpPost]
        public virtual ActionResult SponsorlukSil(SponsorlukKalemleriModel model)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.OtelYönet))
                return ErişimEngellendiView();

            var spo = _sponsorlukServisi.SponsorlukKalemleriAlId(model.Id);
            if (spo == null)
                return RedirectToAction("SponsorlukListe");
            _sponsorlukServisi.SponsorlukKalemleriSil(spo);
            BaşarılıBildirimi("Kalem başarıyla silindi");
            _kullanıcıİşlemServisi.İşlemEkle("SponsorlukSil", "Kalem silindi", spo.Adı);
            return RedirectToAction("SponsorlukListe");
        }
        #endregion
        #region Branslar
        public virtual ActionResult BransListe()
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KongreYönet))
                return ErişimEngellendiView();

            var model = new HekimBranşlarıModel();
            return View(model);
        }

        [HttpPost]
        public virtual ActionResult BransListe(DataSourceİsteği command, HekimBranşlarıModel model)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KongreYönet))
                return ErişimEngellendiKendoGridJson();

            var branşModels = _branşServisi.TümHekimBranşlarıAl()
                .Select(x => x.ToModel())
                .ToList();
            var gridModel = new DataSourceSonucu
            {
                Data = branşModels,
                Toplam = branşModels.Count
            };

            return Json(gridModel);
        }

        public virtual ActionResult BransEkle()
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KongreYönet))
                ErişimEngellendiView();

            var model = new HekimBranşlarıModel();
            return View(model);
        }

        [HttpPost, FormAdıParametresi("kaydet-devam", "düzenlemeyeDevam")]
        public virtual ActionResult BransEkle(HekimBranşlarıModel model, bool düzenlemeyeDevam,string returnUrl)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KongreYönet))
                ErişimEngellendiView();
            if (ModelState.IsValid)
            {
                var branş = model.ToEntity();
                _branşServisi.HekimBranşlarıEkle(branş);
                BaşarılıBildirimi("Hekim branşı başarıyla Eklendi");
                _kullanıcıİşlemServisi.İşlemEkle("YeniBransEklendi", "Yeni Branş Eklendi", branş.Adı);
                if (düzenlemeyeDevam)
                {
                    SeçiliTabKaydet();
                    return RedirectToAction("Düzenle", new { id = branş.Id });
                }
                if (returnUrl !=null)
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction("BransListe");
                }
                
            }
            return View(model);
        }
        public virtual ActionResult BransDüzenle(int id)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KongreYönet))
                ErişimEngellendiView();
            var branş = _branşServisi.HekimBranşlarıAlId(id);
            if (branş == null)
            {
                return RedirectToAction("BransListe");
            }
            var model = branş.ToModel();
            return View(model);
        }
        [HttpPost, FormAdıParametresi("kaydet-devam", "düzenlemeyeDevam")]
        public virtual ActionResult BransDüzenle(HekimBranşlarıModel model, bool düzenlemeyeDevam)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KongreYönet))
                ErişimEngellendiView();
            var branş = _branşServisi.HekimBranşlarıAlId(model.Id);
            if (branş == null)
            {
                return RedirectToAction("BransListe");
            }
            if (ModelState.IsValid)
            {
                branş = model.ToEntity(branş);
                _branşServisi.HekimBranşlarıGüncelle(branş);
                BaşarılıBildirimi("Brans başarıyla güncellenmiştir.");
                _kullanıcıİşlemServisi.İşlemEkle("BransGüncelle", "Brans güncellendi", branş.Adı);
                if (düzenlemeyeDevam)
                {
                    return RedirectToAction("BransDüzenle", new { id = branş.Id });
                }
                return RedirectToAction("BransListe");
            }
            return View(model);
        }

        [HttpPost]
        public virtual ActionResult BransSil(HekimBranşlarıModel model)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KongreYönet))
                return ErişimEngellendiView();

            var branş = _branşServisi.HekimBranşlarıAlId(model.Id);
            if (branş == null)
                return RedirectToAction("BransListe");
            _branşServisi.HekimBranşlarıSil(branş);
            BaşarılıBildirimi("Brans başarıyla silindi");
            _kullanıcıİşlemServisi.İşlemEkle("BransSil", "Brans silindi", branş.Adı);
            return RedirectToAction("BransListe");
        }
        #endregion
        #region TedarikciKategori
        public virtual ActionResult TedarikciKategoriListe()
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KongreYönet))
                return ErişimEngellendiView();

            var model = new TedarikciKategorileriModel();
            return View(model);
        }

        [HttpPost]
        public virtual ActionResult TedarikciKategoriListe(DataSourceİsteği command, HekimBranşlarıModel model)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KongreYönet))
                return ErişimEngellendiKendoGridJson();

            var tKategoriModels = _tKategoriServisi.TümTedarikciKategorileriAl()
                .Select(x => x.ToModel())
                .ToList();
            var gridModel = new DataSourceSonucu
            {
                Data = tKategoriModels,
                Toplam = tKategoriModels.Count
            };

            return Json(gridModel);
        }

        public virtual ActionResult TedarikciKategoriEkle()
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KongreYönet))
                ErişimEngellendiView();

            var model = new TedarikciKategorileriModel();
            return View(model);
        }

        [HttpPost, FormAdıParametresi("kaydet-devam", "düzenlemeyeDevam")]
        public virtual ActionResult TedarikciKategoriEkle(TedarikciKategorileriModel model, bool düzenlemeyeDevam)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KongreYönet))
                ErişimEngellendiView();
            if (ModelState.IsValid)
            {
                var tKategori = model.ToEntity();
                _tKategoriServisi.TedarikciKategorileriEkle(tKategori);
                BaşarılıBildirimi("Tedarikçi kategorisi başarıyla Eklendi");
                _kullanıcıİşlemServisi.İşlemEkle("YeniTedarikciKategoriEklendi", "Yeni Tedarikci Kategori Eklendi", tKategori.Adı);
                if (düzenlemeyeDevam)
                {
                    SeçiliTabKaydet();
                    return RedirectToAction("Düzenle", new { id = tKategori.Id });
                }
                return RedirectToAction("TedarikciKategoriListe");
            }
            return View(model);
        }
        public virtual ActionResult TedarikciKategoriDüzenle(int id)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KongreYönet))
                ErişimEngellendiView();
            var tKategori = _tKategoriServisi.TedarikciKategorileriAlId(id);
            if (tKategori == null)
            {
                return RedirectToAction("TedarikciKategoriListe");
            }
            var model = tKategori.ToModel();
            return View(model);
        }
        [HttpPost, FormAdıParametresi("kaydet-devam", "düzenlemeyeDevam")]
        public virtual ActionResult TedarikciKategoriDüzenle(TedarikciKategorileriModel model, bool düzenlemeyeDevam)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KongreYönet))
                ErişimEngellendiView();
            var tKategori = _tKategoriServisi.TedarikciKategorileriAlId(model.Id);
            if (tKategori == null)
            {
                return RedirectToAction("TedarikciKategoriListe");
            }
            if (ModelState.IsValid)
            {
                tKategori = model.ToEntity(tKategori);
                _tKategoriServisi.TedarikciKategorileriGüncelle(tKategori);
                BaşarılıBildirimi("Tedarikçi kategorisi güncellenmiştir.");
                _kullanıcıİşlemServisi.İşlemEkle("TedarikciKategoriGüncelle", "Tedarikci kategori güncellendi", tKategori.Adı);
                if (düzenlemeyeDevam)
                {
                    return RedirectToAction("TedarikciKategoriDüzenle", new { id = tKategori.Id });
                }
                return RedirectToAction("TedarikciKategoriListe");
            }
            return View(model);
        }

        [HttpPost]
        public virtual ActionResult TedarikciKategoriSil(TedarikciKategorileriModel model)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KongreYönet))
                return ErişimEngellendiView();

            var tKategori = _tKategoriServisi.TedarikciKategorileriAlId(model.Id);
            if (tKategori == null)
                return RedirectToAction("TedarikciKategoriListe");
            _tKategoriServisi.TedarikciKategorileriSil(tKategori);
            BaşarılıBildirimi("Tedarikçi kategorisi başarıyla silindi");
            _kullanıcıİşlemServisi.İşlemEkle("TedarikciKategoriSil", "Tedarikci kategori silindi", tKategori.Adı);
            return RedirectToAction("TedarikciKategoriListe");
        }
        #endregion
        #region KayıtTipi
        public virtual ActionResult KayıtTipiListe()
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KongreYönet))
                return ErişimEngellendiView();

            var model = new KayıtTipiModel();
            return View(model);
        }

        [HttpPost]
        public virtual ActionResult KayıtTipiListe(DataSourceİsteği command, KayıtTipiModel model)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KongreYönet))
                return ErişimEngellendiKendoGridJson();

            var kayitTipiModels = _kayıtTipiServisi.TümKayıtTipiAl()
                .Select(x => x.ToModel())
                .ToList();
            var gridModel = new DataSourceSonucu
            {
                Data = kayitTipiModels,
                Toplam = kayitTipiModels.Count
            };

            return Json(gridModel);
        }

        public virtual ActionResult KayıtTipiEkle()
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KongreYönet))
                ErişimEngellendiView();

            var model = new KayıtTipiModel();
            return View(model);
        }

        [HttpPost, FormAdıParametresi("kaydet-devam", "düzenlemeyeDevam")]
        public virtual ActionResult KayıtTipiEkle(KayıtTipiModel model, bool düzenlemeyeDevam, string returnUrl)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KongreYönet))
                ErişimEngellendiView();
            if (ModelState.IsValid)
            {
                var kayit = model.ToEntity();
                _kayıtTipiServisi.KayıtTipiEkle(kayit);
                BaşarılıBildirimi("Kayıt tipi başarıyla Eklendi");
                _kullanıcıİşlemServisi.İşlemEkle("YeniKayıtTipiEklendi", "Yeni Kayıt Tipi Eklendi", kayit.Adı);
                if (düzenlemeyeDevam)
                {
                    SeçiliTabKaydet();
                    return RedirectToAction("Düzenle", new { id = kayit.Id });
                }
                if (returnUrl != null)
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction("KayıtTipiListe");
                }

            }
            return View(model);
        }
        public virtual ActionResult KayıtTipiDüzenle(int id)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KongreYönet))
                ErişimEngellendiView();
            var kayit = _kayıtTipiServisi.KayıtTipiAlId(id);
            if (kayit == null)
            {
                return RedirectToAction("KayıtTipiListe");
            }
            var model = kayit.ToModel();
            return View(model);
        }
        [HttpPost, FormAdıParametresi("kaydet-devam", "düzenlemeyeDevam")]
        public virtual ActionResult KayıtTipiDüzenle(KayıtTipiModel model, bool düzenlemeyeDevam)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KongreYönet))
                ErişimEngellendiView();
            var kayit = _kayıtTipiServisi.KayıtTipiAlId(model.Id);
            if (kayit == null)
            {
                return RedirectToAction("KayıtTipiListe");
            }
            if (ModelState.IsValid)
            {
                kayit = model.ToEntity(kayit);
                _kayıtTipiServisi.KayıtTipiGüncelle(kayit);
                BaşarılıBildirimi("Kayıt tipi başarıyla güncellenmiştir.");
                _kullanıcıİşlemServisi.İşlemEkle("KayıtTipiGüncelle", "Kayıt tipi güncellendi", kayit.Adı);
                if (düzenlemeyeDevam)
                {
                    return RedirectToAction("KayıtTipiDüzenle", new { id = kayit.Id });
                }
                return RedirectToAction("KayıtTipiListe");
            }
            return View(model);
        }

        [HttpPost]
        public virtual ActionResult KayıtTipiSil(KayıtTipiModel model)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KongreYönet))
                return ErişimEngellendiView();

            var kayit = _kayıtTipiServisi.KayıtTipiAlId(model.Id);
            if (kayit == null)
                return RedirectToAction("KayıtTipiListe");
            _kayıtTipiServisi.KayıtTipiSil(kayit);
            BaşarılıBildirimi("Kayıt tipi başarıyla silindi");
            _kullanıcıİşlemServisi.İşlemEkle("KayıtTipiSil", "KayıtTipi silindi", kayit.Adı);
            return RedirectToAction("KayıtTipiListe");
        }
        #endregion
    }
}