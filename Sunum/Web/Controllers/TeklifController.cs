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
using Newtonsoft.Json;
using Services.Medya;

namespace Web.Controllers
{
    public class TeklifController : TemelPublicController
    {
        private readonly IİzinServisi _izinServisi;
        private readonly IKullanıcıİşlemServisi _kullanıcıİşlemServisi;
        private readonly ISiteServisi _siteServisi;
        private readonly IKonumServisi _konumServisi;
        private readonly ITeklifServisi _teklifServisi;
        private readonly IBagliTeklifOgesiServisi _bagliTeklifServisi;
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
        private readonly IResimServisi _resimServisi;
        public TeklifController(IİzinServisi izinServisi,
            ISiteServisi siteServisi,
            IKonumServisi konumServisi,
            IKullanıcıİşlemServisi kullanıcıİşlemServisi,
            ITeklifServisi teklifServisi,
            ITeklifKalemiServisi teklifKalemiServisi,
            IBagliTeklifOgesiServisi bagliTeklifServisi, 
            IPdfServisi pdfServisi,
            ITeklifHariciServisi teklifHariciServisi,
            IBagliTeklifOgesiHariciServisi bagliTeklifHariciServisi,
            IKullanıcıServisi kullanıcıServisi,
            IDovizServisi dovizServisi,
            IWorkContext workContext,
            INotServisi notServisi,
            IXlsServisi xlsServisi,
            IHariciSektorServisi hariciSektorServisi,
            IResimServisi resimServisi)
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
            this._resimServisi = resimServisi;
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
        public virtual ActionResult PdfTeklif(int teklifId)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.TeklifYönet))
                return ErişimEngellendiKendoGridJson();

            var teklif = _teklifServisi.TeklifAlId(teklifId);
            var teklifler = new List<Teklif>();
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
            var TeklifModels = _bagliTeklifServisi.TümBagliTeklifOgesiAl(true);
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
            
            var model = new TeklifModel();
            return View(model);
        }

        [HttpPost]
        public virtual ActionResult TeklifListe(DataSourceİsteği command, TeklifModel model)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.TeklifYönet))
                return ErişimEngellendiKendoGridJson();

            var TeklifModels = _teklifServisi.TümTeklifAl(true);
            var TamamlananTeklifModels = _teklifServisi.TümTeklifAl(true).Where(x=>x.Biten>0);
            var KonfirmeTeklifModels = _teklifServisi.TümTeklifAl(true).Where(x => x.Konfirme > 0&&x.Biten<1);

            List<Teklif> list = new List<Teklif>();
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

            var model = new TeklifModel();
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
        public virtual ActionResult TeklifEkle(TeklifModel model, bool düzenlemeyeDevam,string Durumu="Hazırlık")
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
                //Teklif.OlusturulmaTarihi = DateTime.Now;
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
        public virtual ActionResult TeklifDüzenle(TeklifModel model, bool düzenlemeyeDevam)
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
        public virtual ActionResult TeklifSil(TeklifModel model)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.TeklifYönet))
                return ErişimEngellendiView();

            var Teklif = _teklifServisi.TeklifAlId(model.Id);
            if (Teklif == null)
                return RedirectToAction("TeklifListe");
            foreach(var teklifOgeleri in _bagliTeklifServisi.BagliTeklifOgesiAlTeklifId(Teklif.Id))
            {
                _bagliTeklifServisi.BagliTeklifOgesiSil(teklifOgeleri);
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
            _teklifServisi.TeklifEkle(Teklif);
            foreach (var oge in _bagliTeklifServisi.BagliTeklifOgesiAlTeklifId(teklifId))
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

                _bagliTeklifServisi.BagliTeklifOgesiEkle(oge);
            }
            var teklif = _teklifServisi.TeklifAlId(Teklif.Id);
            BaşarılıBildirimi("Teklifbaşarıyla Eklendi");
            _kullanıcıİşlemServisi.İşlemEkle("YeniTeklifEklendi", "Yeni Teklif Eklendi", Teklif.Id);
            return RedirectToAction("TeklifDüzenle", new { id = Teklif.Id });

        }
        #endregion
        #region Bağlı Teklif Öğesi

        [HttpPost]
        public virtual ActionResult BağlıTeklifListe(int teklifId,string grid, DataSourceİsteği command, TeklifModel model)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.TeklifYönet))
                return ErişimEngellendiKendoGridJson();
            if (teklifId > 0)
            {
                var TeklifModels = _bagliTeklifServisi.BagliTeklifOgesiAlTeklifId(teklifId,grid);
                var gridModel = new DataSourceSonucu
                {
                    Data = TeklifModels.Select(x =>
                    {
                        var TeklifModel = x.ToModel();
                        TeklifModel.ParabirimiDeger = TeklifModel.Parabirimi == 1 ? "TL" : (TeklifModel.Parabirimi == 2) ? "USD" : "EURO";
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
        public virtual ActionResult BağlıTeklifEkle(DataSourceİsteği command, BagliTeklifOgesiModel model,int treeItemId, int teklifId)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.TeklifYönet))
                ErişimEngellendiView();
            if (ModelState.IsValid)
            {
                var Teklif = new BagliTeklifOgesi();
                //Teklif.OlusturulmaTarihi = DateTime.Now;
                Teklif.TeklifId = teklifId;
                var treeitem = _teklifKalemiServisi.TeklifKalemiAlId(treeItemId);
                var nodetreeitem = _teklifKalemiServisi.TeklifKalemiAlId(treeitem.NodeId.Value);
                Teklif.Adı = treeitem.Adı;
                Teklif.Tparent = nodetreeitem.Adı;
                Teklif.Vparent = nodetreeitem.Id;
                Teklif.Kdv = treeitem.Kdv;
                Teklif.Kurum = "";
                _bagliTeklifServisi.BagliTeklifOgesiEkle(Teklif);
                BaşarılıBildirimi("TeklifOgesibaşarıyla Eklendi");
                _kullanıcıİşlemServisi.İşlemEkle("YeniTeklifEklendi", "Yeni Teklif Eklendi", Teklif.Id);
                return RedirectToAction("TeklifListe");
            }
            return View(model);
        }
        
        
        [HttpPost]
        public virtual ActionResult BagliTeklifDuzenle(IEnumerable<BagliTeklifOgesiModel> ogeler)
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
                    var oge = _bagliTeklifServisi.BagliTeklifOgesiAlId(pModel.Id);
                    if (oge != null)
                    {
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


                        decimal kar = oge.SatisFiyat - oge.AlisFiyat;
                        kar = Math.Round(kar, 2);
                        decimal gelir = oge.SatisFiyat == 0 ? 0 : (kar / oge.SatisFiyat * 100);
                        gelir = Math.Round(gelir, 2);

                        oge.Kar = kar;
                        oge.Gelir = gelir.ToString();
                        _bagliTeklifServisi.BagliTeklifOgesiGüncelle(oge);
                    }
                }
            }
            return new BoşJsonSonucu();
        }

        [HttpPost]
        public virtual ActionResult BagliTeklifDuzenle2(BagliTeklifOgesiModel[] ogeler)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.TeklifYönet))
                ErişimEngellendiView();
            if (ogeler != null)
            {
                /*foreach (var pModel in ogeler)
                {
                    //update
                    var oge = _bagliTeklifServisi.BagliTeklifOgesiAlId(pModel.Id);
                    if (oge != null)
                    {
                        oge = pModel.ToEntity(oge);
                        _bagliTeklifServisi.BagliTeklifOgesiGüncelle(oge);
                    }
                }*/
            }
            return new BoşJsonSonucu();
        }

        [HttpPost]
        public virtual ActionResult BagliTeklifSil(IEnumerable<BagliTeklifOgesiModel> ogeler)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.TeklifYönet))
                return ErişimEngellendiView();
            if (ogeler != null)
            {
                foreach (var pModel in ogeler)
                {
                    var Teklif = _bagliTeklifServisi.BagliTeklifOgesiAlId(pModel.Id);
                    if (Teklif == null)
                        return RedirectToAction("TeklifListe");
                    else
                    {
                        _bagliTeklifServisi.BagliTeklifOgesiSil(Teklif);
                        BaşarılıBildirimi("Teklif başarıyla silindi");
                        _kullanıcıİşlemServisi.İşlemEkle("TeklifSil", "Teklif silindi", Teklif.Id);
                    }
                }
            }
            return new BoşJsonSonucu();
        }
        #endregion
        #region Teklif Harici
        public virtual ActionResult TeklifHariciListe()
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.TeklifHariciYönet))
                return ErişimEngellendiView();

            var model = new TeklifHariciModel();
            model.KullanılabilirHariciSektorler.Add(new Models.EkTanımlamalar.HariciSektorModel { Id = 0, Adı = "Tümü" });
            foreach (var tumHariciSektorler in _hariciSektorServisi.TümHariciSektorleriAl())
            {
                var sektorModel = tumHariciSektorler.ToModel();
                model.KullanılabilirHariciSektorler.Add(sektorModel);
            }
            return View(model);
        }
        protected virtual TeklifHariciModel HariciTeklifListesiHazırla(TeklifHarici teklif)
        {
            var model = new TeklifHariciModel
            {
                Id = teklif.Id,
                Adı = teklif.Adı,
                Acenta = teklif.Acenta,
                Fatura = teklif.Fatura,
                BelgeTuru = teklif.BelgeTuru,
                Onay = teklif.Onay,
                PO = teklif.Po,
                HazırlayanId = teklif.HazırlayanId,
                Tarih = teklif.Tarih,
                TeslimTarihi = teklif.TeslimTarihi,
                UlkeId = teklif.UlkeId,
                SehirId = teklif.SehirId,
                TalepNo = teklif.TalepNo,
                HizmetBedeli = teklif.HizmetBedeli,
                Parabirimi = teklif.Parabirimi,
                FaturaNo = teklif.FaturaNo,
                FaturaResim = teklif.FaturaResim,
                FaturaResimUrl = _resimServisi.ResimUrlAl(teklif.FaturaResim),
                Notlar = teklif.Not.Select(c => c.ToModel()).ToList(),
            };
            return model;
        }
        [HttpPost]
        public virtual ActionResult TeklifHariciListe(DataSourceİsteği command, TeklifHariciModel model)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.TeklifHariciYönet))
                return ErişimEngellendiKendoGridJson();

            var TeklifModels = _teklifHariciServisi.TeklifAra(adı: model.AdAra, acenta: model.AcentaAra, po: model.POAra, belge:model.BelgeAra,
                talepno: model.TalepNoAra, tarihi:null, teslimTarihi:null,enYeniler:false, 
                sayfaIndeksi:command.Page - 1, sayfaBüyüklüğü:command.PageSize);

            var gridModel = new DataSourceSonucu
            {
                Data = TeklifModels.Select(HariciTeklifListesiHazırla),
                Toplam = TeklifModels.TotalCount
            };
            SeçiliSayfaKaydet();
            return Json(gridModel);
        }
        #region TeklifHariciGoruntule
        public virtual ActionResult TeklifHariciGoruntule(int id)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.TeklifHariciYönet))
                ErişimEngellendiView();

            var drn = _teklifHariciServisi.TeklifAlId(id);
            var bagliteklifler = _bagliTeklifHariciServisi.BagliTeklifOgesiAlTeklifId(drn.Id);

            StringBuilder sb = new StringBuilder();
            //DataRow drn = tnm.NumilListele(id, 0, 0).Rows[0];
            double adet = 0, gun = 0, fiyat_alis = 0, fiyat_satis = 0, tutar_alis = 0, hizmet = 0, toplam_tutar_satis = 0,
                    toplam_tutar_alis = 0, toplam_tutar_satisK = 0, toplam_tutar_alisK = 0, tutar_satis = 0, hb = 0, hba = 0,
                    hbK = 0, hbKa = 0, kdv18 = 0, kdv18a = 0, kdv8 = 0, kdv8a = 0, gtoplam = 0, gtoplama = 0, gtoplamKa = 0,
                    gtoplamK = 0, kar = 0, oran = 0,ekaKdv8 = 0, ekKdv8 = 0, ekaKdv18 = 0, ekKdv18 = 0; 
            int kdv = 0, vparent = 0, parabirimi = 0;
            string adi = "", aciklama = "", parabirimi_ = "";
            bool yurtdisi = false;
            if (drn.UlkeId != 1)
                yurtdisi = true;
            string logopath = Server.MapPath("img/symcon-logo.png");
             sb.AppendLine("<table border='1'>");
            sb.AppendLine("<tr><td rowspan='9' style='border:none;'><img src= '../../Content/Images/Thumbs/0000024.png' width='220' /></td><td rowspan='9' style='width:20%; border:none;'></td></tr>");
            sb.AppendLine("<tr><td>Firma:</td><td>" + _hariciSektorServisi.HariciSektorAlId(drn.BelgeTuru).Adı ?? string.Empty + "</td></tr>");
            sb.AppendLine("<tr><td>İşin Adı:</td><td>" + drn.Adı ?? string.Empty + "</td></tr>");
            sb.AppendLine("<tr><td>PO:</td><td>" + drn.Po ?? string.Empty + "</td></tr>");
            sb.AppendLine("<tr><td>Acenta:</td><td>" + drn.Acenta ?? string.Empty + "</td></tr>");
            sb.AppendLine("<tr><td>Bölge:</td><td>" + _konumServisi.SehirAlId(drn.SehirId>0? drn.SehirId:1).Adı?? string.Empty + "/" + _konumServisi.UlkeAlId(drn.UlkeId>0 ? drn.UlkeId : 1).Adı ?? string.Empty + "</td></tr>");
            sb.AppendLine("<tr><td>Kongre Tarih:</td><td>" + drn.Tarih.ToShortDateString() + "</td></tr>");
            sb.AppendLine("<tr><td>Fatura Edilecek Tarihi:</td><td>" + drn.TeslimTarihi.ToShortDateString() + "</td></tr>");
            sb.AppendLine("<tr><td>Fatura No:</td><td>" + drn.FaturaNo + "</td></tr>");
            sb.AppendLine("</table>");
            sb.AppendLine("<br />");

            parabirimi = drn.Parabirimi;
            hizmet = Convert.ToDouble(drn.HizmetBedeli);
            sb.AppendLine("<table border='1'>");
            string header = "";
            sb.AppendLine("<tr style='font-weight: bold;'><td style='padding: 5px;'>ADI</td><td style='padding: 5px;'>ADET</td><td style='padding: 5px;'>ALIŞ BİRİM FİYATI</td><td style='padding: 5px;'>ALIŞ TOPLAM</td><td style='padding: 5px;'>SATIŞ BİRİM FİYATI</td><td style='padding: 5px;'>SATIŞ TOPLAM</td></tr>");
            foreach (var dr in bagliteklifler)
            {
                adi = dr.Adı.ToString();
                adet = Convert.ToDouble(dr.Adet);
                if (parabirimi == 1)
                {
                    parabirimi_ = " TL";
                    fiyat_alis = Convert.ToDouble(dr.AlisBirimFiyat);
                    fiyat_satis = Convert.ToDouble(dr.SatisBirimFiyat);
                    tutar_alis = Convert.ToDouble(dr.AlisFiyat);
                    tutar_satis = Convert.ToDouble(dr.SatisFiyat);
                }
                if (parabirimi == 2)
                {
                    parabirimi_ = " $";
                    fiyat_alis = Convert.ToDouble(dr.AlisBirimFiyatDolar);
                    fiyat_satis = Convert.ToDouble(dr.SatisBirimFiyatDolar);
                    tutar_alis = Convert.ToDouble(dr.AlisFiyatDolar);
                    tutar_satis = Convert.ToDouble(dr.SatisFiyatDolar);
                }
                if (parabirimi == 3)
                {
                    parabirimi_ = " €";
                    fiyat_alis = Convert.ToDouble(dr.AlisBirimFiyatEuro);
                    fiyat_satis = Convert.ToDouble(dr.SatisBirimFiyatEuro);
                    tutar_alis = Convert.ToDouble(dr.AlisFiyatEuro);
                    tutar_satis = Convert.ToDouble(dr.SatisFiyatEuro);
                }
                if (dr.Vparent == 101)
                {
                    toplam_tutar_satisK += tutar_satis;
                    toplam_tutar_alisK += tutar_alis;
                }
                else
                {
                    toplam_tutar_satis += tutar_satis;
                    toplam_tutar_alis += tutar_alis;
                }
                
                kdv = Convert.ToInt32(dr.Kdv);
                sb.AppendLine("<tr>");
                sb.AppendLine("<td style='padding: 5px;'>" + adi + "</td><td style='padding: 5px;'>" + adet + "</td><td style='padding: 5px;'>" + fiyat_alis.ToString("0,0.00", tr) + parabirimi_ + "</td><td style='padding: 5px;'>" + Math.Round(tutar_alis, 2).ToString("0,0.00", tr) + parabirimi_ + "</td><td style='padding: 5px;'>" + fiyat_satis.ToString("0,0.00", tr) + parabirimi_ + "</td><td style='padding: 5px;'>" + Math.Round(tutar_satis, 2).ToString("0,0.00", tr) + parabirimi_ + "</td>");
                sb.AppendLine("</tr>");
                if (yurtdisi)
                {
                    if (dr.KdvRadio > 0)
                    {
                        int kdvOranı = dr.KdvRadio == 1 ? 0 : (dr.KdvRadio == 2 ? 8 : 18);
                        //double ekaKdv8 = 0, ekKdv8 = 0, ekaKdv18 = 0, ekKdv18 = 0;
                        if(dr.KdvRadio == 1)
                        {
                            ekaKdv8 += 0;
                            ekKdv8 += 0;
                            ekaKdv18 += 0;
                            ekKdv18 += 0;
                        }
                        if (dr.KdvRadio == 2)
                        {
                            ekaKdv8 += tutar_alis * kdvOranı / 100;
                            ekKdv8 += tutar_satis * kdvOranı / 100;
                            ekaKdv18 += 0;
                            ekKdv18 += 0;
                        }
                        if (dr.KdvRadio == 3)
                        {
                            ekaKdv18 += tutar_alis * kdvOranı / 100;
                            ekKdv18 += tutar_satis * kdvOranı / 100;
                            ekaKdv8 += 0;
                            ekKdv8 += 0;
                        }
                    }
                }
            }
            sb.AppendLine("<td colspan='7' style='padding:6px;'><br></td>");
            //satis hesaplari
            toplam_tutar_satis = Math.Round(toplam_tutar_satis, 2);
            hb = (toplam_tutar_satis * hizmet) / 100;
            kdv18 = (hb * 18) / 100 + (toplam_tutar_satis * 18) / 100+ ekKdv18;
            hb = Math.Round(hb, 2);
            kdv18 = Math.Round(kdv18, 2);
            //alis hesaplari
            toplam_tutar_alis = Math.Round(toplam_tutar_alis, 2);
            hba = (toplam_tutar_alis * hizmet) / 100;
            kdv18a = (hba * 18) / 100 + (toplam_tutar_alis * 18) / 100+ ekaKdv18;
            hba = Math.Round(hba, 2);
            kdv18a = Math.Round(kdv18a, 2);

            //satis hesaplari %8
            toplam_tutar_satisK = Math.Round(toplam_tutar_satisK, 2);
            hbK = (toplam_tutar_satisK * hizmet) / 100;
            kdv8 = (hbK * 18) / 100 + (toplam_tutar_satisK * 8) / 100+ ekKdv8;
            hbK = Math.Round(hbK, 2);
            kdv8 = Math.Round(kdv8, 2);
            //alis hesaplari %8
            toplam_tutar_alisK = Math.Round(toplam_tutar_alisK, 2);
            hbKa = (toplam_tutar_alisK * hizmet) / 100;
            kdv8a = (hbKa * 18) / 100 + (toplam_tutar_alisK * 8) / 100+ ekaKdv8;
            hbKa = Math.Round(hbKa, 2);
            kdv8a = Math.Round(kdv8a, 2);

            double ttoplam, thb, taratoplam, t18toplam, t8toplam, tgeneltoplam, ttoplama, thba, taratoplama, t18toplama, t8toplama, tgeneltoplama;
            ttoplama = (toplam_tutar_alis + toplam_tutar_alisK);
            ttoplam = (toplam_tutar_satis + toplam_tutar_satisK);
            thba = (hba + hbKa);
            thb = (hb + hbK);
            taratoplama = thba + ttoplama;
            taratoplam = thb + ttoplam;
            t18toplama = yurtdisi ? (thba + toplam_tutar_alis) + thba * 18 / 100 : (thba + toplam_tutar_alis) + (thba + toplam_tutar_alis) * 18 / 100;
            t18toplam = yurtdisi ? (thb + toplam_tutar_alis) + thb * 18 / 100 : (thb + toplam_tutar_satis) + (thb + toplam_tutar_satis) * 18 / 100;
            t8toplama = yurtdisi ? (toplam_tutar_alisK) : (toplam_tutar_alisK) + (toplam_tutar_alisK) * 8 / 100;
            t8toplam = yurtdisi ? (toplam_tutar_satisK) : (toplam_tutar_satisK) + (toplam_tutar_satisK) * 8 / 100;
            tgeneltoplama = (t18toplama + t8toplama);
            tgeneltoplam = (t18toplam + t8toplam);


            sb.AppendLine("<tr style='background-color: #94f500;font-weight: bold;font-weight: bold;'><td style='padding: 5px;'>TOPLAM</td><td></td><td></td><td>" + ttoplama.ToString("0,0.00", tr) + parabirimi_ + "</td><td></td><td style='padding: 5px;'>" + ttoplam.ToString("0,0.00", tr) + parabirimi_ + "</td></tr>");
            sb.AppendLine("<tr style='background-color: #94f500;font-weight: bold;'><td style='padding: 5px;'>%" + hizmet + " HİZMET BEDELİ</td><td></td><td></td><td>" + thba.ToString("0,0.00", tr) + parabirimi_ + "</td><td></td><td style='padding: 5px;'>" + thb.ToString("0,0.00", tr) + parabirimi_ + "</td></tr>");
            sb.AppendLine("<tr style='background-color: #94f500;font-weight: bold;'><td style='padding: 5px;'>ARA TOPLAM</td><td></td><td></td><td>" + taratoplama.ToString("0,0.00", tr) + parabirimi_ + "</td><td></td><td style='padding: 5px;'>" + taratoplam.ToString("0,0.00", tr) + parabirimi_ + "</td></tr>");
            sb.AppendLine("<tr style='background-color: #94f500;font-weight: bold;'><td style='padding: 5px;'>%18 KDV'Lİ TOPLAM</td><td></td><td></td><td>" + (yurtdisi ? thba + thba * 18 / 100 +ekaKdv18 : t18toplama + ekaKdv18).ToString("0,0.00", tr) + parabirimi_ + "</td><td></td><td style='padding: 5px;'>" + (yurtdisi ? thb + thb * 18 / 100 +ekKdv18: t18toplam + ekKdv18).ToString("0,0.00", tr) + parabirimi_ + "</td></tr>");
            sb.AppendLine("<tr style='background-color: #94f500;font-weight: bold;'><td style='padding: 5px;'>%8 KDV'Lİ TOPLAM</td><td></td><td></td><td>" + (yurtdisi ? 0 + ekaKdv8 : t8toplama + ekaKdv8).ToString("0,0.00", tr) + parabirimi_ + "</td><td></td><td style='padding: 5px;'>" + (yurtdisi ? 0 + ekKdv8 : t8toplam + ekKdv8).ToString("0,0.00", tr) + parabirimi_ + "</td></tr>");
            sb.AppendLine("<tr style='background-color: #94f500;font-weight: bold;'><td style='padding: 5px;'>GENEL TOPLAM</td><td></td><td></td><td>" + (yurtdisi ? ttoplama + (thba + thba * 18 / 100) : tgeneltoplama).ToString("0,0.00", tr) + parabirimi_ + "</td><td></td><td style='padding: 5px;'>" + (yurtdisi ? ttoplam + (thb + thb * 18 / 100) : tgeneltoplam).ToString("0,0.00", tr) + parabirimi_ + "</td></tr>");
            sb.AppendLine("</table>");
            sb.AppendLine("<br />");

            sb.AppendLine("<table border='1'>");
            sb.AppendLine("<tr style='font-weight: bold;'>");
            sb.AppendLine("<td style='padding: 5px;'>TALEP NO</td>");
            int cnt = 0;
            string s = drn.TalepNo ?? string.Empty;
            if (s != "" || s != String.Empty)
            {
                List<int> ls = s.Split(',').Select(int.Parse).ToList();
                cnt = ls.Count;
                for (int i = 0; i < cnt; i++)
                {
                    if (i != 0 && i % 16 == 0)
                    {
                        sb.AppendLine("</tr>");
                        sb.AppendLine("<tr style='font-weight: bold;'>");
                        sb.AppendLine("<td style='padding: 5px;'></td>");
                    }
                    sb.AppendLine("<td style='padding: 5px;'>" + ls[i].ToString() + "</td>");
                }
            }
            sb.AppendLine("</tr>");
            sb.AppendLine("</table>");
            sb.AppendLine("<br />");

            var model = new HariciTeklifGoruntuModel { Text = sb.ToString() };
            return View(model);
        }
        #endregion
        #region TeklifHariciGoruntuleGrup
        public virtual ActionResult TeklifHariciGoruntuleGrup(string po)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.TeklifHariciYönet))
                ErişimEngellendiView();

            var drnler = _teklifHariciServisi.TeklifAlPO(po);
            StringBuilder sb = new StringBuilder();

            double adet = 0, gun = 0, fiyat_alis = 0, fiyat_satis = 0, tutar_alis = 0, hizmet = 0, toplam_tutar_satis = 0,
                       toplam_tutar_alis = 0, toplam_tutar_satisK = 0, toplam_tutar_alisK = 0, tutar_satis = 0, hb = 0, hba = 0,
                       hbK = 0, hbKa = 0, kdv18 = 0, kdv18a = 0, kdv8 = 0, kdv8a = 0, gtoplam = 0, gtoplama = 0, gtoplamKa = 0,
                       gtoplamK = 0, kar = 0, oran = 0, toplam_tutar_satis_grup = 0,
                       toplam_tutar_alis_grup = 0, toplam_tutar_satis_grupK = 0, toplam_tutar_alis_grupK = 0, ttoplam = 0,
                       thb = 0, taratoplam = 0, t18toplam = 0, t8toplam = 0, tgeneltoplam = 0, ttoplama = 0, thba = 0, 
                       taratoplama = 0, t18toplama = 0, t8toplama = 0, tgeneltoplama = 0, ttoplamgrupa = 0, ttoplamgrup = 0;
            int kdv = 0, vparent = 0, parabirimi = 0;
            string adi = "", aciklama = "", parabirimi_ = "";
            bool yurtdisi = false;
            
            string logopath = Server.MapPath("img/symcon-logo.png");
            sb.AppendLine("<table border='1' style='font-size: 16px;font-weight: bold;'>");
            sb.AppendLine("<tr><td style='border:none;'><img src= '../../Content/Images/Thumbs/0000024.png' width='220' /></td><td style='padding: 5px;'>PO:" + po+"</td></tr>");
            sb.AppendLine("</table>");
            sb.AppendLine("<table border='1'>");
            string header = "";
            sb.AppendLine("<tr style='font-weight: bold;'><td style='padding: 5px;'>ADI</td><td style='padding: 5px;'>ADET</td><td style='padding: 5px;'>ALIŞ BİRİM FİYATI</td><td style='padding: 5px;'>ALIŞ TOPLAM</td><td style='padding: 5px;'>SATIŞ BİRİM FİYATI</td><td style='padding: 5px;'>SATIŞ TOPLAM</td></tr>");

            foreach (var drn in drnler)
            {
                var bagliteklifler = _bagliTeklifHariciServisi.BagliTeklifOgesiAlTeklifId(drn.Id);
                if (drn.UlkeId != 1)
                    yurtdisi = true;
                parabirimi = drn.Parabirimi;
                hizmet = Convert.ToDouble(drn.HizmetBedeli);
                
                sb.AppendLine("<tr style='font-weight: bold;'>");
                sb.AppendLine("<td style='padding: 5px;' colspan=2>TALEP NO: ");
                int cnt = 0;
                string s = drn.TalepNo == null ? "" : drn.TalepNo.ToString();
                if (s != "" || s != String.Empty)
                {
                    List<int> ls = s.Split(',').Select(int.Parse).ToList();
                    cnt = ls.Count;
                    for (int i = 0; i < cnt; i++)
                    {
                        sb.Append( ls[i].ToString());
                        if (cnt-1!=i)
                        {
                            sb.Append(",");
                        }
                        
                    }
                }
                //sb.AppendLine("<tr style='background-color: #94f500;font-weight: bold;'><td style='padding: 5px;'>GENEL TOPLAM</td><td></td><td></td><td>" + (yurtdisi ? ttoplama + (thba + thba * 18 / 100) : tgeneltoplama).ToString("0,0.00", tr) + parabirimi_ + "</td><td></td><td style='padding: 5px;'>" + (yurtdisi ? ttoplam + (thb + thb * 18 / 100) : tgeneltoplam).ToString("0,0.00", tr) + parabirimi_ + "</td></tr>");
                sb.AppendLine("<td style='padding: 5px;'>" + drn.Adı+ "</td><td colspan=3 style='padding: 5px;'>" + drn.Tarih.ToShortDateString() + "</td></tr>");

                foreach (var dr in bagliteklifler)
                {
                    adi = dr.Adı.ToString();
                    adet = Convert.ToDouble(dr.Adet);
                    fiyat_alis = Convert.ToDouble(dr.AlisBirimFiyat);
                    fiyat_satis = Convert.ToDouble(dr.SatisBirimFiyat);
                    tutar_alis = Convert.ToDouble(dr.AlisFiyat);
                    tutar_satis = Convert.ToDouble(dr.SatisFiyat);
                    if (dr.Tparent == "Konaklama")
                    {
                        toplam_tutar_satisK += tutar_satis;
                        toplam_tutar_alisK += tutar_alis;
                        toplam_tutar_satis_grupK += tutar_satis;
                        toplam_tutar_alis_grupK += tutar_alis;
                    }
                    else
                    {
                        toplam_tutar_satis += tutar_satis;
                        toplam_tutar_alis += tutar_alis;
                        toplam_tutar_satis_grup += tutar_satis;
                        toplam_tutar_alis_grup += tutar_alis;
                    }
                    if (parabirimi == 1)
                    {
                        parabirimi_ = " TL";
                    }
                    if (parabirimi == 2)
                    {
                        parabirimi_ = " $";
                    }
                    if (parabirimi == 3)
                    {
                        parabirimi_ = " €";
                    }
                    kdv = Convert.ToInt32(dr.Kdv);
                    sb.AppendLine("<tr>");

                    sb.AppendLine("<td style='padding: 5px;'>" + adi + "</td><td style='padding: 5px;'>" + adet + "</td><td style='padding: 5px;'>" + fiyat_alis.ToString("0,0.00", tr) + parabirimi_ + "</td><td style='padding: 5px;'>" + Math.Round(tutar_alis, 2).ToString("0,0.00", tr) + parabirimi_ + "</td><td style='padding: 5px;'>" + fiyat_satis.ToString("0,0.00", tr) + parabirimi_ + "</td><td style='padding: 5px;'>" + Math.Round(tutar_satis, 2).ToString("0,0.00", tr) + parabirimi_ + "</td>");
                    sb.AppendLine("</tr>");
                }
                
                //satis hesaplari
                toplam_tutar_satis = Math.Round(toplam_tutar_satis, 2);
                hb = (toplam_tutar_satis * hizmet) / 100;
                kdv18 = (hb * 18) / 100 + (toplam_tutar_satis * 18) / 100;
                hb = Math.Round(hb, 2);
                kdv18 = Math.Round(kdv18, 2);
                //alis hesaplari
                toplam_tutar_alis = Math.Round(toplam_tutar_alis, 2);
                hba = (toplam_tutar_alis * hizmet) / 100;
                kdv18a = (hba * 18) / 100 + (toplam_tutar_alis * 18) / 100;
                hba = Math.Round(hba, 2);
                kdv18a = Math.Round(kdv18a, 2);

                //satis hesaplari %8
                toplam_tutar_satisK = Math.Round(toplam_tutar_satisK, 2);
                hbK = (toplam_tutar_satisK * hizmet) / 100;
                kdv8 = (hbK * 18) / 100 + (toplam_tutar_satisK * 8) / 100;
                hbK = Math.Round(hbK, 2);
                kdv8 = Math.Round(kdv8, 2);
                //alis hesaplari %8
                toplam_tutar_alisK = Math.Round(toplam_tutar_alisK, 2);
                hbKa = (toplam_tutar_alisK * hizmet) / 100;
                kdv8a = (hbKa * 18) / 100 + (toplam_tutar_alisK * 8) / 100;
                hbKa = Math.Round(hbKa, 2);
                kdv8a = Math.Round(kdv8a, 2);

                
                ttoplama = (toplam_tutar_alis + toplam_tutar_alisK);
                ttoplam = (toplam_tutar_satis + toplam_tutar_satisK);
                ttoplamgrupa = (toplam_tutar_alis_grup + toplam_tutar_alis_grupK);
                ttoplamgrup = (toplam_tutar_satis_grup + toplam_tutar_satis_grupK);
                thba = (hba + hbKa);
                thb = (hb + hbK);
                taratoplama = thba + ttoplama;
                taratoplam = thb + ttoplam;
                t18toplama = yurtdisi ? (thba + toplam_tutar_alis) + thba * 18 / 100 : (thba + toplam_tutar_alis) + (thba + toplam_tutar_alis) * 18 / 100;
                t18toplam = yurtdisi ? (thb + toplam_tutar_alis) + thb * 18 / 100 : (thb + toplam_tutar_satis) + (thb + toplam_tutar_satis) * 18 / 100;
                t8toplama = yurtdisi ? (toplam_tutar_alisK) : (toplam_tutar_alisK) + (toplam_tutar_alisK) * 8 / 100;
                t8toplam = yurtdisi ? (toplam_tutar_satisK) : (toplam_tutar_satisK) + (toplam_tutar_satisK) * 8 / 100;
                tgeneltoplama = (t18toplama + t8toplama);
                tgeneltoplam = (t18toplam + t8toplam);
                sb.AppendLine("<tr style='background-color: #94f500;font-weight: bold;'><td style='padding: 5px;'>ARA TOPLAM</td><td></td><td></td><td style='padding: 5px;'>" + Math.Round(ttoplamgrupa, 2).ToString("0,0.00", tr)  + parabirimi_ + "</td><td></td><td style='padding: 5px;'>" + Math.Round(ttoplamgrup, 2).ToString("0,0.00", tr) + parabirimi_ + "</td></tr>");
                sb.AppendLine("<tr><td colspan='6'><br/></td></tr>");
                ttoplamgrup = 0; ttoplamgrupa = 0; toplam_tutar_alis_grup = 0; toplam_tutar_alis_grupK = 0; toplam_tutar_satis_grup = 0; toplam_tutar_satis_grupK = 0;
            }
            
            
            sb.AppendLine("<table  border='1'>");
            sb.AppendLine("<tr style='background-color: #94f500;font-weight: bold;font-weight: bold;'><td style='padding: 5px;'>TOPLAM</td><td style='text-align: right;'>" + ttoplama.ToString("0,0.00", tr) + parabirimi_ + "</td><td style='padding: 5px; text-align: right;'>" + ttoplam.ToString("0,0.00", tr) + parabirimi_ + "</td></tr>");
            sb.AppendLine("<tr style='background-color: #94f500;font-weight: bold;'><td style='padding: 5px;'>%" + hizmet + " HİZMET BEDELİ</td><td style='text-align: right;'>" + thba.ToString("0,0.00", tr) + parabirimi_ + "</td><td style='padding: 5px; text-align: right;'>" + thb.ToString("0,0.00", tr) + parabirimi_ + "</td></tr>");
            sb.AppendLine("<tr style='background-color: #94f500;font-weight: bold;'><td style='padding: 5px;'>ARA TOPLAM</td><td style='text-align: right;'>" + taratoplama.ToString("0,0.00", tr) + parabirimi_ + "</td><td style='padding: 5px; text-align: right;'>" + taratoplam.ToString("0,0.00", tr) + parabirimi_ + "</td></tr>");
            sb.AppendLine("<tr style='background-color: #94f500;font-weight: bold;'><td style='padding: 5px;'>%18 KDV'Lİ TOPLAM</td><td style='text-align: right;'>" + (yurtdisi ? thba + thba * 18 / 100 : t18toplama).ToString("0,0.00", tr) + parabirimi_ + "</td><td style='padding: 5px; text-align: right;'>" + (yurtdisi ? thb + thb * 18 / 100 : t18toplam).ToString("0,0.00", tr) + parabirimi_ + "</td></tr>");
            sb.AppendLine("<tr style='background-color: #94f500;font-weight: bold;'><td style='padding: 5px;'>%8 KDV'Lİ TOPLAM</td><td style='text-align: right;'>" + (yurtdisi ? 0 : t8toplama).ToString("0,0.00", tr) + parabirimi_ + "</td><td style='padding: 5px; text-align: right;'>" + (yurtdisi ? 0 : t8toplam).ToString("0,0.00", tr) + parabirimi_ + "</td></tr>");
            sb.AppendLine("<tr style='background-color: #94f500;font-weight: bold;'><td style='padding: 5px;'>GENEL TOPLAM</td><td style='text-align: right;'>" + (yurtdisi ? ttoplama + (thba + thba * 18 / 100) : tgeneltoplama).ToString("0,0.00", tr) + parabirimi_ + "</td><td style='padding: 5px; text-align: right;'>" + (yurtdisi ? ttoplam + (thb + thb * 18 / 100) : tgeneltoplam).ToString("0,0.00", tr) + parabirimi_ + "</td></tr>");
            sb.AppendLine("</table>");
            
            var model = new HariciTeklifGoruntuModel { Text = sb.ToString() };
            return View(model);
        }
        #endregion
        public virtual ActionResult TeklifHariciEkle()
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.TeklifHariciYönet))
                ErişimEngellendiView();

            var model = new TeklifHariciModel();
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
            foreach (var tumHariciSektorler in _hariciSektorServisi.TümHariciSektorleriAl())
            {
                var sektorModel = tumHariciSektorler.ToModel();
                model.KullanılabilirHariciSektorler.Add(sektorModel);
            }
            return View(model);
        }

        [HttpPost, FormAdıParametresi("kaydet-devam", "düzenlemeyeDevam")]
        public virtual ActionResult TeklifHariciEkle(TeklifHariciModel model, bool düzenlemeyeDevam, string Durumu = "Hazırlık")
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.TeklifYönet))
                ErişimEngellendiView();

            if (ModelState.IsValid)
            {
                var Teklif = model.ToEntity();
                //var not = Teklif.Notlar.to
                _teklifHariciServisi.TeklifEkle(Teklif);

                var teklif = _teklifHariciServisi.TeklifAlId(Teklif.Id);
                BaşarılıBildirimi("Teklifbaşarıyla Eklendi");
                _kullanıcıİşlemServisi.İşlemEkle("YeniTeklifEklendi", "Yeni Teklif Eklendi", Teklif.Id);
                return RedirectToAction("TeklifHariciDüzenle", new { id = Teklif.Id });
            }
            return View(model);
        }
        public virtual ActionResult TeklifHariciDüzenle(int id)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.TeklifHariciYönet))
                ErişimEngellendiView();
            var Teklif = _teklifHariciServisi.TeklifAlId(id);
            if (Teklif == null)
            {
                return RedirectToAction("TeklifHariciListe");
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
            foreach (var tumSehirler in _konumServisi.SehirlerAlUlkeId(1))
            {
                var sehirModel = tumSehirler.ToModel();
                model.Sehirler.Add(sehirModel);
            }
            foreach (var tumHariciSektorler in _hariciSektorServisi.TümHariciSektorleriAl())
            {
                var sektorModel = tumHariciSektorler.ToModel();
                model.KullanılabilirHariciSektorler.Add(sektorModel);
            }
            return View(model);
        }
        [HttpPost, FormAdıParametresi("kaydet-devam", "düzenlemeyeDevam")]
        public virtual ActionResult TeklifHariciDüzenle(TeklifHariciModel model, bool düzenlemeyeDevam)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.TeklifHariciYönet))
                ErişimEngellendiView();
            var Teklif = _teklifHariciServisi.TeklifAlId(model.Id);
            if (Teklif == null)
            {
                return RedirectToAction("TeklifHariciListe");
            }
            if (ModelState.IsValid)
            {
                Teklif = model.ToEntity(Teklif);
                _teklifHariciServisi.TeklifGüncelle(Teklif);
                BaşarılıBildirimi("Teklif başarıyla güncellenmiştir.");
                _kullanıcıİşlemServisi.İşlemEkle("TeklifGüncelle", "Teklif güncellendi", Teklif.Id);
                if (düzenlemeyeDevam)
                {
                    return RedirectToAction("TeklifHariciDüzenle", new { id = Teklif.Id });
                }
                return RedirectToAction("TeklifHariciListe");
            }
            return View(model);
        }
        
        [HttpPost]
        public virtual ActionResult HariciDuzenle(IEnumerable<TeklifHariciModel> models)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.TeklifHariciYönet))
                ErişimEngellendiView();

            if (models != null)
            {
                foreach (var pModel in models)
                {
                    //update
                    var oge = _teklifHariciServisi.TeklifAlId(pModel.Id);
                    if (oge != null)
                    {
                        pModel.Tarih = oge.Tarih;
                        pModel.TeslimTarihi = oge.TeslimTarihi;
                        oge = pModel.ToEntity(oge);
                        _teklifHariciServisi.TeklifGüncelle(oge);
                    }
                }
            }
            return new BoşJsonSonucu();
        }

        [HttpPost]
        public virtual ActionResult TeklifHariciSil(TeklifHariciModel model)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.TeklifHariciYönet))
                return ErişimEngellendiView();

            var Teklif = _teklifHariciServisi.TeklifAlId(model.Id);
            if (Teklif == null)
                return RedirectToAction("TeklifHariciListe");
            _teklifHariciServisi.TeklifSil(Teklif);
            BaşarılıBildirimi("Teklif başarıyla silindi");
            _kullanıcıİşlemServisi.İşlemEkle("TeklifHariciSil", "Teklif silindi", Teklif.Id);
            return RedirectToAction("TeklifHariciListe");
        }

        public virtual ActionResult TeklifHariciKopyala(int teklifId, string durumu)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.TeklifHariciYönet))
                ErişimEngellendiView();

            var Teklif = _teklifHariciServisi.TeklifAlId(teklifId);
            //var Teklif = model.ToEntity();
            _teklifHariciServisi.TeklifEkle(Teklif);
            foreach (var oge in _bagliTeklifHariciServisi.BagliTeklifOgesiAlTeklifId(teklifId))
            {
                oge.TeklifId = Teklif.Id;
                _bagliTeklifHariciServisi.BagliTeklifOgesiEkle(oge);
            }
            var teklif = _teklifServisi.TeklifAlId(Teklif.Id);
            BaşarılıBildirimi("Teklifbaşarıyla Eklendi");
            _kullanıcıİşlemServisi.İşlemEkle("YeniTeklifEklendi", "Yeni Teklif Eklendi", Teklif.Id);
            return RedirectToAction("TeklifHariciDüzenle", new { id = Teklif.Id });

        }
        #endregion
        #region Bağlı Teklif Öğesi Harici

        [HttpPost]
        public virtual ActionResult BağlıTeklifHariciListe(int teklifId, string grid, DataSourceİsteği command, TeklifHariciModel model)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.TeklifHariciYönet))
                return ErişimEngellendiKendoGridJson();
            if (teklifId > 0)
            {
                var TeklifModels = _bagliTeklifHariciServisi.BagliTeklifOgesiAlTeklifId(teklifId, grid);
                var gridModel = new DataSourceSonucu
                {
                    Data = TeklifModels.Select(x =>
                    {
                        var TeklifModel = x.ToModel();
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
        public virtual ActionResult BağlıTeklifHariciEkle(DataSourceİsteği command, BagliTeklifOgesiHariciModel model, int treeItemId, int teklifId)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.TeklifHariciYönet))
                ErişimEngellendiView();
            if (ModelState.IsValid)
            {
                var bagliTeklif = model.ToEntity();
                /*var teklif = _teklifHariciServisi.TeklifAlId(bagliTeklif.TeklifId);
                teklif.TeslimTarihi = DateTime.Now;
                teklif.Tarih = DateTime.Now;
                _teklifHariciServisi.TeklifEkle(teklif);
                //Teklif.OlusturulmaTarihi = DateTime.Now;*/
                bagliTeklif.TeklifId = teklifId;
                var treeitem = _teklifKalemiServisi.TeklifKalemiAlId(treeItemId);
                var nodetreeitem = _teklifKalemiServisi.TeklifKalemiAlId(treeitem.NodeId.Value);
                bagliTeklif.Adı = treeitem.Adı;
                bagliTeklif.Tparent = nodetreeitem.Adı;
                bagliTeklif.Vparent = nodetreeitem.Id;
                bagliTeklif.Kurum = "";
                _bagliTeklifHariciServisi.BagliTeklifOgesiEkle(bagliTeklif);
                BaşarılıBildirimi("TeklifOgesibaşarıyla Eklendi");
                _kullanıcıİşlemServisi.İşlemEkle("YeniTeklifEklendi", "Yeni Teklif Eklendi", bagliTeklif.Id);
                return RedirectToAction("TeklifHariciDüzenle", new { id = teklifId });
            }
            return View(model);
        }


        [HttpPost]
        public virtual ActionResult BagliTeklifHariciDuzenle(IEnumerable<BagliTeklifOgesiHariciModel> ogeler)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.TeklifHariciYönet))
                ErişimEngellendiView();
            if (ogeler != null)
            {
                foreach (var pModel in ogeler)
                {
                    //update
                    var oge = _bagliTeklifHariciServisi.BagliTeklifOgesiAlId(pModel.Id);
                    if (oge != null)
                    {
                        oge = pModel.ToEntity(oge);
                        int  satisadet = 0, gun = 1, p = 1;
                        if (oge.Adet > 0)
                            satisadet = oge.Adet;
                        if (oge.Gun != 0)
                            gun = oge.Gun;
                        p = oge.Parabirimi == 1 ? 1 : oge.Parabirimi == 2 ? 2 : 3;

                        oge.AlisFiyat = p == 1 ? oge.AlisBirimFiyat * satisadet * gun : p == 2 ? oge.AlisBirimFiyat * satisadet * gun  : oge.AlisBirimFiyat * satisadet * gun ;
                        oge.AlisFiyatDolar = p == 2 ? oge.AlisBirimFiyat * satisadet * gun : p == 1 ? oge.AlisBirimFiyat * satisadet * gun  : oge.AlisBirimFiyat * satisadet * gun ;
                        oge.AlisFiyatEuro = p == 3 ? oge.AlisBirimFiyat * satisadet * gun : p == 1 ? oge.AlisBirimFiyat * satisadet * gun  : oge.AlisBirimFiyat * satisadet * gun;
                        oge.SatisFiyat = p == 1 ? oge.SatisBirimFiyat * satisadet * gun : p == 2 ? oge.SatisBirimFiyat * satisadet * gun  : oge.SatisBirimFiyat * satisadet * gun ;
                        oge.SatisFiyatDolar = p == 2 ? oge.SatisBirimFiyat * satisadet * gun : p == 1 ? oge.SatisBirimFiyat * satisadet * gun  : oge.SatisBirimFiyat * satisadet * gun;
                        oge.SatisFiyatEuro = p == 3 ? oge.SatisBirimFiyat * satisadet * gun : p == 1 ? oge.SatisBirimFiyat * satisadet * gun  : oge.SatisBirimFiyat * satisadet * gun;

                        oge.AlisBirimFiyatDolar = p == 1 ? oge.AlisBirimFiyat  : p == 2 ? oge.AlisBirimFiyat : oge.AlisBirimFiyat;
                        oge.AlisBirimFiyatEuro = p == 1 ? oge.AlisBirimFiyat  : p == 2 ? oge.AlisBirimFiyat : oge.AlisBirimFiyat;
                        oge.SatisBirimFiyatDolar = p == 1 ? oge.SatisBirimFiyat  : p == 2 ? oge.SatisBirimFiyat : oge.SatisBirimFiyat;
                        oge.SatisBirimFiyatEuro = p == 1 ? oge.SatisBirimFiyat  : p == 2 ? oge.SatisBirimFiyat : oge.SatisBirimFiyat;
                        oge.AlisBirimFiyat = p == 1 ? oge.AlisBirimFiyat : p == 2 ? oge.AlisBirimFiyat : oge.AlisBirimFiyat;
                        oge.SatisBirimFiyat = p == 1 ? oge.SatisBirimFiyat : p == 2 ? oge.SatisBirimFiyat : oge.SatisBirimFiyat;

                        decimal kar = oge.SatisFiyat - oge.AlisFiyat;
                        kar = Math.Round(kar, 2);
                        decimal gelir = oge.SatisFiyat == 0 ? 0 : (kar / oge.SatisFiyat * 100);
                        gelir = Math.Round(gelir, 2);

                        oge.Kar = kar;
                        oge.Gelir = gelir.ToString();
                        _bagliTeklifHariciServisi.BagliTeklifOgesiGüncelle(oge);
                    }
                }
            }
            return new BoşJsonSonucu();
        }

        [HttpPost]
        public virtual ActionResult BagliTeklifHariciSil(IEnumerable<BagliTeklifOgesiHariciModel> ogeler)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.TeklifHariciYönet))
                return ErişimEngellendiView();
            if (ogeler != null)
            {
                foreach (var pModel in ogeler)
                {
                    var Teklif = _bagliTeklifHariciServisi.BagliTeklifOgesiAlId(pModel.Id);
                    if (Teklif == null)
                        return RedirectToAction("TeklifListe");
                    else
                    {
                        _bagliTeklifHariciServisi.BagliTeklifOgesiSil(Teklif);
                        BaşarılıBildirimi("Teklif başarıyla silindi");
                        _kullanıcıİşlemServisi.İşlemEkle("TeklifHariciSil", "Teklif silindi", Teklif.Id);
                    }
                }
            }
            return new BoşJsonSonucu();
        }
        #endregion
        #region Rapor

        [HttpPost]
        public virtual ActionResult TeklifHariciRapor(TeklifHariciRaporModel model, string selectedIds)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.TeklifHariciYönet))
                ErişimEngellendiView();

   
                var teklifler = new List<TeklifHarici>();
            var ids = selectedIds
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => Convert.ToInt32(x))
                .ToArray();
            teklifler.AddRange(_teklifHariciServisi.TeklifAlIds(ids.ToList()));
            
            //string json = "test";
            //TeklifHariciRaporModel model = JsonConvert.DeserializeObject<TeklifHariciRaporModel>(json);
           // var drnler = _teklifHariciServisi.TeklifAlIds(model.Idler.ToList());
            var drnler = _teklifHariciServisi.TeklifAlIds(ids.ToList());
            StringBuilder sb = new StringBuilder();
            double adet = 0, gun = 0, fiyat_alis = 0, fiyat_satis = 0, tutar_alis = 0, hizmet = 0, toplam_tutar_satis = 0,
                       toplam_tutar_alis = 0, toplam_tutar_satisK = 0, toplam_tutar_alisK = 0, tutar_satis = 0, hb = 0, hba = 0,
                       hbK = 0, hbKa = 0, kdv18 = 0, kdv18a = 0, kdv8 = 0, kdv8a = 0, gtoplam = 0, gtoplama = 0, gtoplamKa = 0,
                       gtoplamK = 0, kar = 0, oran = 0, toplam_tutar_satis_grup = 0,
                       toplam_tutar_alis_grup = 0, toplam_tutar_satis_grupK = 0, toplam_tutar_alis_grupK = 0, ttoplam = 0,
                       thb = 0, taratoplam = 0, t18toplam = 0, t8toplam = 0, tgeneltoplam = 0, ttoplama = 0, thba = 0,
                       taratoplama = 0, t18toplama = 0, t8toplama = 0, tgeneltoplama = 0, ttoplamgrupa = 0, ttoplamgrup = 0,
                       toplam_tutar_satisK_dolar = 0, toplam_tutar_alisK_dolar = 0, toplam_tutar_satisK_euro = 0, toplam_tutar_alisK_euro = 0,
                       toplam_tutar_satis_dolar = 0, toplam_tutar_alis_dolar = 0, toplam_tutar_satis_euro = 0, toplam_tutar_alis_euro = 0,
                       hb_dolar = 0, hba_dolar = 0, hbK_dolar = 0, hbKa_dolar = 0, ttoplama_dolar = 0, ttoplam_dolar = 0,
                    thba_dolar = 0, thb_dolar = 0, taratoplama_dolar = 0, taratoplam_dolar = 0, t18toplama_dolar = 0,
                    t18toplam_dolar = 0, t8toplama_dolar = 0, t8toplam_dolar = 0, tgeneltoplama_dolar = 0, tgeneltoplam_dolar = 0,
                    hb_euro = 0, hba_euro = 0, hbK_euro = 0, hbKa_euro = 0, ttoplama_euro = 0, ttoplam_euro = 0,
                    thba_euro = 0, thb_euro = 0, taratoplama_euro = 0, taratoplam_euro = 0, t18toplama_euro = 0,
                    t18toplam_euro = 0, t8toplama_euro = 0, t8toplam_euro = 0, tgeneltoplama_euro = 0, tgeneltoplam_euro = 0,
                    kar_tl = 0, kar_dolar = 0, kar_euro = 0, toplamkar = 0, toplamkar_dolar = 0, toplamkar_euro = 0, oran_tl = 0, oran_dolar = 0, oran_euro = 0;
            int kdv = 0, vparent = 0, parabirimi = 0;
            string adi = "", aciklama = "", parabirimi_ = "";
            bool yurtdisi = false;

            string logopath = Server.MapPath("img/symcon-logo.png");
            sb.AppendLine("<table border='1' style='font-size: 16px;font-weight: bold;'>");
            sb.AppendLine("<tr><td style='border:none;'><img src= '../../Content/Images/Thumbs/0000024.png' width='220' /></td><td colspan='2' style='font-size:24px; text-align:center;'>KÂRLILIK ANALİZİ</td><td style'width:20%;'></tr>");
            sb.AppendLine("</table>");
            sb.AppendLine("<table border='1'>");
            string header = "";
            sb.AppendLine("<tr style='font-weight: bold;'><td style='padding: 5px;'>ADI</td><td style='padding: 5px;'>ALIŞ TOPLAM</td><td style='padding: 5px;'>SATIŞ TOPLAM</td><td style='padding: 5px;'>KAR</td></tr>");

            foreach (var drn in drnler)
            {
                var bagliteklifler = _bagliTeklifHariciServisi.BagliTeklifOgesiAlTeklifId(drn.Id);
                if (drn.UlkeId != 1)
                    yurtdisi = true;
                parabirimi = drn.Parabirimi;
                hizmet = Convert.ToDouble(drn.HizmetBedeli);

                sb.AppendLine("<tr style='font-weight: bold;'>");
                sb.AppendLine("<td style='padding: 5px;' colspan='4'>" + drn.Adı + "&nbsp&nbsp&nbsp|&nbsp&nbsp&nbsp Acenta: " + drn.Acenta + "&nbsp&nbsp&nbsp|&nbsp&nbsp&nbspTarih: " + drn.Tarih.ToShortDateString() + "&nbsp&nbsp&nbsp|&nbsp&nbsp&nbspPO: " + drn.Po + "</td></tr>");

                foreach (var dr in bagliteklifler)
                {
                    adi = dr.Adı.ToString();
                    fiyat_alis = Convert.ToDouble(dr.AlisBirimFiyat);
                    fiyat_satis = Convert.ToDouble(dr.SatisBirimFiyat);
                    tutar_alis = Convert.ToDouble(dr.AlisFiyat);
                    tutar_satis = Convert.ToDouble(dr.SatisFiyat);
                    if (parabirimi == 1)
                    {
                        parabirimi_ = " TL";
                        if (dr.Tparent == "Konaklama")
                        {
                            toplam_tutar_satisK += tutar_satis;
                            toplam_tutar_alisK += tutar_alis;
                        }
                        else
                        {
                            toplam_tutar_satis += tutar_satis;
                            toplam_tutar_alis += tutar_alis;
                        }
                    }
                    if (parabirimi == 2)
                    {
                        parabirimi_ = " $";
                        if (dr.Tparent == "Konaklama")
                        {
                            toplam_tutar_satisK_dolar += tutar_satis;
                            toplam_tutar_alisK_dolar += tutar_alis;
                        }
                        else
                        {
                            toplam_tutar_satis_dolar += tutar_satis;
                            toplam_tutar_alis_dolar += tutar_alis;
                        }
                    }
                    if (parabirimi == 3)
                    {
                        parabirimi_ = " €";
                        if (dr.Tparent == "Konaklama")
                        {
                            toplam_tutar_satisK_euro += tutar_satis;
                            toplam_tutar_alisK_euro += tutar_alis;
                        }
                        else
                        {
                            toplam_tutar_satis_euro += tutar_satis;
                            toplam_tutar_alis_euro += tutar_alis;
                        }
                    }
                    kdv = Convert.ToInt32(dr.Kdv);
                    sb.AppendLine("<tr>");

                    sb.AppendLine("<td style='padding: 5px;' width='60%'>" + adi + "</td><td style='padding: 5px;'>" + Math.Round(tutar_alis, 2).ToString("0,0.00", tr) + parabirimi_ + "</td><td style='padding: 5px;'>" + Math.Round(tutar_satis, 2).ToString("0,0.00", tr) + parabirimi_ + "</td><td style='padding: 5px;'>" + Math.Round(tutar_satis-tutar_alis, 2).ToString("0,0.00", tr) + parabirimi_ + "</td>");
                    sb.AppendLine("</tr>");
                }
                #region tl hesapları
                //satis hesaplari
                toplam_tutar_satis = Math.Round(toplam_tutar_satis, 2);
                hb = Math.Round((toplam_tutar_satis * hizmet) / 100,2);
                //alis hesaplari
                toplam_tutar_alis = Math.Round(toplam_tutar_alis, 2);
                hba = Math.Round((toplam_tutar_alis * hizmet) / 100,2);

                //satis hesaplari %8
                toplam_tutar_satisK = Math.Round(toplam_tutar_satisK, 2);
                hbK = Math.Round((toplam_tutar_satisK * hizmet) / 100,2);
                //alis hesaplari %8
                toplam_tutar_alisK = Math.Round(toplam_tutar_alisK, 2);
                hbKa = Math.Round((toplam_tutar_alisK * hizmet) / 100,2);

                ttoplama = (toplam_tutar_alis + toplam_tutar_alisK);
                ttoplam = (toplam_tutar_satis + toplam_tutar_satisK);
                thba = (hba + hbKa);
                thb = (hb + hbK);
                taratoplama =  ttoplama;
                taratoplam = thb + ttoplam;
                t18toplama = yurtdisi ? (thba + toplam_tutar_alis) + thba * 18 / 100 : ( toplam_tutar_alis) + (thba + toplam_tutar_alis) * 18 / 100;
                t18toplam = yurtdisi ? (thb + toplam_tutar_alis) + thb * 18 / 100 : (thb + toplam_tutar_satis) + (thb + toplam_tutar_satis) * 18 / 100;
                t8toplama = yurtdisi ? (toplam_tutar_alisK) : (toplam_tutar_alisK) + (toplam_tutar_alisK) * 8 / 100;
                t8toplam = yurtdisi ? (toplam_tutar_satisK) : (toplam_tutar_satisK) + (toplam_tutar_satisK) * 8 / 100;
                tgeneltoplama = (t18toplama + t8toplama);
                tgeneltoplam = (t18toplam + t8toplam);
                kar_tl = ttoplam - ttoplama;
                toplamkar = kar_tl + thb;
                oran_tl = Math.Round((toplamkar / taratoplam) * 100, 2);
                #endregion
                #region usd hesapları
                //satis hesaplari
                toplam_tutar_satis_dolar = Math.Round(toplam_tutar_satis_dolar, 2);
                hb_dolar = Math.Round((toplam_tutar_satis_dolar * hizmet) / 100, 2);
                //alis hesaplari
                toplam_tutar_alis_dolar = Math.Round(toplam_tutar_alis_dolar, 2);
                hba_dolar = Math.Round((toplam_tutar_alis_dolar * hizmet) / 100, 2);

                //satis hesaplari %8
                toplam_tutar_satisK_dolar = Math.Round(toplam_tutar_satisK_dolar, 2);
                hbK_dolar = Math.Round((toplam_tutar_satisK_dolar * hizmet) / 100, 2);
                //alis hesaplari %8
                toplam_tutar_alisK_dolar = Math.Round(toplam_tutar_alisK_dolar, 2);
                hbKa_dolar = Math.Round((toplam_tutar_alisK_dolar * hizmet) / 100, 2);

                ttoplama_dolar = (toplam_tutar_alis_dolar + toplam_tutar_alisK_dolar);
                ttoplam_dolar = (toplam_tutar_satis_dolar + toplam_tutar_satisK_dolar);
                thba_dolar = (hba_dolar + hbKa_dolar);
                thb_dolar = (hb_dolar + hbK_dolar);
                taratoplama_dolar =  ttoplama_dolar;
                taratoplam_dolar = thb_dolar + ttoplam_dolar;
                t18toplama_dolar = yurtdisi ? (thba_dolar + toplam_tutar_alis_dolar) + thba_dolar * 18 / 100 : (thba_dolar + toplam_tutar_alis_dolar) + (thba_dolar + toplam_tutar_alis_dolar) * 18 / 100;
                t18toplam_dolar = yurtdisi ? (thb_dolar + toplam_tutar_alis_dolar) + thb_dolar * 18 / 100 : (thb_dolar + toplam_tutar_satis_dolar) + (thb_dolar + toplam_tutar_satis_dolar) * 18 / 100;
                t8toplama_dolar = yurtdisi ? (toplam_tutar_alisK_dolar) : (toplam_tutar_alisK_dolar) + (toplam_tutar_alisK_dolar) * 8 / 100;
                t8toplam_dolar = yurtdisi ? (toplam_tutar_satisK_dolar) : (toplam_tutar_satisK_dolar) + (toplam_tutar_satisK_dolar) * 8 / 100;
                tgeneltoplama_dolar = (t18toplama_dolar + t8toplama_dolar);
                tgeneltoplam_dolar = (t18toplam_dolar + t8toplam_dolar);
                kar_dolar = ttoplam_dolar - ttoplama_dolar;
                toplamkar_dolar = kar_dolar + thb_dolar;
                oran_dolar = Math.Round((toplamkar_dolar / taratoplam_dolar) * 100, 2);
                #endregion
                #region euro hesapları
                //satis hesaplari
                toplam_tutar_satis_euro = Math.Round(toplam_tutar_satis_euro, 2);
                hb_euro = Math.Round((toplam_tutar_satis_euro * hizmet) / 100, 2);
                //alis hesaplari
                toplam_tutar_alis_euro = Math.Round(toplam_tutar_alis_euro, 2);
                hba_euro = Math.Round((toplam_tutar_alis_euro * hizmet) / 100, 2);

                //satis hesaplari %8
                toplam_tutar_satisK_euro = Math.Round(toplam_tutar_satisK_euro, 2);
                hbK_euro = Math.Round((toplam_tutar_satisK_euro * hizmet) / 100, 2);
                //alis hesaplari %8
                toplam_tutar_alisK_euro = Math.Round(toplam_tutar_alisK_euro, 2);
                hbKa_euro = Math.Round((toplam_tutar_alisK_euro * hizmet) / 100, 2);

                ttoplama_euro = (toplam_tutar_alis_euro + toplam_tutar_alisK_euro);
                ttoplam_euro = (toplam_tutar_satis_euro + toplam_tutar_satisK_euro);
                thba_euro = (hba_euro + hbKa_euro);
                thb_euro = (hb_euro + hbK_euro);
                taratoplama_euro =  ttoplama_euro;
                taratoplam_euro = thb_euro + ttoplam_euro;
                t18toplama_euro = yurtdisi ? (thba_euro + toplam_tutar_alis_euro) + thba_euro * 18 / 100 : (thba_euro + toplam_tutar_alis_euro) + (thba_euro + toplam_tutar_alis_euro) * 18 / 100;
                t18toplam_euro = yurtdisi ? (thb_euro + toplam_tutar_alis_euro) + thb_euro * 18 / 100 : (thb_euro + toplam_tutar_satis_euro) + (thb_euro + toplam_tutar_satis_euro) * 18 / 100;
                t8toplama_euro = yurtdisi ? (toplam_tutar_alisK_euro) : (toplam_tutar_alisK_euro) + (toplam_tutar_alisK_euro) * 8 / 100;
                t8toplam_euro = yurtdisi ? (toplam_tutar_satisK_euro) : (toplam_tutar_satisK_euro) + (toplam_tutar_satisK_euro) * 8 / 100;
                tgeneltoplama_euro = (t18toplama_euro + t8toplama_euro);
                tgeneltoplam_euro = (t18toplam_euro + t8toplam_euro);
                kar_euro = ttoplam_euro - ttoplama_euro;
                toplamkar_euro = kar_euro + thb_euro;
                oran_euro = Math.Round((toplamkar_euro / taratoplam_euro) * 100, 2);
                #endregion
                sb.AppendLine("<tr><td colspan='6'><br/></td></tr>");
                ttoplamgrup = 0; ttoplamgrupa = 0; toplam_tutar_alis_grup = 0; toplam_tutar_alis_grupK = 0; toplam_tutar_satis_grup = 0; toplam_tutar_satis_grupK = 0;
            }
            if (ttoplama > 0)
            {
                sb.AppendLine("<table  border='1'>");
                sb.AppendLine("<tr style='background-color: #C64333;font-weight: bold;color:#fff;'><td style='padding: 5px;' width='60%'>TOPLAM (₺)</td><td style='text-align: right;' width='20%'>" + ttoplama.ToString("0,0.00", tr) + parabirimi_ + "</td><td style='padding: 5px; text-align: right;'>" + ttoplam.ToString("0,0.00", tr) + parabirimi_ + "</td></tr>");
                sb.AppendLine("<tr style='background-color: #C64333;font-weight: bold;color:#fff;'><td style='padding: 5px;' width='60%'>OPERASYON KÂRI (₺)</td><td style='text-align: right;' colspan='2'>" + kar_tl.ToString("0,0.00", tr) + parabirimi_ + "</td></tr>");
                sb.AppendLine("<tr style='background-color: #C64333;font-weight: bold;color:#fff;'><td style='padding: 5px;' width='60%'>%" + hizmet + " HİZMET BEDELİ</td><td style='padding: 5px; text-align: right;' colspan='2'>" + thb.ToString("0,0.00", tr) + parabirimi_ + "</td></tr>");
                sb.AppendLine("<tr style='background-color: #C64333;font-weight: bold;color:#fff;'><td style='padding: 5px;' width='60%'>TOPLAM KÂR (₺)</td><td style='text-align: right;' colspan='2'>" + toplamkar.ToString("0,0.00", tr) + parabirimi_ + "</td></tr>");
                sb.AppendLine("<tr style='background-color: #C64333;font-weight: bold;color:#fff;'><td style='padding: 5px;' width='60%'>ARA TOPLAM</td><td style='text-align: right;' width='20%'>" + taratoplama.ToString("0,0.00", tr) + parabirimi_ + "</td><td style='padding: 5px; text-align: right;'>" + taratoplam.ToString("0,0.00", tr) + parabirimi_ + "</td></tr>");
                sb.AppendLine("<tr style='background-color: #C64333;font-weight: bold;color:#fff;'><td style='padding: 5px;' width='60%'>ORAN</td><td style='text-align: right;' colspan='2'>" + oran_tl.ToString("0,0.00", tr) + " %" + "</td></tr>");
                sb.AppendLine("</table>");
            }
            if (ttoplam_dolar > 0)
            {
                sb.AppendLine("<table  border='1'>");
                sb.AppendLine("<tr style='background-color: #00A65A;font-weight: bold;color:#fff;'><td style='padding: 5px;' width='60%'>TOPLAM ($)</td><td style='text-align: right;' width='20%'>" + ttoplama_dolar.ToString("0,0.00", tr) + " $" + "</td><td style='padding: 5px; text-align: right;'>" + ttoplam_dolar.ToString("0,0.00", tr) + " $" + "</td></tr>");
                sb.AppendLine("<tr style='background-color: #00A65A;font-weight: bold;color:#fff;'><td style='padding: 5px;' width='60%'>OPERASYON KÂRI ($)</td><td style='text-align: right;' colspan='2'>" + kar_dolar.ToString("0,0.00", tr)  + " $"+ " </td></tr>");
                sb.AppendLine("<tr style='background-color: #00A65A;font-weight: bold;color:#fff;'><td style='padding: 5px;' width='60%'>%" + hizmet + " HİZMET BEDELİ</td><td style='padding: 5px; text-align: right;' colspan='2'>" + thb_dolar.ToString("0,0.00", tr) + " $" + "</td></tr>");
                sb.AppendLine("<tr style='background-color: #00A65A;font-weight: bold;color:#fff;'><td style='padding: 5px;' width='60%'>TOPLAM KÂR ($)</td><td style='text-align: right;' colspan='2'>" + toplamkar_dolar.ToString("0,0.00", tr) + parabirimi_ + "</td></tr>");
                sb.AppendLine("<tr style='background-color: #00A65A;font-weight: bold;color:#fff;'><td style='padding: 5px;' width='60%'>ARA TOPLAM</td><td style='text-align: right;' width='20%'>" + taratoplama_dolar.ToString("0,0.00", tr) + " $" + "</td><td style='padding: 5px; text-align: right;'>" + taratoplam_dolar.ToString("0,0.00", tr) + " $" + "</td></tr>");
                sb.AppendLine("<tr style='background-color: #00A65A;font-weight: bold;color:#fff;'><td style='padding: 5px;' width='60%'>ORAN ($)</td><td style='text-align: right;' colspan='2'>" + oran_dolar.ToString("0,0.00", tr) + " %" + "</td></tr>");

                sb.AppendLine("</table>");
            }
            if (ttoplam_euro > 0)
            {
                sb.AppendLine("<table  border='1'>");
                sb.AppendLine("<tr style='background-color: #00C0EF;font-weight: bold;color:#fff;'><td style='padding: 5px;' width='60%'>TOPLAM (€)</td><td style='text-align: right;' width='20%'>" + ttoplama_euro.ToString("0,0.00", tr) + " €" + "</td><td style='padding: 5px; text-align: right;'>" + ttoplam_euro.ToString("0,0.00", tr) + " €" + "</td></tr>");
                sb.AppendLine("<tr style='background-color: #00C0EF;font-weight: bold;color:#fff;'><td style='padding: 5px;' width='60%'>OPERASYON KÂRI (€)</td><td style='text-align: right;' colspan='2'>" + kar_euro.ToString("0,0.00", tr) + " €" + "</td></tr>");
                sb.AppendLine("<tr style='background-color: #00C0EF;font-weight: bold;color:#fff;'><td style='padding: 5px;' width='60%'>%" + hizmet + " HİZMET BEDELİ</td><td style='padding: 5px; text-align: right;' colspan='2'>" + thb_euro.ToString("0,0.00", tr) + " €" + "</td></tr>");
                sb.AppendLine("<tr style='background-color: #00C0EF;font-weight: bold;color:#fff;'><td style='padding: 5px;' width='60%'>TOPLAM KÂR (€)</td><td style='text-align: right;' colspan='2'>" + toplamkar_euro.ToString("0,0.00", tr) + parabirimi_ + "</td></tr>");
                sb.AppendLine("<tr style='background-color: #00C0EF;font-weight: bold;color:#fff;'><td style='padding: 5px;' width='60%'>ARA TOPLAM</td><td style='text-align: right;' width='20%'>" + taratoplama_euro.ToString("0,0.00", tr) + " €" + "</td><td style='padding: 5px; text-align: right;'>" + taratoplam_euro.ToString("0,0.00", tr) + " €" + "</td></tr>");
                sb.AppendLine("<tr style='background-color: #00C0EF;font-weight: bold;color:#fff;'><td style='padding: 5px;' width='60%'>ORAN (€)</td><td style='text-align: right;' colspan='2'>" + oran_euro.ToString("0,0.00", tr) + " %" + " </td></tr>");
                sb.AppendLine("</table>");
            }
            var model2 = new TeklifHariciRaporModel { Text = sb.ToString() }; 
            return View(model2);
        }
        #endregion 
    }
}