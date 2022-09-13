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
    public class CrmGorevServisi : ICrmGorevServisi
    {
        private const string CRMGOREV_ALL_KEY = "crmGorev.all";
        private const string CRMGOREV_BY_ID_KEY = "crmGorev.id-{0}";
        private const string CRMGOREV_PATTERN_KEY = "crmGorev.";
        private readonly IWorkContext _workContext;
        private readonly IOlayYayınlayıcı _olayYayınlayıcı;
        private readonly IÖnbellekYönetici _önbellekYönetici;
        private readonly IDepo<CrmGorev> _crmGorevDepo;
        public CrmGorevServisi(IDepo<CrmGorev> crmGorevDepo,
        IWorkContext workContext,
        IOlayYayınlayıcı olayYayınlayıcı,
        IÖnbellekYönetici önbellekYönetici)
        {
            this._crmGorevDepo = crmGorevDepo;
            this._workContext = workContext;
            this._olayYayınlayıcı = olayYayınlayıcı;
            this._önbellekYönetici = önbellekYönetici;
        }
        public CrmGorev CrmGorevAlId(int crmGorevId)
        {
            if (crmGorevId == 0)
                return null;

            string key = string.Format(CRMGOREV_BY_ID_KEY, crmGorevId);
            return _önbellekYönetici.Al(key, () => _crmGorevDepo.AlId(crmGorevId));
        }

        public void CrmGorevEkle(CrmGorev crmGorev)
        {
            if (crmGorev == null)
                throw new ArgumentNullException("crmGorev");

            _crmGorevDepo.Ekle(crmGorev);
            _önbellekYönetici.KalıpİleSil(CRMGOREV_PATTERN_KEY);
            _olayYayınlayıcı.OlayEklendi(crmGorev);
        }

        public void CrmGorevGüncelle(CrmGorev crmGorev)
        {
            if (crmGorev == null)
                throw new ArgumentNullException("crmGorev");

            _crmGorevDepo.Güncelle(crmGorev);
            _önbellekYönetici.KalıpİleSil(CRMGOREV_PATTERN_KEY);
            _olayYayınlayıcı.OlayGüncellendi(crmGorev);
        }

        public void CrmGorevSil(CrmGorev crmGorev)
        {
            if (crmGorev == null)
                throw new ArgumentNullException("crmGorev");

            _crmGorevDepo.Sil(crmGorev);
            _önbellekYönetici.KalıpİleSil(CRMGOREV_PATTERN_KEY);
            _olayYayınlayıcı.OlaySilindi(crmGorev);
        }

        public IList<CrmGorev> TümCrmGorevAl()
        {
            string key = string.Format(CRMGOREV_ALL_KEY);
            return _önbellekYönetici.Al(key, () =>
            {
                var query = _crmGorevDepo.Tablo;
                return query.ToList();
            });
        }
    }

}
