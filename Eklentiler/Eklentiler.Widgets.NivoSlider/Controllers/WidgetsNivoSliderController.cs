using System.Linq;
using System.Web.Mvc;
using Core;
using Core.Önbellek;
using Eklentiler.Widgets.NivoSlider.Altyapı.Önbellek;
using Eklentiler.Widgets.NivoSlider.Models;
using Services.Yapılandırma;
using Services.Siteler;
using Web.Framework.Controllers;
using Services.Medya;

namespace Eklentiler.Widgets.NivoSlider.Controllers
{
    public class WidgetsNivoSliderController : TemelEklentiController
    {
        private readonly IWorkContext _workContext;
        private readonly ISiteContext _siteContext;
        private readonly ISiteServisi _siteService;
        private readonly IResimServisi _resimServisi;
        private readonly IAyarlarServisi _ayarlarServisi;
        private readonly IÖnbellekYönetici _önbellekYönetici;

        public WidgetsNivoSliderController(IWorkContext workContext,
            ISiteContext siteContext,
            ISiteServisi siteService,
            IResimServisi resimServisi,
            IAyarlarServisi ayarlarServisi,
            IÖnbellekYönetici önbellekYönetici)
        {
            this._workContext = workContext;
            this._siteContext = siteContext;
            this._siteService = siteService;
            this._resimServisi = resimServisi;
            this._ayarlarServisi = ayarlarServisi;
            this._önbellekYönetici = önbellekYönetici;
        }

        protected string ResimUrlAl(int resimId)
        {
            string cacheKey = string.Format(ModelÖnbellekOlayTüketici.RESIM_URL_MODEL_KEY, resimId);
            return _önbellekYönetici.Al(cacheKey, () =>
            {
                var url = _resimServisi.ResimUrlAl(resimId, varsayılanResimGöster: false);
                //null değerler önbelleklenmez
                if (url == null)
                    url = "";

                return url;
            });
        }

        //[AdminAuthorize]
        [ChildActionOnly]
        public ActionResult Configure()
        {
            //load settings for a chosen store scope
            var storeScope = this.AktifSiteKapsamYapılandırmaAl(_siteService, _workContext);
            var nivoSliderSettings = _ayarlarServisi.AyarYükle<NivoSliderSettings>(storeScope);
            var model = new ConfigurationModel();
            model.Picture1Id = nivoSliderSettings.Picture1Id;
            model.Text1 = nivoSliderSettings.Text1;
            model.Link1 = nivoSliderSettings.Link1;
            model.Picture2Id = nivoSliderSettings.Picture2Id;
            model.Text2 = nivoSliderSettings.Text2;
            model.Link2 = nivoSliderSettings.Link2;
            model.Picture3Id = nivoSliderSettings.Picture3Id;
            model.Text3 = nivoSliderSettings.Text3;
            model.Link3 = nivoSliderSettings.Link3;
            model.Picture4Id = nivoSliderSettings.Picture4Id;
            model.Text4 = nivoSliderSettings.Text4;
            model.Link4 = nivoSliderSettings.Link4;
            model.Picture5Id = nivoSliderSettings.Picture5Id;
            model.Text5 = nivoSliderSettings.Text5;
            model.Link5 = nivoSliderSettings.Link5;
            model.ActiveStoreScopeConfiguration = storeScope;
            if (storeScope > 0)
            {
                model.Picture1Id_OverrideForStore = _ayarlarServisi.AyarlarMevcut(nivoSliderSettings, x => x.Picture1Id, storeScope);
                model.Text1_OverrideForStore = _ayarlarServisi.AyarlarMevcut(nivoSliderSettings, x => x.Text1, storeScope);
                model.Link1_OverrideForStore = _ayarlarServisi.AyarlarMevcut(nivoSliderSettings, x => x.Link1, storeScope);
                model.Picture2Id_OverrideForStore = _ayarlarServisi.AyarlarMevcut(nivoSliderSettings, x => x.Picture2Id, storeScope);
                model.Text2_OverrideForStore = _ayarlarServisi.AyarlarMevcut(nivoSliderSettings, x => x.Text2, storeScope);
                model.Link2_OverrideForStore = _ayarlarServisi.AyarlarMevcut(nivoSliderSettings, x => x.Link2, storeScope);
                model.Picture3Id_OverrideForStore = _ayarlarServisi.AyarlarMevcut(nivoSliderSettings, x => x.Picture3Id, storeScope);
                model.Text3_OverrideForStore = _ayarlarServisi.AyarlarMevcut(nivoSliderSettings, x => x.Text3, storeScope);
                model.Link3_OverrideForStore = _ayarlarServisi.AyarlarMevcut(nivoSliderSettings, x => x.Link3, storeScope);
                model.Picture4Id_OverrideForStore = _ayarlarServisi.AyarlarMevcut(nivoSliderSettings, x => x.Picture4Id, storeScope);
                model.Text4_OverrideForStore = _ayarlarServisi.AyarlarMevcut(nivoSliderSettings, x => x.Text4, storeScope);
                model.Link4_OverrideForStore = _ayarlarServisi.AyarlarMevcut(nivoSliderSettings, x => x.Link4, storeScope);
                model.Picture5Id_OverrideForStore = _ayarlarServisi.AyarlarMevcut(nivoSliderSettings, x => x.Picture5Id, storeScope);
                model.Text5_OverrideForStore = _ayarlarServisi.AyarlarMevcut(nivoSliderSettings, x => x.Text5, storeScope);
                model.Link5_OverrideForStore = _ayarlarServisi.AyarlarMevcut(nivoSliderSettings, x => x.Link5, storeScope);
            }

            return View("~/Eklentiler/Widgets.NivoSlider/Views/Configure.cshtml", model);
        }

        [HttpPost]
        //[AdminAuthorize]
        [ChildActionOnly]
        public ActionResult Configure(ConfigurationModel model)
        {
            //load settings for a chosen store scope
            var storeScope = this.AktifSiteKapsamYapılandırmaAl(_siteService, _workContext);
            var nivoSliderSettings = _ayarlarServisi.AyarYükle<NivoSliderSettings>(storeScope);

            //get previous picture identifiers
            var previousPictureIds = new[]
            {
                nivoSliderSettings.Picture1Id,
                nivoSliderSettings.Picture2Id,
                nivoSliderSettings.Picture3Id,
                nivoSliderSettings.Picture4Id,
                nivoSliderSettings.Picture5Id
            };

            nivoSliderSettings.Picture1Id = model.Picture1Id;
            nivoSliderSettings.Text1 = model.Text1;
            nivoSliderSettings.Link1 = model.Link1;
            nivoSliderSettings.Picture2Id = model.Picture2Id;
            nivoSliderSettings.Text2 = model.Text2;
            nivoSliderSettings.Link2 = model.Link2;
            nivoSliderSettings.Picture3Id = model.Picture3Id;
            nivoSliderSettings.Text3 = model.Text3;
            nivoSliderSettings.Link3 = model.Link3;
            nivoSliderSettings.Picture4Id = model.Picture4Id;
            nivoSliderSettings.Text4 = model.Text4;
            nivoSliderSettings.Link4 = model.Link4;
            nivoSliderSettings.Picture5Id = model.Picture5Id;
            nivoSliderSettings.Text5 = model.Text5;
            nivoSliderSettings.Link5 = model.Link5;

            /* We do not clear cache after each setting update.
             * This behavior can increase performance because cached settings will not be cleared 
             * and loaded from database after each update */
            _ayarlarServisi.İptalEdilebilirAyarKaydet(nivoSliderSettings, x => x.Picture1Id, model.Picture1Id_OverrideForStore, storeScope, false);
            _ayarlarServisi.İptalEdilebilirAyarKaydet(nivoSliderSettings, x => x.Text1, model.Text1_OverrideForStore, storeScope, false);
            _ayarlarServisi.İptalEdilebilirAyarKaydet(nivoSliderSettings, x => x.Link1, model.Link1_OverrideForStore, storeScope, false);
            _ayarlarServisi.İptalEdilebilirAyarKaydet(nivoSliderSettings, x => x.Picture2Id, model.Picture2Id_OverrideForStore, storeScope, false);
            _ayarlarServisi.İptalEdilebilirAyarKaydet(nivoSliderSettings, x => x.Text2, model.Text2_OverrideForStore, storeScope, false);
            _ayarlarServisi.İptalEdilebilirAyarKaydet(nivoSliderSettings, x => x.Link2, model.Link2_OverrideForStore, storeScope, false);
            _ayarlarServisi.İptalEdilebilirAyarKaydet(nivoSliderSettings, x => x.Picture3Id, model.Picture3Id_OverrideForStore, storeScope, false);
            _ayarlarServisi.İptalEdilebilirAyarKaydet(nivoSliderSettings, x => x.Text3, model.Text3_OverrideForStore, storeScope, false);
            _ayarlarServisi.İptalEdilebilirAyarKaydet(nivoSliderSettings, x => x.Link3, model.Link3_OverrideForStore, storeScope, false);
            _ayarlarServisi.İptalEdilebilirAyarKaydet(nivoSliderSettings, x => x.Picture4Id, model.Picture4Id_OverrideForStore, storeScope, false);
            _ayarlarServisi.İptalEdilebilirAyarKaydet(nivoSliderSettings, x => x.Text4, model.Text4_OverrideForStore, storeScope, false);
            _ayarlarServisi.İptalEdilebilirAyarKaydet(nivoSliderSettings, x => x.Link4, model.Link4_OverrideForStore, storeScope, false);
            _ayarlarServisi.İptalEdilebilirAyarKaydet(nivoSliderSettings, x => x.Picture5Id, model.Picture5Id_OverrideForStore, storeScope, false);
            _ayarlarServisi.İptalEdilebilirAyarKaydet(nivoSliderSettings, x => x.Text5, model.Text5_OverrideForStore, storeScope, false);
            _ayarlarServisi.İptalEdilebilirAyarKaydet(nivoSliderSettings, x => x.Link5, model.Link5_OverrideForStore, storeScope, false);

            //now clear settings cache
            _ayarlarServisi.ÖnbelleğiTemizle();

            //get current picture identifiers
            var currentPictureIds = new[]
            {
                nivoSliderSettings.Picture1Id,
                nivoSliderSettings.Picture2Id,
                nivoSliderSettings.Picture3Id,
                nivoSliderSettings.Picture4Id,
                nivoSliderSettings.Picture5Id
            };

            //delete an old picture (if deleted or updated)
            foreach (var pictureId in previousPictureIds.Except(currentPictureIds))
            {
                var previousPicture = _resimServisi.ResimAlId(pictureId);
                if (previousPicture != null)
                    _resimServisi.ResimSil(previousPicture);
            }

            BaşarılıBildirimi("Saved");
            return Configure();
        }

        //[ChildActionOnly]
        public ActionResult PublicInfo(string widgetZone, object additionalData = null)
        {
            _önbellekYönetici.Temizle();
            var nivoSliderSettings = _ayarlarServisi.AyarYükle<NivoSliderSettings>(_siteContext.MevcutSite.Id);

            var model = new PublicInfoModel();
            model.Picture1Url = ResimUrlAl(nivoSliderSettings.Picture1Id);
            model.Text1 = nivoSliderSettings.Text1;
            model.Link1 = nivoSliderSettings.Link1;

            model.Picture2Url = ResimUrlAl(nivoSliderSettings.Picture2Id);
            model.Text2 = nivoSliderSettings.Text2;
            model.Link2 = nivoSliderSettings.Link2;

            model.Picture3Url = ResimUrlAl(nivoSliderSettings.Picture3Id);
            model.Text3 = nivoSliderSettings.Text3;
            model.Link3 = nivoSliderSettings.Link3;

            model.Picture4Url = ResimUrlAl(nivoSliderSettings.Picture4Id);
            model.Text4 = nivoSliderSettings.Text4;
            model.Link4 = nivoSliderSettings.Link4;

            model.Picture5Url = ResimUrlAl(nivoSliderSettings.Picture5Id);
            model.Text5 = nivoSliderSettings.Text5;
            model.Link5 = nivoSliderSettings.Link5;

            if (string.IsNullOrEmpty(model.Picture1Url) && string.IsNullOrEmpty(model.Picture2Url) &&
                string.IsNullOrEmpty(model.Picture3Url) && string.IsNullOrEmpty(model.Picture4Url) &&
                string.IsNullOrEmpty(model.Picture5Url))
                //no pictures uploaded
                return Content("");


            return View("~/Eklentiler/Widgets.NivoSlider/Views/PublicInfo.cshtml", model);
        }
    }
}