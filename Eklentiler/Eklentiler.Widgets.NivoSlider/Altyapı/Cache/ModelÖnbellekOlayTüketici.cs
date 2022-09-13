using Core.Önbellek;
using Core.Domain.Yapılandırma;
using Core.Olaylar;
using Core.Altyapı;
using Services.Olaylar;


namespace Eklentiler.Widgets.NivoSlider.Altyapı.Önbellek
{
    public partial class ModelÖnbellekOlayTüketici
    {
        public const string RESIM_URL_MODEL_KEY = "ts.eklentiler.widgets.nivosrlider.resimurl-{0}";
        public const string RESIM_URL_PATTERN_KEY = "ts.eklentiler.widgets.nivosrlider";

        private readonly IÖnbellekYönetici _önbellekYönetici;

        public ModelÖnbellekOlayTüketici()
        {
            this._önbellekYönetici = EngineContext.Current.ContainerManager.Resolve<IÖnbellekYönetici>("ts_cache_static");
        }

        public void HandleEvent(OlayEklendi<Ayarlar> olayMesajı)
        {
            _önbellekYönetici.KalıpİleSil(RESIM_URL_PATTERN_KEY);
        }
        public void HandleEvent(OlayGüncellendi<Ayarlar> olayMesajı)
        {
            _önbellekYönetici.KalıpİleSil(RESIM_URL_PATTERN_KEY);
        }
        public void HandleEvent(OlaySilindi<Ayarlar> olayMesajı)
        {
            _önbellekYönetici.KalıpİleSil(RESIM_URL_PATTERN_KEY);
        }
    }
}
