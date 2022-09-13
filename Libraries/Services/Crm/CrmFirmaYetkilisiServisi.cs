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
    public class CrmFirmaYetkilisiServisi : ICrmFirmaYetkilisiServisi
    {
        private const string CRMFirmaYetkilisi_ALL_KEY = "crmFirmaYetkilisi.all-";
        private const string CRMFirmaYetkilisi_BY_ID_KEY = "crmFirmaYetkilisi.id-{0}";
        private const string CRMFirmaYetkilisi_PATTERN_KEY = "crmFirmaYetkilisi.";
        private readonly IWorkContext _workContext;
        private readonly IOlayYayınlayıcı _olayYayınlayıcı;
        private readonly IÖnbellekYönetici _önbellekYönetici;
        private readonly IDepo<CrmFirmaYetkilisi> _crmFirmaYetkilisiDepo;
        public CrmFirmaYetkilisiServisi(IDepo<CrmFirmaYetkilisi> crmFirmaYetkilisiDepo,
        IWorkContext workContext,
        IOlayYayınlayıcı olayYayınlayıcı,
        IÖnbellekYönetici önbellekYönetici)
        {
            this._crmFirmaYetkilisiDepo = crmFirmaYetkilisiDepo;
            this._workContext = workContext;
            this._olayYayınlayıcı = olayYayınlayıcı;
            this._önbellekYönetici = önbellekYönetici;
        }
        public CrmFirmaYetkilisi CrmFirmaYetkilisiAlId(int crmFirmaYetkilisiId)
        {
            if (crmFirmaYetkilisiId == 0)
                return null;

            string key = string.Format(CRMFirmaYetkilisi_BY_ID_KEY, crmFirmaYetkilisiId);
            return _önbellekYönetici.Al(key, () => _crmFirmaYetkilisiDepo.AlId(crmFirmaYetkilisiId));
        }

        public void CrmFirmaYetkilisiEkle(CrmFirmaYetkilisi crmFirmaYetkilisi)
        {
            if (crmFirmaYetkilisi == null)
                throw new ArgumentNullException("crmFirmaYetkilisi");

            _crmFirmaYetkilisiDepo.Ekle(crmFirmaYetkilisi);
            _önbellekYönetici.KalıpİleSil(CRMFirmaYetkilisi_PATTERN_KEY);
            _olayYayınlayıcı.OlayEklendi(crmFirmaYetkilisi);
        }

        public void CrmFirmaYetkilisiGüncelle(CrmFirmaYetkilisi crmFirmaYetkilisi)
        {
            if (crmFirmaYetkilisi == null)
                throw new ArgumentNullException("crmFirmaYetkilisi");

            _crmFirmaYetkilisiDepo.Güncelle(crmFirmaYetkilisi);
            _önbellekYönetici.KalıpİleSil(CRMFirmaYetkilisi_PATTERN_KEY);
            _olayYayınlayıcı.OlayGüncellendi(crmFirmaYetkilisi);
        }

        public void CrmFirmaYetkilisiSil(CrmFirmaYetkilisi crmFirmaYetkilisi)
        {
            if (crmFirmaYetkilisi == null)
                throw new ArgumentNullException("crmFirmaYetkilisi");

            _crmFirmaYetkilisiDepo.Sil(crmFirmaYetkilisi);
            _önbellekYönetici.KalıpİleSil(CRMFirmaYetkilisi_PATTERN_KEY);
            _olayYayınlayıcı.OlaySilindi(crmFirmaYetkilisi);
        }

        public IList<CrmFirmaYetkilisi> TümCrmFirmaYetkilisiAl()
        {
            string key = string.Format(CRMFirmaYetkilisi_ALL_KEY);
            return _önbellekYönetici.Al(key, () =>
            {
                var query = _crmFirmaYetkilisiDepo.Tablo;
                return query.ToList();
            });
        }
        public IList<CrmFirmaYetkilisi> CrmFirmaYetkilileriAl(int firmaId)
        {
            string key = string.Format(CRMFirmaYetkilisi_ALL_KEY);
            return _önbellekYönetici.Al(key, () =>
            {
                var query = _crmFirmaYetkilisiDepo.Tablo.Where(x => x.FirmaId == firmaId);
                return query.ToList();
            });
        }
    }

}
