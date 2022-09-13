using Core;
using Core.Data;
using Core.Domain.CRM;
using Core.Önbellek;
using Services.Olaylar;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services.Crm
{
    public class CrmYonetimKuruluServisi : ICrmYonetimKuruluServisi
    {
        private const string CRMYonetimKurulu_ALL_KEY = "crmYonetimKurulu.all-";
        private const string CRMYonetimKurulu_BY_ID_KEY = "crmYonetimKurulu.id-{0}";
        private const string CRMYonetimKurulu_PATTERN_KEY = "crmYonetimKurulu.";
        private readonly IWorkContext _workContext;
        private readonly IOlayYayınlayıcı _olayYayınlayıcı;
        private readonly IÖnbellekYönetici _önbellekYönetici;
        private readonly IDepo<CrmYonetimKurulu> _crmYonetimKuruluDepo;
        public CrmYonetimKuruluServisi(IDepo<CrmYonetimKurulu> crmYonetimKuruluDepo,
        IWorkContext workContext,
        IOlayYayınlayıcı olayYayınlayıcı,
        IÖnbellekYönetici önbellekYönetici)
        {
            this._crmYonetimKuruluDepo = crmYonetimKuruluDepo;
            this._workContext = workContext;
            this._olayYayınlayıcı = olayYayınlayıcı;
            this._önbellekYönetici = önbellekYönetici;
        }
        public CrmYonetimKurulu CrmYonetimKuruluAlId(int crmYonetimKuruluId)
        {
            if (crmYonetimKuruluId == 0)
                return null;

            string key = string.Format(CRMYonetimKurulu_BY_ID_KEY, crmYonetimKuruluId);
            return _önbellekYönetici.Al(key, () => _crmYonetimKuruluDepo.AlId(crmYonetimKuruluId));
        }

        public void CrmYonetimKuruluEkle(CrmYonetimKurulu crmYonetimKurulu)
        {
            if (crmYonetimKurulu == null)
                throw new ArgumentNullException("crmYonetimKurulu");

            _crmYonetimKuruluDepo.Ekle(crmYonetimKurulu);
            _önbellekYönetici.KalıpİleSil(CRMYonetimKurulu_PATTERN_KEY);
            _olayYayınlayıcı.OlayEklendi(crmYonetimKurulu);
        }

        public void CrmYonetimKuruluGüncelle(CrmYonetimKurulu crmYonetimKurulu)
        {
            if (crmYonetimKurulu == null)
                throw new ArgumentNullException("crmYonetimKurulu");

            _crmYonetimKuruluDepo.Güncelle(crmYonetimKurulu);
            _önbellekYönetici.KalıpİleSil(CRMYonetimKurulu_PATTERN_KEY);
            _olayYayınlayıcı.OlayGüncellendi(crmYonetimKurulu);
        }

        public void CrmYonetimKuruluSil(CrmYonetimKurulu crmYonetimKurulu)
        {
            if (crmYonetimKurulu == null)
                throw new ArgumentNullException("crmYonetimKurulu");

            _crmYonetimKuruluDepo.Sil(crmYonetimKurulu);
            _önbellekYönetici.KalıpİleSil(CRMYonetimKurulu_PATTERN_KEY);
            _olayYayınlayıcı.OlaySilindi(crmYonetimKurulu);
        }

        public IList<CrmYonetimKurulu> TümCrmYonetimKuruluAl()
        {
            string key = string.Format(CRMYonetimKurulu_ALL_KEY);
            return _önbellekYönetici.Al(key, () =>
            {
                var query = _crmYonetimKuruluDepo.Tablo;
                return query.ToList();
            });
        }
        public IList<CrmYonetimKurulu> CrmYonetimKuruluAlKisiId(int siteId)
        {

            var query = _crmYonetimKuruluDepo.Tablo.Where(x=>x.KisiId==siteId);
            return query.ToList();
        }
        public IList<CrmYonetimKurulu> CrmYonetimKuruluAlKurumId(int siteId)
        {

            var query = _crmYonetimKuruluDepo.Tablo.Where(x => x.KurumId == siteId);
            return query.ToList();
        }
    }

}
