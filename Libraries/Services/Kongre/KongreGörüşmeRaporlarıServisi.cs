using Core;
using Core.Data;
using Core.Domain.Kongre;
using Core.Önbellek;
using Services.Olaylar;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services.Kongre
{
    public class KongreGörüşmeRaporlarıServisi : IKongreGörüşmeRaporlarıServisi
    {
        private const string KONGREGÖRÜŞMERAPORLARI_ALL_KEY = "kongreGörüşmeRaporları.all-";
        private const string KONGREGÖRÜŞMERAPORLARI_BY_ID_KEY = "kongreGörüşmeRaporları.id-{0}";
        private const string KONGREGÖRÜŞMERAPORLARI_BY_KONGRE_KEY = "kongreGörüşmeRaporları.kongreid-{0}-musteriId-{1}";
        private const string KONGREGÖRÜŞMERAPORLARI_PATTERN_KEY = "kongreGörüşmeRaporları.";
        private readonly IWorkContext _workContext;
        private readonly IOlayYayınlayıcı _olayYayınlayıcı;
        private readonly IÖnbellekYönetici _önbellekYönetici;
        private readonly IDepo<KongreGörüşmeRaporları> _kongreGörüşmeRaporlarıDepo;
        public KongreGörüşmeRaporlarıServisi(IDepo<KongreGörüşmeRaporları> kongreGörüşmeRaporlarıDepo,
        IWorkContext workContext,
        IOlayYayınlayıcı olayYayınlayıcı,
        IÖnbellekYönetici önbellekYönetici)
        {
            this._kongreGörüşmeRaporlarıDepo = kongreGörüşmeRaporlarıDepo;
            this._workContext = workContext;
            this._olayYayınlayıcı = olayYayınlayıcı;
            this._önbellekYönetici = önbellekYönetici;
        }
        public KongreGörüşmeRaporları KongreGörüşmeRaporlarıAlId(int kongreGörüşmeRaporlarıId)
        {
            if (kongreGörüşmeRaporlarıId == 0)
                return null;

            string key = string.Format(KONGREGÖRÜŞMERAPORLARI_BY_ID_KEY, kongreGörüşmeRaporlarıId);
            return _önbellekYönetici.Al(key, () => _kongreGörüşmeRaporlarıDepo.AlId(kongreGörüşmeRaporlarıId));
        }
         public KongreGörüşmeRaporları KongreGörüşmeRaporlarıKongreId(int kongreId, int sponsorId)
        {
            if (kongreId == 0)
                return null;
            if (sponsorId == 0)
                return null;

            string key = string.Format(KONGREGÖRÜŞMERAPORLARI_BY_KONGRE_KEY,kongreId,sponsorId);
            return _önbellekYönetici.Al(key, () =>
            {
                var query = _kongreGörüşmeRaporlarıDepo.Tablo;
                return query.Where(x=>x.KongreId==kongreId&&x.MusteriId==sponsorId).FirstOrDefault();
            });
        }

        public void KongreGörüşmeRaporlarıEkle(KongreGörüşmeRaporları kongreGörüşmeRaporları)
        {
            if (kongreGörüşmeRaporları == null)
                throw new ArgumentNullException("kongreGörüşmeRaporları");

            _kongreGörüşmeRaporlarıDepo.Ekle(kongreGörüşmeRaporları);
            _önbellekYönetici.KalıpİleSil(KONGREGÖRÜŞMERAPORLARI_PATTERN_KEY);
            _olayYayınlayıcı.OlayEklendi(kongreGörüşmeRaporları);
        }

        public void KongreGörüşmeRaporlarıGüncelle(KongreGörüşmeRaporları kongreGörüşmeRaporları)
        {
            if (kongreGörüşmeRaporları == null)
                throw new ArgumentNullException("kongreGörüşmeRaporları");

            _kongreGörüşmeRaporlarıDepo.Güncelle(kongreGörüşmeRaporları);
            _önbellekYönetici.KalıpİleSil(KONGREGÖRÜŞMERAPORLARI_PATTERN_KEY);
            _olayYayınlayıcı.OlayGüncellendi(kongreGörüşmeRaporları);
        }

        public void KongreGörüşmeRaporlarıSil(KongreGörüşmeRaporları kongreGörüşmeRaporları)
        {
            if (kongreGörüşmeRaporları == null)
                throw new ArgumentNullException("kongreGörüşmeRaporları");

            _kongreGörüşmeRaporlarıDepo.Sil(kongreGörüşmeRaporları);
            _önbellekYönetici.KalıpİleSil(KONGREGÖRÜŞMERAPORLARI_PATTERN_KEY);
            _olayYayınlayıcı.OlaySilindi(kongreGörüşmeRaporları);
        }

        public IList<KongreGörüşmeRaporları> TümKongreGörüşmeRaporlarıAl()
        {
            string key = string.Format(KONGREGÖRÜŞMERAPORLARI_ALL_KEY);
            return _önbellekYönetici.Al(key, () =>
            {
                var query = _kongreGörüşmeRaporlarıDepo.Tablo;
                return query.ToList();
            });
        }
    }

}
