using Services.Güvenlik;
using Services.Siteler;
using System.Linq;
using System.Web.Mvc;
using System;
using Services.Logging;
using Services.Konum;
using System.Collections.Generic;
using Web.Models.Güvenlik;
using Services.Kullanıcılar;
using Web.Models.Kullanıcılar;
using Core;
using Core.Domain.Kullanıcılar;

namespace Web.Controllers
{
    public class GüvenlikController : TemelPublicController
    {
        private readonly IİzinServisi _izinServisi;
        private readonly IKullanıcıİşlemServisi _kullanıcıİşlemServisi;
        private readonly ISiteServisi _siteServisi;
        private readonly IKonumServisi _konumServisi;
        private readonly IKullanıcıServisi _kullanıcıServisi;
        private readonly IWorkContext _workContext;


        public GüvenlikController(
            IİzinServisi izinServisi,
            ISiteServisi siteServisi,
            IKonumServisi konumServisi,
            IKullanıcıİşlemServisi kullanıcıİşlemServisi,
            IKullanıcıServisi kullanıcıServisi,
            IWorkContext workContext
            )
        {
            this._izinServisi = izinServisi;
            this._siteServisi = siteServisi;
            this._konumServisi = konumServisi;
            this._kullanıcıİşlemServisi = kullanıcıİşlemServisi;
            this._kullanıcıServisi = kullanıcıServisi;
            this._workContext = workContext;
        }

        public virtual ActionResult ErişimEngellendi(string pageUrl)
        {
            var currentCustomer = _workContext.MevcutKullanıcı;
            if (currentCustomer == null || currentCustomer.IsGuest())
            {
                return View();
            }

            return View();
        }
        public virtual ActionResult Izinler()
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.ACLYönet))
                return ErişimEngellendiView();

            var model = new IzinMappingModel();

            var izinKayıtları = _izinServisi.TümİzinKayıtlarınıAl();
            var kullanıcıRolleri = _kullanıcıServisi.TümKullanıcıRolleriniAl(true);
            foreach (var pr in izinKayıtları)
            {
                model.MevcutIzinler.Add(new IzinKaydıModel
                {
                    Adı = pr.Adı,
                    SistemAdı = pr.SistemAdı
                });
            }
            foreach (var cr in kullanıcıRolleri)
            {
                model.MevcutKullanıcıRolleri.Add(new KullanıcıRolModel
                {
                    Id = cr.Id,
                    Adı = cr.Adı
                });
            }
            foreach (var pr in izinKayıtları)
                foreach (var cr in kullanıcıRolleri)
                {
                    bool allowed = pr.KullanıcıRolleri.Count(x => x.Id == cr.Id) > 0;
                    if (!model.Izinli.ContainsKey(pr.SistemAdı))
                        model.Izinli[pr.SistemAdı] = new Dictionary<int, bool>();
                    model.Izinli[pr.SistemAdı][cr.Id] = allowed;
                }

            return View(model);
        }

        [HttpPost, ActionName("Izinler")]
        public virtual ActionResult IzinKaydet(FormCollection form)
        {
            if (!_izinServisi.YetkiVer(StandartİzinSağlayıcı.ACLYönet))
                return ErişimEngellendiView();

            var izinKayıtları = _izinServisi.TümİzinKayıtlarınıAl();
            var kullanıcıRolleri = _kullanıcıServisi.TümKullanıcıRolleriniAl(true);


            foreach (var cr in kullanıcıRolleri)
            {
                string formKey = "allow_" + cr.Id;
                var permissionRecordSystemNamesToRestrict = form[formKey] != null ? form[formKey].Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList() : new List<string>();

                foreach (var pr in izinKayıtları)
                {

                    bool allow = permissionRecordSystemNamesToRestrict.Contains(pr.SistemAdı);
                    if (allow)
                    {
                        if (pr.KullanıcıRolleri.FirstOrDefault(x => x.Id == cr.Id) == null)
                        {
                            pr.KullanıcıRolleri.Add(cr);
                            _izinServisi.İzinKaydıGüncelle(pr);
                        }
                    }
                    else
                    {
                        if (pr.KullanıcıRolleri.FirstOrDefault(x => x.Id == cr.Id) != null)
                        {
                            pr.KullanıcıRolleri.Remove(cr);
                            _izinServisi.İzinKaydıGüncelle(pr);
                        }
                    }
                }
            }

            BaşarılıBildirimi("Güncellendi");
            return RedirectToAction("Izinler");
        }
    }
}