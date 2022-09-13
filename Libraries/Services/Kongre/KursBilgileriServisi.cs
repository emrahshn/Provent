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
    public class KursBilgileriServisi : IKursBilgileriServisi
    {
        private const string KURSBİLGİLERİ_ALL_KEY = "kursBilgileri.all-";
        private const string KURSBİLGİLERİ_BY_ID_KEY = "kursBilgileri.id-{0}";
        private const string KURSBİLGİLERİ_PATTERN_KEY = "kursBilgileri.";
        private readonly IWorkContext _workContext;
        private readonly IOlayYayınlayıcı _olayYayınlayıcı;
        private readonly IÖnbellekYönetici _önbellekYönetici;
        private readonly IDepo<KursBilgileri> _kursBilgileriDepo;
        public KursBilgileriServisi(IDepo<KursBilgileri> kursBilgileriDepo,
        IWorkContext workContext,
        IOlayYayınlayıcı olayYayınlayıcı,
        IÖnbellekYönetici önbellekYönetici)
        {
            this._kursBilgileriDepo = kursBilgileriDepo;
            this._workContext = workContext;
            this._olayYayınlayıcı = olayYayınlayıcı;
            this._önbellekYönetici = önbellekYönetici;
        }
        public KursBilgileri KursBilgileriAlId(int kursBilgileriId)
        {
            if (kursBilgileriId == 0)
                return null;

            string key = string.Format(KURSBİLGİLERİ_BY_ID_KEY, kursBilgileriId);
            return _önbellekYönetici.Al(key, () => _kursBilgileriDepo.AlId(kursBilgileriId));
        }

        public void KursBilgileriEkle(KursBilgileri kursBilgileri)
        {
            if (kursBilgileri == null)
                throw new ArgumentNullException("kursBilgileri");

            _kursBilgileriDepo.Ekle(kursBilgileri);
            _önbellekYönetici.KalıpİleSil(KURSBİLGİLERİ_PATTERN_KEY);
            _olayYayınlayıcı.OlayEklendi(kursBilgileri);
        }

        public void KursBilgileriGüncelle(KursBilgileri kursBilgileri)
        {
            if (kursBilgileri == null)
                throw new ArgumentNullException("kursBilgileri");

            _kursBilgileriDepo.Güncelle(kursBilgileri);
            _önbellekYönetici.KalıpİleSil(KURSBİLGİLERİ_PATTERN_KEY);
            _olayYayınlayıcı.OlayGüncellendi(kursBilgileri);
        }

        public void KursBilgileriSil(KursBilgileri kursBilgileri)
        {
            if (kursBilgileri == null)
                throw new ArgumentNullException("kursBilgileri");

            _kursBilgileriDepo.Sil(kursBilgileri);
            _önbellekYönetici.KalıpİleSil(KURSBİLGİLERİ_PATTERN_KEY);
            _olayYayınlayıcı.OlaySilindi(kursBilgileri);
        }

        public IList<KursBilgileri> TümKursBilgileriAl()
        {
            string key = string.Format(KURSBİLGİLERİ_ALL_KEY);
            return _önbellekYönetici.Al(key, () =>
            {
                var query = _kursBilgileriDepo.Tablo;
                return query.ToList();
            });
        }
    }

}
