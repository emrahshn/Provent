using AutoMapper;
using Core.Altyapı.Mapper;
using Core.Domain.Blogs;
using Core.Domain.CRM;
using Core.Domain.EkTanımlamalar;
using Core.Domain.Finans;
using Core.Domain.Genel;
using Core.Domain.Görüşmeler;
using Core.Domain.Kongre;
using Core.Domain.KongreTanımlama;
using Core.Domain.Konum;
using Core.Domain.Kullanıcılar;
using Core.Domain.Medya;
using Core.Domain.Mesajlar;
using Core.Domain.Notlar;
using Core.Domain.Tanımlamalar;
using Core.Domain.Teklif;
using Core.Domain.Test;
using System;
using Web.Framework.Güvenlik.Captcha;
using Web.Models.Ayarlar;
using Web.Models.Crm;
using Web.Models.EkTanımlamalar;
using Web.Models.Finans;
using Web.Models.Görüşmeler;
using Web.Models.Kongre;
using Web.Models.KongreTanımlamaları;
using Web.Models.Konum;
using Web.Models.Kullanıcılar;
using Web.Models.Mesajlar;
using Web.Models.Notlar;
using Web.Models.Tanımlamalar;
using Web.Models.Teklif;
using Web.Models.Test;

namespace Web.Altyapı.Mapper
{
    public class MapperAyarları : IMapperAyarları
    {
        public Action<IMapperConfigurationExpression> GetConfiguration()
        {
            Action<IMapperConfigurationExpression> action = cfg =>
            {
                //banka
                cfg.CreateMap<Banka, BankaModel>()
                .ForMember(dest => dest.CustomProperties, mo => mo.Ignore());
                cfg.CreateMap<BankaModel, Banka>();
                //musteri
                cfg.CreateMap<MusteriSektor, MusteriSektorModel>()
                .ForMember(dest => dest.CustomProperties, mo => mo.Ignore());
                cfg.CreateMap<MusteriSektorModel, MusteriSektor>();
                //tedarikci
                cfg.CreateMap<TedarikciSektor, TedarikciSektorModel>()
                .ForMember(dest => dest.CustomProperties, mo => mo.Ignore());
                cfg.CreateMap<TedarikciSektorModel, TedarikciSektor>();
                //harici
                cfg.CreateMap<HariciSektor, HariciSektorModel>()
                .ForMember(dest => dest.CustomProperties, mo => mo.Ignore());
                cfg.CreateMap<HariciSektorModel, HariciSektor>();
                //teklif kalemi
                cfg.CreateMap<TeklifKalemi, TeklifKalemiModel>()
                .ForMember(dest => dest.CustomProperties, mo => mo.Ignore());
                cfg.CreateMap<TeklifKalemiModel, TeklifKalemi>();
                //unvanlar
                cfg.CreateMap<Unvanlar, UnvanlarModel>()
                .ForMember(dest => dest.CustomProperties, mo => mo.Ignore());
                cfg.CreateMap<UnvanlarModel, Unvanlar>();
                //konum
                cfg.CreateMap<Ulke, KonumModel.UlkeModel>()
                .ForMember(dest => dest.CustomProperties, mo => mo.Ignore());
                cfg.CreateMap<KonumModel.UlkeModel, Ulke>();
                cfg.CreateMap<Sehir, KonumModel.SehirModel>()
                .ForMember(dest => dest.CustomProperties, mo => mo.Ignore());
                cfg.CreateMap<KonumModel.SehirModel, Sehir>();
                cfg.CreateMap<Ilce, KonumModel.IlceModel>()
                .ForMember(dest => dest.CustomProperties, mo => mo.Ignore());
                cfg.CreateMap<KonumModel.IlceModel, Ilce>();
                //Teklif
                cfg.CreateMap<Teklif, TeklifModel>()
                .ForMember(dest => dest.CustomProperties, mo => mo.Ignore());
                cfg.CreateMap<TeklifModel, Teklif>();
                //Teklif2
                cfg.CreateMap<Teklif2, Teklif2Model>()
                .ForMember(dest => dest.CustomProperties, mo => mo.Ignore());
                cfg.CreateMap<Teklif2Model, Teklif2>();
                //bagliTeklifOgesi
                cfg.CreateMap<BagliTeklifOgesi2, BagliTeklifOgesi2Model>()
                .ForMember(dest => dest.CustomProperties, mo => mo.Ignore());
                cfg.CreateMap<BagliTeklifOgesi2Model, BagliTeklifOgesi2>();
                //bagliTeklifOgesi
                cfg.CreateMap<BagliTeklifOgesi, BagliTeklifOgesiModel>()
                .ForMember(dest => dest.CustomProperties, mo => mo.Ignore());
                cfg.CreateMap<BagliTeklifOgesiModel, BagliTeklifOgesi>();
                //TeklifHarici
                cfg.CreateMap<TeklifHarici, TeklifHariciModel>()
                .ForMember(dest => dest.CustomProperties, mo => mo.Ignore());
                cfg.CreateMap<TeklifHariciModel, TeklifHarici>();
                //bagliTeklifOgesiHarici
                cfg.CreateMap<BagliTeklifOgesiHarici, BagliTeklifOgesiHariciModel>()
                .ForMember(dest => dest.CustomProperties, mo => mo.Ignore());
                cfg.CreateMap<BagliTeklifOgesiHariciModel, BagliTeklifOgesiHarici>();
                //gorusmeRaporlari
                cfg.CreateMap<GorusmeRaporlari, GorusmeRaporlariModel>()
                .ForMember(dest => dest.CustomProperties, mo => mo.Ignore());
                cfg.CreateMap<GorusmeRaporlariModel, GorusmeRaporlari>();
                //odemeFormu
                cfg.CreateMap<OdemeFormu, OdemeFormuModel>()
                .ForMember(dest => dest.CustomProperties, mo => mo.Ignore());
                cfg.CreateMap<OdemeFormuModel, OdemeFormu>();
                //kullanıcı rolleri
                cfg.CreateMap<KullanıcıRolü, KullanıcıRolModel>()
                    .ForMember(dest => dest.CustomProperties, mo => mo.Ignore());
                cfg.CreateMap<KullanıcıRolModel, KullanıcıRolü>()
                    .ForMember(dest => dest.İzinKayıtları, mo => mo.Ignore());
                //katilimci
                cfg.CreateMap<Katilimci, KatilimciModel>()
                .ForMember(dest => dest.CustomProperties, mo => mo.Ignore());
                cfg.CreateMap<KatilimciModel, Katilimci>();
                //Refakatci
                cfg.CreateMap<Refakatci, RefakatciModel>()
                .ForMember(dest => dest.CustomProperties, mo => mo.Ignore());
                cfg.CreateMap<RefakatciModel, Refakatci>();
                //kayit
                cfg.CreateMap<Kayit, KayitModel>()
                .ForMember(dest => dest.CustomProperties, mo => mo.Ignore())
                .ForMember(dest => dest.Kongre, mo => mo.Ignore());
                cfg.CreateMap<KayitModel, Kayit>()
                .ForMember(dest => dest.Kongre, mo => mo.Ignore());
                //konaklama
                cfg.CreateMap<Konaklama, KonaklamaModel>()
                .ForMember(dest => dest.CustomProperties, mo => mo.Ignore());
                cfg.CreateMap<KonaklamaModel, Konaklama>();
                //kurs
                cfg.CreateMap<Kurs, KursModel>()
                .ForMember(dest => dest.CustomProperties, mo => mo.Ignore());
                cfg.CreateMap<KursModel, Kurs>();
                //transfer
                cfg.CreateMap<Transfer, TransferModel>()
                .ForMember(dest => dest.CustomProperties, mo => mo.Ignore());
                cfg.CreateMap<TransferModel, Transfer>();
                //kongreler
                cfg.CreateMap<Kongreler, KongreModel>()
                .ForMember(dest => dest.CustomProperties, mo => mo.Ignore());
                cfg.CreateMap<KongreModel, Kongreler>();
                //test
                cfg.CreateMap<Test, TestModel>()
                .ForMember(dest => dest.CustomProperties, mo => mo.Ignore());
                cfg.CreateMap<TestModel, Test>();

                cfg.CreateMap<BankaHesapBilgileri, BankaHesapBilgileriModel>();
                cfg.CreateMap<BankaHesapBilgileriModel, BankaHesapBilgileri>();

                //gelirGiderHedefi
                cfg.CreateMap<GelirGiderHedefi, GelirGiderHedefiModel>();
                cfg.CreateMap<GelirGiderHedefiModel, GelirGiderHedefi>();

                //kontenjanBilgileri
                cfg.CreateMap<KontenjanBilgileri, KontenjanBilgileriModel>();
                cfg.CreateMap<KontenjanBilgileriModel, KontenjanBilgileri>();

                //kayıtBilgileri
                cfg.CreateMap<KayıtBilgileri, KayıtBilgileriModel>()
                .ForMember(dest => dest.CustomProperties, mo => mo.Ignore());
                cfg.CreateMap<KayıtBilgileriModel, KayıtBilgileri>();

                //kursBilgileri
                cfg.CreateMap<KursBilgileri, KursBilgileriModel>()
                .ForMember(dest => dest.CustomProperties, mo => mo.Ignore());
                cfg.CreateMap<KursBilgileriModel, KursBilgileri>();

                //genelSponsorluk
                cfg.CreateMap<GenelSponsorluk, GenelSponsorlukModel>()
                .ForMember(dest => dest.CustomProperties, mo => mo.Ignore());
                cfg.CreateMap<GenelSponsorlukModel, GenelSponsorluk>();

                //kursBilgileri
                cfg.CreateMap<Transfer, TransferModel>()
                .ForMember(dest => dest.CustomProperties, mo => mo.Ignore());
                cfg.CreateMap<TransferModel, Transfer>();

                //takvim
                cfg.CreateMap<Takvim, TakvimModel>();
                cfg.CreateMap<TakvimModel, Takvim>();

                //gelirGiderTanımlama
                cfg.CreateMap<GelirGiderTanımlama, GelirGiderTanımlamaModel>()
                .ForMember(dest => dest.CustomProperties, mo => mo.Ignore());
                cfg.CreateMap<GelirGiderTanımlamaModel, GelirGiderTanımlama>();

                //sponsorlukKalemleri
                cfg.CreateMap<SponsorlukKalemleri, SponsorlukKalemleriModel>()
                .ForMember(dest => dest.CustomProperties, mo => mo.Ignore());
                cfg.CreateMap<SponsorlukKalemleriModel, SponsorlukKalemleri>();
                /*
                //sponsorFirmalar
                cfg.CreateMap<SponsorFirmalar, SponsorFirmalarModel>()
                .ForMember(dest => dest.CustomProperties, mo => mo.Ignore());
                cfg.CreateMap<SponsorFirmalarModel, SponsorFirmalar>();
                */

                //sponsorlukSatışı
                cfg.CreateMap<SponsorlukSatışı, SponsorlukSatışıModel>()
                .ForMember(dest => dest.CustomProperties, mo => mo.Ignore());
                cfg.CreateMap<SponsorlukSatışıModel, SponsorlukSatışı>();



                //crmGorev
                cfg.CreateMap<CrmGorev, CrmGorevModel>()
                .ForMember(dest => dest.CustomProperties, mo => mo.Ignore());
                cfg.CreateMap<CrmGorevModel, CrmGorev>();

                //crmUnvan
                cfg.CreateMap<CrmUnvan, CrmUnvanModel>()
                .ForMember(dest => dest.CustomProperties, mo => mo.Ignore());
                cfg.CreateMap<CrmUnvanModel, CrmUnvan>();

                //crmKisi
                cfg.CreateMap<CrmKisi, CrmKisiModel>()
                .ForMember(dest => dest.CustomProperties, mo => mo.Ignore());
                cfg.CreateMap<CrmKisiModel, CrmKisi>();

                //crmKurum
                cfg.CreateMap<CrmKurum, CrmKurumModel>()
                .ForMember(dest => dest.CustomProperties, mo => mo.Ignore());
                cfg.CreateMap<CrmKurumModel, CrmKurum>();

                //crmKongre
                cfg.CreateMap<CrmKongre, CrmKongreModel>()
                .ForMember(dest => dest.CustomProperties, mo => mo.Ignore());
                cfg.CreateMap<CrmKongreModel, CrmKongre>();

                //crmGorusme
                cfg.CreateMap<CrmGorusme, CrmGorusmeModel>()
                .ForMember(dest => dest.CustomProperties, mo => mo.Ignore());
                cfg.CreateMap<CrmGorusmeModel, CrmGorusme>();

                //crmFirmaGorusme
                cfg.CreateMap<CrmFirmaGorusme, CrmFirmaGorusmeModel>()
                .ForMember(dest => dest.CustomProperties, mo => mo.Ignore());
                cfg.CreateMap<CrmFirmaGorusmeModel, CrmFirmaGorusme>();

                //crmYK
                cfg.CreateMap<CrmYonetimKurulu, CrmYonetimKuruluModel>()
                .ForMember(dest => dest.CustomProperties, mo => mo.Ignore());
                cfg.CreateMap<CrmYonetimKuruluModel, CrmYonetimKurulu>();

                //crmFirma
                cfg.CreateMap<CrmFirma, CrmFirmaModel>()
                .ForMember(dest => dest.CustomProperties, mo => mo.Ignore());
                cfg.CreateMap<CrmFirmaModel, CrmFirma>();

                //crmFirma
                cfg.CreateMap<CrmFirmaYetkilisi, CrmFirmaYetkilisiModel>()
                .ForMember(dest => dest.CustomProperties, mo => mo.Ignore());
                cfg.CreateMap<CrmFirmaYetkilisiModel, CrmFirmaYetkilisi>();

                //kullanıcı ayarları
                cfg.CreateMap<KullanıcıAyarları, KullanıcıAyarlarModel.KullanıcılarModel>()
                    .ForMember(dest => dest.CustomProperties, mo => mo.Ignore());
                cfg.CreateMap<KullanıcıAyarlarModel.KullanıcılarModel, KullanıcıAyarları>()
                    .ForMember(dest => dest.HashŞifreFormatı, mo => mo.Ignore())
                    .ForMember(dest => dest.MaksProfilResmiByte, mo => mo.Ignore())
                    .ForMember(dest => dest.OnlineKullanıcıDakikaları, mo => mo.Ignore())
                    .ForMember(dest => dest.SilinenKullanıcılarSonek, mo => mo.Ignore());
                cfg.CreateMap<AdresAyarları, KullanıcıAyarlarModel.AdresAyarlarıModel>()
                    .ForMember(dest => dest.CustomProperties, mo => mo.Ignore());
                cfg.CreateMap<KullanıcıAyarlarModel.AdresAyarlarıModel, AdresAyarları>();

                //blog
                cfg.CreateMap<BlogAyarları, BlogAyarlarModel>()
                    .ForMember(dest => dest.ActiveStoreScopeConfiguration, mo => mo.Ignore())
                    .ForMember(dest => dest.CustomProperties, mo => mo.Ignore());
                cfg.CreateMap<BlogAyarlarModel, BlogAyarları>();

                //Ayarlar
                cfg.CreateMap<CaptchaAyarları, GenelAyarlarModel.CaptchaAyarlarıModel>()
                       .ForMember(dest => dest.MevcutCaptchaSürümleri, mo => mo.Ignore())
                       .ForMember(dest => dest.CustomProperties, mo => mo.Ignore());
                cfg.CreateMap<GenelAyarlarModel.CaptchaAyarlarıModel, CaptchaAyarları>()
                    .ForMember(dest => dest.ReCaptchaTeması, mo => mo.Ignore());

                cfg.CreateMap<MedyaAyarları, MedyaAyarlarıModel>()
                .ForMember(dest => dest.ResimVeritabanındaDepola, mo => mo.Ignore());
                cfg.CreateMap<MedyaAyarlarıModel, MedyaAyarları>()
                    .ForMember(dest => dest.GörüntüKareResimBoyutu, mo => mo.Ignore())
                    .ForMember(dest => dest.AutoCompleteAramaThumbResimBoyutu, mo => mo.Ignore());

                //finans ayarları
                cfg.CreateMap<FinansAyarları, FinansAyarlarModel>()
                .ForMember(dest => dest.CustomProperties, mo => mo.Ignore());
                cfg.CreateMap<FinansAyarlarModel, FinansAyarları>();

                //not
                cfg.CreateMap<Not, NotModel>()
                .ForMember(dest => dest.CustomProperties, mo => mo.Ignore());
                cfg.CreateMap<NotModel, Not>();

                //mesajlar
                cfg.CreateMap<Mesaj, MesajModel>()
                .ForMember(dest => dest.CustomProperties, mo => mo.Ignore());
                cfg.CreateMap<MesajModel, Mesaj>();

                //hekimBranşları
                cfg.CreateMap<HekimBranşları, HekimBranşlarıModel>()
                .ForMember(dest => dest.CustomProperties, mo => mo.Ignore());
                cfg.CreateMap<HekimBranşlarıModel, HekimBranşları>();

                //hekimler
                cfg.CreateMap<Hekimler, HekimlerModel>()
                .ForMember(dest => dest.CustomProperties, mo => mo.Ignore());
                cfg.CreateMap<HekimlerModel, Hekimler>();

                //tedarikciKategorileri
                cfg.CreateMap<TedarikciKategorileri, TedarikciKategorileriModel>()
                .ForMember(dest => dest.CustomProperties, mo => mo.Ignore());
                cfg.CreateMap<TedarikciKategorileriModel, TedarikciKategorileri>();
                
                //kongreFirma
                cfg.CreateMap<Firma, FirmaModel>()
                .ForMember(dest => dest.CustomProperties, mo => mo.Ignore());
                cfg.CreateMap<FirmaModel, Firma>();

                cfg.CreateMap<FirmaKategorisi, FirmaKategorisiModel>()
                .ForMember(dest => dest.CustomProperties, mo => mo.Ignore());
                cfg.CreateMap<FirmaKategorisiModel, FirmaKategorisi>();

                cfg.CreateMap<Yetkililer, YetkililerModel>()
                .ForMember(dest => dest.CustomProperties, mo => mo.Ignore());
                cfg.CreateMap<YetkililerModel, Yetkililer>();
                //kongreFirma
                cfg.CreateMap<KongreTedarikçi, KongreTedarikçiModel>()
                .ForMember(dest => dest.CustomProperties, mo => mo.Ignore());
                cfg.CreateMap<KongreTedarikçiModel, KongreTedarikçi>();

                //kongreGörüşmeRaporları
                cfg.CreateMap<KongreGörüşmeRaporları, KongreGörüşmeRaporlarıModel>()
                .ForMember(dest => dest.CustomProperties, mo => mo.Ignore());
                cfg.CreateMap<KongreGörüşmeRaporlarıModel, KongreGörüşmeRaporları>();

                //kayıtTipi
                cfg.CreateMap<KayıtTipi, KayıtTipiModel>()
                .ForMember(dest => dest.CustomProperties, mo => mo.Ignore());
                cfg.CreateMap<KayıtTipiModel, KayıtTipi>();
            };
            return action;
        }
        public int Order
        {
            get { return 0; }
        }
    }
}