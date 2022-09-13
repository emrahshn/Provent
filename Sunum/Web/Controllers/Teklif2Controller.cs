using Services.Güvenlik;
using Services.Siteler;
using System.Linq;
using System.Web.Mvc;
using Web.Framework.Controllers;
using Web.Framework.Kendoui;
using Web.Uzantılar;
using Services.Logging;
using Services.Konum;
using Services.EkTanımlamalar;
using Web.Models.Teklif;
using Services.Teklifler;
using System.Collections.Generic;
using Web.Framework.Mvc;
using Services.Tanımlamalar;
using Core.Domain.Teklif;
using System.IO;
using Services.Klasör;
using Core;
using Services.Kullanıcılar;
using Core.Domain.Kullanıcılar;
using Services.DovizServisi;
using Services.Notlar;
using System;
using System.Text;
using System.Globalization;
using Core.Domain.Notlar;
using Web.Models.Tanımlamalar;

namespace Web.Controllers
{
    public class Teklif2Controller : TemelPublicController
    {
        private readonly IİzinServisi _izinServisi;
        private readonly IKullanıcıİşlemServisi _kullanıcıİşlemServisi;
        private readonly ISiteServisi _siteServisi;
        private readonly IKonumServisi _konumServisi;
        private readonly ITeklif2Servisi _teklifServisi;
        private readonly IBagliTeklifOgesi2Servisi _bagliTeklifServisi;
        private readonly ITeklifHariciServisi _teklifHariciServisi;
        private readonly IBagliTeklifOgesiHariciServisi _bagliTeklifHariciServisi;
        private readonly ITeklifKalemiServisi _teklifKalemiServisi;
        private readonly IPdfServisi _pdfServisi;
        private readonly IKullanıcıServisi _kullanıcıServisi;
        private readonly IDovizServisi _dovizServisi;
        private readonly IWorkContext _workContext;
        private readonly INotServisi _notServisi;
        private readonly IXlsServisi _xlsServisi;
        private readonly IHariciSektorServisi _hariciSektorServisi;
        public Teklif2Controller(IİzinServisi izinServisi,
            ISiteServisi siteServisi,
            IKonumServisi konumServisi,
            IKullanıcıİşlemServisi kullanıcıİşlemServisi,
            ITeklif2Servisi teklifServisi,
            ITeklifKalemiServisi teklifKalemiServisi,
            IBagliTeklifOgesi2Servisi bagliTeklifServisi, 
            IPdfServisi pdfServisi,
            ITeklifHariciServisi teklifHariciServisi,
            IBagliTeklifOgesiHariciServisi bagliTeklifHariciServisi,
            IKullanıcıServisi kullanıcıServisi,
            IDovizServisi dovizServisi,
            IWorkContext workContext,
            INotServisi notServisi,
            IXlsServisi xlsServisi,
            IHariciSektorServisi hariciSektorServisi)
        {
            this._izinServisi = izinServisi;
            this._siteServisi = siteServisi;
            this._konumServisi = konumServisi;
            this._kullanıcıİşlemServisi = kullanıcıİşlemServisi;
            this._teklifServisi = teklifServisi;
            this._teklifKalemiServisi = teklifKalemiServisi;
            this._bagliTeklifServisi = bagliTeklifServisi;
            this._pdfServisi = pdfServisi;
            this._teklifHariciServisi = teklifHariciServisi;
            this._bagliTeklifHariciServisi = bagliTeklifHariciServisi;
            this._kullanıcıServisi = kullanıcıServisi;
            this._dovizServisi = dovizServisi;
            this._notServisi = notServisi;
            this._workContext = workContext;
            this._xlsServisi = xlsServisi;
            this._hariciSektorServisi = hariciSektorServisi;
        }
        #region Utilities
        CultureInfo tr = new CultureInfo("tr-TR");
        [HttpGet]
        public virtual ActionResult DolarKuruAl()
        {
            var dolarKuru = _dovizServisi.DolarKuruAl();
            string kur = dolarKuru.ToString();
            kur = kur.Replace(".", ",");
            return Json(kur, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public virtual ActionResult EuroKuruAl()
        {
            var euroKuru = _dovizServisi.EuroKuruAl();
            string kur = euroKuru.ToString();
            kur = kur.Replace(".", ",");
            return Json(kur, JsonRequestBehavior.AllowGet);
        }
        /*
        public virtual ActionResult PdfTeklif(int teklifId)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.TeklifYönet))
                return ErişimEngellendiKendoGridJson();

            var teklif = _teklifServisi.TeklifAlId(teklifId);
            var teklifler = new List<Teklif2>();
            teklifler.Add(teklif);
            byte[] bytes;
            using (var stream = new MemoryStream())
            {
                _pdfServisi.TeklifPdfOlustur(stream, teklifler);
                bytes = stream.ToArray();
            }
            return File(bytes, MimeTipleri.ApplicationPdf, string.Format("teklif_{0}.pdf", teklif.Id));
        }
        
        public virtual ActionResult PdfRapor(int teklifId)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.TeklifYönet))
                return ErişimEngellendiKendoGridJson();

            var teklif = _teklifServisi.TeklifAlId(teklifId);
            var teklifler = new List<Teklif>();
            teklifler.Add(teklif);
            byte[] bytes;
            using (var stream = new MemoryStream())
            {
                _pdfServisi.TeklifRaporOlustur(stream, teklifler);
                bytes = stream.ToArray();
            }
            return File(bytes, MimeTipleri.ApplicationPdf, string.Format("rapor_{0}.pdf", teklif.Id));
        }
        public virtual ActionResult XlsRapor(int teklifId)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.TeklifYönet))
                return ErişimEngellendiKendoGridJson();

            var teklif = _teklifServisi.TeklifAlId(teklifId);
            var teklifler = new List<Teklif>();
            teklifler.Add(teklif);
            StringBuilder sb = _xlsServisi.TeklifRaporOlustur(teklifler);
            HttpContext.Response.AddHeader("content-disposition", "attachment; filename = "+teklif.Adı+".xls");
            this.Response.ContentType = "application/vnd.ms-excel";
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(sb.ToString());
            return File(buffer, "application/vnd.ms-excel");
        }
        */
        [HttpGet]
        public virtual JsonResult TeklifTree()
        {
            /*if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.TeklifYönet))
                return ErişimEngellendiKendoGridJson();
                */
            var TeklifKalemleri = _teklifKalemiServisi.TümTeklifKalemleriAl();

            //var teklifKalemi =_teklifKalemiServisi.TeklifKalemiAlId()
            var childs = (from a in TeklifKalemleri
                          where (a.NodeId != null)
                          select new
                          {
                              id = a.Id,
                              adi = a.Adı,
                              nodeid = a.NodeId
                          }).ToList();
            var sonuc = (from s in TeklifKalemleri
                         where (s.NodeId == null)
                         select new
                         {
                             id = s.Id,
                             nodeid = s.NodeId,
                             adi = s.Adı,
                             items = (from a in childs
                                      where (a.nodeid == s.Id)
                                      select a)
                         }).ToList();
            return Json(sonuc, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public virtual ActionResult Chart(int teklifId)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.TeklifYönet))
                return ErişimEngellendiKendoGridJson();
            var TeklifModels = _bagliTeklifServisi.TümBagliTeklifOgesi2Al(true);
            var items = (from s in TeklifModels
                         where (s.TeklifId == teklifId)
                         group s by s.Tparent into g
                         let count = g.Count()
                         select new
                         {
                             id = g.First().Id,
                             ad = g.Key,
                             count = g.Count()
                         }).ToList();
            return Json(items, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public virtual ActionResult ChartHarici(int teklifId)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.TeklifHariciYönet))
                return ErişimEngellendiKendoGridJson();
            var TeklifModels = _bagliTeklifHariciServisi.TümBagliTeklifOgesiAl(true);
            var items = (from s in TeklifModels
                         where (s.TeklifId == teklifId)
                         group s by s.Tparent into g
                         let count = g.Count()
                         select new
                         {
                             id = g.First().Id,
                             ad = g.Key,
                             count = g.Count()
                         }).ToList();
            return Json(items, JsonRequestBehavior.AllowGet);
        }
       
        #endregion
        #region Teklif
        public virtual ActionResult TeklifListe()
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.TeklifYönet))
                return ErişimEngellendiView();
            
            var model = new Teklif2Model();
            return View(model);
        }

        [HttpPost]
        public virtual ActionResult TeklifListe(DataSourceİsteği command, Teklif2Model model)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.TeklifYönet))
                return ErişimEngellendiKendoGridJson();

            var TeklifModels = _teklifServisi.TümTeklifAl(true);
            var TamamlananTeklifModels = _teklifServisi.TümTeklifAl(true).Where(x=>x.Biten>0);
            var KonfirmeTeklifModels = _teklifServisi.TümTeklifAl(true).Where(x => x.Konfirme > 0&&x.Biten<1);

            List<Teklif2> list = new List<Teklif2>();
            foreach (var m in _teklifServisi.TümTeklifAl(true))
            {
                bool sorgu1 = TamamlananTeklifModels.Where(x => x.OrijinalTeklifId == m.Id).Count() > 0;
                bool sorgu2 = KonfirmeTeklifModels.Where(x => x.OrijinalTeklifId == m.Id).Count() > 0;
                bool sorgu3 =false;
                if(KonfirmeTeklifModels.Where(x => x.Id == m.Id).Count() > 0)
                {
                    var id = KonfirmeTeklifModels.Where(x => x.Id == m.Id).FirstOrDefault().OrijinalTeklifId;
                    if (TamamlananTeklifModels.Where(x => x.OrijinalTeklifId == id).Count() > 0)
                        sorgu3 = true;
                }
                if ((!sorgu1||!sorgu2)&&!sorgu3)
                {
                    list.Add(m);
                }
            }
            
            var gridModel = new DataSourceSonucu
            {
                Data = list.Select(x =>
                {
                    var TeklifModel = x.ToModel();
                    string hazırlayanAdı = "";
                    if (TeklifModel.HazırlayanId > 0)
                    {
                        var hazirlayan = _kullanıcıServisi.KullanıcıAlId(TeklifModel.HazırlayanId);
                        if (hazirlayan != null)
                        {
                            hazırlayanAdı = hazirlayan.TamAdAl();
                        }
                    }
                    TeklifModel.Hazırlayan = hazırlayanAdı;
                    return TeklifModel;
                }),
                Toplam = TeklifModels.Count
            };
            
            return Json(gridModel);
        }
        
        public virtual ActionResult TeklifEkle()
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.TeklifYönet))
                ErişimEngellendiView();

            var model = new Teklif2Model();
            model.KurDolar = 1;
            model.KurEuro = 1;
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
            return View(model);
        }

        [HttpPost, FormAdıParametresi("kaydet-devam", "düzenlemeyeDevam")]
        public virtual ActionResult TeklifEkle(Teklif2Model model, bool düzenlemeyeDevam,string Durumu="Hazırlık")
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.TeklifYönet))
                ErişimEngellendiView();
            
            if (ModelState.IsValid)
            {
                var Teklif = model.ToEntity();
                Teklif.OlusturulmaTarihi = DateTime.Now;
                Teklif.Durumu = "Operasyon";
                Teklif.Operasyon = 1;
                Teklif.HazırlayanId = _workContext.MevcutKullanıcı.Id;
                _teklifServisi.TeklifEkle(Teklif);
                Teklif.OrijinalTeklifId = Teklif.Id;
                _teklifServisi.TeklifGüncelle(Teklif);
                var teklif = _teklifServisi.TeklifAlId(Teklif.Id);
                BaşarılıBildirimi("Teklifbaşarıyla Eklendi");
                _kullanıcıİşlemServisi.İşlemEkle("YeniTeklifEklendi", "Yeni Teklif Eklendi", Teklif.Id);
                return RedirectToAction("TeklifDüzenle", new { id = Teklif.Id });
            }
            return View(model);
        }
        public virtual ActionResult TeklifDüzenle(int id)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.TeklifYönet))
                ErişimEngellendiView();
            var Teklif = _teklifServisi.TeklifAlId(id);
            if (Teklif == null)
            {
                return RedirectToAction("TeklifListe");
            }
            var model = Teklif.ToModel();
            foreach (var tumUlkeler in _konumServisi.TümUlkeleriAl())
            {
                var ulkeModel = tumUlkeler.ToModel();
                model.Ulkeler.Add(ulkeModel);
            }
            foreach (var tumSehirler in _konumServisi.SehirlerAlUlkeId(model.UlkeId))
            {
                var sehirModel = tumSehirler.ToModel();
                model.Sehirler.Add(sehirModel);
            }
            return View(model);
        }
        [HttpPost, FormAdıParametresi("kaydet-devam", "düzenlemeyeDevam")]
        public virtual ActionResult TeklifDüzenle(Teklif2Model model, bool düzenlemeyeDevam)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.TeklifYönet))
                ErişimEngellendiView();
            var Teklif = _teklifServisi.TeklifAlId(model.Id);
            if (Teklif == null)
            {
                return RedirectToAction("TeklifListe");
            }
            if (ModelState.IsValid)
            {
                Teklif = model.ToEntity(Teklif);
                Teklif.Durumu = "Operasyon";
                Teklif.Operasyon = 1;
                Teklif.OrijinalTeklifId = model.Id;
                Teklif.HazırlayanId = _workContext.MevcutKullanıcı.Id; 
                _teklifServisi.TeklifGüncelle(Teklif);
                BaşarılıBildirimi("Teklif başarıyla güncellenmiştir.");
                _kullanıcıİşlemServisi.İşlemEkle("TeklifGüncelle", "Teklif güncellendi", Teklif.Id);
                if (düzenlemeyeDevam)
                {
                    return RedirectToAction("TeklifDüzenle", new { id = Teklif.Id });
                }
                return RedirectToAction("TeklifDüzenle", new { id = Teklif.Id });
            }
            return View(model);
        }

        [HttpPost]
        public virtual ActionResult TeklifSil(Teklif2Model model)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.TeklifYönet))
                return ErişimEngellendiView();

            var Teklif = _teklifServisi.TeklifAlId(model.Id);
            if (Teklif == null)
                return RedirectToAction("TeklifListe");
            foreach(var teklifOgeleri in _bagliTeklifServisi.BagliTeklifOgesi2AlTeklifId(Teklif.Id))
            {
                _bagliTeklifServisi.BagliTeklifOgesi2Sil(teklifOgeleri);
            }
            _teklifServisi.TeklifSil(Teklif);
            BaşarılıBildirimi("Teklif başarıyla silindi");
            _kullanıcıİşlemServisi.İşlemEkle("TeklifSil", "Teklif silindi", Teklif.Id);
            return RedirectToAction("TeklifListe");
        }

        public virtual ActionResult TeklifKopyala(int teklifId,string durumu)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.TeklifYönet))
                ErişimEngellendiView();

            var Teklif = _teklifServisi.TeklifAlId(teklifId);
            //var Teklif = model.ToEntity();
            Teklif.Durumu = durumu;
            if (durumu == "Konfirme")
            {
                Teklif.Operasyon = 1;
                Teklif.Konfirme = 1;
                Teklif.OrijinalTeklifId = teklifId;
            }
            if (durumu == "Tamamlandı")
            {
                Teklif.Operasyon = 1;
                Teklif.Konfirme = 1;
                Teklif.Biten = 1;
                Teklif.OrijinalTeklifId = _teklifServisi.TeklifAlId(teklifId).OrijinalTeklifId; 
            }
            
            foreach (var oge in _bagliTeklifServisi.BagliTeklifOgesi2AlTeklifId(teklifId))
            {
                oge.TeklifId = Teklif.Id;

                decimal kar = oge.SatisFiyat - oge.AlisFiyat;
                kar = Math.Round(kar, 2);
                decimal gelir = oge.SatisFiyat == 0 ? 0 : (kar / oge.SatisFiyat * 100);
                gelir = Math.Round(gelir, 2);

                oge.Kar = kar;
                oge.KarDolar= Math.Round(kar/Teklif.KurDolar, 2);
                oge.KarEuro= Math.Round(kar/Teklif.KurEuro, 2);
                oge.Gelir = gelir.ToString();

                _bagliTeklifServisi.BagliTeklifOgesi2Ekle(oge);
                Teklif.BagliTeklifOgesi2.Add(oge);
            }
            _teklifServisi.TeklifEkle(Teklif);
            BaşarılıBildirimi("Teklifbaşarıyla Eklendi");
            _kullanıcıİşlemServisi.İşlemEkle("YeniTeklifEklendi", "Yeni Teklif Eklendi", Teklif.Id);
            return RedirectToAction("TeklifDüzenle", new { id = Teklif.Id });

        }
        #endregion
        #region Bağlı Teklif Öğesi

        [HttpPost]
        public virtual ActionResult BağlıTeklifListe(int teklifId,string grid, DataSourceİsteği command, Teklif2Model model)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.TeklifYönet))
                return ErişimEngellendiKendoGridJson();
            if (teklifId > 0)
            {
                var TeklifModels = _teklifServisi.TeklifAlId(teklifId).BagliTeklifOgesi2;
                var gridModel = new DataSourceSonucu
                {
                    Data = TeklifModels.Select(x =>
                    {
                        var TeklifModel = x.ToModel();
                        TeklifModel.ParabirimiDeger = TeklifModel.Parabirimi == 1 ? "TL" : (TeklifModel.Parabirimi == 2) ? "USD" : "EURO";
                        tr.NumberFormat.NumberDecimalSeparator = ",";
                        System.Threading.Thread.CurrentThread.CurrentCulture = tr;
                        return TeklifModel;
                    }),
                    Toplam = TeklifModels.Count()
                };
                return Json(gridModel);
            }
            var bosData = new DataSourceSonucu
            {
                Data = { },
                Toplam = 0
            };
            return Json(bosData);
        }

        [HttpPost]
        public virtual ActionResult BağlıTeklifEkle(DataSourceİsteği command, BagliTeklifOgesi2Model model,int treeItemId, int teklifId)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.TeklifYönet))
                ErişimEngellendiView();
            if (ModelState.IsValid)
            {
                var teklifOgesi = new BagliTeklifOgesi2();
                //Teklif.OlusturulmaTarihi = DateTime.Now;
                teklifOgesi.TeklifId = teklifId;
                var treeitem = _teklifKalemiServisi.TeklifKalemiAlId(treeItemId);
                var nodetreeitem = _teklifKalemiServisi.TeklifKalemiAlId(treeitem.NodeId.Value);
                teklifOgesi.Adı = treeitem.Adı;
                teklifOgesi.Tparent = nodetreeitem.Adı;
                teklifOgesi.Vparent = nodetreeitem.Id;
                teklifOgesi.Kdv = treeitem.Kdv;
                teklifOgesi.Kurum = "";
                var teklif = _teklifServisi.TeklifAlId(teklifId);
                teklif.BagliTeklifOgesi2.Add(teklifOgesi);
                _teklifServisi.TeklifGüncelle(teklif);
                BaşarılıBildirimi("TeklifOgesibaşarıyla Eklendi");
                _kullanıcıİşlemServisi.İşlemEkle("YeniTeklifEklendi", "Yeni Teklif Eklendi", teklif.Id);
                return RedirectToAction("TeklifListe");
            }
            return View(model);
        }
        
        
        [HttpPost]
        public virtual ActionResult BagliTeklifDuzenle(IEnumerable<BagliTeklifOgesi2Model> ogeler)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.TeklifYönet))
                ErişimEngellendiView();
            if (ogeler != null)
            {
                var kurDolar = _teklifServisi.TeklifAlId(ogeler.FirstOrDefault().TeklifId).KurDolar;
                var kurEuro = _teklifServisi.TeklifAlId(ogeler.FirstOrDefault().TeklifId).KurEuro;
                foreach (var pModel in ogeler)
                {
                    //update
                    var oge = _bagliTeklifServisi.BagliTeklifOgesi2AlId(pModel.Id);
                    if (oge != null)
                    {
                        double af1 = Convert.ToDouble(oge.AlisBirimFiyat);
                        decimal af = Convert.ToDecimal(oge.AlisBirimFiyat);
                        decimal sf = Convert.ToDecimal(oge.SatisBirimFiyat);
                        oge = pModel.ToEntity(oge);
                        int alisadet = 0, satisadet = 0, gun = 0, p = 1;
                        if (oge.Adet > 0)
                            satisadet = oge.Adet;
                        if (oge.AlisAdet > 0)
                            alisadet = oge.AlisAdet;
                        if (oge.Gun != 0)
                            gun = oge.Gun;
                        p = oge.Parabirimi == 1 ? 1 : oge.Parabirimi == 2 ? 2 : 3;

                        oge.AlisFiyat = p == 1 ? oge.AlisBirimFiyat * alisadet * gun : p == 2 ? oge.AlisBirimFiyat * alisadet * gun * kurDolar : oge.AlisBirimFiyat * alisadet * gun * kurEuro;
                        oge.AlisFiyatDolar = p == 2 ? oge.AlisBirimFiyat * alisadet * gun  : p == 1 ? oge.AlisBirimFiyat * alisadet * gun / kurDolar : oge.AlisBirimFiyat * alisadet * gun * kurEuro / kurDolar;
                        oge.AlisFiyatEuro =p==3? oge.AlisBirimFiyat * alisadet * gun :p==1? oge.AlisBirimFiyat * alisadet * gun / kurEuro: oge.AlisBirimFiyat * alisadet * gun * kurDolar / kurEuro;
                        oge.SatisFiyat = p == 1 ? oge.SatisBirimFiyat * satisadet * gun : p == 2 ? oge.SatisBirimFiyat * satisadet * gun * kurDolar : oge.SatisBirimFiyat * satisadet * gun * kurEuro;
                        oge.SatisFiyatDolar = p == 2 ? oge.SatisBirimFiyat * satisadet * gun : p == 1 ? oge.SatisBirimFiyat * satisadet * gun / kurDolar : oge.SatisBirimFiyat * satisadet * gun * kurEuro / kurDolar;
                        oge.SatisFiyatEuro = p == 3 ? oge.SatisBirimFiyat * satisadet * gun : p == 1 ? oge.SatisBirimFiyat * satisadet * gun / kurEuro : oge.SatisBirimFiyat * satisadet * gun * kurDolar / kurEuro;

                        oge.AlisBirimFiyatDolar = p == 1 ? oge.AlisBirimFiyat *kurDolar:p==2?oge.AlisBirimFiyat:oge.AlisBirimFiyat*kurEuro/kurDolar;
                        oge.AlisBirimFiyatEuro =  p == 1 ? oge.AlisBirimFiyat * kurEuro:p==2? oge.AlisBirimFiyat * kurDolar / kurEuro:oge.AlisBirimFiyat;
                        oge.SatisBirimFiyatDolar = p == 1 ? oge.SatisBirimFiyat *kurDolar:p==2?oge.SatisBirimFiyat : oge.SatisBirimFiyat * kurEuro/kurDolar;
                        oge.SatisBirimFiyatEuro = p == 1 ? oge.SatisBirimFiyat * kurEuro : p == 2 ? oge.SatisBirimFiyat * kurDolar / kurEuro : oge.SatisBirimFiyat;

                        decimal al = oge.AlisBirimFiyat;
                        decimal al2 = oge.AlisBirimFiyat;
                        decimal al3 = oge.AlisBirimFiyat;
                        decimal al4 = oge.AlisBirimFiyat;
                        decimal al5 = oge.AlisBirimFiyat;

                        decimal kar = oge.SatisFiyat - oge.AlisFiyat;
                        kar = Math.Round(kar, 2);
                        decimal gelir = oge.SatisFiyat == 0 ? 0 : (kar / oge.SatisFiyat * 100);
                        gelir = Math.Round(gelir, 2);

                        oge.Kar = kar;
                        oge.Gelir = gelir.ToString();
                        _bagliTeklifServisi.BagliTeklifOgesi2Güncelle(oge);
                    }
                }
            }
            return new BoşJsonSonucu();
        }
        
        [HttpPost]
        public virtual ActionResult BagliTeklifSil(IEnumerable<BagliTeklifOgesi2Model> ogeler)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.TeklifYönet))
                return ErişimEngellendiView();
            if (ogeler != null)
            {
                foreach (var pModel in ogeler)
                {
                    var teklif = _teklifServisi.TeklifAlId(pModel.TeklifId);
                    teklif.BagliTeklifOgesi2.Remove(pModel.ToEntity());
                    _teklifServisi.TeklifGüncelle(teklif);
                }
                
            }
            return new BoşJsonSonucu();
        }
        #endregion

    }
}