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
using Core.Domain.Katalog;
using Web.Models.Genel;
using System;
using Web.Models.Rehber;
using Core;

namespace Web.Controllers
{
    public class RehberController : TemelPublicController
    {
        private readonly IİzinServisi _izinServisi;
        private readonly IHekimlerServisi _hekimServisi;
        private readonly IKullanıcıİşlemServisi _kullanıcıİşlemServisi;
        private readonly IYetkililerServisi _yetkiliServisi;
        private readonly IFirmaServisi _firmaServisi;
        private readonly KatalogAyarları _katalogAyarları;
        private readonly IFirmaKategorisiServisi _firmaKategorisiServisi;
        private readonly IHekimBranşlarıServisi _branşlarServisi;
        public RehberController(IİzinServisi izinServisi,
            IHekimlerServisi hekimServisi,
            IKullanıcıİşlemServisi kullanıcıİşlemServisi,
            IYetkililerServisi yetkiliServisi,
            IFirmaServisi firmaServisi,
            KatalogAyarları katalogAyarları,
            IFirmaKategorisiServisi firmaKategorisiServisi,
            IHekimBranşlarıServisi branşlarServisi)
        {
            this._izinServisi = izinServisi;
            this._hekimServisi = hekimServisi;
            this._kullanıcıİşlemServisi = kullanıcıİşlemServisi;
            this._firmaServisi=firmaServisi;
            this._katalogAyarları = katalogAyarları;
            this._yetkiliServisi = yetkiliServisi;
            this._firmaKategorisiServisi = firmaKategorisiServisi;
            this._branşlarServisi = branşlarServisi;
        }
        public ActionResult FirmaKategorileri()
        {
            var firmaKategorileri = _firmaKategorisiServisi.TümFirmaKategorisiAl();
            var sonuc = (from s in firmaKategorileri
                         select new
                         {
                             id = s.Id,
                             name = s.Adı
                         }).ToList();
            return Json(sonuc, JsonRequestBehavior.AllowGet);
        }
        public ActionResult HekimBranslari()
        {
            var hekimBranslari = _branşlarServisi.TümHekimBranşlarıAl();
            var sonuc = (from s in hekimBranslari
                         select new
                         {
                             id = s.Id,
                             name = s.Adı
                         }).ToList();
            return Json(sonuc, JsonRequestBehavior.AllowGet);
        }
        #region Yetkili

        [HttpGet]
        public virtual ActionResult RehberListe(DataSourceİsteği command, KisilerModel model,int? page)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.OdemeFormuYönet))
                return ErişimEngellendiKendoGridJson();
            if (ModelState.IsValid)
            {
                var pageSize = _katalogAyarları.ProductReviewsPageSizeOnAccountPage;
                int pageIndex = 0;

                if (page > 0)
                {
                    pageIndex = page.Value - 1;
                }

                var list = _yetkiliServisi.YetkiliAra(model.FirmaAra, model.AdAra, model.SoyadAra, model.TCKNAra,model.EmailAra, false, pageIndex, pageSize);
                var list2 = _hekimServisi.TümHekimlerAl();
                var yetkiliList = model;

                foreach (var yetkili in list)
                {
                    var yetkiliModel = new KisiModel
                    {
                        Id = yetkili.Id,
                        Adı = yetkili.Adı,
                        Soyadı = yetkili.Soyadı,
                        KategoriId = yetkili.KategoriId,
                        CepTel1 = yetkili.CepTel1,
                        CepTel2 = yetkili.CepTel2,
                        Email1 = yetkili.Email1,
                        Email2 = yetkili.Email2,
                        DoğumTarihi = yetkili.DoğumTarihi,
                        Adres = yetkili.Adres,
                        PostaKodu = yetkili.PostaKodu,
                        YSehirId = yetkili.YSehirId,
                        YIlceId = yetkili.YIlceId,
                        UnvanId = yetkili.UnvanId,
                    };
                    yetkiliList.Kisiler.Add(yetkiliModel);
                };
                foreach (var hekim in list2)
                {
                    var yetkiliModel = new KisiModel
                    {
                        Id = hekim.Id,
                        Adı = hekim.Adı,
                        Soyadı = hekim.Soyadı,
                        KategoriId = 0,
                        CepTel1 = hekim.CepTel1,
                        CepTel2 = hekim.CepTel2,
                        Email1 = hekim.Email1,
                        Email2 = hekim.Email2,
                        DoğumTarihi = hekim.DoğumTarihi,
                        Adres = hekim.EvAdresi,
                        PostaKodu = hekim.PostaKodu,
                        YSehirId = hekim.SehirId,
                        YIlceId = hekim.IlceId,
                        UnvanId = hekim.BranşId,
                    };
                    yetkiliList.Kisiler.Add(yetkiliModel);
                };
                var sl = new SayfalıListe<KisiModel>(yetkiliList.Kisiler, pageIndex, pageSize);
                var pagerModel = new PagerModel
                {
                    PageSize = sl.PageSize,
                    TotalRecords = sl.TotalCount,
                    PageIndex = sl.PageIndex,
                    ShowTotalSummary = false,
                    RouteActionName = "SayfalananKisiler",
                    UseRouteLinks = true,
                    RouteValues = new YetkililerPagerModel.YetkililerRouteValues { page = pageIndex }
                };
                model.PagerModel = pagerModel;
                model.Kisiler = sl;
                return View(model);
            }
            return View();
        }
        [HttpGet]
        public virtual ActionResult _RehberKutusu(KisilerModel model, int? page)
        {

            var pageSize = _katalogAyarları.ProductReviewsPageSizeOnAccountPage;
            int pageIndex = 0;

            if (page > 0)
            {
                pageIndex = page.Value - 1;
            }
            var list = _yetkiliServisi.YetkiliAra(model.FirmaAra, model.AdAra, model.SoyadAra, model.TCKNAra, model.EmailAra, false, pageIndex, pageSize);
            var list2 = _hekimServisi.HekimAra(model.BransAra, model.AdAra, model.SoyadAra, model.TCKNAra, model.EmailAra, false, pageIndex, pageSize);
            var yetkiliList = model;
            if (model.BransAra == 0)
            {
                foreach (var yetkili in list)
                {
                    var yetkiliModel = new KisiModel
                    {
                        Id = yetkili.Id,
                        Adı = yetkili.Adı,
                        Soyadı = yetkili.Soyadı,
                        KategoriId = yetkili.KategoriId,
                        CepTel1 = yetkili.CepTel1,
                        CepTel2 = yetkili.CepTel2,
                        Email1 = yetkili.Email1,
                        Email2 = yetkili.Email2,
                        DoğumTarihi = yetkili.DoğumTarihi,
                        Adres = yetkili.Adres,
                        PostaKodu = yetkili.PostaKodu,
                        YSehirId = yetkili.YSehirId,
                        YIlceId = yetkili.YIlceId,
                        UnvanId = yetkili.UnvanId,
                    };
                    yetkiliList.Kisiler.Add(yetkiliModel);
                };
            }
            if (model.FirmaAra == 0)
            {
                foreach (var hekim in list2)
                {
                    var yetkiliModel = new KisiModel
                    {
                        Id = hekim.Id,
                        Adı = hekim.Adı,
                        Soyadı = hekim.Soyadı,
                        KategoriId = 0,
                        CepTel1 = hekim.CepTel1,
                        CepTel2 = hekim.CepTel2,
                        Email1 = hekim.Email1,
                        Email2 = hekim.Email2,
                        DoğumTarihi = hekim.DoğumTarihi,
                        Adres = hekim.EvAdresi,
                        PostaKodu = hekim.PostaKodu,
                        YSehirId = hekim.SehirId,
                        YIlceId = hekim.IlceId,
                        UnvanId = hekim.BranşId,
                    };
                    yetkiliList.Kisiler.Add(yetkiliModel);
                };
            }
            var sl = new SayfalıListe<KisiModel>(yetkiliList.Kisiler, pageIndex, pageSize);
            var pagerModel = new PagerModel
            {
                PageSize = sl.PageSize,
                TotalRecords = sl.TotalCount,
                PageIndex = sl.PageIndex,
                ShowTotalSummary = false,
                RouteActionName = "SayfalananKisiler",
                UseRouteLinks = true,
                RouteValues = new YetkililerPagerModel.YetkililerRouteValues { page = pageIndex }
            };
            model.PagerModel = pagerModel;
            model.Kisiler = sl;
            return PartialView(model);
        }
        public virtual ActionResult RehberEkle()
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.YetkiliYönet))
                ErişimEngellendiView();

            var model = new YetkililerModel();
            return View(model);
        }

        [HttpPost, FormAdıParametresi("kaydet-devam", "düzenlemeyeDevam")]
        public virtual ActionResult RehberEkle(YetkililerModel model, bool düzenlemeyeDevam)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.YetkiliYönet))
                ErişimEngellendiView();
            if (ModelState.IsValid)
            {
                var yetkili = model.ToEntity();
                _yetkiliServisi.YetkiliEkle(yetkili);
                BaşarılıBildirimi("Müşteri sektörü başarıyla Eklendi");
                _kullanıcıİşlemServisi.İşlemEkle("YeniMüşteriEklendi", "Yeni Müşteri Eklendi", yetkili.Adı);
                if (düzenlemeyeDevam)
                {
                    SeçiliTabKaydet();
                    return RedirectToAction("Düzenle", new { id = yetkili.Id });
                }
                return RedirectToAction("YetkiliListe");
            }
            return View(model);
        }
        public virtual ActionResult RehberDüzenle(int id)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.YetkiliYönet))
                ErişimEngellendiView();
            var yetkili = _yetkiliServisi.YetkiliAlId(id);
            if (yetkili == null)
            {
                return RedirectToAction("YetkiliListe");
            }
            var model = yetkili.ToModel();
            return View(model);
        }
        [HttpPost, FormAdıParametresi("kaydet-devam", "düzenlemeyeDevam")]
        public virtual ActionResult RehberDüzenle(YetkililerModel model, bool düzenlemeyeDevam)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.YetkiliYönet))
                ErişimEngellendiView();
            var yetkili = _yetkiliServisi.YetkiliAlId(model.Id);
            if (yetkili == null)
            {
                return RedirectToAction("YetkiliListe");
            }
            if (ModelState.IsValid)
            {
                yetkili = model.ToEntity(yetkili);
                _yetkiliServisi.YetkiliGüncelle(yetkili);
                BaşarılıBildirimi("Yetkili başarıyla güncellenmiştir.");
                _kullanıcıİşlemServisi.İşlemEkle("YetkiliGüncelle", "Yetkili güncellendi", yetkili.Adı);
                if (düzenlemeyeDevam)
                {
                    return RedirectToAction("YetkiliDüzenle", new { id = yetkili.Id });
                }
                return RedirectToAction("YetkiliListe");
            }
            return View(model);
        }

        [HttpPost]
        public virtual ActionResult RehberSil(YetkililerModel model)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.YetkiliYönet))
                return ErişimEngellendiView();

            var yetkili = _yetkiliServisi.YetkiliAlId(model.Id);
            if (yetkili == null)
                return RedirectToAction("YetkiliListe");
            _yetkiliServisi.YetkiliSil(yetkili);
            BaşarılıBildirimi("Yetkili başarıyla silindi");
            _kullanıcıİşlemServisi.İşlemEkle("YetkiliSil", "Yetkili silindi", yetkili.Adı);
            return RedirectToAction("YetkiliListe");
        }
        #endregion
    }
}