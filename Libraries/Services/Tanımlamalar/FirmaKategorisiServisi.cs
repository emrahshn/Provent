using Core;
using Core.Data;
using Core.Domain.Tanımlamalar;
using Core.Önbellek;
using Services.Olaylar;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services.Tanımlamalar
{
    public class FirmaKategorisiServisi : IFirmaKategorisiServisi
    {
        private const string FIRMA_ALL_KEY = "firmaKategorisi.all-";
        private const string FIRMA_BY_ID_KEY = "firmaKategorisi.id-{0}";
        private const string FIRMA_PATTERN_KEY = "firmaKategorisi.";
        private readonly IWorkContext _workContext;
        private readonly IOlayYayınlayıcı _olayYayınlayıcı;
        private readonly IÖnbellekYönetici _önbellekYönetici;
        private readonly IDepo<FirmaKategorisi> _firmaKategorisiDepo;
        public FirmaKategorisiServisi(IDepo<FirmaKategorisi> firmaKategorisiDepo,
        IWorkContext workContext,
        IOlayYayınlayıcı olayYayınlayıcı,
        IÖnbellekYönetici önbellekYönetici)
        {
            this._firmaKategorisiDepo = firmaKategorisiDepo;
            this._workContext = workContext;
            this._olayYayınlayıcı = olayYayınlayıcı;
            this._önbellekYönetici = önbellekYönetici;
        }
        public FirmaKategorisi FirmaKategorisiAlId(int firmaKategorisiId)
        {
            if (firmaKategorisiId == 0)
                return null;

            string key = string.Format(FIRMA_BY_ID_KEY, firmaKategorisiId);
            return _önbellekYönetici.Al(key, () => _firmaKategorisiDepo.AlId(firmaKategorisiId));
        }

        public void FirmaKategorisiEkle(FirmaKategorisi firmaKategorisi)
        {
            if (firmaKategorisi == null)
                throw new ArgumentNullException("firmaKategorisi");

            _firmaKategorisiDepo.Ekle(firmaKategorisi);
            _önbellekYönetici.KalıpİleSil(FIRMA_PATTERN_KEY);
            _olayYayınlayıcı.OlayEklendi(firmaKategorisi);
        }

        public void FirmaKategorisiGüncelle(FirmaKategorisi firmaKategorisi)
        {
            if (firmaKategorisi == null)
                throw new ArgumentNullException("firmaKategorisi");

            _firmaKategorisiDepo.Güncelle(firmaKategorisi);
            _önbellekYönetici.KalıpİleSil(FIRMA_PATTERN_KEY);
            _olayYayınlayıcı.OlayGüncellendi(firmaKategorisi);
        }

        public void FirmaKategorisiSil(FirmaKategorisi firmaKategorisi)
        {
            if (firmaKategorisi == null)
                throw new ArgumentNullException("firmaKategorisi");

            _firmaKategorisiDepo.Sil(firmaKategorisi);
            _önbellekYönetici.KalıpİleSil(FIRMA_PATTERN_KEY);
            _olayYayınlayıcı.OlaySilindi(firmaKategorisi);
        }

        public IList<FirmaKategorisi> TümFirmaKategorisiAl()
        {
            string key = string.Format(FIRMA_ALL_KEY);
            return _önbellekYönetici.Al(key, () =>
            {
                var query = _firmaKategorisiDepo.Tablo;
                return query.ToList();
            });
        }
    }

}
