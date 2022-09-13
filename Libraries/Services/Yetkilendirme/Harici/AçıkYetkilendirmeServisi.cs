using System;
using System.Collections.Generic;
using Core.Domain.Kullanıcılar;
using System.Linq;
using Core.Data;
using Core.Eklentiler;
using Services.Kullanıcılar;

namespace Services.Yetkilendirme.Harici
{
    public partial class AçıkYetkilendirmeServisi : IAçıkYetkilendirmeServisi
    {
        private readonly IDepo<HariciKimlikDoğrulamaKaydı> _hariciKimlikDoğrulamaKaydı;
        private readonly HariciYetkilendirmeAyarları _hariciYetkilendirmeAyarları;
        private readonly IEklentiBulucu _eklentiBulucu;
        private readonly IKullanıcıServisi _kullanıcıServisi;
        public AçıkYetkilendirmeServisi(IDepo<HariciKimlikDoğrulamaKaydı> hariciKimlikDoğrulamaKaydı,
            HariciYetkilendirmeAyarları hariciYetkilendirmeAyarları,
            IEklentiBulucu eklentiBulucu,
            IKullanıcıServisi kullanıcıServisi
            )
        {
            this._hariciKimlikDoğrulamaKaydı = hariciKimlikDoğrulamaKaydı;
            this._hariciYetkilendirmeAyarları = hariciYetkilendirmeAyarları;
            this._eklentiBulucu = eklentiBulucu;
            this._kullanıcıServisi = kullanıcıServisi;
        }
        public IList<IHariciYetkilendirmeMetodu> AktifHariciYetkilendirmeMetodlarınıYükle(Kullanıcı kullanıcı = null, int siteId = 0)
        {
            return TümHariciYetkilendirmeMetodlarınıYükle(kullanıcı, siteId)
                .Where(sağlayıcı => _hariciYetkilendirmeAyarları.AktifYetkilendirmeMetoduSistemAdları
                .Contains(sağlayıcı.EklentiTanımlayıcı.SistemAdı)).ToList();
        }

        public IList<HariciKimlikDoğrulamaKaydı> HariciTanımlayıcıAl(Kullanıcı kullanıcı)
        {
            if (kullanıcı == null)
                throw new ArgumentNullException("kullanıcı");
            return kullanıcı.HariciKimlikDoğrulamaKayıtları.ToList();
        }

        public void HariciHesabıKullanıcıİleİlişkilendir(Kullanıcı kullanıcı, AçıkYetkilendirmeParametreleri parametreler)
        {
            if (kullanıcı == null)
                throw new ArgumentNullException("kullanıcı");
            string email = null;
            if (parametreler.UserClaims != null)
            {
                foreach (var userClaims in parametreler.UserClaims
                    .Where(x => x.Contact != null && !String.IsNullOrEmpty(x.Contact.Email)))
                {
                    email = userClaims.Contact.Email;
                    break;
                }
            }
            var hariciYetkilendirmeKaydı = new HariciKimlikDoğrulamaKaydı
            {
                KullanıcıId = kullanıcı.Id,
                Email = email,
                HariciTanımlayıcı=parametreler.HariciTanımlayıcı,
                HariciGörünümTanımlayıcı=parametreler.HariciGörünümTanımlayıcı,
                OAuthToken=parametreler.OAuthToken,
                OAuthAccessToken=parametreler.OAuthAccessToken,
                SağlayıcıSistemAdı=parametreler.SağlayıcıSistemAdı
            };
            _hariciKimlikDoğrulamaKaydı.Ekle(hariciYetkilendirmeKaydı);
        }

        public void HariciYetkilendirmeKaydıSil(HariciKimlikDoğrulamaKaydı hariciKimlikDoğrulamaKaydı)
        {
            if (hariciKimlikDoğrulamaKaydı == null)
                throw new ArgumentNullException("hariciKimlikDoğrulamaKaydı");
            _hariciKimlikDoğrulamaKaydı.Sil(hariciKimlikDoğrulamaKaydı);
        }

        public IHariciYetkilendirmeMetodu HariciYetkilendirmeMetodlarınıYükleSistemAdı(string sistemAdı)
        {
            var tanımlayıcı = _eklentiBulucu.EklentiTanımlayıcıAlSistemAdı<IHariciYetkilendirmeMetodu>(sistemAdı);
            if (tanımlayıcı != null)
                return tanımlayıcı.Model<IHariciYetkilendirmeMetodu>();
            return null;
        }

        public bool HesapMevcut(AçıkYetkilendirmeParametreleri parametreler)
        {
            return KullanıcıAl(parametreler) != null;
        }

        public Kullanıcı KullanıcıAl(AçıkYetkilendirmeParametreleri parametreler)
        {
            var kayıt = _hariciKimlikDoğrulamaKaydı.Tablo
                .FirstOrDefault(o => o.HariciTanımlayıcı == parametreler.HariciTanımlayıcı && 
                o.SağlayıcıSistemAdı == parametreler.SağlayıcıSistemAdı);
            if (kayıt != null)
                return _kullanıcıServisi.KullanıcıAlId(kayıt.KullanıcıId);
            return null;
        }

        public IList<IHariciYetkilendirmeMetodu> TümHariciYetkilendirmeMetodlarınıYükle(Kullanıcı kullanıcı = null, int siteId = 0)
        {
            return _eklentiBulucu.EklentileriAl<IHariciYetkilendirmeMetodu>(kullanıcı: kullanıcı, siteId: siteId).ToList();
        }

        public void İlişkiSil(AçıkYetkilendirmeParametreleri parametreler)
        {
            var kayıt = _hariciKimlikDoğrulamaKaydı.Tablo
                .Where(o => o.HariciTanımlayıcı == parametreler.HariciTanımlayıcı &&
                o.SağlayıcıSistemAdı == parametreler.SağlayıcıSistemAdı);
            if (kayıt != null)
                _hariciKimlikDoğrulamaKaydı.Sil(kayıt);
        }
    }
}
