using Core;
using Core.Data;
using Core.Domain.Tanımlamalar;
using Core.Önbellek;
using Services.Olaylar;
using System;
using System.Linq;

namespace Services.Tanımlamalar
{
    public class YetkililerServisi : IYetkililerServisi
    {
        private const string YETKILI_ALL_KEY = "yetkililer.all-";
        private const string YETKILI_BY_ID_KEY = "yetkililer.id-{0}";
        private const string YETKILI_PATTERN_KEY = "yetkililer.";
        private readonly IWorkContext _workContext;
        private readonly IOlayYayınlayıcı _olayYayınlayıcı;
        private readonly IÖnbellekYönetici _önbellekYönetici;
        private readonly IDepo<Yetkililer> _yetkililerDepo;
        private readonly IFirmaKategorisiServisi _firmaKategorisiServisi;
        public YetkililerServisi(IDepo<Yetkililer> yetkililerDepo,
        IWorkContext workContext,
        IOlayYayınlayıcı olayYayınlayıcı,
        IÖnbellekYönetici önbellekYönetici,
        IFirmaKategorisiServisi firmaKategorisiServisi)
        {
            this._yetkililerDepo = yetkililerDepo;
            this._workContext = workContext;
            this._olayYayınlayıcı = olayYayınlayıcı;
            this._önbellekYönetici = önbellekYönetici;
            this._firmaKategorisiServisi = firmaKategorisiServisi;
        }
        public Yetkililer YetkiliAlId(int yetkililerId)
        {
            if (yetkililerId == 0)
                return null;

            string key = string.Format(YETKILI_BY_ID_KEY, yetkililerId);
            return _önbellekYönetici.Al(key, () => _yetkililerDepo.AlId(yetkililerId));
        }

        public void YetkiliEkle(Yetkililer yetkililer)
        {
            if (yetkililer == null)
                throw new ArgumentNullException("yetkililer");

            _yetkililerDepo.Ekle(yetkililer);
            _önbellekYönetici.KalıpİleSil(YETKILI_PATTERN_KEY);
            _olayYayınlayıcı.OlayEklendi(yetkililer);
        }

        public void YetkiliGüncelle(Yetkililer yetkililer)
        {
            if (yetkililer == null)
                throw new ArgumentNullException("yetkililer");

            _yetkililerDepo.Güncelle(yetkililer);
            _önbellekYönetici.KalıpİleSil(YETKILI_PATTERN_KEY);
            _olayYayınlayıcı.OlayGüncellendi(yetkililer);
        }

        public void YetkiliSil(Yetkililer yetkililer)
        {
            if (yetkililer == null)
                throw new ArgumentNullException("yetkililer");

            _yetkililerDepo.Sil(yetkililer);
            _önbellekYönetici.KalıpİleSil(YETKILI_PATTERN_KEY);
            _olayYayınlayıcı.OlaySilindi(yetkililer);
        }

        public virtual ISayfalıListe<Yetkililer> TümYetkiliAl(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var sorgu = _yetkililerDepo.Tablo.OrderBy(x => x.Id);
            var tümYetkililer = new SayfalıListe<Yetkililer>(sorgu, pageIndex, pageSize);
            return tümYetkililer;
        }
        public ISayfalıListe<Yetkililer> YetkiliAra(int firma,
           string adı, string soyadı, string tckn, string email, bool enYeniler, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var sorgu = _yetkililerDepo.Tablo;
            if (firma > 0)
            {
                //_firmaKategorisiServisi.FirmaKategorisiAlId(firma);
                sorgu = sorgu.Where(x => x.KategoriId == firma);
            }
            if (!String.IsNullOrEmpty(email))
                sorgu = sorgu.Where(qe => qe.Email1.Contains(email) ||qe.Email2.Contains(email));
            if (!String.IsNullOrEmpty(adı))
                sorgu = sorgu.Where(qe => qe.Adı.Contains(adı));
            if (!String.IsNullOrEmpty(soyadı))
                sorgu = sorgu.Where(qe => qe.Soyadı.Contains(soyadı));
            /*if (!String.IsNullOrEmpty(tckn))
                sorgu = sorgu.Where(qe => qe.tckn.Contains(tckn));*/
           /*
            sorgu = enYeniler ?
                sorgu.OrderByDescending(qe => qe.OdemeTarihi) :
                sorgu.OrderByDescending(qe => qe.OdemeTarihi).ThenBy(qe => qe.OdemeTarihi);*/
            sorgu = sorgu.OrderByDescending(qe => qe.Id);
            
            var yetkililer = new SayfalıListe<Yetkililer>(sorgu, pageIndex, pageSize);
            return yetkililer;
        }
    }

}
