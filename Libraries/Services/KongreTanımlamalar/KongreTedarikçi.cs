using Core;
using Core.Data;
using Core.Domain.KongreTanımlama;
using Core.Önbellek;
using Services.Olaylar;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services.KongreTanımlama
{
    public class KongreTedarikçiServisi : IKongreTedarikçiServisi
    {
        private const string KONGREFİRMA_ALL_KEY = "kongreTedarikçi.all-";
        private const string KONGREFİRMA_BY_ID_KEY = "kongreTedarikçi.id-{0}";
        private const string KONGREFİRMA_PATTERN_KEY = "kongreTedarikçi.";
        private readonly IWorkContext _workContext;
        private readonly IOlayYayınlayıcı _olayYayınlayıcı;
        private readonly IÖnbellekYönetici _önbellekYönetici;
        private readonly IDepo<KongreTedarikçi> _kongreTedarikçiDepo;
        public KongreTedarikçiServisi(IDepo<KongreTedarikçi> kongreTedarikçiDepo,
        IWorkContext workContext,
        IOlayYayınlayıcı olayYayınlayıcı,
        IÖnbellekYönetici önbellekYönetici)
        {
            this._kongreTedarikçiDepo = kongreTedarikçiDepo;
            this._workContext = workContext;
            this._olayYayınlayıcı = olayYayınlayıcı;
            this._önbellekYönetici = önbellekYönetici;
        }
        public KongreTedarikçi KongreTedarikçiAlId(int kongreTedarikçiId)
        {
            if (kongreTedarikçiId == 0)
                return null;

            string key = string.Format(KONGREFİRMA_BY_ID_KEY, kongreTedarikçiId);
            return _önbellekYönetici.Al(key, () => _kongreTedarikçiDepo.AlId(kongreTedarikçiId));
        }

        public void KongreTedarikçiEkle(KongreTedarikçi kongreTedarikçi)
        {
            if (kongreTedarikçi == null)
                throw new ArgumentNullException("kongreTedarikçi");

            _kongreTedarikçiDepo.Ekle(kongreTedarikçi);
            _önbellekYönetici.KalıpİleSil(KONGREFİRMA_PATTERN_KEY);
            _olayYayınlayıcı.OlayEklendi(kongreTedarikçi);
        }

        public void KongreTedarikçiGüncelle(KongreTedarikçi kongreTedarikçi)
        {
            if (kongreTedarikçi == null)
                throw new ArgumentNullException("kongreTedarikçi");

            _kongreTedarikçiDepo.Güncelle(kongreTedarikçi);
            _önbellekYönetici.KalıpİleSil(KONGREFİRMA_PATTERN_KEY);
            _olayYayınlayıcı.OlayGüncellendi(kongreTedarikçi);
        }

        public void KongreTedarikçiSil(KongreTedarikçi kongreTedarikçi)
        {
            if (kongreTedarikçi == null)
                throw new ArgumentNullException("kongreTedarikçi");

            _kongreTedarikçiDepo.Sil(kongreTedarikçi);
            _önbellekYönetici.KalıpİleSil(KONGREFİRMA_PATTERN_KEY);
            _olayYayınlayıcı.OlaySilindi(kongreTedarikçi);
        }

        public IList<KongreTedarikçi> TümKongreTedarikçiAl()
        {
            string key = string.Format(KONGREFİRMA_ALL_KEY);
            return _önbellekYönetici.Al(key, () =>
            {
                var query = _kongreTedarikçiDepo.Tablo;
                return query.ToList();
            });
        }
    }

}
