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
    public class SponsorlukKalemleriServisi : ISponsorlukKalemleriServisi
    {
        private const string SPONSORLUKKALEMLERİ_ALL_KEY = "sponsorlukKalemleri.all";
        private const string SPONSORLUKKALEMLERİ_BY_ID_KEY = "sponsorlukKalemleri.id-{0}";
        private const string SPONSORLUKKALEMLERİ_PATTERN_KEY = "sponsorlukKalemleri.";
        private readonly IWorkContext _workContext;
        private readonly IOlayYayınlayıcı _olayYayınlayıcı;
        private readonly IÖnbellekYönetici _önbellekYönetici;
        private readonly IDepo<SponsorlukKalemleri> _sponsorlukKalemleriDepo;
        public SponsorlukKalemleriServisi(IDepo<SponsorlukKalemleri> sponsorlukKalemleriDepo,
        IWorkContext workContext,
        IOlayYayınlayıcı olayYayınlayıcı,
        IÖnbellekYönetici önbellekYönetici)
        {
            this._sponsorlukKalemleriDepo = sponsorlukKalemleriDepo;
            this._workContext = workContext;
            this._olayYayınlayıcı = olayYayınlayıcı;
            this._önbellekYönetici = önbellekYönetici;
        }
        public SponsorlukKalemleri SponsorlukKalemleriAlId(int sponsorlukKalemleriId)
        {
            if (sponsorlukKalemleriId == 0)
                return null;

            string key = string.Format(SPONSORLUKKALEMLERİ_BY_ID_KEY, sponsorlukKalemleriId);
            return _önbellekYönetici.Al(key, () => _sponsorlukKalemleriDepo.AlId(sponsorlukKalemleriId));
        }

        public void SponsorlukKalemleriEkle(SponsorlukKalemleri sponsorlukKalemleri)
        {
            if (sponsorlukKalemleri == null)
                throw new ArgumentNullException("sponsorlukKalemleri");

            _sponsorlukKalemleriDepo.Ekle(sponsorlukKalemleri);
            _önbellekYönetici.KalıpİleSil(SPONSORLUKKALEMLERİ_PATTERN_KEY);
            _olayYayınlayıcı.OlayEklendi(sponsorlukKalemleri);
        }

        public void SponsorlukKalemleriGüncelle(SponsorlukKalemleri sponsorlukKalemleri)
        {
            if (sponsorlukKalemleri == null)
                throw new ArgumentNullException("sponsorlukKalemleri");

            _sponsorlukKalemleriDepo.Güncelle(sponsorlukKalemleri);
            _önbellekYönetici.KalıpİleSil(SPONSORLUKKALEMLERİ_PATTERN_KEY);
            _olayYayınlayıcı.OlayGüncellendi(sponsorlukKalemleri);
        }

        public void SponsorlukKalemleriSil(SponsorlukKalemleri sponsorlukKalemleri)
        {
            if (sponsorlukKalemleri == null)
                throw new ArgumentNullException("sponsorlukKalemleri");

            _sponsorlukKalemleriDepo.Sil(sponsorlukKalemleri);
            _önbellekYönetici.KalıpİleSil(SPONSORLUKKALEMLERİ_PATTERN_KEY);
            _olayYayınlayıcı.OlaySilindi(sponsorlukKalemleri);
        }

        public IList<SponsorlukKalemleri> TümSponsorlukKalemleriAl()
        {
            string key = string.Format(SPONSORLUKKALEMLERİ_ALL_KEY);
            return _önbellekYönetici.Al(key, () =>
            {
                var query = _sponsorlukKalemleriDepo.Tablo;
                return query.ToList();
            });
        }
    }

}
