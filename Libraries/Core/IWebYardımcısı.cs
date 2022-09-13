using System.Web;

namespace Core
{
    
    public partial interface IWebYardımcısı
    {
        string UrlYönlendiriciAl();
        string MevcutIpAdresiAl();
        string SayfanınUrlsiniAl(bool sorguİçerir);
        string SayfanınUrlsiniAl(bool sorguİçerir, bool SslKullan);
        bool MevcutBağlantıGüvenli();
        string ServerDeğişkenleri(string ad);
        string SiteHostAl(bool useSsl);
        string SiteKonumuAl();
        string SiteKonumuAl(bool SslKullan);
        bool SabitKaynak(HttpRequest istek);
        string SorguDeğiştir(string url, string sorguDeğiştirme, string anchor);
        string SorguSil(string url, string sorgu);
        T Sorgu<T>(string ad);
        void AppDomainYenidenBaşlat(bool yönlendir = false, string yönlendirmeUrlsi = "");
        bool İstekYönlendirildi { get; }
        bool PostTamamlandı { get; set; }
    }
}
