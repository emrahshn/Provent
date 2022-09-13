using Core.Domain.Kongre;

namespace Services.Kongre
{
    public partial interface IBankaBilgileriServisi
    {
        void BankaHesapBilgileriSil(BankaHesapBilgileri bankaHesapBilgileri);
        BankaHesapBilgileri BankaHesapBilgileriAlId(int bankaId);
        void BankaHesapBilgileriEkle(BankaHesapBilgileri bankaHesapBilgileri);
        void BankaHesapBilgileriGüncelle(BankaHesapBilgileri bankaHesapBilgileri);
    }
}
