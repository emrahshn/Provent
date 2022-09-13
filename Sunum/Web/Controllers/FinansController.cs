using Services.Güvenlik;
using Services.Siteler;
using System.Linq;
using System.Web.Mvc;
using Web.Framework.Controllers;
using Web.Framework.Kendoui;
using Web.Uzantılar;
using Services.Logging;
using Services.Konum;
using Web.Models.Finans;
using Services.Finans;
using Services.EkTanımlamalar;
using Services.Kullanıcılar;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel;
using System;
using System.Globalization;
using Services.Notlar;
using Core;
using Web.Framework.Mvc;
using NotificationsExtensions.Toasts;
using Services.Tanımlamalar;

namespace Web.Controllers
{
    public class FinansController : TemelPublicController
    {
        private readonly IİzinServisi _izinServisi;
        private readonly IKullanıcıİşlemServisi _kullanıcıİşlemServisi;
        private readonly ISiteServisi _siteServisi;
        private readonly IOdemeFormuServisi _OdemeFormuServisi;
        private readonly IKonumServisi _konumServisi;
        private readonly IYetkililerServisi _yetkiliServisi;
        private readonly IKullanıcıServisi _kullanıcıServisi;
        private readonly IBankalarServisi _bankalarServisi;
        private readonly INotServisi _notServisi;
        private readonly IWorkContext _workContext;
        private readonly ITeklifKalemiServisi _teklifKalemiServisi;
        private readonly IHariciSektorServisi _hariciSektorServisi;
        public FinansController(
            IİzinServisi izinServisi,
            ISiteServisi siteServisi,
            IOdemeFormuServisi OdemeFormuServisi,
            IKonumServisi konumServisi,
            IKullanıcıİşlemServisi kullanıcıİşlemServisi,
            IYetkililerServisi yetkiliServisi,
            IBankalarServisi bankalarServisi,
            IKullanıcıServisi kullanıcıServisi,
            INotServisi notServisi,
            IWorkContext workContext,
            ITeklifKalemiServisi teklifKalemiServisi,
            IHariciSektorServisi hariciSektorServisi)
        {
            this._izinServisi = izinServisi;
            this._siteServisi = siteServisi;
            this._OdemeFormuServisi = OdemeFormuServisi;
            this._konumServisi = konumServisi;
            this._kullanıcıİşlemServisi = kullanıcıİşlemServisi;
            this._yetkiliServisi = yetkiliServisi;
            this._bankalarServisi = bankalarServisi;
            this._kullanıcıServisi = kullanıcıServisi;
            this._notServisi = notServisi;
            this._workContext = workContext;
            this._teklifKalemiServisi = teklifKalemiServisi;
            this._hariciSektorServisi = hariciSektorServisi;
        }

        public virtual ActionResult notify()
        {
            string title = "Test";
            ToastVisual t = new ToastVisual()
            {
                TitleText = new ToastText { Text = title },
            };
            ToastContent content = new ToastContent { Visual = t };
            //ToastNotification 
            return null;
        }

        #region eBolum
        public enum eBolum : byte
        {
            [Description("Yönetim")]
            Yönetim = 1,
            [Description("KON")]
            KON = 2,
            [Description("DMC")]
            DMC = 3,
            [Description("OUT")]
            OUT = 4,
            [Description("NUMIL")]
            NUMIL = 5,
            [Description("OPERASYON 1")]
            OPERASYON_1 = 6,
            [Description("OPERASYON 2")]
            OPERASYON_2 = 7,
            [Description("OPERASYON 3")]
            OPERASYON_3 = 8,
            [Description("OPERASYON 4")]
            OPERASYON_4 = 9
        }
        #endregion
        #region eOdemeSekli
        public enum eOdemeSekli : byte
        {
            [Description("Nakit")]
            Nakit = 1,
            [Description("Kredi Kartı")]
            Kredi_Kartı = 2,
            [Description("Mail Order")]
            Mail_Order = 3,
            [Description("Havale")]
            Havale = 4,
            [Description("Çek")]
            Çek = 5
        }
        #endregion
        #region eOdemeTuru
        public enum eOdemeTuru : byte
        {
            [Description("Maaş Avansı")]
            Maaş_Avansı = 1,
            [Description("İş Avansı")]
            İş_Avansı = 2,
            [Description("Cari Hesap Kapama")]
            Cari_Hesap_Kapama = 3,
            [Description("Cari Hesap Avansı")]
            Cari_Hesap_Avansı = 4
        }
        #endregion
        #region ParaBirimi
        public enum eParaBirimi : short
        {
            [Description("TL")]
            TL = 1,
            [Description("$")]
            USD = 2,
            [Description("€")]
            EURO = 3
        }
        #endregion

        #region OdemeFormu
        public virtual ActionResult OdemeFormuListe()
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.OdemeFormuYönet))
                return ErişimEngellendiView();

            var model = new OdemeFormuModel();
            return View(model);
        }
        /*
        [HttpPost]
        public virtual ActionResult OdemeFormuListe(DataSourceİsteği command, OdemeFormuModel model)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.OdemeFormuYönet))
                return ErişimEngellendiKendoGridJson();

            var odemeformları = _OdemeFormuServisi.OdemeFormuAra(null, null, null,null,null,null, false, command.Page - 1, command.PageSize);
            var blogModel = new DataSourceSonucu
            {
                Data = odemeformları.Select(x =>
                {
                    var n = x.ToModel();
                    if (_notServisi.NotAlId(_workContext.MevcutKullanıcı.Id, "OdemeFormu", x.Id).Count > 0)
                    {
                        foreach (var m in _notServisi.NotAlId(_workContext.MevcutKullanıcı.Id, "OdemeFormu", x.Id))
                        {
                            n.Notlar.Add(m.ToModel());
                        }
                    }
                    return n;
                }),
                Toplam = _OdemeFormuServisi.TümOdemeFormuAl().Count
            };
            return Json(blogModel);
        }
        */

        public virtual ActionResult OdemeFormuEkle()
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.OdemeFormuYönet))
                ErişimEngellendiView();

            var model = new OdemeFormuModel();
            foreach (var banka in _bankalarServisi.TümBankalarıAl())
            {
                var bankaModel = banka.ToModel();
                model.Bankalar.Add(bankaModel);
            }
            foreach (var agac in _teklifKalemiServisi.TümTeklifKalemleriAl())
            {
                var agacModel = agac.ToModel();
                model.Agac.Add(agacModel);
            }
            foreach (var tumHariciSektorler in _hariciSektorServisi.TümHariciSektorleriAl())
            {
                var sektorModel = tumHariciSektorler.ToModel();
                model.BelgeTurleri.Add(sektorModel);
            }
            return View(model);
        }

        [HttpPost, FormAdıParametresi("kaydet-devam", "düzenlemeyeDevam")]
        public virtual ActionResult OdemeFormuEkle(OdemeFormuModel model, bool düzenlemeyeDevam, FormCollection form)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.OdemeFormuYönet))
                ErişimEngellendiView();
            if (ModelState.IsValid)
            {
                /*
                model.Agac.Add(new SelectListItem { Text = "Kayıt Kalemi", Value = "0", Selected = true });
                foreach (var agac in _teklifKalemiServisi.TümTeklifKalemleriAl())
                    model.Agac.Add(new SelectListItem { Text = agac.Adı, Value = agac.Id.ToString() });
                */
                var OdemeFormu = model.ToEntity();

                List<string> KalemTutarAdları = new List<string>();
                List<string> KalemTutarParabirimi = new List<string>();
                List<string> Parabirimi = new List<string>();
                for (int j = 0; j < 51; j++)
                {
                    if (form["KalemTutarAdları[" + j + "]"] != null)
                    {
                        List<string> ls = form["KalemTutarAdları[" + j + "]"].Split(',').ToList();
                        List<string> ls2 = form["KalemTutarParabirimi[" + j + "]"].Split(',').ToList();
                        for (int i = 0; i < ls.Count; i++)
                        {
                            KalemTutarAdları.Add(ls[i]);
                            KalemTutarParabirimi.Add(ls2[i]);
                        }
                    }
                }
                for (int j = 0; j < 3; j++)
                {
                    if (form["TutarParabirimi[" + j + "]"] != null)
                    {
                        List<string> ls = form["TutarParabirimi[" + j + "]"].Split(',').ToList();
                        for (int i = 0; i < ls.Count; i++)
                        {
                            Parabirimi.Add(ls[i]);
                        }
                    }
                }
                string[] PO = form.GetValues("PO");
                string[] KalemTutar = form.GetValues("KalemTutar");
                string[] Tutar = form.GetValues("Tutar");

                string TutarDb = "";
                string KalemTutarDb = "";
                for (int i = 0; i < KalemTutarAdları.Count(); i++)
                {
                    if (Convert.ToInt32(KalemTutarAdları[i]) > 0)
                    {
                        string parabirimi = model.ParaBirimi == 1 ? "TL" : (model.ParaBirimi == 2) ? "$" : "€";
                        KalemTutarDb += KalemTutarAdları[i] + "|" + PO[i] + "|" + KalemTutar[i] + "|" + KalemTutarParabirimi[i] + (i == (KalemTutarAdları.Count() - 1) ? "" : ":");
                    }
                }

                for (int i = 0; i < Parabirimi.Count(); i++)
                {
                    if (!String.IsNullOrEmpty(Tutar[i]))
                    {
                        int p = Convert.ToInt32(Parabirimi[i]);
                        string parabirimi2 = p == 1 ? "TL" : (p == 2) ? "$" : "€";
                        TutarDb += Tutar[i] + "|" + Parabirimi[i] + (i == (Parabirimi.Count() - 1) ? "" : ":");
                    }
                }
                OdemeFormu.TutarGrup = TutarDb;
                OdemeFormu.KalemGrup = KalemTutarDb;
                //OdemeFormu.OlusturulmaTarihi = DateTime.Now;
                _OdemeFormuServisi.OdemeFormuEkle(OdemeFormu);
                BaşarılıBildirimi("Odeme formu başarıyla Eklendi");
                _kullanıcıİşlemServisi.İşlemEkle("YeniOdemeFormuEklendi", "Yeni Odeme Formu Eklendi", OdemeFormu.Aciklama);
                if (düzenlemeyeDevam)
                {
                    SeçiliTabKaydet();
                    return RedirectToAction("Düzenle", new { id = OdemeFormu.Id });
                }
                return RedirectToAction("Liste");
            }
            return View(model);
        }
        public virtual ActionResult OdemeFormuDüzenle(int id)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.OdemeFormuYönet))
                ErişimEngellendiView();
            var OdemeFormu = _OdemeFormuServisi.OdemeFormuAlId(id);
            if (OdemeFormu == null)
            {
                return RedirectToAction("Liste");
            }
            var model = OdemeFormu.ToModel();
            foreach (var banka in _bankalarServisi.TümBankalarıAl())
            {
                var bankaModel = banka.ToModel();
                model.Bankalar.Add(bankaModel);
            }
            foreach (var agac in _teklifKalemiServisi.TümTeklifKalemleriAl())
            {
                var agacModel = agac.ToModel();
                model.Agac.Add(agacModel);
            }
            foreach (var kalem in _teklifKalemiServisi.TümTeklifKalemleriAl())
            {
                var bankaModel = kalem.ToModel();
                model.Agac.Add(bankaModel);
            }
            foreach (var rol in _kullanıcıServisi.TümKullanıcıRolleriniAl())
            {
                var rolModel = rol.ToModel();
                model.BolumRol.Add(rolModel);
            }
            foreach (var tumHariciSektorler in _hariciSektorServisi.TümHariciSektorleriAl())
            {
                var sektorModel = tumHariciSektorler.ToModel();
                model.BelgeTurleri.Add(sektorModel);
            }
            string parsedTutarKalemGrup = model.KalemGrup;
            int cnt = 0;
            int cnt2 = 0;
            if (parsedTutarKalemGrup != "" && parsedTutarKalemGrup != String.Empty && parsedTutarKalemGrup != null)
            {
                List<string> ls = parsedTutarKalemGrup.Split(':').ToList();
                cnt = ls.Count;
                for (int i = 0; i < cnt; i++)
                {
                    List<string> ls2 = ls[i].Split('|').ToList();
                    cnt2 = ls2.Count();
                    for (int j = 0; j < cnt2; j++)
                    {
                        string ps = ls2[j].ToString();
                        model.KalemTutarAdları.Add(ls2[j].ToString());
                        model.KalemTutarPOları.Add(ls2[j + 1].ToString());
                        model.KalemTutarları.Add(ls2[j + 2].ToString());
                        model.KalemTutarParabirimi.Add(ls2[j + 3].ToString());
                        j = j + 3;
                    }
                }
            }
            string parsedTutarGrup = model.TutarGrup;
            int cnt3 = 0;
            int cnt4 = 0;
            if (parsedTutarGrup != "" && parsedTutarGrup != String.Empty && parsedTutarGrup != null)
            {
                List<string> ls3 = parsedTutarGrup.Split(':').ToList();
                cnt3 = ls3.Count;
                for (int i = 0; i < cnt3; i++)
                {
                    List<string> ls4 = ls3[i].Split('|').ToList();
                    cnt4 = ls4.Count();
                    for (int j = 0; j < cnt4; j++)
                    {
                        string ps = ls4[j].ToString();
                        model.Tutarlar.Add(ls4[j].ToString());
                        model.TutarParabirimi.Add(ls4[j + 1].ToString());
                        j = j +1;
                    }
                }
            }
            else
            {
                OdemeFormu = model.ToEntity(OdemeFormu);
                OdemeFormu.TutarGrup = model.Tutar + "|" + model.ParaBirimi;
                _OdemeFormuServisi.OdemeFormuGüncelle(OdemeFormu);
            }
            return View(model);
        }
        [HttpPost, FormAdıParametresi("kaydet-devam", "düzenlemeyeDevam")]
        public virtual ActionResult OdemeFormuDüzenle(OdemeFormuModel model, bool düzenlemeyeDevam, FormCollection form)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.OdemeFormuYönet))
                ErişimEngellendiView();
            var OdemeFormu = _OdemeFormuServisi.OdemeFormuAlId(model.Id);
            if (OdemeFormu == null)
            {
                return RedirectToAction("Liste");
            }
            if (ModelState.IsValid)
            {
                foreach (var banka in _bankalarServisi.TümBankalarıAl())
                {
                    var bankaModel = banka.ToModel();
                    model.Bankalar.Add(bankaModel);
                }
                foreach (var rol in _kullanıcıServisi.TümKullanıcıRolleriniAl())
                {
                    var rolModel = rol.ToModel();
                    model.BolumRol.Add(rolModel);
                }
                OdemeFormu = model.ToEntity(OdemeFormu);

                List<string> KalemTutarAdları = new List<string>();
                List<string> KalemTutarParabirimi = new List<string>();
                List<string> Parabirimi = new List<string>();
                for (int j = 0; j < 51; j++)
                {
                    if (form["KalemTutarAdları[" + j + "]"] != null)
                    {
                        List<string> ls = form["KalemTutarAdları[" + j + "]"].Split(',').ToList();
                        List<string> ls2 = form["KalemTutarParabirimi[" + j + "]"].Split(',').ToList();
                        for (int i = 0; i < ls.Count; i++)
                        {
                            KalemTutarAdları.Add(ls[i]);
                            KalemTutarParabirimi.Add(ls2[i]);
                        }
                    }
                }
                for (int j = 0; j < 3; j++)
                {
                    if (form["TutarParabirimi[" + j + "]"] != null)
                    {
                        List<string> ls = form["TutarParabirimi[" + j + "]"].Split(',').ToList();
                        for (int i = 0; i < ls.Count; i++)
                        {
                            Parabirimi.Add(ls[i]);
                        }
                    }
                }
                string[] PO = form.GetValues("PO");
                string[] KalemTutar = form.GetValues("KalemTutar");
                string[] Tutar = form.GetValues("Tutar");

                string TutarDb = "";
                string KalemTutarDb = "";
                for (int i = 0; i < KalemTutarAdları.Count(); i++)
                {
                    if (Convert.ToInt32(KalemTutarAdları[i]) > 0)
                    {
                        string parabirimi = model.ParaBirimi == 1 ? "TL" : (model.ParaBirimi == 2) ? "$" : "€";
                        KalemTutarDb += KalemTutarAdları[i] + "|" + PO[i] + "|" + KalemTutar[i] + "|" + KalemTutarParabirimi[i] + (i == (KalemTutarAdları.Count() - 1) ? "" : ":");
                    }
                }

                for (int i = 0; i < Parabirimi.Count(); i++)
                {
                    if (!String.IsNullOrEmpty(Tutar[i]))
                    {
                        int p = Convert.ToInt32(Parabirimi[i]);
                        string parabirimi2 = p == 1 ? "TL" : (p == 2) ? "$" : "€";
                        TutarDb += Tutar[i] + "|" + Parabirimi[i] + (i == (Parabirimi.Count() - 1) ? "" : ":");
                    }
                }

                OdemeFormu.TutarGrup = TutarDb;
                OdemeFormu.KalemGrup = KalemTutarDb;
                _OdemeFormuServisi.OdemeFormuGüncelle(OdemeFormu);
                BaşarılıBildirimi("OdemeFormu başarıyla güncellenmiştir.");
                _kullanıcıİşlemServisi.İşlemEkle("OdemeFormuGüncelle", "OdemeFormu güncellendi", OdemeFormu.Aciklama);
                if (düzenlemeyeDevam)
                {
                    return RedirectToAction("OdemeFormuDüzenle", new { id = OdemeFormu.Id });
                }
                return RedirectToAction("Liste");
            }
            return View(model);
        }

        [HttpPost]
        public virtual ActionResult OdemeDuzenle(IEnumerable<OdemeFormuModel> models)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.TeklifHariciYönet))
                ErişimEngellendiView();

            if (models != null)
            {
                foreach (var pModel in models)
                {
                    //update
                    var oge = _OdemeFormuServisi.OdemeFormuAlId(pModel.Id);
                    if (oge != null)
                    {
                        pModel.KongreTarihi = oge.KongreTarihi;
                        pModel.OdemeTarihi = oge.OdemeTarihi;
                        oge = pModel.ToEntity(oge);
                        _OdemeFormuServisi.OdemeFormuGüncelle(oge);
                    }
                }
            }
            return new BoşJsonSonucu();
        }

        [HttpPost]
        public virtual ActionResult OdemeFormuSil(OdemeFormuModel model)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.OdemeFormuYönet))
                return ErişimEngellendiView();

            foreach (var banka in _bankalarServisi.TümBankalarıAl())
            {
                var bankaModel = banka.ToModel();
                model.Bankalar.Add(bankaModel);
            }
            foreach (var rol in _kullanıcıServisi.TümKullanıcıRolleriniAl())
            {
                var rolModel = rol.ToModel();
                model.BolumRol.Add(rolModel);
            }
            var OdemeFormu = _OdemeFormuServisi.OdemeFormuAlId(model.Id);
            if (OdemeFormu == null)
                return RedirectToAction("Liste");
            _OdemeFormuServisi.OdemeFormuSil(OdemeFormu);
            BaşarılıBildirimi("OdemeFormu başarıyla silindi");
            _kullanıcıİşlemServisi.İşlemEkle("OdemeFormuSil", "OdemeFormu silindi", OdemeFormu.Aciklama);
            return RedirectToAction("Liste");
        }

        public virtual ActionResult OdemeFormuGoruntule(int id)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.OdemeFormuGoruntule))
                ErişimEngellendiView();

            var OdemeFormu = _OdemeFormuServisi.OdemeFormuAlId(id);
            var model = OdemeFormu.ToModel();
            model.BankaAdı = model.Banka == 0 ? "" : _bankalarServisi.BankaAlId(model.Banka).Adı;
            model.BelgeTuruAdı = model.Banka == 0 ? "" : _hariciSektorServisi.HariciSektorAlId(model.BelgeTuru).Adı;
            List<string> KalemTutarAdları = new List<string>();
            List<string> KalemTutarPOları = new List<string>();
            List<string> KalemTutarları = new List<string>();
            List<string> KalemTutarParabirimi = new List<string>();
            string parsedTutarKalemGrup = model.KalemGrup;
            int cnt = 0;
            int cnt2 = 0;
            if (parsedTutarKalemGrup != "" && parsedTutarKalemGrup != String.Empty && parsedTutarKalemGrup != null)
            {
                List<string> ls = parsedTutarKalemGrup.Split(':').ToList();
                cnt = ls.Count;
                for (int i = 0; i < cnt; i++)
                {
                    List<string> ls2 = ls[i].Split('|').ToList();
                    cnt2 = ls2.Count();
                    for (int j = 0; j < cnt2; j++)
                    {
                        string ps = ls2[j].ToString();
                        KalemTutarAdları.Add(ls2[j].ToString());
                        KalemTutarPOları.Add(ls2[j + 1].ToString());
                        KalemTutarları.Add(ls2[j + 2].ToString());
                        string parabirimi = Convert.ToInt32(ls2[j + 3]) == 1 ? "TL" : (Convert.ToInt32(ls2[j + 3]) == 2) ? "$" : "€";
                        KalemTutarParabirimi.Add(parabirimi);
                        j = j + 3;
                    }
                }
            }
            for (int i = 0; i < KalemTutarları.Count; i++)
            {
                model.KalemTutarS += "PO:" + KalemTutarPOları[i] + ", " + _teklifKalemiServisi.TeklifKalemiAlId(Convert.ToInt32(KalemTutarAdları[i])).Adı + " " + KalemTutarları[i] + " " + KalemTutarParabirimi[i] + (i != KalemTutarları.Count - 1 ? " | " : "");
            }
            return View(model);

        }
        #endregion
        #region Bolum
        [HttpPost]
        public JsonResult Bolum(int formId)
        {
            StringBuilder sbBolum = new StringBuilder();
            StringBuilder sbOdemeSekli = new StringBuilder();
            StringBuilder sbOdemeTuru = new StringBuilder();
            StringBuilder sbMeblag = new StringBuilder();
            List<string> kat = new List<string>();
            var form = _OdemeFormuServisi.OdemeFormuAlId(formId);
            CultureInfo tr = new CultureInfo("tr-TR");
            #region Bölüm
            /*
            for (int i = 1; i <= Enum.GetNames(typeof(eBolum)).Length; i++)
            {
                if (i == 1)
                    sbBolum.AppendLine("<div class=\"row\"><ul>");
                if (i != form.Bolum)
                    sbBolum.AppendLine("<li><img src=\"../../Content/Images/check-blank.png\" width=\"20px\" />" + (eBolum)i + "</li>");
                else
                    sbBolum.AppendLine("<li><span style=\"border: 2px solid; border-radius: 2px; padding: 0 5px 0 5px; \">" + form.BolumNo + "</span>" + (eBolum)i + "</li>");
                if (i == 5)
                    sbBolum.AppendLine("</ul></div><div class=\"row\"><ul>");
                if (i == 9)
                    sbBolum.AppendLine("</ul></div>");
            }
            */
            sbBolum.AppendLine("<img src=\"../../Content/Images/check-blank.png\" width=\"20px\" />" + (eBolum)form.Bolum + "");
            #endregion
            #region Ödeme Şekli
            for (int i = 1; i <= Enum.GetNames(typeof(eOdemeSekli)).Length; i++)
            {
                if (i == 1)
                    sbOdemeSekli.AppendLine("<div class=\"row\"><ul>");
                if (i != form.OdemeSekli)
                    sbOdemeSekli.AppendLine("<li><img src=\"../../Content/Images/check-blank.png\" width=\"20px\" />" + (eOdemeSekli)i + "</li>");
                else
                    sbOdemeSekli.AppendLine("<li><img src=\"../../Content/Images/check.png\" width=\"20px\" />" + (eOdemeSekli)i + "</li>");
                if (i == 5)
                    sbOdemeSekli.AppendLine("</ul></div><div class=\"row\"><ul>");
                if (i == 9)
                    sbOdemeSekli.AppendLine("</ul></div>");
            }
            #endregion
            #region Ödeme Türü
            for (int i = 1; i <= Enum.GetNames(typeof(eOdemeTuru)).Length; i++)
            {
                if (i == 1)
                    sbOdemeTuru.AppendLine("<div class=\"row\"><ul>");
                if (i != form.OdemeTuru)
                    sbOdemeTuru.AppendLine("<li><img src=\"../../Content/Images/check-blank.png\" width=\"20px\" />" + (eOdemeTuru)i + "</li>");
                else
                    sbOdemeTuru.AppendLine("<li><img src=\"../../Content/Images/check.png\" width=\"20px\" />" + (eOdemeTuru)i + "</li>");
                if (i == 5)
                    sbOdemeTuru.AppendLine("</ul></div><div class=\"row\"><ul>");
                if (i == 9)
                    sbOdemeTuru.AppendLine("</ul></div>");
            }
            #endregion
            #region Meblag
            decimal tutar = form.Tutar;
            string t = tutar.ToString("0,0.00", tr);
            sbMeblag.AppendLine(t + " " + ((eParaBirimi)form.ParaBirimi).ToString());
            #endregion
            kat.Add(sbBolum.ToString());
            kat.Add(sbOdemeSekli.ToString());
            kat.Add(sbOdemeTuru.ToString());
            kat.Add(sbMeblag.ToString());
            return Json(kat);
        }
        #endregion
        public virtual ActionResult Liste()
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.OdemeFormuYönet))
                return ErişimEngellendiView();

            var model = new OdemeFormuModel();
            return View(model);
        }

        [HttpPost]
        public virtual ActionResult Liste(DataSourceİsteği command, OdemeFormuModel model)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.OdemeFormuYönet))
                return ErişimEngellendiKendoGridJson();

            var odemeGünüAra = 0;
            int odemeAyıAra = 0;
            var kongreGünüAra = 0;
            int kongreAyıAra = 0;
            if (!String.IsNullOrWhiteSpace(model.KongreGünüAra))
                kongreGünüAra = Convert.ToInt32(model.KongreGünüAra);
            if (!String.IsNullOrWhiteSpace(model.KongreAyıAra))
                kongreAyıAra = Convert.ToInt32(model.KongreAyıAra);
            if (!String.IsNullOrWhiteSpace(model.OdemeGünüAra))
                odemeGünüAra = Convert.ToInt32(model.OdemeGünüAra);
            if (!String.IsNullOrWhiteSpace(model.OdemeAyıAra))
                odemeAyıAra = Convert.ToInt32(model.OdemeAyıAra);

            var formlar = _OdemeFormuServisi.OdemeFormuAra(Id:model.IdAra,firma:model.FirmaAra,kongreGunu: kongreGünüAra,kongreAyı: kongreAyıAra,odemeGunu:odemeGünüAra,
                odemeAyı:odemeAyıAra,aciklama:model.AciklamaAra,alisFatura:model.AlisFaturaAra, 
                satisFatura:model.SatisFaturaAra,enYeniler:false,sayfaIndeksi: command.Page - 1,sayfaBüyüklüğü: command.PageSize);
            var formModel = new DataSourceSonucu
            {
                Data = formlar.Select(x =>
                {
                    var n = x.ToModel();
                    if (_notServisi.NotAlId(_workContext.MevcutKullanıcı.Id, "OdemeFormu", x.Id).Count > 0)
                    {
                        foreach (var m in _notServisi.NotAlId(_workContext.MevcutKullanıcı.Id, "OdemeFormu", x.Id))
                        {
                            n.Notlar.Add(m.ToModel());
                        }
                    }
                    return n;
                }),
                Toplam = _OdemeFormuServisi.TümOdemeFormuAl().Count
            };
            SeçiliSayfaKaydet();
            return Json(formModel);
        }
    }
}