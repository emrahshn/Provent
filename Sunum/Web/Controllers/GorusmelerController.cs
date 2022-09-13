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
using Web.Models.Görüşmeler;
using Services.Görüşmeler;
using Services.Tanımlamalar;
using System.Collections.Generic;

namespace Web.Controllers
{
    public class GorusmelerController : TemelPublicController
    {
        private readonly IİzinServisi _izinServisi;
        private readonly IKullanıcıİşlemServisi _kullanıcıİşlemServisi;
        private readonly ISiteServisi _siteServisi;
        private readonly IGorusmeRaporlariServisi _GorusmeRaporlariServisi;
        private readonly IKonumServisi _konumServisi;
        private readonly IYetkililerServisi _yetkiliServisi;
        private readonly IFirmaServisi _firmaServisi;


        public GorusmelerController(
            IİzinServisi izinServisi,
            ISiteServisi siteServisi,
            IGorusmeRaporlariServisi GorusmeRaporlariServisi,
            IKonumServisi konumServisi,
            IKullanıcıİşlemServisi kullanıcıİşlemServisi,
            IYetkililerServisi yetkiliServisi,
            IFirmaServisi firmaServisi)
        {
            this._izinServisi = izinServisi;
            this._siteServisi = siteServisi;
            this._GorusmeRaporlariServisi = GorusmeRaporlariServisi;
            this._konumServisi = konumServisi;
            this._kullanıcıİşlemServisi = kullanıcıİşlemServisi;
            this._yetkiliServisi = yetkiliServisi;
            this._firmaServisi = firmaServisi;
        }

        [HttpPost]
        public virtual ActionResult FirmalarListe(DataSourceİsteği command)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.GorusmeRaporlariYönet))
                return ErişimEngellendiKendoGridJson();

            var musteriModels = _firmaServisi.TümFirmaAl();
            var firmalar = new List<GörüsülenFirmaModel>();
            foreach(var musteri in musteriModels)
            {
                var item = new GörüsülenFirmaModel {
                    FirmaAdı = musteri.Adı,
                    Grup = 1,
                    GrupId = musteri.Id
                };
                firmalar.Add(item);
            };
           
            return Json(firmalar);
        }
        /*
        [HttpPost]
        public virtual ActionResult GorusulenListe(DataSourceİsteği command,int grup,int grupId)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.GorusmeRaporlariYönet))
                return ErişimEngellendiKendoGridJson();

            //var yetkililerModels = _yetkiliServisi.YetkiliAlGrup(grup,grupId);
            //return Json(yetkililerModels);
        }
        */
        public virtual ActionResult PazarlamaAnketi()
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.GorusmeRaporlariYönet))
                return ErişimEngellendiView();

            var model = new PazarlamaAnketiModel();
            return View(model);
        }

        #region GorusmeRaporlari
        public virtual ActionResult GorusmeRaporlariListe()
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.GorusmeRaporlariYönet))
                return ErişimEngellendiView();

            var model = new GorusmeRaporlariModel();
            return View(model);
        }

        [HttpPost]
        public virtual ActionResult GorusmeRaporlariListe(DataSourceİsteği command, GorusmeRaporlariModel model)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.GorusmeRaporlariYönet))
                return ErişimEngellendiKendoGridJson();

            var GorusmeRaporlariModels = _GorusmeRaporlariServisi.TümGorusmeRaporlariAl(true);
            var gridModel = new DataSourceSonucu
            {
                Data = GorusmeRaporlariModels.Select(x=>
                {
                    var GorusmeRaporlariModel = x.ToModel();
                    return GorusmeRaporlariModel;
                }),
                Toplam = GorusmeRaporlariModels.Count
            };

            return Json(gridModel);
        }

        
        public virtual ActionResult GorusmeRaporlariEkle()
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.GorusmeRaporlariYönet))
                ErişimEngellendiView();

            var model = new GorusmeRaporlariModel();
            return View(model);
        }

        [HttpPost, FormAdıParametresi("kaydet-devam", "düzenlemeyeDevam")]
        public virtual ActionResult GorusmeRaporlariEkle(GorusmeRaporlariModel model, bool düzenlemeyeDevam)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.GorusmeRaporlariYönet))
                ErişimEngellendiView();
            if (ModelState.IsValid)
            {
                var GorusmeRaporlari = model.ToEntity();
                GorusmeRaporlari.OlusturulmaTarihi = DateTime.Now;
                _GorusmeRaporlariServisi.GorusmeRaporlariEkle(GorusmeRaporlari);
                BaşarılıBildirimi("Müşteri sektörü başarıyla Eklendi");
                _kullanıcıİşlemServisi.İşlemEkle("YeniMüşteriEklendi", "Yeni Müşteri Eklendi", GorusmeRaporlari.FirmaAdı);
                if (düzenlemeyeDevam)
                {
                    SeçiliTabKaydet();
                    return RedirectToAction("Düzenle", new { id = GorusmeRaporlari.Id });
                }
                return RedirectToAction("GorusmeRaporlariListe");
            }
            return View(model);
        }
        public virtual ActionResult GorusmeRaporlariDüzenle(int id)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.GorusmeRaporlariYönet))
                ErişimEngellendiView();
            var GorusmeRaporlari = _GorusmeRaporlariServisi.GorusmeRaporlariAlId(id);
            if (GorusmeRaporlari == null)
            {
                return RedirectToAction("GorusmeRaporlariListe");
            }
            var model = GorusmeRaporlari.ToModel();
            return View(model);
        }
        [HttpPost, FormAdıParametresi("kaydet-devam", "düzenlemeyeDevam")]
        public virtual ActionResult GorusmeRaporlariDüzenle(GorusmeRaporlariModel model, bool düzenlemeyeDevam)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.GorusmeRaporlariYönet))
                ErişimEngellendiView();
            var GorusmeRaporlari = _GorusmeRaporlariServisi.GorusmeRaporlariAlId(model.Id);
            if (GorusmeRaporlari == null)
            {
                return RedirectToAction("GorusmeRaporlariListe");
            }
            if (ModelState.IsValid)
            {
                GorusmeRaporlari = model.ToEntity(GorusmeRaporlari);
                GorusmeRaporlari.OlusturulmaTarihi = DateTime.Now;
                _GorusmeRaporlariServisi.GorusmeRaporlariGüncelle(GorusmeRaporlari);
                BaşarılıBildirimi("GorusmeRaporlari başarıyla güncellenmiştir.");
                _kullanıcıİşlemServisi.İşlemEkle("GorusmeRaporlariGüncelle", "GorusmeRaporlari güncellendi", GorusmeRaporlari.FirmaAdı);
                if (düzenlemeyeDevam)
                {
                    return RedirectToAction("GorusmeRaporlariDüzenle", new { id = GorusmeRaporlari.Id });
                }
                return RedirectToAction("GorusmeRaporlariListe");
            }
            return View(model);
        }

        [HttpPost]
        public virtual ActionResult GorusmeRaporlariSil(GorusmeRaporlariModel model)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.GorusmeRaporlariYönet))
                return ErişimEngellendiView();

            var GorusmeRaporlari = _GorusmeRaporlariServisi.GorusmeRaporlariAlId(model.Id);
            if (GorusmeRaporlari == null)
                return RedirectToAction("GorusmeRaporlariListe");
            _GorusmeRaporlariServisi.GorusmeRaporlariSil(GorusmeRaporlari);
            BaşarılıBildirimi("GorusmeRaporlari başarıyla silindi");
            _kullanıcıİşlemServisi.İşlemEkle("GorusmeRaporlariSil", "GorusmeRaporlari silindi", GorusmeRaporlari.FirmaAdı);
            return RedirectToAction("GorusmeRaporlariListe");
        }
        #endregion
    }
}