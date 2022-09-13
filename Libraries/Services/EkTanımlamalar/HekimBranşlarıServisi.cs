using Core;
using Core.Data;
using Core.Domain.EkTanımlamalar;
using Core.Önbellek;
using Services.Olaylar;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services.EkTanımlamalar
{
    public class HekimBranşlarıServisi : IHekimBranşlarıServisi
    {
        private const string HEKİMBRANŞLARI_ALL_KEY = "hekimBranşları.all-";
        private const string HEKİMBRANŞLARI_BY_ID_KEY = "hekimBranşları.id-{0}";
        private const string HEKİMBRANŞLARI_PATTERN_KEY = "hekimBranşları.";
        private readonly IWorkContext _workContext;
        private readonly IOlayYayınlayıcı _olayYayınlayıcı;
        private readonly IÖnbellekYönetici _önbellekYönetici;
        private readonly IDepo<HekimBranşları> _hekimBranşlarıDepo;
        public HekimBranşlarıServisi(IDepo<HekimBranşları> hekimBranşlarıDepo,
        IWorkContext workContext,
        IOlayYayınlayıcı olayYayınlayıcı,
        IÖnbellekYönetici önbellekYönetici)
        {
            this._hekimBranşlarıDepo = hekimBranşlarıDepo;
            this._workContext = workContext;
            this._olayYayınlayıcı = olayYayınlayıcı;
            this._önbellekYönetici = önbellekYönetici;
        }
        public HekimBranşları HekimBranşlarıAlId(int hekimBranşlarıId)
        {
            if (hekimBranşlarıId == 0)
                return null;

            string key = string.Format(HEKİMBRANŞLARI_BY_ID_KEY, hekimBranşlarıId);
            return _önbellekYönetici.Al(key, () => _hekimBranşlarıDepo.AlId(hekimBranşlarıId));
        }

        public void HekimBranşlarıEkle(HekimBranşları hekimBranşları)
        {
            if (hekimBranşları == null)
                throw new ArgumentNullException("hekimBranşları");

            _hekimBranşlarıDepo.Ekle(hekimBranşları);
            _önbellekYönetici.KalıpİleSil(HEKİMBRANŞLARI_PATTERN_KEY);
            _olayYayınlayıcı.OlayEklendi(hekimBranşları);
        }

        public void HekimBranşlarıGüncelle(HekimBranşları hekimBranşları)
        {
            if (hekimBranşları == null)
                throw new ArgumentNullException("hekimBranşları");

            _hekimBranşlarıDepo.Güncelle(hekimBranşları);
            _önbellekYönetici.KalıpİleSil(HEKİMBRANŞLARI_PATTERN_KEY);
            _olayYayınlayıcı.OlayGüncellendi(hekimBranşları);
        }

        public void HekimBranşlarıSil(HekimBranşları hekimBranşları)
        {
            if (hekimBranşları == null)
                throw new ArgumentNullException("hekimBranşları");

            _hekimBranşlarıDepo.Sil(hekimBranşları);
            _önbellekYönetici.KalıpİleSil(HEKİMBRANŞLARI_PATTERN_KEY);
            _olayYayınlayıcı.OlaySilindi(hekimBranşları);
        }

        public IList<HekimBranşları> TümHekimBranşlarıAl()
        {
            string key = string.Format(HEKİMBRANŞLARI_ALL_KEY);
            return _önbellekYönetici.Al(key, () =>
            {
                var query = _hekimBranşlarıDepo.Tablo;
                return query.ToList();
            });
        }
    }
}
