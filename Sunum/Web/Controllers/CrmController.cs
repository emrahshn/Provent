using Services.Güvenlik;
using Services.Siteler;
using System.Linq;
using System.Web.Mvc;
using Web.Framework.Controllers;
using Web.Framework.Kendoui;
using Web.Uzantılar;
using System;
using Services.Logging;
using Services.Konum;
using System.Collections.Generic;
using Web.Models.Crm;
using Services.Crm;
using Core.Domain.CRM;
using Services.Kullanıcılar;
using Core;
using Services.EkTanımlamalar;

namespace Web.Controllers
{
    public class CrmController : TemelPublicController
    {
        private readonly IİzinServisi _izinServisi;
        private readonly IKullanıcıİşlemServisi _kullanıcıİşlemServisi;
        private readonly ISiteServisi _siteServisi;
        private readonly IKonumServisi _konumServisi;
        private readonly ICrmUnvanServisi _unvanServisi;
        private readonly ICrmKisiServisi _kisiServisi;
        private readonly ICrmKurumServisi _kurumServisi;
        private readonly ICrmGorusmeServisi _gorusmeServisi;
        private readonly ICrmFirmaGorusmeServisi _firmaGorusmeServisi;
        private readonly IKullanıcıServisi _kullanıcıServisi;
        private readonly ICrmYonetimKuruluServisi _ykServisi;
        private readonly ICrmGorevServisi _gorevServisi;
        private readonly ICrmFirmaServisi _firmaServisi;
        private readonly ICrmFirmaYetkilisiServisi _firmayetkilisiServisi;
        private readonly ICrmKongreServisi _kongreServisi;
        private readonly IUnvanlarServisi _unvanlarServisi;
        private readonly IWorkContext _workContext;

        public CrmController(IİzinServisi izinServisi,
            ISiteServisi siteServisi,
            IKonumServisi konumServisi,
            IKullanıcıİşlemServisi kullanıcıİşlemServisi,
            ICrmUnvanServisi unvanServisi,
            ICrmKisiServisi kisiServisi,
            ICrmKurumServisi kurumServisi,
            ICrmGorusmeServisi gorusmeServisi,
            ICrmFirmaGorusmeServisi firmaGorusmeServisi,
            IKullanıcıServisi kullanıcıServisi,
            ICrmYonetimKuruluServisi ykServisi,
            ICrmGorevServisi gorevServisi,
            ICrmFirmaServisi firmaServisi,
            ICrmFirmaYetkilisiServisi firmayetkilisiServisi,
            ICrmKongreServisi kongreServisi,
            IUnvanlarServisi unvanlarServisi,
            IWorkContext workContext)
        {
            this._izinServisi = izinServisi;
            this._siteServisi = siteServisi;
            this._konumServisi = konumServisi;
            this._kullanıcıİşlemServisi = kullanıcıİşlemServisi;
            this._unvanServisi = unvanServisi;
            this._kisiServisi = kisiServisi;
            this._kurumServisi = kurumServisi;
            this._gorusmeServisi = gorusmeServisi;
            this._firmaGorusmeServisi = firmaGorusmeServisi;
            this._kullanıcıServisi = kullanıcıServisi;
            this._ykServisi = ykServisi;
            this._gorevServisi = gorevServisi;
            this._firmaServisi = firmaServisi;
            this._firmayetkilisiServisi = firmayetkilisiServisi;
            this._kongreServisi = kongreServisi;
            this._workContext = workContext;
            this._unvanlarServisi = unvanlarServisi;
        }
        #region Utilities
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

        [HttpGet]
        public virtual JsonResult CrmGorusmeListele(int kisiId)
        {
            var kisi = _kisiServisi.CrmKisiAlId(kisiId);
            if (kisi != null)
            {
                List<CrmGorusmeModel> b = new List<CrmGorusmeModel>();
                foreach (var gorusme in kisi.Gorusmeler)
                {
                    var gr = gorusme.ToModel();
                    gr.GorusenAdı = _kullanıcıServisi.KullanıcıAlId(gr.Gorusen).TamAdAl();
                    b.Add(gr);
                }
                if (b != null)
                    return Json(b.ToList(), JsonRequestBehavior.AllowGet);
                else
                    return null;
            }
            else
            {
                return null;
            }
        }
        [HttpGet]
        public virtual JsonResult CrmFirmaGorusmeListele(int firmaId)
        {
            var firma = _firmaServisi.CrmFirmaAlId(firmaId);
            if (firma != null)
            {
                List<CrmFirmaGorusmeModel> b = new List<CrmFirmaGorusmeModel>();
                foreach (var gorusme in firma.Gorusmeler)
                {
                    var gr = gorusme.ToModel();
                    gr.GorusenAdı = _kullanıcıServisi.KullanıcıAlId(gr.Gorusen).TamAdAl();
                    CrmFirmaYetkilisi yetkili = new CrmFirmaYetkilisi();
                    if (_firmaServisi.CrmFirmaAlId(firma.Id).Yetkililer.Where(x => x.Id == gr.YetkiliId).FirstOrDefault() != null)
                        yetkili = _firmaServisi.CrmFirmaAlId(firma.Id).Yetkililer.Where(x => x.Id == gr.YetkiliId).FirstOrDefault();
                    gr.GorusulenAdı = yetkili.Adı + " " + yetkili.Soyadı;
                    b.Add(gr);
                }
                if (b != null)
                    return Json(b.ToList(), JsonRequestBehavior.AllowGet);
                else
                    return null;
            }
            else
            {
                return null;
            }
        }
        [HttpGet]
        public virtual JsonResult CrmGorusmeUcGun()
        {
            var firmalar = _firmaServisi.TümCrmFirmaAl();
            if (firmalar != null)
            {
                List<CrmFirmaGorusmeModel> b = new List<CrmFirmaGorusmeModel>();
                foreach (var firma in firmalar)
                {
                    foreach (var gorusme in firma.Gorusmeler)
                    {
                        bool ucgun = false;
                        if (gorusme.UcGun)
                        {
                            DateTime ucgunonce = gorusme.GorusmeTarihi.AddDays(3);
                            DateTime uchaftaonce = gorusme.GorusmeTarihi.AddDays(21);
                            if (ucgunonce.DayOfWeek.ToString() != "Saturday" && ucgunonce.DayOfWeek.ToString() != "Sunday")
                            {
                                if (ucgunonce <= DateTime.Today&& uchaftaonce >= DateTime.Today)
                                    ucgun = true;
                            }
                            if (DateTime.Today.ToString() == "Saturday")
                            {
                                if (ucgunonce.AddDays(2) <= DateTime.Today && uchaftaonce >= DateTime.Today)
                                    ucgun = true;
                            }
                            if (DateTime.Today.ToString() == "Sunday")
                            {
                                if (ucgunonce.AddDays(1) <= DateTime.Today && uchaftaonce >= DateTime.Today)
                                    ucgun = true;
                            }
                            if (ucgun)
                            {
                                var gr = gorusme.ToModel();
                                gr.GorusenAdı = _kullanıcıServisi.KullanıcıAlId(gr.Gorusen).TamAdAl();
                                gr.GorusulenAdı = firma.Adı;
                                b.Add(gr);
                            }
                        }
                    }
                }
                if (b != null)
                    return Json(b.ToList(), JsonRequestBehavior.AllowGet);
                else
                    return null;
            }
            else
            {
                return null;
            }
        }
        [HttpGet]
        public virtual JsonResult CrmGorusmeUcHafta()
        {
            var firmalar = _firmaServisi.TümCrmFirmaAl();
            if (firmalar != null)
            {
                List<CrmFirmaGorusmeModel> b = new List<CrmFirmaGorusmeModel>();
                foreach (var firma in firmalar)
                {
                    foreach (var gorusme in firma.Gorusmeler)
                    {
                        bool uchafta = false;
                        if (gorusme.UcHafta)
                        {
                            DateTime uchaftaonce = gorusme.GorusmeTarihi.AddDays(21);
                            DateTime ucayonce = gorusme.GorusmeTarihi.AddMonths(3);
                            if (uchaftaonce.DayOfWeek.ToString() != "Saturday" && uchaftaonce.DayOfWeek.ToString() != "Sunday")
                            {
                                if (uchaftaonce <= DateTime.Today && ucayonce >= DateTime.Today)
                                    uchafta = true;
                            }
                            if (DateTime.Today.ToString() == "Saturday")
                            {
                                if (uchaftaonce.AddDays(2) <= DateTime.Today && ucayonce.AddDays(2) >= DateTime.Today)
                                    uchafta = true;
                            }
                            if (DateTime.Today.ToString() == "Sunday")
                            {
                                if (uchaftaonce.AddDays(1) <= DateTime.Today && ucayonce.AddDays(2) >= DateTime.Today)
                                    uchafta = true;
                            }
                            if (uchafta)
                            {
                                var gr = gorusme.ToModel();
                                gr.GorusenAdı = _kullanıcıServisi.KullanıcıAlId(gr.Gorusen).TamAdAl();
                                gr.GorusulenAdı = firma.Adı;
                                b.Add(gr);
                            }
                        }
                    }
                }
                if (b != null)
                    return Json(b.ToList(), JsonRequestBehavior.AllowGet);
                else
                    return null;
            }
            else
            {
                return null;
            }
        }
        [HttpGet]
        public virtual JsonResult CrmGorusmeUcAy()
        {
            var firmalar = _firmaServisi.TümCrmFirmaAl();
            if (firmalar != null)
            {
                List<CrmFirmaGorusmeModel> b = new List<CrmFirmaGorusmeModel>();
                foreach (var firma in firmalar)
                {
                    foreach (var gorusme in firma.Gorusmeler)
                    {
                        bool ucay = false;
                        if (gorusme.UcAy)
                        {
                            DateTime ucayOnce = gorusme.GorusmeTarihi.AddMonths(3);
                            if (ucayOnce.DayOfWeek.ToString() != "Saturday" && ucayOnce.DayOfWeek.ToString() != "Sunday")
                            {
                                if (ucayOnce <= DateTime.Today)
                                    ucay = true;
                            }
                            if (DateTime.Today.ToString() == "Saturday")
                            {
                                if (ucayOnce.AddDays(2) <= DateTime.Today)
                                    ucay = true;
                            }
                            if (DateTime.Today.ToString() == "Sunday")
                            {
                                if (ucayOnce.AddDays(1) <= DateTime.Today)
                                    ucay = true;
                            }
                            if (ucay)
                            {
                                var gr = gorusme.ToModel();
                                gr.GorusenAdı = _kullanıcıServisi.KullanıcıAlId(gr.Gorusen).TamAdAl();
                                gr.GorusulenAdı = firma.Adı;
                                b.Add(gr);
                            }
                        }
                    }
                }
                if (b != null)
                    return Json(b.ToList(), JsonRequestBehavior.AllowGet);
                else
                    return null;
            }
            else
            {
                return null;
            }
        }
        [HttpPost]
        public virtual JsonResult CrmGorusmeGuncelle(int donem,int gorusmeId)
        {
            var gorusme = _firmaGorusmeServisi.CrmFirmaGorusmeAlId(gorusmeId);
            if (donem == 1)
                gorusme.UcGun = false;
            if (donem == 2)
                gorusme.UcHafta = false;
            if (donem == 3)
                gorusme.UcAy = false;
            _firmaGorusmeServisi.CrmFirmaGorusmeGüncelle(gorusme);

            return null;
        }
        [HttpGet]
        public virtual JsonResult CrmIhaleHatırlatması()
        {
            var kurumlar = _kurumServisi.TümCrmKurumAl();
            if (kurumlar != null)
            {
                List<CrmKongreModel> b = new List<CrmKongreModel>();
                foreach (var kurum in kurumlar)
                {
                    foreach (var kongre in kurum.Kongreler)
                    {
                        bool biray = false;
                        DateTime birayOnce = DateTime.Now.AddMonths(-1);
                        if (kongre.IhaleTarihi> birayOnce)
                            biray = true;
                        if (biray)
                        {
                            var gr = kongre.ToModel();
                            b.Add(gr);
                        }
                    }
                }
                if (b != null)
                    return Json(b.ToList(), JsonRequestBehavior.AllowGet);
                else
                    return null;
            }
            else
            {
                return null;
            }
        }
        [HttpGet]
        public virtual JsonResult CrmKurumListele(int kisiId)
        {
            var kisi = _kisiServisi.CrmKisiAlId(kisiId);
            if (kisi != null)
            {
                List<CrmYonetimKuruluModel> b = new List<CrmYonetimKuruluModel>();
                foreach (var yk in _ykServisi.CrmYonetimKuruluAlKisiId(kisiId))
                {
                    var ykList = yk.ToModel();
                    ykList.KurumAdı = _kurumServisi.CrmKurumAlId(yk.KurumId).Adı;
                    ykList.GorevAdı = _gorevServisi.CrmGorevAlId(yk.Gorevi).Adı;
                    b.Add(ykList);
                }
                if (b != null)
                    return Json(b.ToList(), JsonRequestBehavior.AllowGet);
                else
                    return null;
            }
            else
            {
                return null;
            }
        }
        [HttpGet]
        public virtual JsonResult CrmGorusmeListeleKurum(int kurumId)
        {
            var kurum = _kurumServisi.CrmKurumAlId(kurumId);
            if (kurum != null)
            {
                List<CrmGorusmeModel> b = new List<CrmGorusmeModel>();
                foreach (var yk in _ykServisi.CrmYonetimKuruluAlKurumId(kurumId))
                {
                    var kisi = _kisiServisi.CrmKisiAlId(yk.KisiId);
                    foreach (var gorusme in kisi.Gorusmeler)
                    {
                        var gr = gorusme.ToModel();
                        gr.GorusenAdı = _kullanıcıServisi.KullanıcıAlId(gr.Gorusen).TamAdAl();
                        gr.GorusulenAdı = kisi.Adı + " " + kisi.Soyadı;
                        b.Add(gr);
                    }
                }
                if (b != null)
                    return Json(b.ToList(), JsonRequestBehavior.AllowGet);
                else
                    return null;
            }
            else
            {
                return null;
            }
        }
        [HttpGet]
        public virtual JsonResult CrmYetkiliListeleFirma(int firmaId)
        {
            var kurum = _firmaServisi.CrmFirmaAlId(firmaId);
            if (kurum != null)
            {
                List<CrmFirmaYetkilisiModel> b = new List<CrmFirmaYetkilisiModel>();
                foreach (var yk in kurum.Yetkililer)
                {
                    b.Add(yk.ToModel());
                }
                if (b != null)
                    return Json(b.ToList(), JsonRequestBehavior.AllowGet);
                else
                    return null;
            }
            else
            {
                return null;
            }
        }
        [HttpGet]
        public virtual JsonResult CrmGorusmeModal(int gorusmeId=0)
        {
            CrmFirmaGorusmeModel b = _firmaGorusmeServisi.CrmFirmaGorusmeAlId(gorusmeId).ToModel();
            CrmFirmaYetkilisi y = b.YetkiliId>0? _firmayetkilisiServisi.CrmFirmaYetkilisiAlId(b.YetkiliId):null;
            var k =b.Gorusen>0? _kullanıcıServisi.KullanıcıAlId(b.Gorusen):null;
            b.YetkiliAdı = b.YetkiliId>0? y.Adı + " " + y.Soyadı:"";
            b.GorusenAdı =k!=null? k.TamAdAl():"";
            b.GorusulenAdı = y != null ? y.Adı + " " + y.Soyadı:"";
            if (b != null)
                return Json(b, JsonRequestBehavior.AllowGet);
            else
                return null;
        }
        [HttpGet]
        public virtual JsonResult CrmKongreListele(int kurumId)
        {
            var kurum = _kurumServisi.CrmKurumAlId(kurumId);
            if (kurum != null)
            {
                List<CrmKongreModel> b = new List<CrmKongreModel>();
                foreach (var kon in kurum.Kongreler)
                {
                    var konList = kon.ToModel();
                    b.Add(konList);
                }
                if (b != null)
                    return Json(b.ToList(), JsonRequestBehavior.AllowGet);
                else
                    return null;
            }
            else
            {
                return null;
            }
        }
        [HttpGet]
        public virtual JsonResult CrmYKListele(int kurumId)
        {
            var kurum = _kurumServisi.CrmKurumAlId(kurumId);
            if (kurum != null)
            {
                List<CrmYonetimKuruluModel> b = new List<CrmYonetimKuruluModel>();
                foreach (var yk in _ykServisi.CrmYonetimKuruluAlKurumId(kurumId))
                {
                    var ykList = yk.ToModel();
                    ykList.KisiAdı = _kisiServisi.CrmKisiAlId(ykList.KisiId).Adı + " " + _kisiServisi.CrmKisiAlId(ykList.KisiId).Soyadı;
                    ykList.GorevAdı = _gorevServisi.CrmGorevAlId(ykList.Gorevi).Adı;
                    b.Add(ykList);
                }
                if (b != null)
                    return Json(b.ToList(), JsonRequestBehavior.AllowGet);
                else
                    return null;
            }
            else
            {
                return null;
            }
        }
        #endregion
        public virtual ActionResult Anasayfa()
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.CrmYönet))
                return ErişimEngellendiView();

            var model = new CrmAnasayfaModel();
            return View(model);
        }
        #region Kişi
        public virtual ActionResult KişiListe()
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.CrmYönet))
                return ErişimEngellendiView();

            var model = new CrmKisiModel();
            return View(model);
        }

        [HttpPost]
        public virtual ActionResult KişiListe(DataSourceİsteği command, CrmKisiModel model)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.CrmYönet))
                return ErişimEngellendiKendoGridJson();

            var firmaModels = _kisiServisi.TümCrmKisiAl()
                .Select(x => x.ToModel())
                .ToList();
            var gridModel = new DataSourceSonucu
            {
                Data = firmaModels,
                Toplam = firmaModels.Count
            };

            return Json(gridModel);
        }
        public virtual ActionResult KişiKartı(int id)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.CrmYönet))
                ErişimEngellendiView();
            var kisi = _kisiServisi.CrmKisiAlId(id);

            if (kisi == null)
            {
                return RedirectToAction("KişiListe");
            }
            var model = kisi.ToModel();
            if(kisi.SehirId>0)
                model.SehirAdı = _konumServisi.SehirAlId(kisi.SehirId).Adı;
            if (kisi.IlceId > 0)
                model.IlceAdı = _konumServisi.IlceAlId(kisi.IlceId).Adı;
            if (kisi.KurumId > 0)
                model.KurumAdı = _kurumServisi.CrmKurumAlId(kisi.KurumId).Adı;
            return View(model);
        }
        public virtual ActionResult KişiEkle()
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.CrmYönet))
                ErişimEngellendiView();

            var model = new CrmKisiModel();
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
            foreach (var tumUnvanlar in _unvanServisi.TümCrmUnvanAl())
            {
                var unvan = tumUnvanlar.ToModel();
                model.Unvanlar.Add(unvan);
            }

            return View(model);
        }

        [HttpPost, FormAdıParametresi("kaydet-devam", "düzenlemeyeDevam")]
        public virtual ActionResult KişiEkle(CrmKisiModel model, bool düzenlemeyeDevam)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.CrmYönet))
                ErişimEngellendiView();
            if (ModelState.IsValid)
            {
                var kisi = model.ToEntity();
                _kisiServisi.CrmKisiEkle(kisi);

                BaşarılıBildirimi("Crm kişi başarıyla Eklendi");
                _kullanıcıİşlemServisi.İşlemEkle("YeniKişiEklendi", "Yeni Kişi Eklendi", kisi.Adı);
                if (düzenlemeyeDevam)
                {
                    SeçiliTabKaydet();
                    return RedirectToAction("KişiDüzenle", new { id = kisi.Id });
                }
                return RedirectToAction("KişiListe");

            }
            return View(model);
        }
        public virtual ActionResult KişiDüzenle(int id)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.CrmYönet))
                ErişimEngellendiView();
            var kisi = _kisiServisi.CrmKisiAlId(id);

            if (kisi == null)
            {
                return RedirectToAction("KişiListe");
            }
            var model = kisi.ToModel();
            //model.Yetkililer = l;
            foreach (var tumSehirler in _konumServisi.SehirlerAlUlkeId(1))
            {
                var sehirModel = tumSehirler.ToModel();
                model.Sehirler.Add(sehirModel);
            }
            foreach (var tumIlceler in _konumServisi.IlcelerAlSehirId(kisi.SehirId))
            {
                var ilceModel = tumIlceler.ToModel();
                model.Ilceler.Add(ilceModel);
            }
            foreach (var tumUnvanlar in _unvanServisi.TümCrmUnvanAl())
            {
                var unvan = tumUnvanlar.ToModel();
                model.Unvanlar.Add(unvan);
            }
            return View(model);
        }
        [HttpPost, FormAdıParametresi("kaydet-devam", "düzenlemeyeDevam")]
        public virtual ActionResult KişiDüzenle(CrmKisiModel model, bool düzenlemeyeDevam)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.CrmYönet))
                ErişimEngellendiView();
            var kisi = _kisiServisi.CrmKisiAlId(model.Id);
            if (kisi == null)
            {
                return RedirectToAction("FirmaListe");
            }
            if (ModelState.IsValid)
            {
                kisi = model.ToEntity(kisi);
               
                _kisiServisi.CrmKisiGüncelle(kisi);
                BaşarılıBildirimi("Crm kişi başarıyla güncellenmiştir.");
                _kullanıcıİşlemServisi.İşlemEkle("KişiGüncelle", "Crm kişi güncellendi", kisi.Adı);
                if (düzenlemeyeDevam)
                {
                    return RedirectToAction("KişiDüzenle", new { id = kisi.Id });
                }
                return RedirectToAction("KişiListe");
            }
            return View(model);
        }

        [HttpPost]
        public virtual ActionResult KişiSil(CrmKisiModel model)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.CrmYönet))
                return ErişimEngellendiView();

            var kisi = _kisiServisi.CrmKisiAlId(model.Id);
            if (kisi == null)
                return RedirectToAction("KişiListe");
            _kisiServisi.CrmKisiSil(kisi);
            BaşarılıBildirimi("Crm kişi başarıyla silindi");
            _kullanıcıİşlemServisi.İşlemEkle("KişiSil", "Crm kişi silindi", kisi.Adı);
            return RedirectToAction("KişiListe");
        }
        #endregion
        #region Kurum
        public virtual ActionResult KurumListe()
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.CrmYönet))
                return ErişimEngellendiView();

            var model = new CrmKurumModel();
            return View(model);
        }

        [HttpPost]
        public virtual ActionResult KurumListe(DataSourceİsteği command, CrmKurumModel model)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.CrmYönet))
                return ErişimEngellendiKendoGridJson();

            var firmaModels = _kurumServisi.TümCrmKurumAl()
                .Select(x => x.ToModel())
                .ToList();
            var gridModel = new DataSourceSonucu
            {
                Data = firmaModels,
                Toplam = firmaModels.Count
            };

            return Json(gridModel);
        }
        public virtual ActionResult KurumKartı(int id)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.CrmYönet))
                ErişimEngellendiView();
            var kurum = _kurumServisi.CrmKurumAlId(id);

            if (kurum == null)
            {
                return RedirectToAction("KurumListe");
            }
            var model = kurum.ToModel();
            //model.Yetkililer = l;
            foreach (var tumSehirler in _konumServisi.SehirlerAlUlkeId(1))
            {
                var sehirModel = tumSehirler.ToModel();
                model.Sehirler.Add(sehirModel);
            }
            foreach (var tumIlceler in _konumServisi.IlcelerAlSehirId(kurum.SehirId))
            {
                var ilceModel = tumIlceler.ToModel();
                model.Ilceler.Add(ilceModel);
            }
            return View(model);
        }
        public virtual ActionResult KurumEkle()
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.CrmYönet))
                ErişimEngellendiView();

            var model = new CrmKurumModel();
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
        public virtual ActionResult KurumEkle(CrmKurumModel model, bool düzenlemeyeDevam)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.CrmYönet))
                ErişimEngellendiView();
            if (ModelState.IsValid)
            {
                var kisi = model.ToEntity();
                _kurumServisi.CrmKurumEkle(kisi);

                BaşarılıBildirimi("Crm kişi başarıyla Eklendi");
                _kullanıcıİşlemServisi.İşlemEkle("YeniKurumEklendi", "Yeni Kurum Eklendi", kisi.Adı);
                if (düzenlemeyeDevam)
                {
                    SeçiliTabKaydet();
                    return RedirectToAction("KurumDüzenle", new { id = kisi.Id });
                }
                return RedirectToAction("KurumListe");

            }
            return View(model);
        }
        public virtual ActionResult KurumDüzenle(int id)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.CrmYönet))
                ErişimEngellendiView();
            var kisi = _kurumServisi.CrmKurumAlId(id);

            if (kisi == null)
            {
                return RedirectToAction("KurumListe");
            }
            var model = kisi.ToModel();
            //model.Yetkililer = l;
            foreach (var tumSehirler in _konumServisi.SehirlerAlUlkeId(1))
            {
                var sehirModel = tumSehirler.ToModel();
                model.Sehirler.Add(sehirModel);
            }
            foreach (var tumIlceler in _konumServisi.IlcelerAlSehirId(kisi.SehirId))
            {
                var ilceModel = tumIlceler.ToModel();
                model.Ilceler.Add(ilceModel);
            }
            return View(model);
        }
        [HttpPost, FormAdıParametresi("kaydet-devam", "düzenlemeyeDevam")]
        public virtual ActionResult KurumDüzenle(CrmKurumModel model, bool düzenlemeyeDevam)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.CrmYönet))
                ErişimEngellendiView();
            var kisi = _kurumServisi.CrmKurumAlId(model.Id);
            if (kisi == null)
            {
                return RedirectToAction("FirmaListe");
            }
            if (ModelState.IsValid)
            {
                kisi = model.ToEntity(kisi);

                _kurumServisi.CrmKurumGüncelle(kisi);
                BaşarılıBildirimi("Crm kişi başarıyla güncellenmiştir.");
                _kullanıcıİşlemServisi.İşlemEkle("KurumGüncelle", "Crm kişi güncellendi", kisi.Adı);
                if (düzenlemeyeDevam)
                {
                    return RedirectToAction("KurumDüzenle", new { id = kisi.Id });
                }
                return RedirectToAction("KurumListe");
            }
            return View(model);
        }

        [HttpPost]
        public virtual ActionResult KurumSil(CrmKurumModel model)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.CrmYönet))
                return ErişimEngellendiView();

            var kisi = _kurumServisi.CrmKurumAlId(model.Id);
            if (kisi == null)
                return RedirectToAction("KurumListe");
            _kurumServisi.CrmKurumSil(kisi);
            BaşarılıBildirimi("Crm kişi başarıyla silindi");
            _kullanıcıİşlemServisi.İşlemEkle("KurumSil", "Crm kişi silindi", kisi.Adı);
            return RedirectToAction("KurumListe");
        }
        #endregion
        #region Firma
        public virtual ActionResult FirmaListe()
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.CrmYönet))
                return ErişimEngellendiView();

            var model = new CrmFirmaModel();
            return View(model);
        }
        public virtual ActionResult FirmaKartı(int id)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.CrmYönet))
                ErişimEngellendiView();
            var kisi = _firmaServisi.CrmFirmaAlId(id);

            if (kisi == null)
            {
                return RedirectToAction("FirmaListe");
            }
            var model = kisi.ToModel();
            //model.Yetkililer = l;
            foreach (var tumSehirler in _konumServisi.SehirlerAlUlkeId(1))
            {
                var sehirModel = tumSehirler.ToModel();
                model.Sehirler.Add(sehirModel);
            }
            foreach (var tumIlceler in _konumServisi.IlcelerAlSehirId(kisi.SehirId))
            {
                var ilceModel = tumIlceler.ToModel();
                model.Ilceler.Add(ilceModel);
            }
            foreach (var yetkililer in _firmaServisi.CrmFirmaAlId(kisi.Id).Yetkililer)
            {
                var s = new SelectListItem();
                s.Text = yetkililer.Adı+" "+yetkililer.Soyadı;
                s.Value = yetkililer.Id.ToString();
                //var yetkili = yetkililer.ToModel();
                model.Yetkili.Add(s);
            }
            foreach (var gorusmeler in _firmaServisi.CrmFirmaAlId(kisi.Id).Gorusmeler)
            {
                //var s = new CrmFirmaGorusmeModel();
                model.Görüsmeler.Add(gorusmeler.ToModel());
            }
            model.SehirAdı = model.SehirId > 0 ? _konumServisi.SehirAlId(model.SehirId).Adı : "-";
            model.IlceAdı = model.IlceId > 0 ? _konumServisi.IlceAlId(model.IlceId).Adı : "-";
            return View(model);
        }
        [HttpPost]
        public virtual ActionResult FirmaListe(DataSourceİsteği command, CrmFirmaModel model)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.CrmYönet))
                return ErişimEngellendiKendoGridJson();

            var firmaModels = _firmaServisi.TümCrmFirmaAl()
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
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.CrmYönet))
                ErişimEngellendiView();

            var model = new CrmFirmaModel();
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
            foreach (var yetkililer in _firmayetkilisiServisi.TümCrmFirmaYetkilisiAl())
            {
                var s = new SelectListItem();
                s.Text = yetkililer.Adı+" "+yetkililer.Soyadı;
                s.Value = yetkililer.Id.ToString();
                //var yetkili = yetkililer.ToModel();
                model.Yetkili.Add(s);
            }
            return View(model);
        }

        [HttpPost, FormAdıParametresi("kaydet-devam", "düzenlemeyeDevam")]
        public virtual ActionResult FirmaEkle(CrmFirmaModel model, bool düzenlemeyeDevam)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.CrmYönet))
                ErişimEngellendiView();
            if (ModelState.IsValid)
            {
                var kisi = model.ToEntity();
                if (model.YetkiliIdleri.Count > 0)
                {
                    foreach (var yetkiliId in model.YetkiliIdleri)
                    {
                        var yetkili = _firmayetkilisiServisi.CrmFirmaYetkilisiAlId(yetkiliId);
                        kisi.Yetkililer.Add(yetkili);
                    }
                }
                _firmaServisi.CrmFirmaEkle(kisi);

                BaşarılıBildirimi("Crm Firma başarıyla Eklendi");
                _kullanıcıİşlemServisi.İşlemEkle("YeniFirmaEklendi", "Yeni Firma Eklendi", kisi.Adı);
                if (düzenlemeyeDevam)
                {
                    SeçiliTabKaydet();
                    return RedirectToAction("FirmaDüzenle", new { id = kisi.Id });
                }
                return RedirectToAction("FirmaListe");

            }
            return View(model);
        }
        public virtual ActionResult FirmaDüzenle(int id)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.CrmYönet))
                ErişimEngellendiView();
            var kisi = _firmaServisi.CrmFirmaAlId(id);

            if (kisi == null)
            {
                return RedirectToAction("FirmaListe");
            }
            var model = kisi.ToModel();
            //model.Yetkililer = l;
            foreach (var tumSehirler in _konumServisi.SehirlerAlUlkeId(1))
            {
                var sehirModel = tumSehirler.ToModel();
                model.Sehirler.Add(sehirModel);
            }
            foreach (var tumIlceler in _konumServisi.IlcelerAlSehirId(kisi.SehirId))
            {
                var ilceModel = tumIlceler.ToModel();
                model.Ilceler.Add(ilceModel);
            }
            foreach (var yetkililer in _firmayetkilisiServisi.TümCrmFirmaYetkilisiAl())
            {
                var s = new SelectListItem();
                s.Text = yetkililer.Adı+" "+yetkililer.Soyadı;
                s.Value = yetkililer.Id.ToString();
                //var yetkili = yetkililer.ToModel();
                s.Selected = kisi.Yetkililer.Contains(yetkililer) ? true : false ;
                model.Yetkili.Add(s);
            }
            foreach (var gorusmeler in _firmaServisi.CrmFirmaAlId(kisi.Id).Gorusmeler)
            {
                //var s = new CrmFirmaGorusmeModel();
                model.Görüsmeler.Add(gorusmeler.ToModel());
            }
            return View(model);
        }
        [HttpPost, FormAdıParametresi("kaydet-devam", "düzenlemeyeDevam")]
        public virtual ActionResult FirmaDüzenle(CrmFirmaModel model, bool düzenlemeyeDevam)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.CrmYönet))
                ErişimEngellendiView();
            var kisi = _firmaServisi.CrmFirmaAlId(model.Id);
            if (kisi == null)
            {
                return RedirectToAction("FirmaListe");
            }
            if (ModelState.IsValid)
            {
                kisi = model.ToEntity(kisi);
                if (model.YetkiliIdleri.Count > 0)
                {
                    foreach (var yetkiliId in model.YetkiliIdleri)
                    {
                        var yetkili = _firmayetkilisiServisi.CrmFirmaYetkilisiAlId(yetkiliId);
                        kisi.Yetkililer.Add(yetkili);
                    }
                }
                _firmaServisi.CrmFirmaGüncelle(kisi);
                BaşarılıBildirimi("Crm Firma başarıyla güncellenmiştir.");
                _kullanıcıİşlemServisi.İşlemEkle("FirmaGüncelle", "Crm Firma güncellendi", kisi.Adı);
                if (düzenlemeyeDevam)
                {
                    return RedirectToAction("FirmaDüzenle", new { id = kisi.Id });
                }
                return RedirectToAction("FirmaListe");
            }
            return View(model);
        }

        [HttpPost]
        public virtual ActionResult FirmaSil(CrmFirmaModel model)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.CrmYönet))
                return ErişimEngellendiView();

            var firma = _firmaServisi.CrmFirmaAlId(model.Id);
            if (firma == null)
                return RedirectToAction("FirmaListe");
            _firmaServisi.CrmFirmaSil(firma);
            BaşarılıBildirimi("Crm Firma başarıyla silindi");
            _kullanıcıİşlemServisi.İşlemEkle("FirmaSil", "Crm Firma silindi", firma.Adı);
            return RedirectToAction("FirmaListe");
        }
        #endregion
        #region Firma Yetkilisi
        public virtual ActionResult FirmaYetkilisiListe()
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.CrmYönet))
                return ErişimEngellendiView();

            var model = new CrmFirmaYetkilisiModel();
            return View(model);
        }

        [HttpPost]
        public virtual ActionResult FirmaYetkilisiListe(DataSourceİsteği command, CrmFirmaYetkilisiModel model)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.CrmYönet))
                return ErişimEngellendiKendoGridJson();

            var firmaModels = _firmayetkilisiServisi.TümCrmFirmaYetkilisiAl()
                .Select(x => x.ToModel())
                .ToList();
            var gridModel = new DataSourceSonucu
            {
                Data = firmaModels,
                Toplam = firmaModels.Count
            };

            return Json(gridModel);
        }
        public virtual ActionResult FirmaYetkilisiKartı(int id)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.CrmYönet))
                ErişimEngellendiView();
            var kisi = _firmayetkilisiServisi.CrmFirmaYetkilisiAlId(id);

            if (kisi == null)
            {
                return RedirectToAction("FirmaYetkilisiListe");
            }
            var model = kisi.ToModel();
            if (kisi.SehirId > 0)
                model.SehirAdı = _konumServisi.SehirAlId(kisi.SehirId).Adı;
            if (kisi.IlceId > 0)
                model.IlceAdı = _konumServisi.IlceAlId(kisi.IlceId).Adı;
            if (kisi.FirmaId > 0)
                model.KurumAdı = _kurumServisi.CrmKurumAlId(kisi.FirmaId).Adı;
            return View(model);
        }
        public virtual ActionResult FirmaYetkilisiEkle()
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.CrmYönet))
                ErişimEngellendiView();

            var model = new CrmFirmaYetkilisiModel();
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
            foreach (var unvanlar in _unvanlarServisi.TümUnvanlarıAl())
            {
                var unvan = unvanlar.ToModel();
                model.Görevler.Add(unvanlar);
            }

            return View(model);
        }

        [HttpPost, FormAdıParametresi("kaydet-devam", "düzenlemeyeDevam")]
        public virtual ActionResult FirmaYetkilisiEkle(CrmFirmaYetkilisiModel model, bool düzenlemeyeDevam, string returnUrl)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.CrmYönet))
                ErişimEngellendiView();
            if (ModelState.IsValid)
            {
                var kisi = model.ToEntity();
                _firmayetkilisiServisi.CrmFirmaYetkilisiEkle(kisi);

                BaşarılıBildirimi("Crm yetkili başarıyla Eklendi");
                _kullanıcıİşlemServisi.İşlemEkle("YeniYetkiliEklendi", "Yeni Yetkili Eklendi", kisi.Adı);
                if (düzenlemeyeDevam)
                {
                    SeçiliTabKaydet();
                    return RedirectToAction("FirmaYetkilisiDüzenle", new { id = kisi.Id });
                }
                if (returnUrl != null)
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction("FirmaListe");
                }
                return RedirectToAction("FirmaYetkilisiListe");
            }
            return View(model);
        }
        public virtual ActionResult FirmaYetkilisiDüzenle(int id)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.CrmYönet))
                ErişimEngellendiView();
            var kisi = _firmayetkilisiServisi.CrmFirmaYetkilisiAlId(id);

            if (kisi == null)
            {
                return RedirectToAction("FirmaYetkilisiListe");
            }
            var model = kisi.ToModel();
            //model.Yetkililer = l;
            foreach (var tumSehirler in _konumServisi.SehirlerAlUlkeId(1))
            {
                var sehirModel = tumSehirler.ToModel();
                model.Sehirler.Add(sehirModel);
            }
            foreach (var tumIlceler in _konumServisi.IlcelerAlSehirId(kisi.SehirId))
            {
                var ilceModel = tumIlceler.ToModel();
                model.Ilceler.Add(ilceModel);
            }
            foreach (var unvanlar in _unvanlarServisi.TümUnvanlarıAl())
            {
                var unvan = unvanlar.ToModel();
                model.Görevler.Add(unvanlar);
            }
            return View(model);
        }
        [HttpPost, FormAdıParametresi("kaydet-devam", "düzenlemeyeDevam")]
        public virtual ActionResult FirmaYetkilisiDüzenle(CrmFirmaYetkilisiModel model, bool düzenlemeyeDevam)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.CrmYönet))
                ErişimEngellendiView();
            var kisi = _firmayetkilisiServisi.CrmFirmaYetkilisiAlId(model.Id);
            if (kisi == null)
            {
                return RedirectToAction("FirmaYetkilisiListe");
            }
            if (ModelState.IsValid)
            {
                kisi = model.ToEntity(kisi);

                _firmayetkilisiServisi.CrmFirmaYetkilisiGüncelle(kisi);
                BaşarılıBildirimi("Crm yetkili başarıyla güncellenmiştir.");
                _kullanıcıİşlemServisi.İşlemEkle("FirmaYetkilisiGüncelle", "Crm firma yetkilsi güncellendi", kisi.Adı);
                if (düzenlemeyeDevam)
                {
                    return RedirectToAction("FirmaYetkilisiDüzenle", new { id = kisi.Id });
                }
                return RedirectToAction("FirmaYetkilisiListe");
            }
            return View(model);
        }

        [HttpPost]
        public virtual ActionResult FirmaYetkilisiSil(CrmFirmaYetkilisiModel model)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.CrmYönet))
                return ErişimEngellendiView();

            var kisi = _firmayetkilisiServisi.CrmFirmaYetkilisiAlId(model.Id);
            if (kisi == null)
                return RedirectToAction("FirmaYetkilisiListe");
            _firmayetkilisiServisi.CrmFirmaYetkilisiSil(kisi);
            BaşarılıBildirimi("Crm yetkili başarıyla silindi");
            _kullanıcıİşlemServisi.İşlemEkle("FirmaYetkilisiSil", "Crm yetkili silindi", kisi.Adı);
            return RedirectToAction("FirmaYetkilisiListe");
        }
        #endregion
        #region KişiGörüşmeleri
        [NonAction]
        private void CrmGorusmeModelHazırla(CrmKisiGorusmeModel model, CrmGorusme CrmGorusme, CrmKisi kisi)
        {
            if (kisi == null)
                throw new ArgumentNullException("kisi");

            model.KisiId = kisi.Id;
            if (CrmGorusme != null)
            {
                model.Gorusmeler = CrmGorusme.ToModel();
            }

            if (model.Gorusmeler == null)
            {
                model.Gorusmeler = new CrmGorusmeModel();
            }
        }
        public virtual ActionResult CrmGorusmeEkle(int kisiId)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.CrmYönet))
                return ErişimEngellendiView();
                var kisi = _kisiServisi.CrmKisiAlId(kisiId);
                if (kisi == null)
                    return RedirectToAction("KişiListe");
                var model = new CrmKisiGorusmeModel();
                CrmGorusmeModelHazırla(model, null, kisi);
                return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public virtual ActionResult CrmGorusmeEkle(CrmKisiGorusmeModel model, FormCollection form)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.CrmYönet))
                return ErişimEngellendiView();

            var kisi = _kisiServisi.CrmKisiAlId(model.KisiId);
            if (kisi == null)

                return RedirectToAction("KişiListe");

            if (ModelState.IsValid)
            {
                var kayıt = model.Gorusmeler.ToEntity();
                kisi.Gorusmeler.Add(kayıt);
                kayıt.Gorusen = _workContext.MevcutKullanıcı.Id;
                _kisiServisi.CrmKisiGüncelle(kisi);

                BaşarılıBildirimi("Gorusmeler Bilgileri Eklendi");
                return RedirectToAction("KişiDüzenle", new { id = kisi.Id });
            }

            CrmGorusmeModelHazırla(model, null, kisi);
            return View(model);
        }
        
        public virtual ActionResult CrmGorusmeDüzenle(int kayıtId, int kisiId)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.CrmYönet))
                return ErişimEngellendiView();

            var kongre = _kisiServisi.CrmKisiAlId(kisiId);
            if (kongre == null)
                return RedirectToAction("KişiListe");

            var kayıtBilgi = _gorusmeServisi.CrmGorusmeAlId(kayıtId);
            if (kayıtBilgi == null)
                return RedirectToAction("KişiDüzenle", new { id = kongre.Id });

            var model = new CrmKisiGorusmeModel();
            CrmGorusmeModelHazırla(model, kayıtBilgi, kongre);
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public virtual ActionResult CrmGorusmeDüzenle(CrmKisiGorusmeModel model, FormCollection form)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.CrmYönet))
                return ErişimEngellendiView();

            var kisi = _kisiServisi.CrmKisiAlId(model.KisiId);
            if (kisi == null)
                return RedirectToAction("KişiListe");

            var kayıtBilgi = _gorusmeServisi.CrmGorusmeAlId(model.Gorusmeler.Id);
            if (kayıtBilgi == null)
                return RedirectToAction("KişiDüzenle", new { id = kisi.Id });

            if (ModelState.IsValid)
            {
                kayıtBilgi = model.Gorusmeler.ToEntity(kayıtBilgi);
                _gorusmeServisi.CrmGorusmeGüncelle(kayıtBilgi);

                BaşarılıBildirimi("Görüşme bilgileri güncellendi");
                return RedirectToAction("KişiDüzenle", new { id = kisi.Id });
            }
            CrmGorusmeModelHazırla(model, kayıtBilgi, kisi);
            return View(model);
        }
        #endregion
        #region FirmaGörüşmeleri
        [NonAction]
        private void CrmFirmaGorusmeModelHazırla(CrmFirmaGorusmeGorusmeModel model, CrmFirmaGorusme CrmGorusme, CrmFirma firma)
        {
            if (firma == null)
                throw new ArgumentNullException("firma");

            model.FirmaId = firma.Id;
            if (CrmGorusme != null)
            {
                model.Gorusmeler = CrmGorusme.ToModel();
                foreach(var yetkili in _firmaServisi.CrmFirmaAlId(firma.Id).Yetkililer)
                {
                    SelectListItem s = new SelectListItem();
                    s.Text = yetkili.Adı + " " + yetkili.Soyadı;
                    s.Value = yetkili.Id.ToString();
                    model.Gorusmeler.Yetkililer.Add(s);
                }
            }

            if (model.Gorusmeler == null)
            {
                model.Gorusmeler = new CrmFirmaGorusmeModel();
                foreach (var yetkili in _firmaServisi.CrmFirmaAlId(firma.Id).Yetkililer)
                {
                    SelectListItem s = new SelectListItem();
                    s.Text = yetkili.Adı + " " + yetkili.Soyadı;
                    s.Value = yetkili.Id.ToString();
                    model.Gorusmeler.Yetkililer.Add(s);
                }
            }
        }
        public virtual ActionResult TumGorusmelerModal()
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.CrmYönet))
                return ErişimEngellendiView();
            /*
            var firma = _firmaServisi.CrmFirmaAlId(firmaId);
            if (firma == null)
                return RedirectToAction("FirmaListe");
                */
            var model = new CrmFirmaGorusmeGorusmeModel();
            model.FirmaId = 1;
            model.Gorusmeler = _firmaServisi.CrmFirmaAlId(1).Gorusmeler.FirstOrDefault().ToModel();

            return View(model);
        }
        public virtual ActionResult CrmFirmaGorusmeEkle(int firmaId)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.CrmYönet))
                return ErişimEngellendiView();
            var firma = _firmaServisi.CrmFirmaAlId(firmaId);
            if (firma == null)
                return RedirectToAction("FirmaListe");
            var model = new CrmFirmaGorusmeGorusmeModel();
            CrmFirmaGorusmeModelHazırla(model, null, firma);
            return View(model);
        }
        [HttpPost]
        [ValidateInput(false)]
        public virtual ActionResult CrmFirmaGorusmeEkle(CrmFirmaGorusmeGorusmeModel model, FormCollection form)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.CrmYönet))
                return ErişimEngellendiView();

            var firma = _firmaServisi.CrmFirmaAlId(model.FirmaId);
            if (firma == null)

                return RedirectToAction("FirmaListe");

            if (ModelState.IsValid)
            {
                var kayıt = model.Gorusmeler.ToEntity();
                kayıt.Gorusen= _workContext.MevcutKullanıcı.Id;
                firma.Gorusmeler.Add(kayıt);
                _firmaServisi.CrmFirmaGüncelle(firma);

                BaşarılıBildirimi("Gorusmeler Bilgileri Eklendi");
                return RedirectToAction("FirmaDüzenle", new { id = firma.Id });
            }

            CrmFirmaGorusmeModelHazırla(model, null, firma);
            return View(model);
        }
        public virtual ActionResult CrmFirmaGorusmeDüzenle(int kayıtId, int firmaId)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.CrmYönet))
                return ErişimEngellendiView();

            var kongre = _firmaServisi.CrmFirmaAlId(firmaId);
            if (kongre == null)
                return RedirectToAction("FirmaListe");

            var kayıtBilgi = _firmaGorusmeServisi.CrmFirmaGorusmeAlId(kayıtId);
            if (kayıtBilgi == null)
                return RedirectToAction("FirmaDüzenle", new { id = kongre.Id });

            var model = new CrmFirmaGorusmeGorusmeModel();
            CrmFirmaGorusmeModelHazırla(model, kayıtBilgi, kongre);
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public virtual ActionResult CrmFirmaGorusmeDüzenle(CrmFirmaGorusmeGorusmeModel model, FormCollection form)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.CrmYönet))
                return ErişimEngellendiView();

            var firma = _firmaServisi.CrmFirmaAlId(model.FirmaId);
            if (firma == null)
                return RedirectToAction("FirmaListe");

            var kayıtBilgi = _firmaGorusmeServisi.CrmFirmaGorusmeAlId(model.Gorusmeler.Id);
            if (kayıtBilgi == null)
                return RedirectToAction("FirmaDüzenle", new { id = firma.Id });

            if (ModelState.IsValid)
            {
                kayıtBilgi = model.Gorusmeler.ToEntity(kayıtBilgi);
                _firmaGorusmeServisi.CrmFirmaGorusmeGüncelle(kayıtBilgi);

                BaşarılıBildirimi("Görüşme bilgileri güncellendi");
                return RedirectToAction("FirmaDüzenle", new { id = firma.Id });
            }
            CrmFirmaGorusmeModelHazırla(model, kayıtBilgi, firma);
            return View(model);
        }
        #endregion
        #region Görevler
        public virtual ActionResult CrmGorevEkle(int kisiId)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.CrmYönet))
                ErişimEngellendiView();

            var model = new CrmYonetimKuruluModel();
            model.KisiId = kisiId;
            foreach (var tümKurumlar in _kurumServisi.TümCrmKurumAl())
            {
                var kurumModel = tümKurumlar.ToModel();
                model.Kurumlar.Add(kurumModel);
            }
            foreach (var tümGorevler in _gorevServisi.TümCrmGorevAl())
            {
                var gorevModel = tümGorevler.ToModel();
                model.Gorevler.Add(gorevModel);
            }

            return View(model);
        }

        [HttpPost, FormAdıParametresi("kaydet-devam", "düzenlemeyeDevam")]
        public virtual ActionResult CrmGorevEkle(CrmYonetimKuruluModel model, bool düzenlemeyeDevam)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.CrmYönet))
                ErişimEngellendiView();
            if (ModelState.IsValid)
            {
                var kisi = model.ToEntity();
                _ykServisi.CrmYonetimKuruluEkle(kisi);

                BaşarılıBildirimi("Crm kişi başarıyla Eklendi");
                _kullanıcıİşlemServisi.İşlemEkle("YeniCrmGorevEklendi", "Yeni CrmGorev Eklendi", kisi.Id);
                if (düzenlemeyeDevam)
                {
                    SeçiliTabKaydet();
                    return RedirectToAction("KişiDüzenle", new { kisiId = kisi.Id });
                }
                return RedirectToAction("KişiDüzenle", new { kisiId = kisi.Id });

            }
            return View(model);
        }
        public virtual ActionResult CrmGorevDüzenle(int id)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.CrmYönet))
                ErişimEngellendiView();
            var kisi = _ykServisi.CrmYonetimKuruluAlId(id);

            if (kisi == null)
            {
                return RedirectToAction("CrmGorevListe");
            }
            var model = kisi.ToModel();
            //model.Yetkililer = l;
            foreach (var tümKurumlar in _kurumServisi.TümCrmKurumAl())
            {
                var kurumModel = tümKurumlar.ToModel();
                model.Kurumlar.Add(kurumModel);
            }
            foreach (var tümGorevler in _gorevServisi.TümCrmGorevAl())
            {
                var gorevModel = tümGorevler.ToModel();
                model.Gorevler.Add(gorevModel);
            }
            return View(model);
        }
        [HttpPost, FormAdıParametresi("kaydet-devam", "düzenlemeyeDevam")]
        public virtual ActionResult CrmGorevDüzenle(CrmYonetimKuruluModel model, bool düzenlemeyeDevam)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.CrmYönet))
                ErişimEngellendiView();
            var kisi = _ykServisi.CrmYonetimKuruluAlId(model.Id);
            if (kisi == null)
            {
                return RedirectToAction("FirmaListe");
            }
            if (ModelState.IsValid)
            {
                kisi = model.ToEntity(kisi);

                _ykServisi.CrmYonetimKuruluGüncelle(kisi);
                BaşarılıBildirimi("Crm kişi başarıyla güncellenmiştir.");
                _kullanıcıİşlemServisi.İşlemEkle("CrmGorevGüncelle", "Crm kişi güncellendi", kisi.Id);
                if (düzenlemeyeDevam)
                {
                    return RedirectToAction("KişiDüzenle", new { kisiId = kisi.Id });
                }
                return RedirectToAction("KişiDüzenle", new { kisiId = kisi.Id });
            }
            return View(model);
        }

        [HttpPost]
        public virtual ActionResult CrmGorevSil(CrmYonetimKuruluModel model)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.CrmYönet))
                return ErişimEngellendiView();

            var kisi = _ykServisi.CrmYonetimKuruluAlId(model.Id);
            if (kisi == null)
                return RedirectToAction("CrmGorevListe");
            _ykServisi.CrmYonetimKuruluSil(kisi);
            BaşarılıBildirimi("Crm kişi başarıyla silindi");
            _kullanıcıİşlemServisi.İşlemEkle("CrmGorevSil", "Crm kişi silindi", kisi.Id);
            return RedirectToAction("KişiDüzenle", new { kisiId = kisi.Id });
        }
        #endregion
        #region Kongreler
        [NonAction]
        private void CrmKurumKongreModelHazırla(CrmKurumKongreModel model, CrmKongre CrmGorusme, CrmKurum firma)
        {
            if (firma == null)
                throw new ArgumentNullException("firma");

            model.KurumId= firma.Id;
            if (CrmGorusme != null)
            {
                model.Kongreler = CrmGorusme.ToModel();
            }

            if (model.Kongreler == null)
            {
                model.Kongreler = new CrmKongreModel();
            }
        }
        public virtual ActionResult CrmKongreEkle(int kurumId)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.CrmYönet))
                return ErişimEngellendiView();
            var kurum = _kurumServisi.CrmKurumAlId(kurumId);
            if (kurum == null)
                return RedirectToAction("KurumListe");
            var model = new CrmKurumKongreModel();
            CrmKurumKongreModelHazırla(model, null, kurum);
            foreach (var tümKisiler in _kisiServisi.TümCrmKisiAl())
            {
                var kisiModel = tümKisiler.ToModel();
                kisiModel.KisiTamAd = kisiModel.Adı + " " + kisiModel.Soyadı;
                model.Kongreler.Kisiler.Add(kisiModel);
            }
            return View(model);
        }
        [HttpPost]
        [ValidateInput(false)]
        public virtual ActionResult CrmKongreEkle(CrmKurumKongreModel model, FormCollection form)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.CrmYönet))
                return ErişimEngellendiView();
            
            var kurum = _kurumServisi.CrmKurumAlId(model.KurumId);
            if (kurum == null)

                return RedirectToAction("KurumListe");

            if (ModelState.IsValid)
            {
                var kayıt = model.Kongreler.ToEntity();
                kurum.Kongreler.Add(kayıt);
                _kurumServisi.CrmKurumGüncelle(kurum);

                BaşarılıBildirimi("Kongre Bilgileri Eklendi");
                return RedirectToAction("KurumDüzenle", new { id = kurum.Id });
            }
            CrmKurumKongreModelHazırla(model, null, kurum);
            return View(model);
        }
        public virtual ActionResult CrmKongreDüzenle(int kayıtId, int kurumId)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.CrmYönet))
                return ErişimEngellendiView();

            var kongre = _kurumServisi.CrmKurumAlId(kurumId);
            if (kongre == null)
                return RedirectToAction("KurumListe");

            var kayıtBilgi = _kongreServisi.CrmKongreAlId(kayıtId);
            if (kayıtBilgi == null)
                return RedirectToAction("KurumDüzenle", new { id = kongre.Id });

            var model = new CrmKurumKongreModel();
            CrmKurumKongreModelHazırla(model, kayıtBilgi, kongre);
            foreach (var tümKisiler in _kisiServisi.TümCrmKisiAl())
            {
                var kisiModel = tümKisiler.ToModel();
                kisiModel.KisiTamAd = kisiModel.Adı + " " + kisiModel.Soyadı;
                model.Kongreler.Kisiler.Add(kisiModel);
            }
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public virtual ActionResult CrmKongreDüzenle(CrmKurumKongreModel model, FormCollection form)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.CrmYönet))
                return ErişimEngellendiView();

            var kurum = _kurumServisi.CrmKurumAlId(model.KurumId);
            if (kurum == null)
                return RedirectToAction("KurumListe");

            var kayıtBilgi = _kongreServisi.CrmKongreAlId(model.Kongreler.Id);
            if (kayıtBilgi == null)
                return RedirectToAction("KurumDüzenle", new { id = kurum.Id });

            if (ModelState.IsValid)
            {
                kayıtBilgi = model.Kongreler.ToEntity(kayıtBilgi);
                _kongreServisi.CrmKongreGüncelle(kayıtBilgi);

                BaşarılıBildirimi("Görüşme bilgileri güncellendi");
                return RedirectToAction("KurumDüzenle", new { id = kurum.Id });
            }
            CrmKurumKongreModelHazırla(model, kayıtBilgi, kurum);
            return View(model);
        }
        #endregion
    }
}