using Core;
using Core.Data;
using Core.Domain.KongreTanımlama;
using Core.Domain.Tanımlamalar;
using Core.Önbellek;
using Services.Olaylar;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services.Tanımlamalar
{
    public class FirmaServisi : IFirmaServisi
    {
        private const string FIRMA_ALL_KEY = "firma.all-";
        private const string FIRMA_ALLKATEGORI_KEY = "firma.allkategori-";
        private const string FIRMA_BY_ID_KEY = "firma.id-{0}";
        private const string FIRMA_PATTERN_KEY = "firma.";
        private readonly IWorkContext _workContext;
        private readonly IOlayYayınlayıcı _olayYayınlayıcı;
        private readonly IÖnbellekYönetici _önbellekYönetici;
        private readonly IDepo<Firma> _firmaDepo;
        public FirmaServisi(IDepo<Firma> firmaDepo,
        IWorkContext workContext,
        IOlayYayınlayıcı olayYayınlayıcı,
        IÖnbellekYönetici önbellekYönetici)
        {
            this._firmaDepo = firmaDepo;
            this._workContext = workContext;
            this._olayYayınlayıcı = olayYayınlayıcı;
            this._önbellekYönetici = önbellekYönetici;
        }
        public Firma FirmaAlId(int firmaId)
        {
            if (firmaId == 0)
                return null;

            string key = string.Format(FIRMA_BY_ID_KEY, firmaId);
            return _önbellekYönetici.Al(key, () => _firmaDepo.AlId(firmaId));
        }

        public void FirmaEkle(Firma firma)
        {
            if (firma == null)
                throw new ArgumentNullException("firma");

            _firmaDepo.Ekle(firma);
            _önbellekYönetici.KalıpİleSil(FIRMA_PATTERN_KEY);
            _olayYayınlayıcı.OlayEklendi(firma);
        }

        public void FirmaGüncelle(Firma firma)
        {
            if (firma == null)
                throw new ArgumentNullException("firma");

            _firmaDepo.Güncelle(firma);
            _önbellekYönetici.KalıpİleSil(FIRMA_PATTERN_KEY);
            _olayYayınlayıcı.OlayGüncellendi(firma);
        }

        public void FirmaSil(Firma firma)
        {
            if (firma == null)
                throw new ArgumentNullException("firma");

            _firmaDepo.Sil(firma);
            _önbellekYönetici.KalıpİleSil(FIRMA_PATTERN_KEY);
            _olayYayınlayıcı.OlaySilindi(firma);
        }

        public IList<Firma> TümFirmaAl()
        {
            string key = string.Format(FIRMA_ALL_KEY);
            return _önbellekYönetici.Al(key, () =>
            {
                var query = _firmaDepo.Tablo;
                return query.ToList();
            });
        }
        public IList<Firma> FirmaAlKategoriId(int kategoriId=1)
        {
            string key = string.Format(FIRMA_ALLKATEGORI_KEY);
            return _önbellekYönetici.Al(key, () =>
            {
                var query = _firmaDepo.Tablo.Where(x => x.KategoriId == kategoriId);
                return query.ToList();
            });
        }
    }

}
