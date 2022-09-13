using Services.Güvenlik;
using Services.Siteler;
using System.Linq;
using System.Web.Mvc;
using Web.Framework.Controllers;
using Web.Framework.Kendoui;
using Web.Uzantılar;
using Services.Logging;
using Services.Konum;
using Web.Models.Test;
using Services.Testler;
using Services.Tanımlamalar;
using Core.Domain.Test;
using Services.Klasör;
using Core;
using Services.Kullanıcılar;
using Services.DovizServisi;
using Services.Notlar;

namespace Web.Controllers
{
    public class TestController : TemelPublicController
    {
        private readonly IİzinServisi _izinServisi;
        private readonly IKullanıcıİşlemServisi _kullanıcıİşlemServisi;
        private readonly ISiteServisi _siteServisi;
        private readonly IKonumServisi _konumServisi;
        private readonly ITestServisi _testServisi;
        private readonly IPdfServisi _pdfServisi;
        private readonly IKullanıcıServisi _kullanıcıServisi;
        private readonly IDovizServisi _dovizServisi;
        private readonly IWorkContext _workContext;
        private readonly INotServisi _notServisi;
        public TestController(IİzinServisi izinServisi,
            ISiteServisi siteServisi,
            IKonumServisi konumServisi,
            IKullanıcıİşlemServisi kullanıcıİşlemServisi,
            ITestServisi testServisi,
            IPdfServisi pdfServisi,
            IKullanıcıServisi kullanıcıServisi,
            IDovizServisi dovizServisi,
            IWorkContext workContext,
            INotServisi notServisi)
        {
            this._izinServisi = izinServisi;
            this._siteServisi = siteServisi;
            this._konumServisi = konumServisi;
            this._kullanıcıİşlemServisi = kullanıcıİşlemServisi;
            this._testServisi = testServisi;
            this._pdfServisi = pdfServisi;
            this._kullanıcıServisi = kullanıcıServisi;
            this._dovizServisi = dovizServisi;
            this._notServisi = notServisi;
            this._workContext = workContext;
        }
    
        #region Test
        public virtual ActionResult TestListe()
        {
            var model = new TestModel();
            return View(model);
        }

        [HttpPost]
        public virtual ActionResult TestListe(DataSourceİsteği command, TestModel model)
        {
            var TestModels = _testServisi.TümTestAl(true);

            
            var gridModel = new DataSourceSonucu
            {
                Data = TestModels.Select(x =>
                {
                    var TestModel = x.ToModel();
                    return TestModel;
                }),
                Toplam = TestModels.Count
            };

            return Json(gridModel);
        }
        
        public virtual ActionResult TestEkle()
        {

            var model = new TestModel();
            return View(model);
        }

        [HttpPost, FormAdıParametresi("kaydet-devam", "düzenlemeyeDevam")]
        public virtual ActionResult TestEkle(TestModel model, bool düzenlemeyeDevam,string Durumu="Hazırlık")
        {
            
            if (ModelState.IsValid)
            {
                var Test = model.ToEntity();
                _testServisi.TestEkle(Test);
                var test = _testServisi.TestAlId(Test.Id);
                BaşarılıBildirimi("Testbaşarıyla Eklendi");
                _kullanıcıİşlemServisi.İşlemEkle("YeniTestEklendi", "Yeni Test Eklendi", Test.Id);
                return RedirectToAction("TestDüzenle", new { id = Test.Id });
            }
            return View(model);
        }
        public virtual ActionResult TestDüzenle(int id)
        {
            var Test = _testServisi.TestAlId(id);
            if (Test == null)
            {
                return RedirectToAction("TestListe");
            }
            var model = Test.ToModel();
            return View(model);
        }
        [HttpPost, FormAdıParametresi("kaydet-devam", "düzenlemeyeDevam")]
        public virtual ActionResult TestDüzenle(TestModel model, bool düzenlemeyeDevam)
        {
            var Test = _testServisi.TestAlId(model.Id);
            if (Test == null)
            {
                return RedirectToAction("TestListe");
            }
            if (ModelState.IsValid)
            {
                Test = model.ToEntity(Test);
                _testServisi.TestGüncelle(Test);
                BaşarılıBildirimi("Test başarıyla güncellenmiştir.");
                _kullanıcıİşlemServisi.İşlemEkle("TestGüncelle", "Test güncellendi", Test.Id);
                if (düzenlemeyeDevam)
                {
                    return RedirectToAction("TestDüzenle", new { id = Test.Id });
                }
                return RedirectToAction("TestDüzenle", new { id = Test.Id });
            }
            return View(model);
        }

        [HttpPost]
        public virtual ActionResult TestSil(TestModel model)
        {

            var Test = _testServisi.TestAlId(model.Id);
            if (Test == null)
                return RedirectToAction("TestListe");
            _testServisi.TestSil(Test);
            BaşarılıBildirimi("Test başarıyla silindi");
            _kullanıcıİşlemServisi.İşlemEkle("TestSil", "Test silindi", Test.Id);
            return RedirectToAction("TestListe");
        }

        public virtual ActionResult TestKopyala(int testId,string durumu)
        {

            var Test = _testServisi.TestAlId(testId);
        
            var test = _testServisi.TestAlId(Test.Id);
            BaşarılıBildirimi("Testbaşarıyla Eklendi");
            _kullanıcıİşlemServisi.İşlemEkle("YeniTestEklendi", "Yeni Test Eklendi", Test.Id);
            return RedirectToAction("TestDüzenle", new { id = Test.Id });

        }
        #endregion

    }
}