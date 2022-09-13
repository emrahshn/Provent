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
    public class CrmGorusmeServisi : ICrmGorusmeServisi
    {
        private const string CRMGORUSME_ALL_KEY = "crmGorusme.all";
        private const string CRMGORUSME_BY_ID_KEY = "crmGorusme.id-{0}";
        private const string CRMGORUSME_PATTERN_KEY = "crmGorusme.";
        private readonly IWorkContext _workContext;
        private readonly IOlayYayınlayıcı _olayYayınlayıcı;
        private readonly IÖnbellekYönetici _önbellekYönetici;
        private readonly IDepo<CrmGorusme> _crmGorusmeDepo;
        public CrmGorusmeServisi(IDepo<CrmGorusme> crmGorusmeDepo,
        IWorkContext workContext,
        IOlayYayınlayıcı olayYayınlayıcı,
        IÖnbellekYönetici önbellekYönetici)
        {
            this._crmGorusmeDepo = crmGorusmeDepo;
            this._workContext = workContext;
            this._olayYayınlayıcı = olayYayınlayıcı;
            this._önbellekYönetici = önbellekYönetici;
        }
        public CrmGorusme CrmGorusmeAlId(int crmGorusmeId)
        {
            if (crmGorusmeId == 0)
                return null;

            string key = string.Format(CRMGORUSME_BY_ID_KEY, crmGorusmeId);
            return _önbellekYönetici.Al(key, () => _crmGorusmeDepo.AlId(crmGorusmeId));
        }

        public void CrmGorusmeEkle(CrmGorusme crmGorusme)
        {
            if (crmGorusme == null)
                throw new ArgumentNullException("crmGorusme");

            _crmGorusmeDepo.Ekle(crmGorusme);
            _önbellekYönetici.KalıpİleSil(CRMGORUSME_PATTERN_KEY);
            _olayYayınlayıcı.OlayEklendi(crmGorusme);
        }

        public void CrmGorusmeGüncelle(CrmGorusme crmGorusme)
        {
            if (crmGorusme == null)
                throw new ArgumentNullException("crmGorusme");

            _crmGorusmeDepo.Güncelle(crmGorusme);
            _önbellekYönetici.KalıpİleSil(CRMGORUSME_PATTERN_KEY);
            _olayYayınlayıcı.OlayGüncellendi(crmGorusme);
        }

        public void CrmGorusmeSil(CrmGorusme crmGorusme)
        {
            if (crmGorusme == null)
                throw new ArgumentNullException("crmGorusme");

            _crmGorusmeDepo.Sil(crmGorusme);
            _önbellekYönetici.KalıpİleSil(CRMGORUSME_PATTERN_KEY);
            _olayYayınlayıcı.OlaySilindi(crmGorusme);
        }

        public IList<CrmGorusme> TümCrmGorusmeAl()
        {
            string key = string.Format(CRMGORUSME_ALL_KEY);
            return _önbellekYönetici.Al(key, () =>
            {
                var query = _crmGorusmeDepo.Tablo;
                return query.ToList();
            });
        }
    }

}
