using Core.Data;
using Core.Domain.Kongre;
using System;

namespace Services.Kongre
{
    public partial class BankaBilgileriServisi:IBankaBilgileriServisi
    {
        private readonly IDepo<BankaHesapBilgileri> _bankaDepo;
        public BankaBilgileriServisi(IDepo<BankaHesapBilgileri> bankaDepo)
        {
            this._bankaDepo = bankaDepo;
        }

        public virtual BankaHesapBilgileri BankaHesapBilgileriAlId(int bankaId)
        {
            if (bankaId == 0)
                return null;
            return _bankaDepo.AlId(bankaId);
        }

        public virtual void BankaHesapBilgileriEkle(BankaHesapBilgileri bankaHesapBilgileri)
        {
            if (bankaHesapBilgileri == null)
                throw new ArgumentNullException("bankaHesapBilgileri");
            _bankaDepo.Ekle(bankaHesapBilgileri);
        }

        public virtual void BankaHesapBilgileriGüncelle(BankaHesapBilgileri bankaHesapBilgileri)
        {
            if (bankaHesapBilgileri == null)
                throw new ArgumentNullException("bankaHesapBilgileri");
            _bankaDepo.Güncelle(bankaHesapBilgileri);
        }

        public virtual void BankaHesapBilgileriSil(BankaHesapBilgileri bankaHesapBilgileri)
        {
            if (bankaHesapBilgileri == null)
                throw new ArgumentNullException("bankaHesapBilgileri");
            _bankaDepo.Sil(bankaHesapBilgileri);
        }
    }
}
