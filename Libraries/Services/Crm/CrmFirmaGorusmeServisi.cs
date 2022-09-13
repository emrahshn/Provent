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
    public class CrmFirmaGorusmeServisi : ICrmFirmaGorusmeServisi
    {
        private const string CRMGORUSME_ALL_KEY = "crmFirmaGorusme.all";
        private const string CRMGORUSME_BY_ID_KEY = "crmFirmaGorusme.id-{0}";
        private const string CRMGORUSME_PATTERN_KEY = "crmFirmaGorusme.";
        private readonly IWorkContext _workContext;
        private readonly IOlayYayınlayıcı _olayYayınlayıcı;
        private readonly IÖnbellekYönetici _önbellekYönetici;
        private readonly IDepo<CrmFirmaGorusme> _crmFirmaGorusmeDepo;
        public CrmFirmaGorusmeServisi(IDepo<CrmFirmaGorusme> crmFirmaGorusmeDepo,
        IWorkContext workContext,
        IOlayYayınlayıcı olayYayınlayıcı,
        IÖnbellekYönetici önbellekYönetici)
        {
            this._crmFirmaGorusmeDepo = crmFirmaGorusmeDepo;
            this._workContext = workContext;
            this._olayYayınlayıcı = olayYayınlayıcı;
            this._önbellekYönetici = önbellekYönetici;
        }
        public CrmFirmaGorusme CrmFirmaGorusmeAlId(int crmFirmaGorusmeId)
        {
            if (crmFirmaGorusmeId == 0)
                return null;

            string key = string.Format(CRMGORUSME_BY_ID_KEY, crmFirmaGorusmeId);
            return _önbellekYönetici.Al(key, () => _crmFirmaGorusmeDepo.AlId(crmFirmaGorusmeId));
        }

        public void CrmFirmaGorusmeEkle(CrmFirmaGorusme crmFirmaGorusme)
        {
            if (crmFirmaGorusme == null)
                throw new ArgumentNullException("crmFirmaGorusme");

            _crmFirmaGorusmeDepo.Ekle(crmFirmaGorusme);
            _önbellekYönetici.KalıpİleSil(CRMGORUSME_PATTERN_KEY);
            _olayYayınlayıcı.OlayEklendi(crmFirmaGorusme);
        }

        public void CrmFirmaGorusmeGüncelle(CrmFirmaGorusme crmFirmaGorusme)
        {
            if (crmFirmaGorusme == null)
                throw new ArgumentNullException("crmFirmaGorusme");

            _crmFirmaGorusmeDepo.Güncelle(crmFirmaGorusme);
            _önbellekYönetici.KalıpİleSil(CRMGORUSME_PATTERN_KEY);
            _olayYayınlayıcı.OlayGüncellendi(crmFirmaGorusme);
        }

        public void CrmFirmaGorusmeSil(CrmFirmaGorusme crmFirmaGorusme)
        {
            if (crmFirmaGorusme == null)
                throw new ArgumentNullException("crmFirmaGorusme");

            _crmFirmaGorusmeDepo.Sil(crmFirmaGorusme);
            _önbellekYönetici.KalıpİleSil(CRMGORUSME_PATTERN_KEY);
            _olayYayınlayıcı.OlaySilindi(crmFirmaGorusme);
        }

        public IList<CrmFirmaGorusme> TümCrmFirmaGorusmeAl()
        {
            string key = string.Format(CRMGORUSME_ALL_KEY);
            return _önbellekYönetici.Al(key, () =>
            {
                var query = _crmFirmaGorusmeDepo.Tablo;
                return query.ToList();
            });
        }
    }

}
