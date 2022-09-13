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
    public class TedarikciKategorileriServisi : ITedarikciKategorileriServisi
    {
        private const string TEDARİKCİKATEGORİLERİ_ALL_KEY = "tedarikciKategorileri.all-";
        private const string TEDARİKCİKATEGORİLERİ_BY_ID_KEY = "tedarikciKategorileri.id-{0}";
        private const string TEDARİKCİKATEGORİLERİ_PATTERN_KEY = "tedarikciKategorileri.";
        private readonly IWorkContext _workContext;
        private readonly IOlayYayınlayıcı _olayYayınlayıcı;
        private readonly IÖnbellekYönetici _önbellekYönetici;
        private readonly IDepo<TedarikciKategorileri> _tedarikciKategorileriDepo;
        public TedarikciKategorileriServisi(IDepo<TedarikciKategorileri> tedarikciKategorileriDepo,
        IWorkContext workContext,
        IOlayYayınlayıcı olayYayınlayıcı,
        IÖnbellekYönetici önbellekYönetici)
        {
            this._tedarikciKategorileriDepo = tedarikciKategorileriDepo;
            this._workContext = workContext;
            this._olayYayınlayıcı = olayYayınlayıcı;
            this._önbellekYönetici = önbellekYönetici;
        }
        public TedarikciKategorileri TedarikciKategorileriAlId(int tedarikciKategorileriId)
        {
            if (tedarikciKategorileriId == 0)
                return null;

            string key = string.Format(TEDARİKCİKATEGORİLERİ_BY_ID_KEY, tedarikciKategorileriId);
            return _önbellekYönetici.Al(key, () => _tedarikciKategorileriDepo.AlId(tedarikciKategorileriId));
        }

        public void TedarikciKategorileriEkle(TedarikciKategorileri tedarikciKategorileri)
        {
            if (tedarikciKategorileri == null)
                throw new ArgumentNullException("tedarikciKategorileri");

            _tedarikciKategorileriDepo.Ekle(tedarikciKategorileri);
            _önbellekYönetici.KalıpİleSil(TEDARİKCİKATEGORİLERİ_PATTERN_KEY);
            _olayYayınlayıcı.OlayEklendi(tedarikciKategorileri);
        }

        public void TedarikciKategorileriGüncelle(TedarikciKategorileri tedarikciKategorileri)
        {
            if (tedarikciKategorileri == null)
                throw new ArgumentNullException("tedarikciKategorileri");

            _tedarikciKategorileriDepo.Güncelle(tedarikciKategorileri);
            _önbellekYönetici.KalıpİleSil(TEDARİKCİKATEGORİLERİ_PATTERN_KEY);
            _olayYayınlayıcı.OlayGüncellendi(tedarikciKategorileri);
        }

        public void TedarikciKategorileriSil(TedarikciKategorileri tedarikciKategorileri)
        {
            if (tedarikciKategorileri == null)
                throw new ArgumentNullException("tedarikciKategorileri");

            _tedarikciKategorileriDepo.Sil(tedarikciKategorileri);
            _önbellekYönetici.KalıpİleSil(TEDARİKCİKATEGORİLERİ_PATTERN_KEY);
            _olayYayınlayıcı.OlaySilindi(tedarikciKategorileri);
        }

        public IList<TedarikciKategorileri> TümTedarikciKategorileriAl()
        {
            string key = string.Format(TEDARİKCİKATEGORİLERİ_ALL_KEY);
            return _önbellekYönetici.Al(key, () =>
            {
                var query = _tedarikciKategorileriDepo.Tablo;
                return query.ToList();
            });
        }
    }

}
