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
    public class KayıtTipiServisi : IKayıtTipiServisi
    {
        private const string KAYITTİPİ_ALL_KEY = "kayıtTipi.all-";
        private const string KAYITTİPİ_BY_ID_KEY = "kayıtTipi.id-{0}";
        private const string KAYITTİPİ_PATTERN_KEY = "kayıtTipi.";
        private readonly IWorkContext _workContext;
        private readonly IOlayYayınlayıcı _olayYayınlayıcı;
        private readonly IÖnbellekYönetici _önbellekYönetici;
        private readonly IDepo<KayıtTipi> _kayıtTipiDepo;
        public KayıtTipiServisi(IDepo<KayıtTipi> kayıtTipiDepo,
        IWorkContext workContext,
        IOlayYayınlayıcı olayYayınlayıcı,
        IÖnbellekYönetici önbellekYönetici)
        {
            this._kayıtTipiDepo = kayıtTipiDepo;
            this._workContext = workContext;
            this._olayYayınlayıcı = olayYayınlayıcı;
            this._önbellekYönetici = önbellekYönetici;
        }
        public KayıtTipi KayıtTipiAlId(int kayıtTipiId)
        {
            if (kayıtTipiId == 0)
                return null;

            string key = string.Format(KAYITTİPİ_BY_ID_KEY, kayıtTipiId);
            return _önbellekYönetici.Al(key, () => _kayıtTipiDepo.AlId(kayıtTipiId));
        }

        public void KayıtTipiEkle(KayıtTipi kayıtTipi)
        {
            if (kayıtTipi == null)
                throw new ArgumentNullException("kayıtTipi");

            _kayıtTipiDepo.Ekle(kayıtTipi);
            _önbellekYönetici.KalıpİleSil(KAYITTİPİ_PATTERN_KEY);
            _olayYayınlayıcı.OlayEklendi(kayıtTipi);
        }

        public void KayıtTipiGüncelle(KayıtTipi kayıtTipi)
        {
            if (kayıtTipi == null)
                throw new ArgumentNullException("kayıtTipi");

            _kayıtTipiDepo.Güncelle(kayıtTipi);
            _önbellekYönetici.KalıpİleSil(KAYITTİPİ_PATTERN_KEY);
            _olayYayınlayıcı.OlayGüncellendi(kayıtTipi);
        }

        public void KayıtTipiSil(KayıtTipi kayıtTipi)
        {
            if (kayıtTipi == null)
                throw new ArgumentNullException("kayıtTipi");

            _kayıtTipiDepo.Sil(kayıtTipi);
            _önbellekYönetici.KalıpİleSil(KAYITTİPİ_PATTERN_KEY);
            _olayYayınlayıcı.OlaySilindi(kayıtTipi);
        }

        public IList<KayıtTipi> TümKayıtTipiAl()
        {
            string key = string.Format(KAYITTİPİ_ALL_KEY);
            return _önbellekYönetici.Al(key, () =>
            {
                var query = _kayıtTipiDepo.Tablo;
                return query.ToList();
            });
        }
    }

}
