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
    public class CrmUnvanServisi : ICrmUnvanServisi
    {
        private const string CRMUNVAN_ALL_KEY = "crmUnvan.all-";
        private const string CRMUNVAN_BY_ID_KEY = "crmUnvan.id-{0}";
        private const string CRMUNVAN_PATTERN_KEY = "crmUnvan.";
        private readonly IWorkContext _workContext;
        private readonly IOlayYayınlayıcı _olayYayınlayıcı;
        private readonly IÖnbellekYönetici _önbellekYönetici;
        private readonly IDepo<CrmUnvan> _crmUnvanDepo;
        public CrmUnvanServisi(IDepo<CrmUnvan> crmUnvanDepo,
        IWorkContext workContext,
        IOlayYayınlayıcı olayYayınlayıcı,
        IÖnbellekYönetici önbellekYönetici)
        {
            this._crmUnvanDepo = crmUnvanDepo;
            this._workContext = workContext;
            this._olayYayınlayıcı = olayYayınlayıcı;
            this._önbellekYönetici = önbellekYönetici;
        }
        public CrmUnvan CrmUnvanAlId(int crmUnvanId)
        {
            if (crmUnvanId == 0)
                return null;

            string key = string.Format(CRMUNVAN_BY_ID_KEY, crmUnvanId);
            return _önbellekYönetici.Al(key, () => _crmUnvanDepo.AlId(crmUnvanId));
        }

        public void CrmUnvanEkle(CrmUnvan crmUnvan)
        {
            if (crmUnvan == null)
                throw new ArgumentNullException("crmUnvan");

            _crmUnvanDepo.Ekle(crmUnvan);
            _önbellekYönetici.KalıpİleSil(CRMUNVAN_PATTERN_KEY);
            _olayYayınlayıcı.OlayEklendi(crmUnvan);
        }

        public void CrmUnvanGüncelle(CrmUnvan crmUnvan)
        {
            if (crmUnvan == null)
                throw new ArgumentNullException("crmUnvan");

            _crmUnvanDepo.Güncelle(crmUnvan);
            _önbellekYönetici.KalıpİleSil(CRMUNVAN_PATTERN_KEY);
            _olayYayınlayıcı.OlayGüncellendi(crmUnvan);
        }

        public void CrmUnvanSil(CrmUnvan crmUnvan)
        {
            if (crmUnvan == null)
                throw new ArgumentNullException("crmUnvan");

            _crmUnvanDepo.Sil(crmUnvan);
            _önbellekYönetici.KalıpİleSil(CRMUNVAN_PATTERN_KEY);
            _olayYayınlayıcı.OlaySilindi(crmUnvan);
        }

        public IList<CrmUnvan> TümCrmUnvanAl()
        {
            string key = string.Format(CRMUNVAN_ALL_KEY);
            return _önbellekYönetici.Al(key, () =>
            {
                var query = _crmUnvanDepo.Tablo;
                return query.ToList();
            });
        }
    }

}
