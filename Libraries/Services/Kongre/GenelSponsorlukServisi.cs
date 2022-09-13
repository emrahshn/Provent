using Core;
using Core.Data;
using Core.Domain.Kongre;
using Core.Önbellek;
using Services.Olaylar;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services.Kongre
{
    public class GenelSponsorlukServisi : IGenelSponsorlukServisi
    {
        private const string GENELSPONSORLUK_ALL_KEY = "genelSponsorluk.all-";
        private const string GENELSPONSORLUK_BY_ID_KEY = "genelSponsorluk.id-{0}";
        private const string GENELSPONSORLUK_PATTERN_KEY = "genelSponsorluk.";
        private readonly IWorkContext _workContext;
        private readonly IOlayYayınlayıcı _olayYayınlayıcı;
        private readonly IÖnbellekYönetici _önbellekYönetici;
        private readonly IDepo<GenelSponsorluk> _genelSponsorlukDepo;
        public GenelSponsorlukServisi(IDepo<GenelSponsorluk> genelSponsorlukDepo,
        IWorkContext workContext,
        IOlayYayınlayıcı olayYayınlayıcı,
        IÖnbellekYönetici önbellekYönetici)
        {
            this._genelSponsorlukDepo = genelSponsorlukDepo;
            this._workContext = workContext;
            this._olayYayınlayıcı = olayYayınlayıcı;
            this._önbellekYönetici = önbellekYönetici;
        }
        public GenelSponsorluk GenelSponsorlukAlId(int genelSponsorlukId)
        {
            if (genelSponsorlukId == 0)
                return null;

            string key = string.Format(GENELSPONSORLUK_BY_ID_KEY, genelSponsorlukId);
            return _önbellekYönetici.Al(key, () => _genelSponsorlukDepo.AlId(genelSponsorlukId));
        }

        public void GenelSponsorlukEkle(GenelSponsorluk genelSponsorluk)
        {
            if (genelSponsorluk == null)
                throw new ArgumentNullException("genelSponsorluk");

            _genelSponsorlukDepo.Ekle(genelSponsorluk);
            _önbellekYönetici.KalıpİleSil(GENELSPONSORLUK_PATTERN_KEY);
            _olayYayınlayıcı.OlayEklendi(genelSponsorluk);
        }

        public void GenelSponsorlukGüncelle(GenelSponsorluk genelSponsorluk)
        {
            if (genelSponsorluk == null)
                throw new ArgumentNullException("genelSponsorluk");

            _genelSponsorlukDepo.Güncelle(genelSponsorluk);
            _önbellekYönetici.KalıpİleSil(GENELSPONSORLUK_PATTERN_KEY);
            _olayYayınlayıcı.OlayGüncellendi(genelSponsorluk);
        }

        public void GenelSponsorlukSil(GenelSponsorluk genelSponsorluk)
        {
            if (genelSponsorluk == null)
                throw new ArgumentNullException("genelSponsorluk");

            _genelSponsorlukDepo.Sil(genelSponsorluk);
            _önbellekYönetici.KalıpİleSil(GENELSPONSORLUK_PATTERN_KEY);
            _olayYayınlayıcı.OlaySilindi(genelSponsorluk);
        }

        public IList<GenelSponsorluk> TümGenelSponsorlukAl()
        {
            string key = string.Format(GENELSPONSORLUK_ALL_KEY);
            return _önbellekYönetici.Al(key, () =>
            {
                var query = _genelSponsorlukDepo.Tablo;
                return query.ToList();
            });
        }
    }

}
