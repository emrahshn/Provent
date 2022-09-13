using Core.Domain.Genel;

namespace Core.Domain.Kongre
{
    public static class KongreUzantıları
    {
        public static void BankaHesabıAdresSil(this Kongreler kongre, BankaHesapBilgileri BHB)
        {
            if (kongre.BankaHesapBilgileri.Contains(BHB))
            {
                kongre.BankaHesapBilgileri.Remove(BHB);
            }
        }
    }
}
