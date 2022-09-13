using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Core;
using Core.Domain.Kullanıcılar;
using Core.Eklentiler;
using Services.Finans;
using Services.Güvenlik;
using Services.Kongre;
using Services.Kullanıcılar;
using Services.Teklifler;
using Web.Framework.Kendoui;
using Web.Models.Home;

namespace Web.Controllers
{

    public class HomeController : TemelPublicController
    {
        private readonly IEklentiBulucu _eklentiBulucu;
        private readonly IİzinServisi _izinServisi;
        private readonly IWorkContext _workContext;
        private readonly IKullanıcıServisi _kullanıcıServisi;
        private readonly ITeklifServisi _teklifServisi;
        private readonly IOdemeFormuServisi _odemeFormuServisi;
        private readonly IKongreServisi _kongreServisi;
        public HomeController(IEklentiBulucu eklentiBulucu,
            IİzinServisi izinServisi,
            IWorkContext _workContext,
            IKullanıcıServisi kullanıcıServisi,
            ITeklifServisi teklifServisi,
            IOdemeFormuServisi odemeFormuServisi,
            IKongreServisi kongreServisi)
        {
            this._eklentiBulucu = eklentiBulucu;
            this._izinServisi = izinServisi;
            this._workContext = _workContext;
            this._kullanıcıServisi = kullanıcıServisi;
            this._teklifServisi = teklifServisi;
            this._odemeFormuServisi = odemeFormuServisi;
            this._kongreServisi = kongreServisi;
        }

        public ActionResult Index()
        {
            /*try
            {
                string sistemAdı = "Widgets.NivoSlider";
                var eklentiTanımlayıcı = _eklentiBulucu.EklentiTanımlayıcıAlSistemAdı(sistemAdı, EklentiModuYükle.Tümü);
                eklentiTanımlayıcı.Model().Yükle();
            }
            catch (Exception)
            {
                
            }*/
            return View();
        }
        [ChildActionOnly]
        public virtual ActionResult GenelIstatistikler()
        {
  
            var model = new GenelIstatistiklerModel();
            /*
            model.KongreSayısı = _kongreServisi.TümKongrelerAl(
                pageIndex: 0,
                pageSize: 1).TotalCount;
*/
            model.KullanıcıSayısı = _kullanıcıServisi.TümKullanıcılarıAl(
                kullanıcıRolIdleri: new[] { _kullanıcıServisi.KullanıcıRolüAlSistemAdı(SistemKullanıcıRolAdları.Kayıtlı).Id },
                sayfaIndeksi: 0,
                sayfaBüyüklüğü: 1).TotalCount;
            
            model.TeklifSayısı = _teklifServisi.TümTeklifAl().Count;

            model.OdemeFormuSayısı = _odemeFormuServisi.TümOdemeFormuAl().Count;

            return PartialView(model);
        }
        public virtual ActionResult TeklifTakvimi()
        {

            var model = new TeklifTakvimiModel();
         
            return PartialView(model);
        }
        [HttpPost]
        public virtual JsonResult TeklifTakvimiAl()
        {
            var teklif = _teklifServisi.TümTeklifAl();
            List<TeklifTakvimiModel> list = new List<TeklifTakvimiModel>();
            foreach(var t in teklif)
            {
                TeklifTakvimiModel tt = new TeklifTakvimiModel
                {
                    TaskID = t.Id,
                    Title = t.Adı,
                    Start = t.BaslamaTarihi,
                    End=t.BitisTarihi,
                    StartTimezone = "Etc/UTC",
                    EndTimezone = "Etc/UTC",
                    Description = t.Aciklama,
                };
                list.Add(tt);
            }
            
            return Json(list.ToList());
        }
    }
}
