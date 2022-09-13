using Services.Güvenlik;
using Services.Siteler;
using System.Linq;
using System.Web.Mvc;
using Services.Logging;
using Web.Models.Notlar;
using Services.Notlar;
using Core;
using System.Collections.Generic;
using Web.Uzantılar;
using Web.Framework.Controllers;
using Services.Teklifler;

namespace Web.Controllers
{
    public class NotlarController : TemelPublicController
    {
        private readonly IİzinServisi _izinServisi;
        private readonly ISiteServisi _siteServisi;
        private readonly IKullanıcıİşlemServisi _kullanıcıİşlemServisi;
        private readonly INotServisi _notServisi;
        private readonly IWorkContext _workContext;
        private readonly ITeklifHariciServisi _hariciTeklifServisi;

        public NotlarController(IİzinServisi izinServisi,
            ISiteServisi siteServisi,
            IKullanıcıİşlemServisi kullanıcıİşlemServisi,
            INotServisi notServisi,
            IWorkContext workContext,
            ITeklifHariciServisi hariciTeklifServisi
            )
        {
            this._izinServisi = izinServisi;
            this._siteServisi = siteServisi;
            this._kullanıcıİşlemServisi = kullanıcıİşlemServisi;
            this._notServisi = notServisi;
            this._workContext = workContext;
            this._hariciTeklifServisi = hariciTeklifServisi;
        }
        #region Not
        [ChildActionOnly]
        public virtual ActionResult NotBox(string grup, int? grupid)
        {
            int userid = _workContext.MevcutKullanıcı.Id;
            var model = new List<NotModel>();
            foreach(var n in _notServisi.NotAlId(userid, grup, grupid))
            {
                var notModel = n.ToModel();
                model.Add(notModel);
            }
           // model = _notServisi.NotAlId(userid, "OdemeFormu", grupid);
            return PartialView(model);
        }
        public virtual ActionResult NotEkle(string grup, int grupId, string returnUrl)
        {

            var model = new NotModel();
            return View(model);
        }
        [HttpPost, FormAdıParametresi("kaydet-devam", "düzenlemeyeDevam")]
        public virtual ActionResult NotEkle(NotModel model,bool düzenlemeyeDevam,string grup,int grupId,string returnUrl)
        {
            var Not = _notServisi.NotAlId(model.Id);
            if (ModelState.IsValid)
            {
                Not = model.ToEntity();
                Not.Grup = grup;
                Not.GrupId = grupId;
                Not.KullanıcıId = _workContext.MevcutKullanıcı.Id;
                _notServisi.NotEkle(Not);
                BaşarılıBildirimi("Not başarıyla eklenmiştir.");
                _kullanıcıİşlemServisi.İşlemEkle("NotEkle", "Not eklendi", Not.Icerik);
                if(grup == "HariciTeklif")
                {
                    var teklif = _hariciTeklifServisi.TeklifAlId(grupId);
                    teklif.Not.Add(Not);
                    _hariciTeklifServisi.TeklifGüncelle(teklif);
                }
                if (düzenlemeyeDevam)
                {
                    return RedirectToAction("NotDüzenle", new { id = Not.Id });
                }
                return RedirectToAction(returnUrl);
            }
            return View(model);
        }
        public virtual ActionResult NotDüzenle(int id)
        {
            var Not = _notServisi.NotAlId(id);
            var model = Not.ToModel();
           
            return View(model);
        }
        [HttpPost, FormAdıParametresi("kaydet-devam", "düzenlemeyeDevam")]
        public virtual ActionResult NotDüzenle(NotModel model, bool düzenlemeyeDevam, string returnUrl)
        {
            var Not = _notServisi.NotAlId(model.Id);
            if (ModelState.IsValid)
            {
                model.Grup = Not.Grup;
                model.GrupId = Not.GrupId;
                model.KullanıcıId = Not.KullanıcıId;
                Not = model.ToEntity(Not);
                _notServisi.NotGüncelle(Not);
                BaşarılıBildirimi("Not başarıyla güncellenmiştir.");
                _kullanıcıİşlemServisi.İşlemEkle("NotGüncelle", "Not güncellendi", Not.Icerik);
                if (düzenlemeyeDevam)
                {
                    return RedirectToAction("NotDüzenle", new { id = Not.Id });
                }
                return RedirectToAction(returnUrl);
            }
            return View(model);
        }

        public virtual ActionResult NotSil(int id, string returnUrl)
        {
            var Not = _notServisi.NotAlId(id);
            _notServisi.NotSil(Not);
            BaşarılıBildirimi("Not başarıyla silindi");
            _kullanıcıİşlemServisi.İşlemEkle("NotSil", "Not silindi", Not.Icerik);
            return RedirectToAction(returnUrl);
        }
        #endregion
    }
}