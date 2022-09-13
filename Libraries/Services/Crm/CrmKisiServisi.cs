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
    public class CrmKisiServisi : ICrmKisiServisi
    {
        private const string CRMKİSİ_ALL_KEY = "crmKisi.all-";
        private const string CRMKİSİ_BY_ID_KEY = "crmKisi.id-{0}";
        private const string CRMKİSİ_PATTERN_KEY = "crmKisi.";
        private readonly IWorkContext _workContext;
        private readonly IOlayYayınlayıcı _olayYayınlayıcı;
        private readonly IÖnbellekYönetici _önbellekYönetici;
        private readonly IDepo<CrmKisi> _crmKisiDepo;
        public CrmKisiServisi(IDepo<CrmKisi> crmKisiDepo,
        IWorkContext workContext,
        IOlayYayınlayıcı olayYayınlayıcı,
        IÖnbellekYönetici önbellekYönetici)
        {
            this._crmKisiDepo = crmKisiDepo;
            this._workContext = workContext;
            this._olayYayınlayıcı = olayYayınlayıcı;
            this._önbellekYönetici = önbellekYönetici;
        }
        public CrmKisi CrmKisiAlId(int crmKisiId)
        {
            if (crmKisiId == 0)
                return null;

            string key = string.Format(CRMKİSİ_BY_ID_KEY, crmKisiId);
            return _önbellekYönetici.Al(key, () => _crmKisiDepo.AlId(crmKisiId));
        }

        public void CrmKisiEkle(CrmKisi crmKisi)
        {
            if (crmKisi == null)
                throw new ArgumentNullException("crmKisi");

            _crmKisiDepo.Ekle(crmKisi);
            _önbellekYönetici.KalıpİleSil(CRMKİSİ_PATTERN_KEY);
            _olayYayınlayıcı.OlayEklendi(crmKisi);
        }

        public void CrmKisiGüncelle(CrmKisi crmKisi)
        {
            if (crmKisi == null)
                throw new ArgumentNullException("crmKisi");

            _crmKisiDepo.Güncelle(crmKisi);
            _önbellekYönetici.KalıpİleSil(CRMKİSİ_PATTERN_KEY);
            _olayYayınlayıcı.OlayGüncellendi(crmKisi);
        }

        public void CrmKisiSil(CrmKisi crmKisi)
        {
            if (crmKisi == null)
                throw new ArgumentNullException("crmKisi");

            _crmKisiDepo.Sil(crmKisi);
            _önbellekYönetici.KalıpİleSil(CRMKİSİ_PATTERN_KEY);
            _olayYayınlayıcı.OlaySilindi(crmKisi);
        }

        public IList<CrmKisi> TümCrmKisiAl()
        {
            string key = string.Format(CRMKİSİ_ALL_KEY);
            return _önbellekYönetici.Al(key, () =>
            {
                var query = _crmKisiDepo.Tablo;
                return query.ToList();
            });
        }
    }

}
