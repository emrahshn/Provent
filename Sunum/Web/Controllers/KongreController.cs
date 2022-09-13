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
using Services.Kongre;
using Web.Models.Kongre;
using Core.Domain.Katalog;
using System.Collections.Generic;
using Web.Models.Genel;
using Core.Domain.Kongre;
using Services.Tanımlamalar;
using Core;
using Services.Notlar;
using Services.Klasör;
using System.Web.UI.WebControls;
using System.Data;
using Services.EkTanımlamalar;
using Web.Models.Tanımlamalar;
using System.Web.Script.Serialization;
using Web.Framework.Mvc;
using Core.Domain.EkTanımlamalar;

namespace Web.Controllers
{
    public class KongreController : TemelPublicController
    {
        private readonly IİzinServisi _izinServisi;
        private readonly IKullanıcıİşlemServisi _kullanıcıİşlemServisi;
        private readonly ISiteServisi _siteServisi;
        private readonly IKonumServisi _konumServisi;
        private readonly IKatilimciServisi _katilimciServisi;
        private readonly IRefakatciServisi _refakatciServisi;
        private readonly IKayitServisi _kayitServisi;
        private readonly IKonaklamaServisi _konaklamaServisi;
        private readonly IKursServisi _kursServisi;
        private readonly ITransferServisi _transferServisi;
        private readonly IKongreServisi _kongreServisi;
        private readonly KatalogAyarları _katalogAyarları;
        private readonly IWorkContext _workContext;
        private readonly INotServisi _notServisi;
        private readonly IXlsServisi _xlsServisi;
        private readonly IKontenjanServisi _kontenjanServisi;
        private readonly IBankaBilgileriServisi _bankaBilgileriServisi;
        private readonly ITeklifKalemiServisi _teklifKalemiServisi;
        private readonly IGelirGiderHedefiServisi _gelirGiderServisi;
        private readonly IKontenjanBilgileriServisi _kontenjanBilgileriServisi;
        private readonly ITakvimServisi _takvimServisi;
        private readonly IGelirGiderTanımlamaServisi _gelirTanımlamaServisi;
        private readonly IFirmaServisi _firmaServisi;
        private readonly IKongreGörüşmeRaporlarıServisi _görüsmeServisi;
        private readonly ISponsorlukSatışıServisi _sponsorlukSatisServisi;
        private readonly IBankalarServisi _bankaServisi;
        private readonly IKayıtTipiServisi _kayıtTipiServisi;
        private readonly IKayıtBilgileriServisi _kayıtBilgileriServisi;
        private readonly IKursBilgileriServisi _kursBilgileriServisi;
        private readonly IGenelSponsorlukServisi _genelSponsorlukServisi;
        public KongreController(IİzinServisi izinServisi,
            ISiteServisi siteServisi,
            IKonumServisi konumServisi,
            IKullanıcıİşlemServisi kullanıcıİşlemServisi,
            IKatilimciServisi katilimciServisi,
            IKayitServisi kayitServisi,
            IKonaklamaServisi konaklamaServisi,
            IKursServisi kursServisi,
            ITransferServisi transferServisi,
            IKongreServisi kongreServisi,
            KatalogAyarları katalogAyarları,
            IRefakatciServisi refakatciServisi,
            IWorkContext workContext,
            INotServisi notServisi,
            IXlsServisi xlsServisi,
            IKontenjanServisi kontenjanServisi,
            IBankaBilgileriServisi bankaBilgileriServisi,
            ITeklifKalemiServisi teklifKalemiServisi,
            IGelirGiderHedefiServisi gelirGiderServisi,
            IKontenjanBilgileriServisi kontenjanBilgileriServisi,
            ITakvimServisi takvimServisi,
            IGelirGiderTanımlamaServisi gelirTanımlamaServisi,
            IFirmaServisi kongreFirmaServisi,
            IKongreGörüşmeRaporlarıServisi görüsmeServisi,
            ISponsorlukSatışıServisi sponsorlukSatisServisi,
            IBankalarServisi bankaServisi,
            IKayıtTipiServisi kayıtTipiServisi,
            IKayıtBilgileriServisi kayıtBilgileriServisi,
            IKursBilgileriServisi kursBilgileriServisi,
            IGenelSponsorlukServisi genelSponsorlukServisi)
        {
            this._izinServisi = izinServisi;
            this._siteServisi = siteServisi;
            this._konumServisi = konumServisi;
            this._katilimciServisi = katilimciServisi;
            this._konaklamaServisi = konaklamaServisi;
            this._kursServisi = kursServisi;
            this._transferServisi = transferServisi;
            this._kayitServisi = kayitServisi;
            this._kongreServisi = kongreServisi;
            this._kullanıcıİşlemServisi = kullanıcıİşlemServisi;
            this._katalogAyarları = katalogAyarları;
            this._refakatciServisi = refakatciServisi;
            this._workContext = workContext;
            this._notServisi = notServisi;
            this._xlsServisi = xlsServisi;
            this._kontenjanServisi = kontenjanServisi;
            this._bankaBilgileriServisi = bankaBilgileriServisi;
            this._teklifKalemiServisi = teklifKalemiServisi;
            this._gelirGiderServisi = gelirGiderServisi;
            this._kontenjanBilgileriServisi = kontenjanBilgileriServisi;
            this._takvimServisi = takvimServisi;
            this._gelirTanımlamaServisi = gelirTanımlamaServisi;
            this._firmaServisi = kongreFirmaServisi;
            this._görüsmeServisi = görüsmeServisi;
            this._sponsorlukSatisServisi=sponsorlukSatisServisi;
            this._bankaServisi = bankaServisi;
            this._kayıtTipiServisi = kayıtTipiServisi;
            this._kayıtBilgileriServisi = kayıtBilgileriServisi;
            this._kursBilgileriServisi = kursBilgileriServisi;
            this._genelSponsorlukServisi = genelSponsorlukServisi;
        }
        #region Utilities
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GelirKalemleriAl(bool gelir)
        {
            var kalemler = _gelirTanımlamaServisi.TümGelirGiderTanımlamaAl();
            var sonuc = (from s in kalemler where s.Gelir==gelir
                         select new
                         {
                             id = s.Id,
                             name = s.Adı
                         }).ToList();
            return Json(sonuc, JsonRequestBehavior.AllowGet);
        }
        class SponsorlukSatisModel
        {
            public int id { get; set; }
            public int tipi { get; set; }
            public string name { get; set; }
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult SponsorlukKalemleriAl(int kongreId,int sponsorlukTipi)
        {
            var sonuc = new List<SponsorlukSatisModel>();
            SponsorlukSatisModel ssm = new SponsorlukSatisModel();
            if (sponsorlukTipi == 1)
            {
                var kalemler = _kongreServisi.KongrelerAlId(kongreId).KayıtBilgileri;
                var list = _kongreServisi.KongrelerAlId(kongreId).KayıtBilgileri.Where(x => x.Tarihinden <= DateTime.Now && x.Tarihine >= DateTime.Now);
                foreach (var s in list)
                    sonuc.Add(new SponsorlukSatisModel { id = s.Id, name = s.Adı, tipi = 2 });
            }
            if (sponsorlukTipi == 2)
            {
                var kalemler = _kongreServisi.KongrelerAlId(kongreId).KontenjanBilgileri;
                var list = _kongreServisi.KongrelerAlId(kongreId).KontenjanBilgileri.Where(x => x.Tarihinden <= DateTime.Now && x.Tarihine >= DateTime.Now);
                foreach (var s in list)
                    sonuc.Add(new SponsorlukSatisModel { id = s.Id, name = s.Adı, tipi = 1 });
            }
            if (sponsorlukTipi == 3)
            {
                var kalemler = _kongreServisi.KongrelerAlId(kongreId).KursBilgileri;
                foreach (var s in _kongreServisi.KongrelerAlId(kongreId).KursBilgileri)
                    sonuc.Add(new SponsorlukSatisModel { id = s.Id, name = s.Adı, tipi = 2 });
            }
            return Json(sonuc, JsonRequestBehavior.AllowGet);
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

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult KayıtAlKongreId(string kongreId)
        {
            if (String.IsNullOrEmpty(kongreId))
            {
                throw new ArgumentNullException("kongreId");
            }
            int id = 0;
            bool isValid = Int32.TryParse(kongreId, out id);
            var kayıtlar = _kayitServisi.KayitAlKongreId(id);
            var sonuc = (from s in kayıtlar
                         select new
                         {
                             id = s.Id,
                             name = s.KayıtTipi
                         }).ToList();
            return Json(sonuc, JsonRequestBehavior.AllowGet);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult KonaklamaAlKongreId(string kongreId)
        {
            if (String.IsNullOrEmpty(kongreId))
            {
                throw new ArgumentNullException("kongreId");
            }
            int id = 0;
            bool isValid = Int32.TryParse(kongreId, out id);
            var konaklamalar = _konaklamaServisi.KonaklamaAlKongreId(id);
            var sonuc = (from s in konaklamalar
                         select new
                         {
                             id = s.Id,
                             name = s.KonaklamaAdı
                         }).ToList();
            return Json(sonuc, JsonRequestBehavior.AllowGet);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult KursAlKongreId(string kongreId)
        {
            if (String.IsNullOrEmpty(kongreId))
            {
                throw new ArgumentNullException("kongreId");
            }
            int id = 0;
            bool isValid = Int32.TryParse(kongreId, out id);
            var kurslar = _kursServisi.KursAlKongreId(id);
            var sonuc = (from s in kurslar
                         select new
                         {
                             id = s.Id,
                             name = s.KursAdı
                         }).ToList();
            return Json(sonuc, JsonRequestBehavior.AllowGet);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult TransferAlKongreId(string kongreId)
        {
            if (String.IsNullOrEmpty(kongreId))
            {
                throw new ArgumentNullException("kongreId");
            }
            int id = 0;
            bool isValid = Int32.TryParse(kongreId, out id);
            var transferler = _transferServisi.TransferAlKongreId(id);
            var sonuc = (from s in transferler
                         select new
                         {
                             id = s.Id,
                             name = s.Adı
                         }).ToList();
            return Json(sonuc, JsonRequestBehavior.AllowGet);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult OtelYukle(string kongreId)
        {
            if (String.IsNullOrEmpty(kongreId))
            {
                throw new ArgumentNullException("kongreId");
            }
            int id = 0;
            bool isValid = Int32.TryParse(kongreId, out id);
            var konts = _kontenjanServisi.KontenjanAlKongreId(id).ToList();
            var list = konts.GroupBy(x => x.OtelId)
                         .Where(g => g.Count() > 0)
                         .Select(g => g.Key)
                         .ToList();

            List<SelectListItem> n = new List<SelectListItem>();
            foreach(var a in list)
            {
                var otel = _firmaServisi.FirmaAlId(a);
                string adi = otel.Adı;
                int otelid = otel.Id;

                SelectListItem j = new SelectListItem();
                j.Text = adi;
                j.Value = otelid.ToString();
                n.Add(j);
            }
            return Json(n.ToList(), JsonRequestBehavior.AllowGet);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult YetkiliAlMusteriId(string musteriId)
        {
            if (String.IsNullOrEmpty(musteriId))
            {
                throw new ArgumentNullException("musteriId");
            }
            int id = 0;
            bool isValid = Int32.TryParse(musteriId, out id);
            var firma = _firmaServisi.FirmaAlId(id);
            var yetkililer = firma.Yetkililer;
            var sonuc = (from s in yetkililer
                         select new
                         {
                             id = s.Id,
                             name = s.Adı + " " + s.Soyadı
                         }).ToList();
            return Json(sonuc, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Kongre

        public virtual ActionResult KongreListe(int? page)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KonaklamaYönet))
                return ErişimEngellendiView();

            var pageSize = _katalogAyarları.ProductReviewsPageSizeOnAccountPage;
            int pageIndex = 0;

            if (page > 0)
            {
                pageIndex = page.Value - 1;
            }

            var list = _kongreServisi.TümKongrelerAl(pageIndex, pageSize);
            var kongreList = new List<KongreModel>();
            foreach (var kongre in list)
            {
                var kongreModel = new KongreModel
                {
                    Id = kongre.Id,
                    Adı = kongre.Adı,
                    BaslamaTarihi = kongre.BaslamaTarihi,
                    BitisTarihi = kongre.BitisTarihi,
                };
                kongreList.Add(kongreModel);
            };

            var pagerModel = new PagerModel
            {
                PageSize = list.PageSize,
                TotalRecords = list.TotalCount,
                PageIndex = list.PageIndex,
                ShowTotalSummary = false,
                RouteActionName = "SayfalananKongreler",
                UseRouteLinks = true,
                RouteValues = new KongrelerModel.KongrelerRouteValues { page = pageIndex }
            };

            var model = new KongrelerModel
            {
                Kongreler = kongreList,
                PagerModel = pagerModel
            };
           
            return View(model);
        }

        public virtual ActionResult KongreEkle()
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KonaklamaYönet))
                ErişimEngellendiView();

            var model = new KongreModel();
            foreach (var sehirler in _konumServisi.TümSehirleriAl())
                model.Sehirler.Add(new SelectListItem { Text = sehirler.Adı, Value = sehirler.Id.ToString() });
            foreach (var tumIlceler in _konumServisi.IlcelerAlSehirId(1))
                model.Ilceler.Add(new SelectListItem { Text = tumIlceler.Adı, Value = tumIlceler.Id.ToString() });

            return View(model);
        }

        [HttpPost, FormAdıParametresi("kaydet-devam", "düzenlemeyeDevam")]
        public virtual ActionResult KongreEkle(KongreModel model, bool düzenlemeyeDevam)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KonaklamaYönet))
                ErişimEngellendiView();
            if (ModelState.IsValid)
            {
                var Kongre = model.ToEntity();
                _kongreServisi.KongrelerEkle(Kongre);
                BaşarılıBildirimi("Kongre başarıyla Eklendi");
                _kullanıcıİşlemServisi.İşlemEkle("YeniKongreEklendi", "Yeni Kongre Eklendi", Kongre.Adı);
                if (düzenlemeyeDevam)
                {
                    SeçiliTabKaydet();
                    return RedirectToAction("KongreDüzenle", new { id = Kongre.Id });
                }
                return RedirectToAction("KongreListe");
            }
            return View(model);
        }
        public virtual ActionResult KongreDüzenle(int id)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KonaklamaYönet))
                ErişimEngellendiView();
            var Kongre = _kongreServisi.KongrelerAlId(id);
            if (Kongre == null)
            {
                return RedirectToAction("KongreListe");
            }
            var model = Kongre.ToModel();
            foreach (var sehirler in _konumServisi.TümSehirleriAl())
                model.Sehirler.Add(new SelectListItem { Text = sehirler.Adı, Value = sehirler.Id.ToString() });
            foreach (var tumIlceler in _konumServisi.IlcelerAlSehirId(model.SehirId))
                model.Ilceler.Add(new SelectListItem { Text = tumIlceler.Adı, Value = tumIlceler.Id.ToString() });
            SeçiliTabKaydet();
            return View(model);
        }
        [HttpPost, FormAdıParametresi("kaydet-devam", "düzenlemeyeDevam")]
        public virtual ActionResult KongreDüzenle(KongreModel model, bool düzenlemeyeDevam)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KonaklamaYönet))
                ErişimEngellendiView();
            var Kongre = _kongreServisi.KongrelerAlId(model.Id);
            if (Kongre == null)
            {
                return RedirectToAction("KongreListe");
            }
            if (ModelState.IsValid)
            {
                Kongre = model.ToEntity(Kongre);
                _kongreServisi.KongrelerGüncelle(Kongre);
                BaşarılıBildirimi("Kongre başarıyla güncellenmiştir.");
                _kullanıcıİşlemServisi.İşlemEkle("KongreGüncelle", "Kongre güncellendi", Kongre.Adı);
                if (düzenlemeyeDevam)
                {
                    SeçiliTabKaydet();
                    return RedirectToAction("KongreDüzenle", new { id = Kongre.Id });
                }
                return RedirectToAction("KongreListe");
            }
            SeçiliTabKaydet();
            return View(model);
        }

        [HttpPost]
        public virtual ActionResult KongreSil(KongreModel model)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KonaklamaYönet))
                return ErişimEngellendiView();
            var Kongre = _kongreServisi.KongrelerAlId(model.Id);
            if (Kongre == null)
                return RedirectToAction("KongreListe");
            _kongreServisi.KongrelerSil(Kongre);
            BaşarılıBildirimi("Kongre başarıyla silindi");
            _kullanıcıİşlemServisi.İşlemEkle("KongreSil", "Kongre silindi", Kongre.Adı);
            return RedirectToAction("KongreListe");
        }
        #endregion
        #region KongreBankaHesapBilgileri
        [NonAction]
        private void BankaBilgileriModelHazırla(KongreBankaBilgileriModel model, BankaHesapBilgileri kongreBankaBilgileri, Kongreler kongre)
        {
            if (kongre == null)
                throw new ArgumentNullException("kongre");

            model.KongreId = kongre.Id;
            if (kongreBankaBilgileri != null)
            {
                model.BankaHesapBilgileri = kongreBankaBilgileri.ToModel();
            }

            if (model.BankaHesapBilgileri == null)
                model.BankaHesapBilgileri = new BankaHesapBilgileriModel();
        }
        public partial class Bankamodel {
            public string HesapAdı { get; set; }
            public string VergiDairesi { get; set; }
            public string VergiNo { get; set; }
            public string TlHesabı { get; set; }
            public string DolarHesabı { get; set; }
            public string EuroHesabı { get; set; }
            public string Swift { get; set; }
            public string BankaAdı { get; set; }
        }

        [HttpGet]
        public virtual JsonResult BankaBilgileriListele(int kongreId)
        {
            var kongre = _kongreServisi.KongrelerAlId(kongreId);
            if (kongre != null)
            {
                BankaHesapBilgileri b = kongre.BankaHesapBilgileri.FirstOrDefault();
                if (b != null)
                {
                    Bankamodel l = new Bankamodel
                    {
                        BankaAdı = _bankaServisi.BankaAlId(b.BankaId).Adı,
                        HesapAdı = b.HesapAdı,
                        TlHesabı = b.TlHesabı,
                        DolarHesabı = b.DolarHesabı,
                        EuroHesabı = b.EuroHesabı,
                        VergiDairesi = b.VergiDairesi,
                        VergiNo = b.VergiNo,
                        Swift = b.Swift
                    };
                    return Json(l, JsonRequestBehavior.AllowGet);
                }
                else
                    return null;
            }
            else
            {
                return null;
            }
        }
        public virtual ActionResult BankaBilgileriEkle(int kongreId)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KatılımcıYönet))
                return ErişimEngellendiView();

            var kongre = _kongreServisi.KongrelerAlId(kongreId);
            if (kongre == null)
                return RedirectToAction("KongreListe");

            var model = new KongreBankaBilgileriModel();
            BankaBilgileriModelHazırla(model, null, kongre);
            foreach (var b in _bankaServisi.TümBankalarıAl())
                model.BankaHesapBilgileri.KullanılabilirBankalar.Add(new SelectListItem { Value = b.Id.ToString(), Text = b.Adı });
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public virtual ActionResult BankaBilgileriEkle(KongreBankaBilgileriModel model, FormCollection form)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KatılımcıYönet))
                return ErişimEngellendiView();

            var kongre = _kongreServisi.KongrelerAlId(model.KongreId);
            if (kongre == null)

                return RedirectToAction("KongreListe");

            if (ModelState.IsValid)
            {
                var banka = model.BankaHesapBilgileri.ToEntity();
                kongre.BankaHesapBilgileri.Add(banka);
                _kongreServisi.KongrelerGüncelle(kongre);

                BaşarılıBildirimi("Banka Hesap Bilgileri Eklendi");
                return RedirectToAction("KongreDüzenle", new { id = kongre.Id });
            }

            BankaBilgileriModelHazırla(model, null, kongre);
            return View(model);
        }
        public virtual ActionResult BankaBilgileriDüzenle(int kongreId)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KatılımcıYönet))
                return ErişimEngellendiView();

            var kongre = _kongreServisi.KongrelerAlId(kongreId);
            if (kongre == null)
                return RedirectToAction("KongreListe");

            var bankBilgi = kongre.BankaHesapBilgileri.FirstOrDefault();
            if (bankBilgi == null)
                return RedirectToAction("KongreDüzenle", new { id = kongre.Id });

            var model = new KongreBankaBilgileriModel();
            BankaBilgileriModelHazırla(model, bankBilgi, kongre);
            foreach (var b in _bankaServisi.TümBankalarıAl())
                model.BankaHesapBilgileri.KullanılabilirBankalar.Add(new SelectListItem { Value = b.Id.ToString(), Text = b.Adı });
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public virtual ActionResult BankaBilgileriDüzenle(KongreBankaBilgileriModel model, FormCollection form)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KatılımcıYönet))
                return ErişimEngellendiView();

            var kongre = _kongreServisi.KongrelerAlId(model.KongreId);
            if (kongre == null)
                return RedirectToAction("KongreListe");

            var bankaBilgi = _bankaBilgileriServisi.BankaHesapBilgileriAlId(model.BankaHesapBilgileri.Id);
            if (bankaBilgi == null)
                return RedirectToAction("KongreDüzenle", new { id = kongre.Id });
            
            if (ModelState.IsValid)
            {
                bankaBilgi = model.BankaHesapBilgileri.ToEntity(bankaBilgi);
                _bankaBilgileriServisi.BankaHesapBilgileriGüncelle(bankaBilgi);

                BaşarılıBildirimi("Banka hesap bilgileri güncellendi");
                return RedirectToAction("KongreDüzenle", new { id = kongre.Id });
            }
            BankaBilgileriModelHazırla(model, bankaBilgi, kongre);
            return View(model);
        }
        #endregion
        #region KongreGelirGiderHedefleri
        public virtual ActionResult Hedefler(int kongreId)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KongreYönet))
                return ErişimEngellendiView();

            var kongre = _kongreServisi.KongrelerAlId(kongreId);
            if (kongre == null)
                return RedirectToAction("KongreListe");
            var model = kongre.ToModel();
            return View(model);
        }
        [HttpPost]
        public virtual ActionResult Hedefler(DataSourceİsteği command, KongreModel model)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KongreYönet))
                return ErişimEngellendiView();

            /*var kongre = _kongreServisi.KongrelerAlId(kongreId);
            if (kongre == null)
                return RedirectToAction("KongreListe");
            var model = kongre.ToModel();*/
            return View(model);
        }

        [HttpPost]
        public virtual ActionResult HedefListeGelir(DataSourceİsteği command, int modelId)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.MusteriYönet))
                return ErişimEngellendiKendoGridJson();

            var teklifKalemiModels = _kongreServisi.KongrelerAlId(modelId).GelirGiderHedefi
                .Select(x =>
                {
                    var m = x.ToModel();
                    return m;
                }).Where(m=>m.Gelir==true)
                .ToList();
            var gridModel = new DataSourceSonucu
            {
                Data = teklifKalemiModels,
                Toplam = teklifKalemiModels.Count()
            };
            return Json(gridModel);
        }
        [HttpPost]
        public virtual ActionResult HedefListeGider(DataSourceİsteği command, int modelId)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.MusteriYönet))
                return ErişimEngellendiKendoGridJson();

            var teklifKalemiModels = _kongreServisi.KongrelerAlId(modelId).GelirGiderHedefi
                .Select(x =>
                {
                    var m = x.ToModel();
                    return m;
                }).Where(m => m.Gelir == false)
                .ToList();
            var gridModel = new DataSourceSonucu
            {
                Data = teklifKalemiModels,
                Toplam = teklifKalemiModels.Count()
            };
            return Json(gridModel);
        }
       
        [HttpPost]
        public virtual ActionResult HedefDüzenleGrid(IEnumerable<GelirGiderHedefiModel> models)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.TeklifHariciYönet))
                ErişimEngellendiView();

            if (models != null)
            {
                foreach (var pModel in models)
                {
                    //update
                    var oge = _gelirGiderServisi.GelirGiderHedefiAlId(pModel.Id);
                    if (oge != null)
                    {
                        oge = pModel.ToEntity(oge);
                        oge.Tutar = oge.BirimFiyat * oge.Adet*oge.Gün;
                        oge.GerçekleşenTutar = oge.GerçekleşenBirimFiyat * oge.GerçekleşenAdet * oge.Gün;
                        oge.Fark =  oge.GerçekleşenTutar- oge.Tutar;
                        decimal yüzde=0;
                        if(oge.Tutar != 0 && oge.GerçekleşenTutar != 0)
                        {
                            yüzde = oge.Fark / oge.Tutar*100;
                        }
                        else if(oge.Tutar == 0)
                        {
                            yüzde = oge.Fark;
                        }
                        else
                        {
                            yüzde = 0;
                        }
                        oge.GelirYüzde = yüzde;
                        _gelirGiderServisi.GelirGiderHedefiGüncelle(oge);
                    }
                }
            }
            return new BoşJsonSonucu();
        }

        [NonAction]
        private void GelirGideriModelHazırla(KongreGelirGiderHedefModel model, GelirGiderHedefi kongreGelirGiderHedefi, Kongreler kongre)
        {
            if (kongre == null)
                throw new ArgumentNullException("kongre");

            model.KongreId = kongre.Id;
            if (kongreGelirGiderHedefi != null)
            {
                model.GelirGiderHedefiModel = kongreGelirGiderHedefi.ToModel();
            }

            if (model.GelirGiderHedefiModel == null)
                model.GelirGiderHedefiModel = new GelirGiderHedefiModel();
        }
        [HttpGet]
        public virtual JsonResult GelirGideriBilgileriListele(int kongreId)
        {
            var kongre = _kongreServisi.KongrelerAlId(kongreId);
            if (kongre != null)
            {
                var b = kongre.GelirGiderHedefi;
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
        public virtual ActionResult HedefEkle(int kongreId)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KatılımcıYönet))
                return ErişimEngellendiView();

            var kongre = _kongreServisi.KongrelerAlId(kongreId);
            if (kongre == null)
                return RedirectToAction("KongreListe");
            
            var model = new KongreGelirGiderHedefModel();
            GelirGideriModelHazırla(model, null, kongre);
            List<GelirGiderTanımlama> l = _gelirTanımlamaServisi.TümGelirGiderTanımlamaAl().Where(x => x.Gelir == true).ToList();
            foreach (var musteri in l)
                model.GelirGiderHedefiModel.KullanılabilirKalemler.Add(new SelectListItem { Text = musteri.Adı, Value = musteri.Id.ToString() });
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public virtual ActionResult HedefEkle(KongreGelirGiderHedefModel model, FormCollection form)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KatılımcıYönet))
                return ErişimEngellendiView();

            var kongre = _kongreServisi.KongrelerAlId(model.KongreId);
            if (kongre == null)
                return RedirectToAction("KongreListe");

            if (ModelState.IsValid)
            {
                var gelir = model.GelirGiderHedefiModel.ToEntity();
                string ad = _gelirTanımlamaServisi.GelirGiderTanımlamaAlId(gelir.GelirKalemiId).Adı;
                gelir.Adı = ad;
                gelir.Tutar = gelir.Adet * gelir.BirimFiyat;
                gelir.GerçekleşenAdet = 1;
                gelir.GerçekleşenBirimFiyat = 1;
                kongre.GelirGiderHedefi.Add(gelir);
                _kongreServisi.KongrelerGüncelle(kongre);

                BaşarılıBildirimi("Gelir Gider Hedef Bilgileri Eklendi");
                return RedirectToAction("KongreDüzenle", new { id = kongre.Id });
            }

            GelirGideriModelHazırla(model, null, kongre);
            foreach (var musteri in _gelirTanımlamaServisi.TümGelirGiderTanımlamaAl())
                model.GelirGiderHedefiModel.KullanılabilirKalemler.Add(new SelectListItem { Text = musteri.Adı, Value = musteri.Id.ToString() });

            return View(model);
        }
        public virtual ActionResult HedefDüzenle(int hedefId, int kongreId)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KatılımcıYönet))
                return ErişimEngellendiView();

            var kongre = _kongreServisi.KongrelerAlId(kongreId);
            if (kongre == null)
                return RedirectToAction("KongreListe");

            var gelirBilgi = _gelirGiderServisi.GelirGiderHedefiAlId(hedefId);
            if (gelirBilgi == null)
                return RedirectToAction("KongreDüzenle", new { id = kongre.Id });

            var model = new KongreGelirGiderHedefModel();
            GelirGideriModelHazırla(model, gelirBilgi, kongre);
            /*foreach (var musteri in _gelirTanımlamaServisi.TümGelirGiderTanımlamaAl())
                model.GelirGiderHedefiModel.KullanılabilirKalemler.Add(new SelectListItem { Text = musteri.Adı, Value = musteri.Id.ToString() });
*/
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public virtual ActionResult HedefDüzenle(KongreGelirGiderHedefModel model, FormCollection form)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KatılımcıYönet))
                return ErişimEngellendiView();

            var kongre = _kongreServisi.KongrelerAlId(model.KongreId);
            if (kongre == null)
                return RedirectToAction("KongreListe");

            var gelirBilgi = _gelirGiderServisi.GelirGiderHedefiAlId(model.GelirGiderHedefiModel.Id);
            if (gelirBilgi == null)
                return RedirectToAction("KongreDüzenle", new { id = kongre.Id });

            if (ModelState.IsValid)
            {
                gelirBilgi = model.GelirGiderHedefiModel.ToEntity(gelirBilgi);
                _gelirGiderServisi.GelirGiderHedefiGüncelle(gelirBilgi);

                BaşarılıBildirimi("Gelir gider hedef bilgileri güncellendi");
                return RedirectToAction("HedefDüzenle", new { bankaId = model.GelirGiderHedefiModel.Id, customerId = model.KongreId });
            }
            GelirGideriModelHazırla(model, gelirBilgi, kongre);
            foreach (var musteri in _gelirTanımlamaServisi.TümGelirGiderTanımlamaAl())
                model.GelirGiderHedefiModel.KullanılabilirKalemler.Add(new SelectListItem { Text = musteri.Adı, Value = musteri.Id.ToString() });
            return View(model);
        }
        #endregion
        #region KongreKontenjanBilgileri
        [NonAction]
        private void KontenjanBilgileriModelHazırla(KongreKontenjanBilgileriModel model, KontenjanBilgileri kongreKontenjanBilgileri, Kongreler kongre)
        {
            if (kongre == null)
                throw new ArgumentNullException("kongre");

            model.KongreId = kongre.Id;
            if (kongreKontenjanBilgileri != null)
            {
                model.KontenjanBilgileriModel = kongreKontenjanBilgileri.ToModel();
                foreach (var oteller in _firmaServisi.FirmaAlKategoriId(2))
                    model.KontenjanBilgileriModel.Oteller.Add(new SelectListItem { Text = oteller.Adı, Value = oteller.Id.ToString() });
            }

            if (model.KontenjanBilgileriModel == null)
            {
                model.KontenjanBilgileriModel = new KontenjanBilgileriModel();
                foreach (var oteller in _firmaServisi.FirmaAlKategoriId(2))
                    model.KontenjanBilgileriModel.Oteller.Add(new SelectListItem { Text = oteller.Adı, Value = oteller.Id.ToString() });
            }
                
        }
        [HttpGet]
        public virtual JsonResult KontenjanBilgileriListele(int kongreId)
        {
            var kongre = _kongreServisi.KongrelerAlId(kongreId);
            if (kongre != null)
            {
                var b = kongre.KontenjanBilgileri;
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
        public virtual ActionResult KontenjanBilgileriEkle(int kongreId)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KatılımcıYönet))
                return ErişimEngellendiView();

            var kongre = _kongreServisi.KongrelerAlId(kongreId);
            if (kongre == null)
                return RedirectToAction("KongreListe");

            var model = new KongreKontenjanBilgileriModel();
            KontenjanBilgileriModelHazırla(model, null, kongre);
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public virtual ActionResult KontenjanBilgileriEkle(KongreKontenjanBilgileriModel model, FormCollection form)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KatılımcıYönet))
                return ErişimEngellendiView();

            var kongre = _kongreServisi.KongrelerAlId(model.KongreId);
            if (kongre == null)

                return RedirectToAction("KongreListe");

            if (ModelState.IsValid)
            {
                var kontenjan = model.KontenjanBilgileriModel.ToEntity();
                kontenjan.Adı = _firmaServisi.FirmaAlId(kontenjan.OtelId).Adı + "_" + kontenjan.Adı;
                kongre.KontenjanBilgileri.Add(kontenjan);
                _kongreServisi.KongrelerGüncelle(kongre);

                BaşarılıBildirimi("Kontenjan Bilgileri Eklendi");
                return RedirectToAction("KongreDüzenle", new { id = kongre.Id });
            }

            KontenjanBilgileriModelHazırla(model, null, kongre);
            return View(model);
        }
        public virtual ActionResult KontenjanBilgileriDüzenle(int kontenjanId, int kongreId)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KatılımcıYönet))
                return ErişimEngellendiView();

            var kongre = _kongreServisi.KongrelerAlId(kongreId);
            if (kongre == null)
                return RedirectToAction("KongreListe");

            var kontenjanBilgi = _kontenjanBilgileriServisi.KontenjanBilgileriAlId(kontenjanId);
            if (kontenjanBilgi == null)
                return RedirectToAction("KongreDüzenle", new { id = kongre.Id });

            var model = new KongreKontenjanBilgileriModel();
            KontenjanBilgileriModelHazırla(model, kontenjanBilgi, kongre);
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public virtual ActionResult KontenjanBilgileriDüzenle(KongreKontenjanBilgileriModel model, FormCollection form)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KatılımcıYönet))
                return ErişimEngellendiView();

            var kongre = _kongreServisi.KongrelerAlId(model.KongreId);
            if (kongre == null)
                return RedirectToAction("KongreListe");

            var kontenjanBilgi = _kontenjanBilgileriServisi.KontenjanBilgileriAlId(model.KontenjanBilgileriModel.Id);
            if (kontenjanBilgi == null)
                return RedirectToAction("KongreDüzenle", new { id = kongre.Id });

            if (ModelState.IsValid)
            {
                kontenjanBilgi = model.KontenjanBilgileriModel.ToEntity(kontenjanBilgi);
                kontenjanBilgi.Adı = _firmaServisi.FirmaAlId(kontenjanBilgi.OtelId).Adı + "_" + kontenjanBilgi.Adı;
                _kontenjanBilgileriServisi.KontenjanBilgileriGüncelle(kontenjanBilgi);

                BaşarılıBildirimi("Kontenjan bilgileri güncellendi");
                return RedirectToAction("KongreDüzenle", new { id = kongre.Id });
            }
            KontenjanBilgileriModelHazırla(model, kontenjanBilgi, kongre);
            return View(model);
        }
        #endregion
        #region KongreKayıtBilgileri
        [NonAction]
        private void KayıtBilgileriModelHazırla(KongreKayıtBilgileriModel model, KayıtBilgileri kongreKayıtBilgileri, Kongreler kongre)
        {
            if (kongre == null)
                throw new ArgumentNullException("kongre");

            model.KongreId = kongre.Id;
            if (kongreKayıtBilgileri != null)
            {
                model.KayıtBilgileriModel = kongreKayıtBilgileri.ToModel();
                foreach (var kayittipleri in _kayıtTipiServisi.TümKayıtTipiAl())
                    model.KayıtBilgileriModel.KayıtTipleri.Add(new SelectListItem { Text = kayittipleri.Adı, Value = kayittipleri.Id.ToString() });
            }

            if (model.KayıtBilgileriModel == null)
            {
                model.KayıtBilgileriModel = new KayıtBilgileriModel();
                foreach (var kayittipleri in _kayıtTipiServisi.TümKayıtTipiAl())
                    model.KayıtBilgileriModel.KayıtTipleri.Add(new SelectListItem { Text = kayittipleri.Adı, Value = kayittipleri.Id.ToString() });
            }
        }
        [HttpGet]
        public virtual JsonResult KayıtBilgileriListele(int kongreId)
        {
            var kongre = _kongreServisi.KongrelerAlId(kongreId);
            if (kongre != null)
            {
                var b = kongre.KayıtBilgileri;
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
        public virtual ActionResult KayıtBilgileriEkle(int kongreId)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KatılımcıYönet))
                return ErişimEngellendiView();

            var kongre = _kongreServisi.KongrelerAlId(kongreId);
            if (kongre == null)
                return RedirectToAction("KongreListe");

            var model = new KongreKayıtBilgileriModel();
            KayıtBilgileriModelHazırla(model, null, kongre);
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public virtual ActionResult KayıtBilgileriEkle(KongreKayıtBilgileriModel model, FormCollection form)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KatılımcıYönet))
                return ErişimEngellendiView();

            var kongre = _kongreServisi.KongrelerAlId(model.KongreId);
            if (kongre == null)

                return RedirectToAction("KongreListe");

            if (ModelState.IsValid)
            {
                var kayıt = model.KayıtBilgileriModel.ToEntity();
                kayıt.Adı = _kayıtTipiServisi.KayıtTipiAlId(kayıt.KayıtTipiId).Adı + "_" + kayıt.Adı;
                kongre.KayıtBilgileri.Add(kayıt);
                _kongreServisi.KongrelerGüncelle(kongre);

                BaşarılıBildirimi("Kontenjan Bilgileri Eklendi");
                return RedirectToAction("KongreDüzenle", new { id = kongre.Id });
            }

            KayıtBilgileriModelHazırla(model, null, kongre);
            return View(model);
        }
        public virtual ActionResult KayıtBilgileriDüzenle(int kayıtId, int kongreId)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KatılımcıYönet))
                return ErişimEngellendiView();

            var kongre = _kongreServisi.KongrelerAlId(kongreId);
            if (kongre == null)
                return RedirectToAction("KongreListe");

            var kayıtBilgi = _kayıtBilgileriServisi.KayıtBilgileriAlId(kayıtId);
            if (kayıtBilgi == null)
                return RedirectToAction("KongreDüzenle", new { id = kongre.Id });

            var model = new KongreKayıtBilgileriModel();
            KayıtBilgileriModelHazırla(model, kayıtBilgi, kongre);
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public virtual ActionResult KayıtBilgileriDüzenle(KongreKayıtBilgileriModel model, FormCollection form)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KatılımcıYönet))
                return ErişimEngellendiView();

            var kongre = _kongreServisi.KongrelerAlId(model.KongreId);
            if (kongre == null)
                return RedirectToAction("KongreListe");

            var kayıtBilgi = _kayıtBilgileriServisi.KayıtBilgileriAlId(model.KayıtBilgileriModel.Id);
            if (kayıtBilgi == null)
                return RedirectToAction("KongreDüzenle", new { id = kongre.Id });

            if (ModelState.IsValid)
            {
                kayıtBilgi = model.KayıtBilgileriModel.ToEntity(kayıtBilgi);
                kayıtBilgi.Adı = _firmaServisi.FirmaAlId(kayıtBilgi.KayıtTipiId).Adı + "_" + kayıtBilgi.Adı;
                _kayıtBilgileriServisi.KayıtBilgileriGüncelle(kayıtBilgi);

                BaşarılıBildirimi("Kayıt bilgileri güncellendi");
                return RedirectToAction("KongreDüzenle", new { id = kongre.Id });
            }
            KayıtBilgileriModelHazırla(model, kayıtBilgi, kongre);
            return View(model);
        }
        #endregion
        #region KongreKursBilgileri
        [NonAction]
        private void KursBilgileriModelHazırla(KongreKursBilgileriModel model, KursBilgileri kongreKursBilgileri, Kongreler kongre)
        {
            if (kongre == null)
                throw new ArgumentNullException("kongre");

            model.KongreId = kongre.Id;
            if (kongreKursBilgileri != null)
            {
                model.KursBilgileriModel = kongreKursBilgileri.ToModel();
            }

            if (model.KursBilgileriModel == null)
            {
                model.KursBilgileriModel = new KursBilgileriModel();
            }
        }
        [HttpGet]
        public virtual JsonResult KursBilgileriListele(int kongreId)
        {
            var kongre = _kongreServisi.KongrelerAlId(kongreId);
            if (kongre != null)
            {
                var b = kongre.KursBilgileri;
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
        public virtual ActionResult KursBilgileriEkle(int kongreId)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KatılımcıYönet))
                return ErişimEngellendiView();

            var kongre = _kongreServisi.KongrelerAlId(kongreId);
            if (kongre == null)
                return RedirectToAction("KongreListe");

            var model = new KongreKursBilgileriModel();
            KursBilgileriModelHazırla(model, null, kongre);
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public virtual ActionResult KursBilgileriEkle(KongreKursBilgileriModel model, FormCollection form)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KatılımcıYönet))
                return ErişimEngellendiView();

            var kongre = _kongreServisi.KongrelerAlId(model.KongreId);
            if (kongre == null)

                return RedirectToAction("KongreListe");

            if (ModelState.IsValid)
            {
                var kayıt = model.KursBilgileriModel.ToEntity();
                kongre.KursBilgileri.Add(kayıt);
                _kongreServisi.KongrelerGüncelle(kongre);

                BaşarılıBildirimi("Kontenjan Bilgileri Eklendi");
                return RedirectToAction("KongreDüzenle", new { id = kongre.Id });
            }

            KursBilgileriModelHazırla(model, null, kongre);
            return View(model);
        }
        public virtual ActionResult KursBilgileriDüzenle(int kayıtId, int kongreId)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KatılımcıYönet))
                return ErişimEngellendiView();

            var kongre = _kongreServisi.KongrelerAlId(kongreId);
            if (kongre == null)
                return RedirectToAction("KongreListe");

            var kayıtBilgi = _kursBilgileriServisi.KursBilgileriAlId(kayıtId);
            if (kayıtBilgi == null)
                return RedirectToAction("KongreDüzenle", new { id = kongre.Id });

            var model = new KongreKursBilgileriModel();
            KursBilgileriModelHazırla(model, kayıtBilgi, kongre);
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public virtual ActionResult KursBilgileriDüzenle(KongreKursBilgileriModel model, FormCollection form)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KatılımcıYönet))
                return ErişimEngellendiView();

            var kongre = _kongreServisi.KongrelerAlId(model.KongreId);
            if (kongre == null)
                return RedirectToAction("KongreListe");

            var kayıtBilgi = _kursBilgileriServisi.KursBilgileriAlId(model.KursBilgileriModel.Id);
            if (kayıtBilgi == null)
                return RedirectToAction("KongreDüzenle", new { id = kongre.Id });

            if (ModelState.IsValid)
            {
                kayıtBilgi = model.KursBilgileriModel.ToEntity(kayıtBilgi);
                kayıtBilgi.Adı =kayıtBilgi.Adı;
                _kursBilgileriServisi.KursBilgileriGüncelle(kayıtBilgi);

                BaşarılıBildirimi("Kurs bilgileri güncellendi");
                return RedirectToAction("KongreDüzenle", new { id = kongre.Id });
            }
            KursBilgileriModelHazırla(model, kayıtBilgi, kongre);
            return View(model);
        }
        #endregion
        #region KongreGenelSponsorlukBilgileri
        [NonAction]
        private void GenelSponsorlukModelHazırla(KongreGenelSponsorlukModel model, GenelSponsorluk kongreGenelSponsorluk, Kongreler kongre)
        {
            if (kongre == null)
                throw new ArgumentNullException("kongre");

            model.KongreId = kongre.Id;
            if (kongreGenelSponsorluk != null)
            {
                model.GenelSponsorlukModel = kongreGenelSponsorluk.ToModel();
            }

            if (model.GenelSponsorlukModel == null)
            {
                model.GenelSponsorlukModel = new GenelSponsorlukModel();
            }
        }
        [HttpGet]
        public virtual JsonResult GenelSponsorlukListele(int kongreId)
        {
            var kongre = _kongreServisi.KongrelerAlId(kongreId);
            if (kongre != null)
            {
                var b = kongre.GenelSponsorluk;
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
        public virtual ActionResult GenelSponsorlukEkle(int kongreId)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KatılımcıYönet))
                return ErişimEngellendiView();

            var kongre = _kongreServisi.KongrelerAlId(kongreId);
            if (kongre == null)
                return RedirectToAction("KongreListe");

            var model = new KongreGenelSponsorlukModel();
            GenelSponsorlukModelHazırla(model, null, kongre);
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public virtual ActionResult GenelSponsorlukEkle(KongreGenelSponsorlukModel model, FormCollection form)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KatılımcıYönet))
                return ErişimEngellendiView();

            var kongre = _kongreServisi.KongrelerAlId(model.KongreId);
            if (kongre == null)

                return RedirectToAction("KongreListe");

            if (ModelState.IsValid)
            {
                var kayıt = model.GenelSponsorlukModel.ToEntity();
                kongre.GenelSponsorluk.Add(kayıt);
                _kongreServisi.KongrelerGüncelle(kongre);

                BaşarılıBildirimi("Genel Sponsorluk Eklendi");
                return RedirectToAction("KongreDüzenle", new { id = kongre.Id });
            }

            GenelSponsorlukModelHazırla(model, null, kongre);
            return View(model);
        }
        public virtual ActionResult GenelSponsorlukDüzenle(int kayıtId, int kongreId)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KatılımcıYönet))
                return ErişimEngellendiView();

            var kongre = _kongreServisi.KongrelerAlId(kongreId);
            if (kongre == null)
                return RedirectToAction("KongreListe");

            var kayıtBilgi = _genelSponsorlukServisi.GenelSponsorlukAlId(kayıtId);
            if (kayıtBilgi == null)
                return RedirectToAction("KongreDüzenle", new { id = kongre.Id });

            var model = new KongreGenelSponsorlukModel();
            GenelSponsorlukModelHazırla(model, kayıtBilgi, kongre);
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public virtual ActionResult GenelSponsorlukDüzenle(KongreGenelSponsorlukModel model, FormCollection form)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KatılımcıYönet))
                return ErişimEngellendiView();

            var kongre = _kongreServisi.KongrelerAlId(model.KongreId);
            if (kongre == null)
                return RedirectToAction("KongreListe");

            var kayıtBilgi = _genelSponsorlukServisi.GenelSponsorlukAlId(model.GenelSponsorlukModel.Id);
            if (kayıtBilgi == null)
                return RedirectToAction("KongreDüzenle", new { id = kongre.Id });

            if (ModelState.IsValid)
            {
                kayıtBilgi = model.GenelSponsorlukModel.ToEntity(kayıtBilgi);
                kayıtBilgi.Adı = kayıtBilgi.Adı;
                _genelSponsorlukServisi.GenelSponsorlukGüncelle(kayıtBilgi);

                BaşarılıBildirimi("Genel Sponsorluk güncellendi");
                return RedirectToAction("KongreDüzenle", new { id = kongre.Id });
            }
            GenelSponsorlukModelHazırla(model, kayıtBilgi, kongre);
            return View(model);
        }
        #endregion
        #region KongreTransfer
        [NonAction]
        private void TransferModelHazırla(KongreTransferModel model, Transfer kongreTransfer, Kongreler kongre)
        {
            if (kongre == null)
                throw new ArgumentNullException("kongre");

            model.KongreId = kongre.Id;
            if (kongreTransfer != null)
            {
                model.TransferModel = kongreTransfer.ToModel();
            }

            if (model.TransferModel == null)
            {
                model.TransferModel = new TransferModel();
            }
        }
        [HttpGet]
        public virtual JsonResult TransferListele(int kongreId)
        {
            var kongre = _kongreServisi.KongrelerAlId(kongreId);
            if (kongre != null)
            {
                var b = kongre.Transfer;
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
        public virtual ActionResult TransferEkle(int kongreId)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KatılımcıYönet))
                return ErişimEngellendiView();

            var kongre = _kongreServisi.KongrelerAlId(kongreId);
            if (kongre == null)
                return RedirectToAction("KongreListe");

            var model = new KongreTransferModel();
            TransferModelHazırla(model, null, kongre);
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public virtual ActionResult TransferEkle(KongreTransferModel model, FormCollection form)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KatılımcıYönet))
                return ErişimEngellendiView();

            var kongre = _kongreServisi.KongrelerAlId(model.KongreId);
            if (kongre == null)

                return RedirectToAction("KongreListe");

            if (ModelState.IsValid)
            {
                var kayıt = model.TransferModel.ToEntity();
                kongre.Transfer.Add(kayıt);
                _kongreServisi.KongrelerGüncelle(kongre);

                BaşarılıBildirimi("Transfer Eklendi");
                return RedirectToAction("KongreDüzenle", new { id = kongre.Id });
            }

            TransferModelHazırla(model, null, kongre);
            return View(model);
        }
        public virtual ActionResult TransferDüzenle(int kayıtId, int kongreId)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KatılımcıYönet))
                return ErişimEngellendiView();

            var kongre = _kongreServisi.KongrelerAlId(kongreId);
            if (kongre == null)
                return RedirectToAction("KongreListe");

            var kayıtBilgi = _transferServisi.TransferAlId(kayıtId);
            if (kayıtBilgi == null)
                return RedirectToAction("KongreDüzenle", new { id = kongre.Id });

            var model = new KongreTransferModel();
            TransferModelHazırla(model, kayıtBilgi, kongre);
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public virtual ActionResult TransferDüzenle(KongreTransferModel model, FormCollection form)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KatılımcıYönet))
                return ErişimEngellendiView();

            var kongre = _kongreServisi.KongrelerAlId(model.KongreId);
            if (kongre == null)
                return RedirectToAction("KongreListe");

            var kayıtBilgi = _transferServisi.TransferAlId(model.TransferModel.Id);
            if (kayıtBilgi == null)
                return RedirectToAction("KongreDüzenle", new { id = kongre.Id });

            if (ModelState.IsValid)
            {
                kayıtBilgi = model.TransferModel.ToEntity(kayıtBilgi);
                kayıtBilgi.Adı = kayıtBilgi.Adı;
                _transferServisi.TransferGüncelle(kayıtBilgi);

                BaşarılıBildirimi("Transfer güncellendi");
                return RedirectToAction("KongreDüzenle", new { id = kongre.Id });
            }
            TransferModelHazırla(model, kayıtBilgi, kongre);
            return View(model);
        }
        #endregion
        #region KongreFirmaBilgileri

        [NonAction]
        private void FirmaBilgileriModelHazırla(KongreFirmaBilgileriModel model, List<FirmaModel> kongreFirmaBilgileri, Kongreler kongre)
        {
            if (kongre == null)
                throw new ArgumentNullException("kongre");

            model.KongreId = kongre.Id;
            if (kongreFirmaBilgileri != null)
            {
                model.FirmaBilgileriModel = kongreFirmaBilgileri;
            }

            if (model.FirmaBilgileriModel == null)
                model.FirmaBilgileriModel = new List<FirmaModel>();
        }
        [HttpGet]
        public virtual JsonResult FirmaBilgileriListele(int kongreId)
        {
            var kongre = _kongreServisi.KongrelerAlId(kongreId);
            if (kongre != null)
            {
                var b = kongre.FirmaBilgileri;
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
        public virtual ActionResult FirmaBilgileriEkle(int kongreId)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KatılımcıYönet))
                return ErişimEngellendiView();

            var kongre = _kongreServisi.KongrelerAlId(kongreId);
            if (kongre == null)
                return RedirectToAction("KongreListe");

            var model = new KongreFirmaBilgileriModel();
            foreach (var musteri in _firmaServisi.FirmaAlKategoriId(1))
                model.FirmaMusterileri.Add(new SelectListItem { Text = musteri.Adı, Value = musteri.Id.ToString() });

            //FirmaBilgileriModelHazırla(model, null, kongre);
            return View(model);
        }
        
        [HttpPost]
        [ValidateInput(false)]
        public virtual ActionResult FirmaBilgileriEkle(KongreFirmaBilgileriModel model, FormCollection form)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KatılımcıYönet))
                return ErişimEngellendiView();

            var kongre = _kongreServisi.KongrelerAlId(model.KongreId);
            if (kongre == null)

                return RedirectToAction("KongreListe");

            if (ModelState.IsValid)
            {
                if (model.MusteriIdleri.Count > 0)
                {
                    foreach (var musteriId in model.MusteriIdleri)
                    {
                        var musteri = _firmaServisi.FirmaAlId(musteriId);
                        kongre.FirmaBilgileri.Add(musteri);
                    }
                    _kongreServisi.KongrelerGüncelle(kongre);
                    BaşarılıBildirimi("Firma Bilgileri Eklendi");
                    return RedirectToAction("KongreDüzenle", new { id = kongre.Id });
                }
            }

            //FirmaBilgileriModelHazırla(model, null, kongre);
            return View(model);
        }
        
        public virtual ActionResult FirmaBilgileriDüzenle(int kongreId)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KatılımcıYönet))
                return ErişimEngellendiView();

            var kongre = _kongreServisi.KongrelerAlId(kongreId);
            if (kongre == null)
                return RedirectToAction("KongreListe");
            var model = new KongreFirmaBilgileriModel();
            List<FirmaModel> l = new List<FirmaModel>();

            foreach (var musteri in _firmaServisi.FirmaAlKategoriId(1))
                model.FirmaMusterileri.Add(new SelectListItem { Text = musteri.Adı, Value = musteri.Id.ToString() });

            foreach (var musteriId in kongre.FirmaBilgileri)
            {
                var musteri = _firmaServisi.FirmaAlId(musteriId.Id);
                SelectListItem a = model.FirmaMusterileri.FirstOrDefault(x => x.Value == musteri.Id.ToString());
                a.Selected = true;
                model.MusteriIdleri.Add(musteri.Id);
                l.Add(musteri.ToModel());
            }
            model.FirmaBilgileriModel = l;
            FirmaBilgileriModelHazırla(model, l, kongre);
            return View(model);
        }
        
        [HttpPost]
        [ValidateInput(false)]
        public virtual ActionResult FirmaBilgileriDüzenle(KongreFirmaBilgileriModel model, FormCollection form)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KatılımcıYönet))
                return ErişimEngellendiView();

            var kongre = _kongreServisi.KongrelerAlId(model.KongreId);
            if (kongre == null)
                return RedirectToAction("KongreListe");
            List<FirmaModel> l = new List<FirmaModel>();
            if (ModelState.IsValid)
            {
                if (model.MusteriIdleri.Count > 0)
                {
                    kongre.FirmaBilgileri.Clear();
                    foreach (var musteriId in model.MusteriIdleri)
                    {
                        var musteri = _firmaServisi.FirmaAlId(musteriId);
                        kongre.FirmaBilgileri.Add(musteri);
                        l.Add(musteri.ToModel());
                    }
                }
                _kongreServisi.KongrelerGüncelle(kongre);
                BaşarılıBildirimi("Firma Bilgileri Eklendi");
                return RedirectToAction("KongreDüzenle", new { id = kongre.Id });
            }
            
            FirmaBilgileriModelHazırla(model, l, kongre);
            return View(model);
        }
        #endregion
        #region KongreSponsorlukBilgileri
        
        [HttpGet]
        public virtual JsonResult SponsorlukBilgileriListele(int kongreId)
        {
            var kongre = _kongreServisi.KongrelerAlId(kongreId);
            if (kongre != null)
            {
                var b = kongre.SponsorlukBilgileri;
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
        [NonAction]
        private void SponsorlukBilgileriModelHazırla(KongreSponsorlukBilgileriModel model, List<FirmaModel> kongreSponsorlukBilgileri, Kongreler kongre)
        {
            if (kongre == null)
                throw new ArgumentNullException("kongre");

            model.KongreId = kongre.Id;
            if (kongreSponsorlukBilgileri != null)
            {
                model.SponsorlukBilgileriModel = kongreSponsorlukBilgileri;
            }

            if (model.SponsorlukBilgileriModel == null)
                model.SponsorlukBilgileriModel = new List<FirmaModel>();
        }
        public virtual ActionResult SponsorlukBilgileri(int kongreId)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KatılımcıYönet))
                return ErişimEngellendiView();

            var kongre = _kongreServisi.KongrelerAlId(kongreId);
            if (kongre == null)
                return RedirectToAction("KongreListe");
            var model = new KongreSponsorlukBilgileriModel();
            model.KongreId = kongreId;
            foreach (var musteriler in kongre.SponsorlukBilgileri)
            {
                var mustid = musteriler.Id;
                var sponsormodel = new KongreSponsorlukBilgileriModel();
                var gorusme = _görüsmeServisi.KongreGörüşmeRaporlarıKongreId(kongre.Id, mustid);
                var firma = _firmaServisi.FirmaAlId(mustid);
                string yetkiliadi="";
                if (firma != null)
                {
                    YetkililerModel kym = firma.Yetkililer.Where(x => x.Id == gorusme.YetkiliId).FirstOrDefault().ToModel();
                    if(kym!=null)
                        yetkiliadi = kym.Adı + " " + kym.Soyadı;
                }
                
                KongreGörüşmeRaporlarıModel kgr = new KongreGörüşmeRaporlarıModel
                {
                    KongreId = gorusme.KongreId,
                    MusteriId = gorusme.MusteriId,
                    YetkiliId = gorusme.YetkiliId,
                    GörüsmeTarihi = gorusme.GörüsmeTarihi,
                    OlusturulmaTarihi = gorusme.OlusturulmaTarihi,
                    Rapor = gorusme.Rapor,
                    Durumu = gorusme.Durumu,
                    Görüsen = gorusme.GörüsenId,
                    MusteriAdı = musteriler.Adı,
                    YetkiliAdı = yetkiliadi
                };
                model.SponsorlukBilgileriModel.Add(musteriler.ToModel());
                model.GörüsmeRaporlarıModel.Add(kgr);
            }
            return View(model);
        }
        public virtual ActionResult SponsorlukBilgileriEkle(int kongreId)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KatılımcıYönet))
                return ErişimEngellendiView();

            var kongre = _kongreServisi.KongrelerAlId(kongreId);
            if (kongre == null)
                return RedirectToAction("KongreListe");

            var model = new KongreSponsorlukBilgileriModel();
            foreach (var musteri in kongre.FirmaBilgileri)
                model.SponsorlukMusterileri.Add(new SelectListItem { Text = musteri.Adı, Value = musteri.Id.ToString() });
            foreach (var yetkililer in kongre.FirmaBilgileri.FirstOrDefault().Yetkililer)
                model.MusteriYetkilileri.Add(new SelectListItem { Text = yetkililer.Adı, Value = yetkililer.Id.ToString() });
            //SponsorlukBilgileriModelHazırla(model, null, kongre);
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public virtual ActionResult SponsorlukBilgileriEkle(KongreSponsorlukBilgileriModel model, FormCollection form)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KatılımcıYönet))
                return ErişimEngellendiView();

            var kongre = _kongreServisi.KongrelerAlId(model.KongreId);
            if (kongre == null)
                return RedirectToAction("KongreListe");

            if (ModelState.IsValid)
            {
                KongreGörüşmeRaporları t = new KongreGörüşmeRaporları
                {
                    MusteriId=model.MusteriId,
                    KongreId=model.KongreId,
                    YetkiliId=model.YetkiliId,
                    OlusturulmaTarihi=DateTime.Now,
                    GörüsmeTarihi=model.GörüsmeTarihi,
                    GörüsenId=_workContext.MevcutKullanıcı.Id,
                    Rapor=model.Rapor
                };
                kongre.SponsorlukBilgileri.Add(_firmaServisi.FirmaAlId(model.MusteriId));
                _görüsmeServisi.KongreGörüşmeRaporlarıEkle(t);
                _kongreServisi.KongrelerGüncelle(kongre);
                BaşarılıBildirimi("Firma Bilgileri Eklendi");
                return RedirectToAction("SponsorlukBilgileri", new { kongreId = kongre.Id });
            }
            //SponsorlukBilgileriModelHazırla(model, null, kongre);
            return View(model);
        }

        public virtual ActionResult SponsorlukBilgileriDüzenle(int kongreId,int musteriId)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KatılımcıYönet))
                return ErişimEngellendiView();

            var kongre = _kongreServisi.KongrelerAlId(kongreId);
            if (kongre == null)
                return RedirectToAction("KongreListe");
            var musteri = _firmaServisi.FirmaAlId(musteriId);
            if (musteri == null)
                return RedirectToAction("KongreListe");
            var model = new KongreSponsorlukBilgileriModel();
            List<FirmaModel> l = new List<FirmaModel>();
            foreach (var m in kongre.SponsorlukBilgileri)
                model.SponsorlukBilgileriModel.Add(m.ToModel());
            foreach (var musteriler in kongre.FirmaBilgileri)
                model.SponsorlukMusterileri.Add(new SelectListItem { Text = musteriler.Adı, Value = musteriler.Id.ToString() });
            foreach (var yetkili in _firmaServisi.FirmaAlId(musteriId).Yetkililer)
                model.MusteriYetkilileri.Add(new SelectListItem { Text = yetkili.Adı, Value = yetkili.Id.ToString() });

            var t = _görüsmeServisi.KongreGörüşmeRaporlarıKongreId(kongreId, musteriId);
            model.MusteriYetkilileri.Where(x => x.Value == t.YetkiliId.ToString()).Select(x => x.Selected);
            model.SponsorlukMusterileri.Where(x => x.Value == musteri.Id.ToString()).Select(x => x.Selected);
            model.YetkiliId = t.YetkiliId;
            model.GörüsmeTarihi = t.GörüsmeTarihi;
            model.Rapor = t.Rapor;

            SponsorlukBilgileriModelHazırla(model, l, kongre);
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public virtual ActionResult SponsorlukBilgileriDüzenle(KongreSponsorlukBilgileriModel model, FormCollection form)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KatılımcıYönet))
                return ErişimEngellendiView();

            var kongre = _kongreServisi.KongrelerAlId(model.KongreId);
            if (kongre == null)
                return RedirectToAction("KongreListe");
            List<FirmaModel> l = new List<FirmaModel>();
            if (ModelState.IsValid)
            {
                
                KongreGörüşmeRaporları t = _görüsmeServisi.KongreGörüşmeRaporlarıKongreId(model.KongreId, model.MusteriId);
                
                t.YetkiliId = model.YetkiliId;
                t.OlusturulmaTarihi = DateTime.Now;
                t.GörüsmeTarihi = model.GörüsmeTarihi;
                t.GörüsenId = _workContext.MevcutKullanıcı.Id;
                t.Rapor = model.Rapor;
                
                foreach (var m in kongre.SponsorlukBilgileri)
                    model.SponsorlukBilgileriModel.Add(m.ToModel());

                _görüsmeServisi.KongreGörüşmeRaporlarıGüncelle(t);
                _kongreServisi.KongrelerGüncelle(kongre);
                BaşarılıBildirimi("Firma Bilgileri Eklendi");
                return RedirectToAction("SponsorlukBilgileri", new { kongreId = kongre.Id });
                
            }
            SponsorlukBilgileriModelHazırla(model, l, kongre);
            return View(model);
        }
        #endregion
        #region KongreSponsorlukSatışBilgileri
        [HttpPost]
        public virtual ActionResult SponsorlukSatisListe(DataSourceİsteği command, int kongreId,int sponsorId)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KongreYönet))
                return ErişimEngellendiKendoGridJson();

            var sponsorlukSatislariModels = _sponsorlukSatisServisi.SponsorlukSatışıAlKongreId(kongreId,sponsorId)
                .Where(y => y.KongreId == kongreId&&y.SponsorId==sponsorId)
                .Select(x =>
                {
                    var m = x.ToModel();
                    return m;
                }).ToList();
            var gridModel = new DataSourceSonucu
            {
                Data = sponsorlukSatislariModels,
                Toplam = sponsorlukSatislariModels.Count()
            };
            return Json(gridModel);
        }
        
        [HttpPost]
        public virtual ActionResult SponsorlukSatisGuncelleGrid(IEnumerable<SponsorlukSatışıModel> models)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KongreYönet))
                ErişimEngellendiView();

            if (models != null)
            {
                foreach (var pModel in models)
                {
                    //update
                    var oge = _sponsorlukSatisServisi.SponsorlukSatışıAlId(pModel.Id);
                    decimal birimfiyat = 0;
                    int ogeid = 0;
                    string ogeadi = "";
                    if (oge != null)
                    {
                        if (oge.Tipi != pModel.Tipi)
                        {
                            if (pModel.Tipi == 1)
                            {
                                ogeid = _kongreServisi.KongrelerAlId(pModel.KongreId).KayıtBilgileri.FirstOrDefault().Id;
                                ogeadi = _kongreServisi.KongrelerAlId(pModel.KongreId).KayıtBilgileri.FirstOrDefault().Adı;
                                birimfiyat = _kongreServisi.KongrelerAlId(pModel.KongreId).KayıtBilgileri.FirstOrDefault().Tutar;
                            }
                            if (pModel.Tipi == 2)
                            {
                                ogeid = _kongreServisi.KongrelerAlId(pModel.KongreId).KontenjanBilgileri.FirstOrDefault().Id;
                                ogeadi = _kongreServisi.KongrelerAlId(pModel.KongreId).KontenjanBilgileri.FirstOrDefault().Adı;
                                birimfiyat = _kongreServisi.KongrelerAlId(pModel.KongreId).KontenjanBilgileri.FirstOrDefault().Tutar;
                            }
                            if (pModel.Tipi == 3)
                            {
                                ogeid = _kongreServisi.KongrelerAlId(pModel.KongreId).KursBilgileri.FirstOrDefault().Id;
                                ogeadi = _kongreServisi.KongrelerAlId(pModel.KongreId).KursBilgileri.FirstOrDefault().Adı;
                                birimfiyat = _kongreServisi.KongrelerAlId(pModel.KongreId).KursBilgileri.FirstOrDefault().Tutar;
                            }
                            oge = pModel.ToEntity(oge);
                            oge.BirimFiyat = birimfiyat;
                            oge.OgeId = ogeid;
                            oge.Adı = ogeadi;
                            oge.Tutar = oge.BirimFiyat * oge.Adet * oge.Gün;
                            _sponsorlukSatisServisi.SponsorlukSatışıGüncelle(oge);
                        }
                        else
                        {
                            
                            if (pModel.Adı != oge.Adı)
                            {
                                if (oge.Tipi == 1)
                                {
                                    ogeid = _kongreServisi.KongrelerAlId(pModel.KongreId).KayıtBilgileri.Where(x => x.Adı == pModel.Adı).FirstOrDefault().Id;
                                    birimfiyat = _kongreServisi.KongrelerAlId(pModel.KongreId).KayıtBilgileri.Where(x => x.Id == oge.OgeId).FirstOrDefault().Tutar;
                                }
                                if (oge.Tipi == 2)
                                {
                                    ogeid = _kongreServisi.KongrelerAlId(pModel.KongreId).KontenjanBilgileri.Where(x => x.Adı == pModel.Adı).FirstOrDefault().Id;
                                    birimfiyat = _kongreServisi.KongrelerAlId(pModel.KongreId).KontenjanBilgileri.Where(x => x.Id == oge.OgeId).FirstOrDefault().Tutar;
                                }
                                if (oge.Tipi == 3)
                                {
                                    ogeid = _kongreServisi.KongrelerAlId(pModel.KongreId).KursBilgileri.Where(x => x.Adı == pModel.Adı).FirstOrDefault().Id;
                                    birimfiyat = _kongreServisi.KongrelerAlId(pModel.KongreId).KursBilgileri.Where(x => x.Id == oge.OgeId).FirstOrDefault().Tutar;
                                }
                            }
                            else
                            {
                                birimfiyat = pModel.BirimFiyat;
                                ogeid = pModel.OgeId;
                            }
                            oge = pModel.ToEntity(oge);
                            oge.BirimFiyat = birimfiyat;
                            oge.OgeId = ogeid;
                            oge.Tutar = oge.BirimFiyat * oge.Adet * oge.Gün;
                            _sponsorlukSatisServisi.SponsorlukSatışıGüncelle(oge);
                        }

                    }
                }
            }
            return new BoşJsonSonucu();
        }
        [HttpPost]
        [ValidateInput(false)]
        public virtual ActionResult SponsorlukSatisEkleGrid(int kongreId, int sponsorId)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KongreYönet))
                return ErişimEngellendiView();

            if (kongreId == 0 || sponsorId == 0)
                return null;
            var kalem = _kongreServisi.KongrelerAlId(kongreId).KontenjanBilgileri.FirstOrDefault();
            SponsorlukSatışı model = new SponsorlukSatışı
            {
                Adı = kalem == null ? "" : kalem.Adı,
                Adet = 0,
                BirimFiyat = kalem.Tutar,
                Tutar = 0,
                Döviz = 1,
                Gün = 0,
                KongreId = kongreId,
                SponsorId = sponsorId,
                Tipi = 2,
                OgeId=kalem.Id
            };

            _sponsorlukSatisServisi.SponsorlukSatışıEkle(model);
            BaşarılıBildirimi("Sponsorluk Satışı Eklendi");
            var sponsorlukSatislariModels = _sponsorlukSatisServisi.SponsorlukSatışıAlKongreId(kongreId, sponsorId)
                .Where(y => y.KongreId == kongreId && y.SponsorId == sponsorId)
                .Select(x =>
                {
                    var m = x.ToModel();
                    return m;
                }).ToList();
            var gridModel = new DataSourceSonucu
            {
                Data = sponsorlukSatislariModels,
                Toplam = sponsorlukSatislariModels.Count()
            };
            return Json(gridModel);
        }
        [HttpPost]
        public virtual ActionResult SponsorlukSatisSilGrid(IEnumerable<SponsorlukSatışıModel> ogeler)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.KongreYönet))
                return ErişimEngellendiView();
            var model = _sponsorlukSatisServisi.SponsorlukSatışıAlId(ogeler.FirstOrDefault().Id);
            _sponsorlukSatisServisi.SponsorlukSatışıSil(model);
            BaşarılıBildirimi("Satış başarıyla silindi");
            var sponsorlukSatislariModels = _sponsorlukSatisServisi.SponsorlukSatışıAlKongreId(model.KongreId, model.SponsorId)
                .Where(y => y.KongreId == model.KongreId && y.SponsorId == model.SponsorId)
                .Select(x =>
                {
                    var m = x.ToModel();
                    return m;
                }).ToList();
            var gridModel = new DataSourceSonucu
            {
                Data = sponsorlukSatislariModels,
                Toplam = sponsorlukSatislariModels.Count()
            };
            return Json(gridModel);
        }
        
        #endregion
        #region Takvim
        [HttpPost]
        public virtual JsonResult TakvimListe(int kongreId)
        {
            var kongre = _kongreServisi.KongrelerAlId(kongreId);
            return Json(kongre.TakvimBilgileri.ToList());
        }
        [HttpPost]
        public JsonResult TakvimGüncelle(int kongreId, string models)
        {
            var kongre = _kongreServisi.KongrelerAlId(kongreId);
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            var tasks = javaScriptSerializer.Deserialize<IEnumerable<TakvimModel>>(models);

            if (tasks != null)
            {
                foreach (var task in tasks)
                {
                    kongre.TakvimBilgileri.Add(task.ToEntity());
                }
                _kongreServisi.KongrelerGüncelle(kongre);
            }

            return this.Json(tasks);
        }
        [HttpPost]
        public JsonResult TakvimSil(int kongreId,string models)
        {
            var kongre = _kongreServisi.KongrelerAlId(kongreId);
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            var tasks = javaScriptSerializer.Deserialize<IEnumerable<TakvimModel>>(models);

            if (tasks != null)
            {
                foreach (var task in tasks)
                {
                    kongre.TakvimBilgileri.Remove(task.ToEntity());
                }
                _kongreServisi.KongrelerGüncelle(kongre);
            }

            return this.Json(tasks);
        }
        [HttpPost]
        public JsonResult TakvimEkle(int kongreId,string models)
        {
            var kongre = _kongreServisi.KongrelerAlId(kongreId);
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            var tasks = javaScriptSerializer.Deserialize<IEnumerable<TakvimModel>>(models);
            if (tasks != null)
            {
               // _takvimServisi.TakvimEkle(tasks.ToEntity());
                foreach (var task in tasks)
                {
                    kongre.TakvimBilgileri.Add(task.ToEntity());
                }
                _kongreServisi.KongrelerGüncelle(kongre);
            }

            return Json(tasks);
        }
        #endregion

    }
}
