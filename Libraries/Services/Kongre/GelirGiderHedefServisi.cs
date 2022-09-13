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
    public class GelirGiderHedefiServisi : IGelirGiderHedefiServisi
    {
        private readonly IWorkContext _workContext;
        private readonly IOlayYayınlayıcı _olayYayınlayıcı;
        private readonly IÖnbellekYönetici _önbellekYönetici;
        private readonly IDepo<GelirGiderHedefi> _gelirGiderHedefiDepo;
        public GelirGiderHedefiServisi(IDepo<GelirGiderHedefi> gelirGiderHedefiDepo,
        IWorkContext workContext,
        IOlayYayınlayıcı olayYayınlayıcı,
        IÖnbellekYönetici önbellekYönetici)
        {
            this._gelirGiderHedefiDepo = gelirGiderHedefiDepo;
            this._workContext = workContext;
            this._olayYayınlayıcı = olayYayınlayıcı;
            this._önbellekYönetici = önbellekYönetici;
        }
        public GelirGiderHedefi GelirGiderHedefiAlId(int gelirGiderHedefiId)
        {
            if (gelirGiderHedefiId == 0)
                return null;

            return  _gelirGiderHedefiDepo.AlId(gelirGiderHedefiId);
        }

        public void GelirGiderHedefiEkle(GelirGiderHedefi gelirGiderHedefi)
        {
            if (gelirGiderHedefi == null)
                throw new ArgumentNullException("gelirGiderHedefi");

            _gelirGiderHedefiDepo.Ekle(gelirGiderHedefi);
            _olayYayınlayıcı.OlayEklendi(gelirGiderHedefi);
        }

        public void GelirGiderHedefiGüncelle(GelirGiderHedefi gelirGiderHedefi)
        {
            if (gelirGiderHedefi == null)
                throw new ArgumentNullException("gelirGiderHedefi");

            _gelirGiderHedefiDepo.Güncelle(gelirGiderHedefi);
            _olayYayınlayıcı.OlayGüncellendi(gelirGiderHedefi);
        }

        public void GelirGiderHedefiSil(GelirGiderHedefi gelirGiderHedefi)
        {
            if (gelirGiderHedefi == null)
                throw new ArgumentNullException("gelirGiderHedefi");

            _gelirGiderHedefiDepo.Sil(gelirGiderHedefi);
            _olayYayınlayıcı.OlaySilindi(gelirGiderHedefi);
        }

        public IList<GelirGiderHedefi> TümGelirGiderHedefiAl()
        {
            return _gelirGiderHedefiDepo.Tablo.ToList();
        }
    }

}
