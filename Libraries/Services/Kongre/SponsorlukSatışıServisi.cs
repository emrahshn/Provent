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
    public class SponsorlukSatışıServisi : ISponsorlukSatışıServisi
    {
        private const string SPONSORLUKSATIŞI_ALL_KEY = "sponsorlukSatışı.all-";
        private const string SPONSORLUKSATIŞI_BY_ID_KEY = "sponsorlukSatışı.id-{0}";
        private const string SPONSORLUKSATIŞI_PATTERN_KEY = "sponsorlukSatışı.";
        private readonly IWorkContext _workContext;
        private readonly IOlayYayınlayıcı _olayYayınlayıcı;
        private readonly IÖnbellekYönetici _önbellekYönetici;
        private readonly IDepo<SponsorlukSatışı> _sponsorlukSatışıDepo;
        public SponsorlukSatışıServisi(IDepo<SponsorlukSatışı> sponsorlukSatışıDepo,
        IWorkContext workContext,
        IOlayYayınlayıcı olayYayınlayıcı,
        IÖnbellekYönetici önbellekYönetici)
        {
            this._sponsorlukSatışıDepo = sponsorlukSatışıDepo;
            this._workContext = workContext;
            this._olayYayınlayıcı = olayYayınlayıcı;
            this._önbellekYönetici = önbellekYönetici;
        }
        public SponsorlukSatışı SponsorlukSatışıAlId(int sponsorlukSatışıId)
        {
            if (sponsorlukSatışıId == 0)
                return null;

            string key = string.Format(SPONSORLUKSATIŞI_BY_ID_KEY, sponsorlukSatışıId);
            return _önbellekYönetici.Al(key, () => _sponsorlukSatışıDepo.AlId(sponsorlukSatışıId));
        }
        public IList<SponsorlukSatışı> SponsorlukSatışıAlKongreId(int kongreId, int sponsorId)
        {
            if (sponsorId == 0)
                return null;
            if (kongreId == 0)
                return null;
            var query = _sponsorlukSatışıDepo.Tablo;
            query.Where(x => x.KongreId == kongreId && x.SponsorId == sponsorId);
            return query.ToList();
        }

        public void SponsorlukSatışıEkle(SponsorlukSatışı sponsorlukSatışı)
        {
            if (sponsorlukSatışı == null)
                throw new ArgumentNullException("sponsorlukSatışı");

            _sponsorlukSatışıDepo.Ekle(sponsorlukSatışı);
            _önbellekYönetici.KalıpİleSil(SPONSORLUKSATIŞI_PATTERN_KEY);
            _olayYayınlayıcı.OlayEklendi(sponsorlukSatışı);
        }

        public void SponsorlukSatışıGüncelle(SponsorlukSatışı sponsorlukSatışı)
        {
            if (sponsorlukSatışı == null)
                throw new ArgumentNullException("sponsorlukSatışı");

            _sponsorlukSatışıDepo.Güncelle(sponsorlukSatışı);
            _önbellekYönetici.KalıpİleSil(SPONSORLUKSATIŞI_PATTERN_KEY);
            _olayYayınlayıcı.OlayGüncellendi(sponsorlukSatışı);
        }

        public void SponsorlukSatışıSil(SponsorlukSatışı sponsorlukSatışı)
        {
            if (sponsorlukSatışı == null)
                throw new ArgumentNullException("sponsorlukSatışı");

            _sponsorlukSatışıDepo.Sil(sponsorlukSatışı);
            _önbellekYönetici.KalıpİleSil(SPONSORLUKSATIŞI_PATTERN_KEY);
            _olayYayınlayıcı.OlaySilindi(sponsorlukSatışı);
        }

        public IList<SponsorlukSatışı> TümSponsorlukSatışıAl()
        {
            string key = string.Format(SPONSORLUKSATIŞI_ALL_KEY);
            return _önbellekYönetici.Al(key, () =>
            {
                var query = _sponsorlukSatışıDepo.Tablo;
                return query.ToList();
            });
        }
    }
}
