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
    public class CrmKongreServisi : ICrmKongreServisi
    {
        private const string CRMKONGRE_ALL_KEY = "crmKongre.all-";
        private const string CRMKONGRE_BY_ID_KEY = "crmKongre.id-{0}";
        private const string CRMKONGRE_PATTERN_KEY = "crmKongre.";
        private readonly IWorkContext _workContext;
        private readonly IOlayYayınlayıcı _olayYayınlayıcı;
        private readonly IÖnbellekYönetici _önbellekYönetici;
        private readonly IDepo<CrmKongre> _crmKongreDepo;
        public CrmKongreServisi(IDepo<CrmKongre> crmKongreDepo,
        IWorkContext workContext,
        IOlayYayınlayıcı olayYayınlayıcı,
        IÖnbellekYönetici önbellekYönetici)
        {
            this._crmKongreDepo = crmKongreDepo;
            this._workContext = workContext;
            this._olayYayınlayıcı = olayYayınlayıcı;
            this._önbellekYönetici = önbellekYönetici;
        }
        public CrmKongre CrmKongreAlId(int crmKongreId)
        {
            if (crmKongreId == 0)
                return null;

            string key = string.Format(CRMKONGRE_BY_ID_KEY, crmKongreId);
            return _önbellekYönetici.Al(key, () => _crmKongreDepo.AlId(crmKongreId));
        }

        public void CrmKongreEkle(CrmKongre crmKongre)
        {
            if (crmKongre == null)
                throw new ArgumentNullException("crmKongre");

            _crmKongreDepo.Ekle(crmKongre);
            _önbellekYönetici.KalıpİleSil(CRMKONGRE_PATTERN_KEY);
            _olayYayınlayıcı.OlayEklendi(crmKongre);
        }

        public void CrmKongreGüncelle(CrmKongre crmKongre)
        {
            if (crmKongre == null)
                throw new ArgumentNullException("crmKongre");

            _crmKongreDepo.Güncelle(crmKongre);
            _önbellekYönetici.KalıpİleSil(CRMKONGRE_PATTERN_KEY);
            _olayYayınlayıcı.OlayGüncellendi(crmKongre);
        }

        public void CrmKongreSil(CrmKongre crmKongre)
        {
            if (crmKongre == null)
                throw new ArgumentNullException("crmKongre");

            _crmKongreDepo.Sil(crmKongre);
            _önbellekYönetici.KalıpİleSil(CRMKONGRE_PATTERN_KEY);
            _olayYayınlayıcı.OlaySilindi(crmKongre);
        }

        public IList<CrmKongre> TümCrmKongreAl()
        {
            string key = string.Format(CRMKONGRE_ALL_KEY);
            return _önbellekYönetici.Al(key, () =>
            {
                var query = _crmKongreDepo.Tablo;
                return query.ToList();
            });
        }
    }

}
