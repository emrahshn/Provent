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
    public class CrmFirmaServisi : ICrmFirmaServisi
    {
        private const string CRMFirma_ALL_KEY = "crmFirma.all-";
        private const string CRMFirma_BY_ID_KEY = "crmFirma.id-{0}";
        private const string CRMFirma_PATTERN_KEY = "crmFirma.";
        private readonly IWorkContext _workContext;
        private readonly IOlayYayınlayıcı _olayYayınlayıcı;
        private readonly IÖnbellekYönetici _önbellekYönetici;
        private readonly IDepo<CrmFirma> _crmFirmaDepo;
        public CrmFirmaServisi(IDepo<CrmFirma> crmFirmaDepo,
        IWorkContext workContext,
        IOlayYayınlayıcı olayYayınlayıcı,
        IÖnbellekYönetici önbellekYönetici)
        {
            this._crmFirmaDepo = crmFirmaDepo;
            this._workContext = workContext;
            this._olayYayınlayıcı = olayYayınlayıcı;
            this._önbellekYönetici = önbellekYönetici;
        }
        public CrmFirma CrmFirmaAlId(int crmFirmaId)
        {
            if (crmFirmaId == 0)
                return null;

            string key = string.Format(CRMFirma_BY_ID_KEY, crmFirmaId);
            return _önbellekYönetici.Al(key, () => _crmFirmaDepo.AlId(crmFirmaId));
        }

        public void CrmFirmaEkle(CrmFirma crmFirma)
        {
            if (crmFirma == null)
                throw new ArgumentNullException("crmFirma");

            _crmFirmaDepo.Ekle(crmFirma);
            _önbellekYönetici.KalıpİleSil(CRMFirma_PATTERN_KEY);
            _olayYayınlayıcı.OlayEklendi(crmFirma);
        }

        public void CrmFirmaGüncelle(CrmFirma crmFirma)
        {
            if (crmFirma == null)
                throw new ArgumentNullException("crmFirma");

            _crmFirmaDepo.Güncelle(crmFirma);
            _önbellekYönetici.KalıpİleSil(CRMFirma_PATTERN_KEY);
            _olayYayınlayıcı.OlayGüncellendi(crmFirma);
        }

        public void CrmFirmaSil(CrmFirma crmFirma)
        {
            if (crmFirma == null)
                throw new ArgumentNullException("crmFirma");

            _crmFirmaDepo.Sil(crmFirma);
            _önbellekYönetici.KalıpİleSil(CRMFirma_PATTERN_KEY);
            _olayYayınlayıcı.OlaySilindi(crmFirma);
        }

        public IList<CrmFirma> TümCrmFirmaAl()
        {
            string key = string.Format(CRMFirma_ALL_KEY);
            return _önbellekYönetici.Al(key, () =>
            {
                var query = _crmFirmaDepo.Tablo;
                return query.ToList();
            });
        }
    }

}
