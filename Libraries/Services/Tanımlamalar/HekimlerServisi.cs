using Core;
using Core.Data;
using Core.Domain.Tanımlamalar;
using Core.Önbellek;
using Services.Olaylar;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services.Tanımlamalar
{
    public class HekimlerServisi : IHekimlerServisi
    {
        private const string HEKİMLER_ALL_KEY = "hekimler.all-";
        private const string HEKİMLER_BY_ID_KEY = "hekimler.id-{0}";
        private const string HEKİMLER_PATTERN_KEY = "hekimler.";
        private readonly IWorkContext _workContext;
        private readonly IOlayYayınlayıcı _olayYayınlayıcı;
        private readonly IÖnbellekYönetici _önbellekYönetici;
        private readonly IDepo<Hekimler> _hekimlerDepo;
        public HekimlerServisi(IDepo<Hekimler> hekimlerDepo,
        IWorkContext workContext,
        IOlayYayınlayıcı olayYayınlayıcı,
        IÖnbellekYönetici önbellekYönetici)
        {
            this._hekimlerDepo = hekimlerDepo;
            this._workContext = workContext;
            this._olayYayınlayıcı = olayYayınlayıcı;
            this._önbellekYönetici = önbellekYönetici;
        }
        public Hekimler HekimlerAlId(int hekimlerId)
        {
            if (hekimlerId == 0)
                return null;

            string key = string.Format(HEKİMLER_BY_ID_KEY, hekimlerId);
            return _önbellekYönetici.Al(key, () => _hekimlerDepo.AlId(hekimlerId));
        }

        public void HekimlerEkle(Hekimler hekimler)
        {
            if (hekimler == null)
                throw new ArgumentNullException("hekimler");

            _hekimlerDepo.Ekle(hekimler);
            _önbellekYönetici.KalıpİleSil(HEKİMLER_PATTERN_KEY);
            _olayYayınlayıcı.OlayEklendi(hekimler);
        }

        public void HekimlerGüncelle(Hekimler hekimler)
        {
            if (hekimler == null)
                throw new ArgumentNullException("hekimler");

            _hekimlerDepo.Güncelle(hekimler);
            _önbellekYönetici.KalıpİleSil(HEKİMLER_PATTERN_KEY);
            _olayYayınlayıcı.OlayGüncellendi(hekimler);
        }

        public void HekimlerSil(Hekimler hekimler)
        {
            if (hekimler == null)
                throw new ArgumentNullException("hekimler");

            _hekimlerDepo.Sil(hekimler);
            _önbellekYönetici.KalıpİleSil(HEKİMLER_PATTERN_KEY);
            _olayYayınlayıcı.OlaySilindi(hekimler);
        }

        public IList<Hekimler> TümHekimlerAl()
        {
            string key = string.Format(HEKİMLER_ALL_KEY);
            return _önbellekYönetici.Al(key, () =>
            {
                var query = _hekimlerDepo.Tablo;
                return query.ToList();
            });
        }
        public ISayfalıListe<Hekimler> HekimAra(int brans,
           string adı, string soyadı, string tckn, string email, bool enYeniler, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var sorgu = _hekimlerDepo.Tablo;
            if (brans > 0)
            {
                //_firmaKategorisiServisi.FirmaKategorisiAlId(firma);
                sorgu = sorgu.Where(x => x.BranşId == brans);
            }
            if (!String.IsNullOrEmpty(email))
                sorgu = sorgu.Where(qe => qe.Email1.Contains(email) || qe.Email2.Contains(email));
            if (!String.IsNullOrEmpty(adı))
                sorgu = sorgu.Where(qe => qe.Adı.Contains(adı));
            if (!String.IsNullOrEmpty(soyadı))
                sorgu = sorgu.Where(qe => qe.Soyadı.Contains(soyadı));
            if (!String.IsNullOrEmpty(tckn))
                sorgu = sorgu.Where(qe => qe.TCKN.Contains(tckn));
            sorgu = sorgu.OrderByDescending(qe => qe.Id);

            var hekimler = new SayfalıListe<Hekimler>(sorgu, pageIndex, pageSize);
            return hekimler;
        }
    }

}
