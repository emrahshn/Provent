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
using Services.Mesajlar;
using Web.Models.Mesajlar;

namespace Web.Controllers
{
    public class MesajlarController : TemelPublicController
    {
        private readonly IİzinServisi _izinServisi;
        private readonly ISiteServisi _siteServisi;
        private readonly IKullanıcıİşlemServisi _kullanıcıİşlemServisi;
        private readonly IMesajlarServisi _mesajServisi;
        private readonly IWorkContext _workContext;

        public MesajlarController(IİzinServisi izinServisi,
            ISiteServisi siteServisi,
            IKullanıcıİşlemServisi kullanıcıİşlemServisi,
            IMesajlarServisi mesajServisi,
            IWorkContext workContext
            )
        {
            this._izinServisi = izinServisi;
            this._siteServisi = siteServisi;
            this._kullanıcıİşlemServisi = kullanıcıİşlemServisi;
            this._mesajServisi = mesajServisi;
            this._workContext = workContext;
        }
        #region Not
        [ChildActionOnly]
        public virtual ActionResult MesajKutusu()
        {
            int userid = _workContext.MevcutKullanıcı.Id;
            var model = new List<MesajModel>();
            foreach(var n in _mesajServisi.TümMesajlarAl())
            {
                var mesajModel = n.ToModel();
                model.Add(mesajModel);
            }
            return PartialView(model);
        }
        /*
        public virtual ActionResult NotEkle(string grup, int grupId, string returnUrl)
        {

            var model = new NotModel();
            return View(model);
        }
        [HttpPost, FormAdıParametresi("kaydet-devam", "düzenlemeyeDevam")]
        public virtual ActionResult NotEkle(NotModel model,bool düzenlemeyeDevam,string grup,int grupId,string returnUrl)
        {
            var Not = _mesajServisi.NotAlId(model.Id);
            if (ModelState.IsValid)
            {
                Not = model.ToEntity();
                Not.Grup = grup;
                Not.GrupId = grupId;
                Not.KullanıcıId = _workContext.MevcutKullanıcı.Id;
                _mesajServisi.NotEkle(Not);
                BaşarılıBildirimi("Not başarıyla eklenmiştir.");
                _kullanıcıİşlemServisi.İşlemEkle("NotEkle", "Not eklendi", Not.Icerik);
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
            var Not = _mesajServisi.NotAlId(id);
            var model = Not.ToModel();
           
            return View(model);
        }
        [HttpPost, FormAdıParametresi("kaydet-devam", "düzenlemeyeDevam")]
        public virtual ActionResult NotDüzenle(NotModel model, bool düzenlemeyeDevam, string returnUrl)
        {
            var Not = _mesajServisi.NotAlId(model.Id);
            if (ModelState.IsValid)
            {
                model.Grup = Not.Grup;
                model.GrupId = Not.GrupId;
                model.KullanıcıId = Not.KullanıcıId;
                Not = model.ToEntity(Not);
                _mesajServisi.NotGüncelle(Not);
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
            var Not = _mesajServisi.NotAlId(id);
            _mesajServisi.NotSil(Not);
            BaşarılıBildirimi("Not başarıyla silindi");
            _kullanıcıİşlemServisi.İşlemEkle("NotSil", "Not silindi", Not.Icerik);
            return RedirectToAction(returnUrl);
        }
        */
        #endregion
    }
}