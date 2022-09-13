using Core;
using Core.Data;
using Core.Domain.EkTanımlamalar;
using Core.Önbellek;
using Services.Olaylar;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services.EkTanımlamalar
{
    public class GelirGiderTanımlamaServisi : IGelirGiderTanımlamaServisi
    {
        private const string GELİRGİDERTANIMLAMA_ALL_KEY = "gelirGiderTanımlama.all";
        private const string GELİRGİDERTANIMLAMA_BY_ID_KEY = "gelirGiderTanımlama.id-{0}";
        private const string GELİRGİDERTANIMLAMA_PATTERN_KEY = "gelirGiderTanımlama.";
        private readonly IWorkContext _workContext;
        private readonly IOlayYayınlayıcı _olayYayınlayıcı;
        private readonly IÖnbellekYönetici _önbellekYönetici;
        private readonly IDepo<GelirGiderTanımlama> _gelirGiderTanımlamaDepo;
        public GelirGiderTanımlamaServisi(IDepo<GelirGiderTanımlama> gelirGiderTanımlamaDepo,
        IWorkContext workContext,
        IOlayYayınlayıcı olayYayınlayıcı,
        IÖnbellekYönetici önbellekYönetici)
        {
            this._gelirGiderTanımlamaDepo = gelirGiderTanımlamaDepo;
            this._workContext = workContext;
            this._olayYayınlayıcı = olayYayınlayıcı;
            this._önbellekYönetici = önbellekYönetici;
        }
        public GelirGiderTanımlama GelirGiderTanımlamaAlId(int gelirGiderTanımlamaId)
        {
            if (gelirGiderTanımlamaId == 0)
                return null;

            string key = string.Format(GELİRGİDERTANIMLAMA_BY_ID_KEY, gelirGiderTanımlamaId);
            return _önbellekYönetici.Al(key, () => _gelirGiderTanımlamaDepo.AlId(gelirGiderTanımlamaId));
        }

        public void GelirGiderTanımlamaEkle(GelirGiderTanımlama gelirGiderTanımlama)
        {
            if (gelirGiderTanımlama == null)
                throw new ArgumentNullException("gelirGiderTanımlama");

            _gelirGiderTanımlamaDepo.Ekle(gelirGiderTanımlama);
            _önbellekYönetici.KalıpİleSil(GELİRGİDERTANIMLAMA_PATTERN_KEY);
            _olayYayınlayıcı.OlayEklendi(gelirGiderTanımlama);
        }

        public void GelirGiderTanımlamaGüncelle(GelirGiderTanımlama gelirGiderTanımlama)
        {
            if (gelirGiderTanımlama == null)
                throw new ArgumentNullException("gelirGiderTanımlama");

            _gelirGiderTanımlamaDepo.Güncelle(gelirGiderTanımlama);
            _önbellekYönetici.KalıpİleSil(GELİRGİDERTANIMLAMA_PATTERN_KEY);
            _olayYayınlayıcı.OlayGüncellendi(gelirGiderTanımlama);
        }

        public void GelirGiderTanımlamaSil(GelirGiderTanımlama gelirGiderTanımlama)
        {
            if (gelirGiderTanımlama == null)
                throw new ArgumentNullException("gelirGiderTanımlama");

            _gelirGiderTanımlamaDepo.Sil(gelirGiderTanımlama);
            _önbellekYönetici.KalıpİleSil(GELİRGİDERTANIMLAMA_PATTERN_KEY);
            _olayYayınlayıcı.OlaySilindi(gelirGiderTanımlama);
        }

        public IList<GelirGiderTanımlama> TümGelirGiderTanımlamaAl()
        {
            var query = _gelirGiderTanımlamaDepo.Tablo;
            return query.ToList();
        }
        public IList<GelirGiderTanımlama> AnaTeklifKalemleriAl(bool gelir)
        {
            string key = string.Format(GELİRGİDERTANIMLAMA_ALL_KEY);
            return _önbellekYönetici.Al(key, () =>
            {
                if (gelir)
                {
                    var query = from ft in _gelirGiderTanımlamaDepo.Tablo
                                where ft.Anabaşlık == true && ft.Gelir==true
                                select ft;
                    return query.ToList();
                }
                else
                {
                    var query = from ft in _gelirGiderTanımlamaDepo.Tablo
                                where ft.NodeId.HasValue == false && ft.Gelir == false
                                select ft;
                    return query.ToList();
                }
            });
        }
    }

}
