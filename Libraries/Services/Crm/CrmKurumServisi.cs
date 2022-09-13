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
    public class CrmKurumServisi : ICrmKurumServisi
    {
        private const string CRMKURUM_ALL_KEY = "crmKurum.all";
        private const string CRMKURUM_BY_ID_KEY = "crmKurum.id-{0}";
        private const string CRMKURUM_PATTERN_KEY = "crmKurum.";
        private readonly IWorkContext _workContext;
        private readonly IOlayYayınlayıcı _olayYayınlayıcı;
        private readonly IÖnbellekYönetici _önbellekYönetici;
        private readonly IDepo<CrmKurum> _crmKurumDepo;
        public CrmKurumServisi(IDepo<CrmKurum> crmKurumDepo,
        IWorkContext workContext,
        IOlayYayınlayıcı olayYayınlayıcı,
        IÖnbellekYönetici önbellekYönetici)
        {
            this._crmKurumDepo = crmKurumDepo;
            this._workContext = workContext;
            this._olayYayınlayıcı = olayYayınlayıcı;
            this._önbellekYönetici = önbellekYönetici;
        }
        public CrmKurum CrmKurumAlId(int crmKurumId)
        {
            if (crmKurumId == 0)
                return null;

            string key = string.Format(CRMKURUM_BY_ID_KEY, crmKurumId);
            return _önbellekYönetici.Al(key, () => _crmKurumDepo.AlId(crmKurumId));
        }

        public void CrmKurumEkle(CrmKurum crmKurum)
        {
            if (crmKurum == null)
                throw new ArgumentNullException("crmKurum");

            _crmKurumDepo.Ekle(crmKurum);
            _önbellekYönetici.KalıpİleSil(CRMKURUM_PATTERN_KEY);
            _olayYayınlayıcı.OlayEklendi(crmKurum);
        }

        public void CrmKurumGüncelle(CrmKurum crmKurum)
        {
            if (crmKurum == null)
                throw new ArgumentNullException("crmKurum");

            _crmKurumDepo.Güncelle(crmKurum);
            _önbellekYönetici.KalıpİleSil(CRMKURUM_PATTERN_KEY);
            _olayYayınlayıcı.OlayGüncellendi(crmKurum);
        }

        public void CrmKurumSil(CrmKurum crmKurum)
        {
            if (crmKurum == null)
                throw new ArgumentNullException("crmKurum");

            _crmKurumDepo.Sil(crmKurum);
            _önbellekYönetici.KalıpİleSil(CRMKURUM_PATTERN_KEY);
            _olayYayınlayıcı.OlaySilindi(crmKurum);
        }

        public IList<CrmKurum> TümCrmKurumAl()
        {
            string key = string.Format(CRMKURUM_ALL_KEY);
            return _önbellekYönetici.Al(key, () =>
            {
                var query = _crmKurumDepo.Tablo;
                return query.ToList();
            });
        }
    }

}
