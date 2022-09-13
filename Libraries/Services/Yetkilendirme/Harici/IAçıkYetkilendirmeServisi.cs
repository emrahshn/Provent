using Core.Domain.Kullanıcılar;
using System.Collections.Generic;


namespace Services.Yetkilendirme.Harici
{
    public partial interface IAçıkYetkilendirmeServisi
    {
        #region Harici yetkilendirme metodları
        IList<IHariciYetkilendirmeMetodu> AktifHariciYetkilendirmeMetodlarınıYükle(Kullanıcı kullanıcı = null, int siteId = 0);
        IHariciYetkilendirmeMetodu HariciYetkilendirmeMetodlarınıYükleSistemAdı(string sistemAdı);
        IList<IHariciYetkilendirmeMetodu> TümHariciYetkilendirmeMetodlarınıYükle(Kullanıcı kullanıcı = null, int siteId = 0);
        #endregion
        void HariciHesabıKullanıcıİleİlişkilendir(Kullanıcı kullanıcı, AçıkYetkilendirmeParametreleri parametreler);
        bool HesapMevcut(AçıkYetkilendirmeParametreleri parametreler);
        Kullanıcı KullanıcıAl(AçıkYetkilendirmeParametreleri parametreler);
        IList<HariciKimlikDoğrulamaKaydı> HariciTanımlayıcıAl(Kullanıcı kullanıcı);
        void HariciYetkilendirmeKaydıSil(HariciKimlikDoğrulamaKaydı hariciKimlikDoğrulamaKaydı);
        void İlişkiSil(AçıkYetkilendirmeParametreleri parametreler);
    }
}

