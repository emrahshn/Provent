using Core.Domain.KongreTanımlama;
using Core.Domain.Tanımlamalar;
using System;
using System.Collections.Generic;

namespace Core.Domain.Kongre
{
    public partial class Kongreler : TemelVarlık
    {
        private ICollection<BankaHesapBilgileri> _bankaHesapBilgileri;
        private ICollection<GelirGiderHedefi> _gelirGiderHedefi;
        private ICollection<KontenjanBilgileri> _kontenjanBilgileri;
        private ICollection<KayıtBilgileri> _kayıtBilgileri;
        private ICollection<KursBilgileri> _kursBilgileri;
        private ICollection<GenelSponsorluk> _genelSponsorluk;
        private ICollection<Transfer> _transfer;
        private ICollection<Firma> _sponsorlukBilgileri;
        private ICollection<Firma> _firmaBilgileri;
        private ICollection<Takvim> _takvimBilgileri;
        private ICollection<KongreGörüşmeRaporları> _görüşmeRaporları;
        public string Kodu { get; set; }
        public string Adı { get; set; }
        public int SehirId { get; set; }
        public int IlceId { get; set; }
        public DateTime BaslamaTarihi { get; set; }
        public DateTime BitisTarihi { get; set; }
        public decimal KurDolar { get; set; }
        public decimal KurEuro { get; set; }
        public string Açıklama { get; set; }
        public string KongreIptalSartları { get; set; }
        
        public virtual ICollection<BankaHesapBilgileri> BankaHesapBilgileri
        {
            get { return _bankaHesapBilgileri ?? (_bankaHesapBilgileri = new List<BankaHesapBilgileri>()); }
            protected set { BankaHesapBilgileri = value; }
        }
        public virtual ICollection<GelirGiderHedefi> GelirGiderHedefi
        {
            get { return _gelirGiderHedefi ?? (_gelirGiderHedefi = new List<GelirGiderHedefi>()); }
            protected set { GelirGiderHedefi = value; }
        }
        public virtual ICollection<KontenjanBilgileri> KontenjanBilgileri
        {
            get { return _kontenjanBilgileri ?? (_kontenjanBilgileri = new List<KontenjanBilgileri>()); }
            protected set { KontenjanBilgileri = value; }
        }
        public virtual ICollection<Firma> FirmaBilgileri
        {
            get { return _firmaBilgileri ?? (_firmaBilgileri = new List<Firma>()); }
            protected set { FirmaBilgileri = value; }
        }
        public virtual ICollection<Firma> SponsorlukBilgileri
        {
            get { return _sponsorlukBilgileri ?? (_sponsorlukBilgileri = new List<Firma>()); }
            protected set { SponsorlukBilgileri = value; }
        }
        public virtual ICollection<Takvim> TakvimBilgileri
        {
            get { return _takvimBilgileri ?? (_takvimBilgileri = new List<Takvim>()); }
            protected set { TakvimBilgileri = value; }
        }
        public virtual ICollection<KayıtBilgileri> KayıtBilgileri
        {
            get { return _kayıtBilgileri ?? (_kayıtBilgileri = new List<KayıtBilgileri>()); }
            protected set { KayıtBilgileri = value; }
        }
        public virtual ICollection<KursBilgileri> KursBilgileri
        {
            get { return _kursBilgileri ?? (_kursBilgileri = new List<KursBilgileri>()); }
            protected set { _kursBilgileri = value; }
        }
        public virtual ICollection<GenelSponsorluk> GenelSponsorluk
        {
            get { return _genelSponsorluk ?? (_genelSponsorluk = new List<GenelSponsorluk>()); }
            protected set { _genelSponsorluk = value; }
        }
        public virtual ICollection<Transfer> Transfer
        {
            get { return _transfer ?? (_transfer = new List<Transfer>()); }
            protected set { _transfer = value; }
        }
        /*
        public virtual ICollection<KongreGörüşmeRaporları> GörüşmeRaporları
        {
            get { return _görüşmeRaporları ?? (_görüşmeRaporları = new List<KongreGörüşmeRaporları>()); }
            protected set { GörüşmeRaporları = value; }
        }
        */
    }
}
