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
    public class KayıtBilgileriServisi : IKayıtBilgileriServisi
    {
        private const string KAYITBİLGİLERİ_ALL_KEY = "kayıtBilgileri.all-";
        private const string KAYITBİLGİLERİ_BY_ID_KEY = "kayıtBilgileri.id-{0}";
        private const string KAYITBİLGİLERİ_PATTERN_KEY = "kayıtBilgileri.";
        private readonly IWorkContext _workContext;
        private readonly IOlayYayınlayıcı _olayYayınlayıcı;
        private readonly IÖnbellekYönetici _önbellekYönetici;
        private readonly IDepo<KayıtBilgileri> _kayıtBilgileriDepo;
        public KayıtBilgileriServisi(IDepo<KayıtBilgileri> kayıtBilgileriDepo,
        IWorkContext workContext,
        IOlayYayınlayıcı olayYayınlayıcı,
        IÖnbellekYönetici önbellekYönetici)
        {
            this._kayıtBilgileriDepo = kayıtBilgileriDepo;
            this._workContext = workContext;
            this._olayYayınlayıcı = olayYayınlayıcı;
            this._önbellekYönetici = önbellekYönetici;
        }
        public KayıtBilgileri KayıtBilgileriAlId(int kayıtBilgileriId)
        {
            if (kayıtBilgileriId == 0)
                return null;

            string key = string.Format(KAYITBİLGİLERİ_BY_ID_KEY, kayıtBilgileriId);
            return _önbellekYönetici.Al(key, () => _kayıtBilgileriDepo.AlId(kayıtBilgileriId));
        }

        public void KayıtBilgileriEkle(KayıtBilgileri kayıtBilgileri)
        {
            if (kayıtBilgileri == null)
                throw new ArgumentNullException("kayıtBilgileri");

            _kayıtBilgileriDepo.Ekle(kayıtBilgileri);
            _önbellekYönetici.KalıpİleSil(KAYITBİLGİLERİ_PATTERN_KEY);
            _olayYayınlayıcı.OlayEklendi(kayıtBilgileri);
        }

        public void KayıtBilgileriGüncelle(KayıtBilgileri kayıtBilgileri)
        {
            if (kayıtBilgileri == null)
                throw new ArgumentNullException("kayıtBilgileri");

            _kayıtBilgileriDepo.Güncelle(kayıtBilgileri);
            _önbellekYönetici.KalıpİleSil(KAYITBİLGİLERİ_PATTERN_KEY);
            _olayYayınlayıcı.OlayGüncellendi(kayıtBilgileri);
        }

        public void KayıtBilgileriSil(KayıtBilgileri kayıtBilgileri)
        {
            if (kayıtBilgileri == null)
                throw new ArgumentNullException("kayıtBilgileri");

            _kayıtBilgileriDepo.Sil(kayıtBilgileri);
            _önbellekYönetici.KalıpİleSil(KAYITBİLGİLERİ_PATTERN_KEY);
            _olayYayınlayıcı.OlaySilindi(kayıtBilgileri);
        }

        public IList<KayıtBilgileri> TümKayıtBilgileriAl()
        {
            string key = string.Format(KAYITBİLGİLERİ_ALL_KEY);
            return _önbellekYönetici.Al(key, () =>
            {
                var query = _kayıtBilgileriDepo.Tablo;
                return query.ToList();
            });
        }
    }

}
