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

namespace Web.Uzantılar
{
    public static class MappingUzantıları
    {
        public static TDestination MapTo<TSource, TDestination>(this TSource source)
        {
            return AutoMapperAyarları.Mapper.Map<TSource, TDestination>(source);
        }

        public static TDestination MapTo<TSource, TDestination>(this TSource source, TDestination destination)
        {
            return AutoMapperAyarları.Mapper.Map(source, destination);
        }
        #region Bankalar

        public static BankaModel ToModel(this Banka entity)
        {
            return entity.MapTo<Banka, BankaModel>();
        }

        public static Banka ToEntity(this BankaModel model)
        {
            return model.MapTo<BankaModel, Banka>();
        }

        public static Banka ToEntity(this BankaModel model, Banka destination)
        {
            return model.MapTo(destination);
        }

        #endregion
        #region Musteri

        public static MusteriSektorModel ToModel(this MusteriSektor entity)
        {
            return entity.MapTo<MusteriSektor, MusteriSektorModel>();
        }

        public static MusteriSektor ToEntity(this MusteriSektorModel model)
        {
            return model.MapTo<MusteriSektorModel, MusteriSektor>();
        }

        public static MusteriSektor ToEntity(this MusteriSektorModel model, MusteriSektor destination)
        {
            return model.MapTo(destination);
        }

        #endregion
        #region Tedarikci

        public static TedarikciSektorModel ToModel(this TedarikciSektor entity)
        {
            return entity.MapTo<TedarikciSektor, TedarikciSektorModel>();
        }

        public static TedarikciSektor ToEntity(this TedarikciSektorModel model)
        {
            return model.MapTo<TedarikciSektorModel, TedarikciSektor>();
        }

        public static TedarikciSektor ToEntity(this TedarikciSektorModel model, TedarikciSektor destination)
        {
            return model.MapTo(destination);
        }

        #endregion
        #region Harici

        public static HariciSektorModel ToModel(this HariciSektor entity)
        {
            return entity.MapTo<HariciSektor, HariciSektorModel>();
        }

        public static HariciSektor ToEntity(this HariciSektorModel model)
        {
            return model.MapTo<HariciSektorModel, HariciSektor>();
        }

        public static HariciSektor ToEntity(this HariciSektorModel model, HariciSektor destination)
        {
            return model.MapTo(destination);
        }

        #endregion
        #region Teklif Kalemi

        public static TeklifKalemiModel ToModel(this TeklifKalemi entity)
        {
            return entity.MapTo<TeklifKalemi, TeklifKalemiModel>();
        }

        public static TeklifKalemi ToEntity(this TeklifKalemiModel model)
        {
            return model.MapTo<TeklifKalemiModel, TeklifKalemi>();
        }

        public static TeklifKalemi ToEntity(this TeklifKalemiModel model, TeklifKalemi destination)
        {
            return model.MapTo(destination);
        }

        #endregion
        #region Unvanlar

        public static UnvanlarModel ToModel(this Unvanlar entity)
        {
            return entity.MapTo<Unvanlar, UnvanlarModel>();
        }

        public static Unvanlar ToEntity(this UnvanlarModel model)
        {
            return model.MapTo<UnvanlarModel, Unvanlar>();
        }

        public static Unvanlar ToEntity(this UnvanlarModel model, Unvanlar destination)
        {
            return model.MapTo(destination);
        }

        #endregion
        #region Branşlar

        public static HekimBranşlarıModel ToModel(this HekimBranşları entity)
        {
            return entity.MapTo<HekimBranşları, HekimBranşlarıModel>();
        }

        public static HekimBranşları ToEntity(this HekimBranşlarıModel model)
        {
            return model.MapTo<HekimBranşlarıModel, HekimBranşları>();
        }

        public static HekimBranşları ToEntity(this HekimBranşlarıModel model, HekimBranşları destination)
        {
            return model.MapTo(destination);
        }

        #endregion
        #region TedarikciKategorileri

        public static TedarikciKategorileriModel ToModel(this TedarikciKategorileri entity)
        {
            return entity.MapTo<TedarikciKategorileri, TedarikciKategorileriModel>();
        }

        public static TedarikciKategorileri ToEntity(this TedarikciKategorileriModel model)
        {
            return model.MapTo<TedarikciKategorileriModel, TedarikciKategorileri>();
        }

        public static TedarikciKategorileri ToEntity(this TedarikciKategorileriModel model, TedarikciKategorileri destination)
        {
            return model.MapTo(destination);
        }

        #endregion
        #region Ulke

        public static KonumModel.UlkeModel ToModel(this Ulke entity)
        {
            return entity.MapTo<Ulke, KonumModel.UlkeModel>();
        }

        public static Ulke ToEntity(this KonumModel.UlkeModel model)
        {
            return model.MapTo<KonumModel.UlkeModel, Ulke>();
        }

        public static Ulke ToEntity(this KonumModel.UlkeModel model, Ulke destination)
        {
            return model.MapTo(destination);
        }

        #endregion
        #region Sehir

        public static KonumModel.SehirModel ToModel(this Sehir entity)
        {
            return entity.MapTo<Sehir, KonumModel.SehirModel>();
        }

        public static Sehir ToEntity(this KonumModel.SehirModel model)
        {
            return model.MapTo<KonumModel.SehirModel, Sehir>();
        }

        public static Sehir ToEntity(this KonumModel.SehirModel model, Sehir destination)
        {
            return model.MapTo(destination);
        }

        #endregion
        #region İlce

        public static KonumModel.IlceModel ToModel(this Ilce entity)
        {
            return entity.MapTo<Ilce, KonumModel.IlceModel>();
        }

        public static Ilce ToEntity(this KonumModel.IlceModel model)
        {
            return model.MapTo<KonumModel.IlceModel, Ilce>();
        }

        public static Ilce ToEntity(this KonumModel.IlceModel model, Ilce destination)
        {
            return model.MapTo(destination);
        }

        #endregion
        #region GelirGiderTanımlama

        public static GelirGiderTanımlamaModel ToModel(this GelirGiderTanımlama entity)
        {
            return entity.MapTo<GelirGiderTanımlama, GelirGiderTanımlamaModel>();
        }

        public static GelirGiderTanımlama ToEntity(this GelirGiderTanımlamaModel model)
        {
            return model.MapTo<GelirGiderTanımlamaModel, GelirGiderTanımlama>();
        }

        public static GelirGiderTanımlama ToEntity(this GelirGiderTanımlamaModel model, GelirGiderTanımlama destination)
        {
            return model.MapTo(destination);
        }
        #endregion
        #region SponsorlukKalemleri

        public static SponsorlukKalemleriModel ToModel(this SponsorlukKalemleri entity)
        {
            return entity.MapTo<SponsorlukKalemleri, SponsorlukKalemleriModel>();
        }

        public static SponsorlukKalemleri ToEntity(this SponsorlukKalemleriModel model)
        {
            return model.MapTo<SponsorlukKalemleriModel, SponsorlukKalemleri>();
        }

        public static SponsorlukKalemleri ToEntity(this SponsorlukKalemleriModel model, SponsorlukKalemleri destination)
        {
            return model.MapTo(destination);
        }

        #endregion
        #region Branşlar

        public static KayıtTipiModel ToModel(this KayıtTipi entity)
        {
            return entity.MapTo<KayıtTipi, KayıtTipiModel>();
        }

        public static KayıtTipi ToEntity(this KayıtTipiModel model)
        {
            return model.MapTo<KayıtTipiModel, KayıtTipi>();
        }

        public static KayıtTipi ToEntity(this KayıtTipiModel model, KayıtTipi destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Teklif
        public static TeklifModel ToModel(this Teklif entity)
        {
            return entity.MapTo<Teklif, TeklifModel>();
        }

        public static Teklif ToEntity(this TeklifModel model)
        {
            return model.MapTo<TeklifModel, Teklif>();
        }

        public static Teklif ToEntity(this TeklifModel model, Teklif destination)
        {
            return model.MapTo(destination);
        }

        #endregion
        #region Teklif2
        public static Teklif2Model ToModel(this Teklif2 entity)
        {
            return entity.MapTo<Teklif2, Teklif2Model>();
        }

        public static Teklif2 ToEntity(this Teklif2Model model)
        {
            return model.MapTo<Teklif2Model, Teklif2>();
        }

        public static Teklif2 ToEntity(this Teklif2Model model, Teklif2 destination)
        {
            return model.MapTo(destination);
        }

        #endregion
        #region BagliTeklif
        public static BagliTeklifOgesiModel ToModel(this BagliTeklifOgesi entity)
        {
            return entity.MapTo<BagliTeklifOgesi, BagliTeklifOgesiModel>();
        }

        public static BagliTeklifOgesi ToEntity(this BagliTeklifOgesiModel model)
        {
            return model.MapTo<BagliTeklifOgesiModel, BagliTeklifOgesi>();
        }

        public static BagliTeklifOgesi ToEntity(this BagliTeklifOgesiModel model, BagliTeklifOgesi destination)
        {
            return model.MapTo(destination);
        }

        #endregion
        #region BagliTeklif2
        public static BagliTeklifOgesi2Model ToModel(this BagliTeklifOgesi2 entity)
        {
            return entity.MapTo<BagliTeklifOgesi2, BagliTeklifOgesi2Model>();
        }

        public static BagliTeklifOgesi2 ToEntity(this BagliTeklifOgesi2Model model)
        {
            return model.MapTo<BagliTeklifOgesi2Model, BagliTeklifOgesi2>();
        }

        public static BagliTeklifOgesi2 ToEntity(this BagliTeklifOgesi2Model model, BagliTeklifOgesi2 destination)
        {
            return model.MapTo(destination);
        }

        #endregion
        #region TeklifHarici
        public static TeklifHariciModel ToModel(this TeklifHarici entity)
        {
            return entity.MapTo<TeklifHarici, TeklifHariciModel>();
        }

        public static TeklifHarici ToEntity(this TeklifHariciModel model)
        {
            return model.MapTo<TeklifHariciModel, TeklifHarici>();
        }

        public static TeklifHarici ToEntity(this TeklifHariciModel model, TeklifHarici destination)
        {
            return model.MapTo(destination);
        }

        #endregion
        #region BagliTeklifHarici
        public static BagliTeklifOgesiHariciModel ToModel(this BagliTeklifOgesiHarici entity)
        {
            return entity.MapTo<BagliTeklifOgesiHarici, BagliTeklifOgesiHariciModel>();
        }

        public static BagliTeklifOgesiHarici ToEntity(this BagliTeklifOgesiHariciModel model)
        {
            return model.MapTo<BagliTeklifOgesiHariciModel, BagliTeklifOgesiHarici>();
        }

        public static BagliTeklifOgesiHarici ToEntity(this BagliTeklifOgesiHariciModel model, BagliTeklifOgesiHarici destination)
        {
            return model.MapTo(destination);
        }

        #endregion


        #region Firma

        public static FirmaModel ToModel(this Firma entity)
        {
            return entity.MapTo<Firma, FirmaModel>();
        }

        public static Firma ToEntity(this FirmaModel model)
        {
            return model.MapTo<FirmaModel, Firma>();
        }

        public static Firma ToEntity(this FirmaModel model, Firma destination)
        {
            return model.MapTo(destination);
        }

        #endregion
        #region FirmaKategorisi

        public static FirmaKategorisiModel ToModel(this FirmaKategorisi entity)
        {
            return entity.MapTo<FirmaKategorisi, FirmaKategorisiModel>();
        }

        public static FirmaKategorisi ToEntity(this FirmaKategorisiModel model)
        {
            return model.MapTo<FirmaKategorisiModel, FirmaKategorisi>();
        }

        public static FirmaKategorisi ToEntity(this FirmaKategorisiModel model, FirmaKategorisi destination)
        {
            return model.MapTo(destination);
        }

        #endregion
        #region Yetkililer

        public static YetkililerModel ToModel(this Yetkililer entity)
        {
            return entity.MapTo<Yetkililer, YetkililerModel>();
        }

        public static Yetkililer ToEntity(this YetkililerModel model)
        {
            return model.MapTo<YetkililerModel, Yetkililer>();
        }

        public static Yetkililer ToEntity(this YetkililerModel model, Yetkililer destination)
        {
            return model.MapTo(destination);
        }

        #endregion
        #region GörüşmeRaporları
        public static GorusmeRaporlariModel ToModel(this GorusmeRaporlari entity)
        {
            return entity.MapTo<GorusmeRaporlari, GorusmeRaporlariModel>();
        }

        public static GorusmeRaporlari ToEntity(this GorusmeRaporlariModel model)
        {
            return model.MapTo<GorusmeRaporlariModel, GorusmeRaporlari>();
        }

        public static GorusmeRaporlari ToEntity(this GorusmeRaporlariModel model, GorusmeRaporlari destination)
        {
            return model.MapTo(destination);
        }

        #endregion
        #region OdemeFormu
        public static OdemeFormuModel ToModel(this OdemeFormu entity)
        {
            return entity.MapTo<OdemeFormu, OdemeFormuModel>();
        }

        public static OdemeFormu ToEntity(this OdemeFormuModel model)
        {
            return model.MapTo<OdemeFormuModel, OdemeFormu>();
        }

        public static OdemeFormu ToEntity(this OdemeFormuModel model, OdemeFormu destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Kullanıcı Rolleri

        //Kullanıcı Rolleri
        public static KullanıcıRolModel ToModel(this KullanıcıRolü entity)
        {
            return entity.MapTo<KullanıcıRolü, KullanıcıRolModel>();
        }

        public static KullanıcıRolü ToEntity(this KullanıcıRolModel model)
        {
            return model.MapTo<KullanıcıRolModel, KullanıcıRolü>();
        }

        public static KullanıcıRolü ToEntity(this KullanıcıRolModel model, KullanıcıRolü destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Kongre
        #region Konaklama
        //Konaklama
        public static KonaklamaModel ToModel(this Konaklama entity)
        {
            return entity.MapTo<Konaklama, KonaklamaModel>();
        }

        public static Konaklama ToEntity(this KonaklamaModel model)
        {
            return model.MapTo<KonaklamaModel, Konaklama>();
        }

        public static Konaklama ToEntity(this KonaklamaModel model, Konaklama destination)
        {
            return model.MapTo(destination);
        }
        #endregion
        #region Katilimci
        //Konaklama
        public static KatilimciModel ToModel(this Katilimci entity)
        {
            return entity.MapTo<Katilimci, KatilimciModel>();
        }

        public static Katilimci ToEntity(this KatilimciModel model)
        {
            return model.MapTo<KatilimciModel, Katilimci>();
        }

        public static Katilimci ToEntity(this KatilimciModel model, Katilimci destination)
        {
            return model.MapTo(destination);
        }
        #endregion
        #region Refakatci
        //Refakatci
        public static RefakatciModel ToModel(this Refakatci entity)
        {
            return entity.MapTo<Refakatci, RefakatciModel>();
        }

        public static Refakatci ToEntity(this RefakatciModel model)
        {
            return model.MapTo<RefakatciModel, Refakatci>();
        }

        public static Refakatci ToEntity(this RefakatciModel model, Refakatci destination)
        {
            return model.MapTo(destination);
        }
        #endregion
        #region Kayit
        //Konaklama
        public static KayitModel ToModel(this Kayit entity)
        {
            return entity.MapTo<Kayit, KayitModel>();
        }

        public static Kayit ToEntity(this KayitModel model)
        {
            return model.MapTo<KayitModel, Kayit>();
        }

        public static Kayit ToEntity(this KayitModel model, Kayit destination)
        {
            return model.MapTo(destination);
        }
        #endregion
        #region Kurs
        //Konaklama
        public static KursModel ToModel(this Kurs entity)
        {
            return entity.MapTo<Kurs, KursModel>();
        }

        public static Kurs ToEntity(this KursModel model)
        {
            return model.MapTo<KursModel, Kurs>();
        }

        public static Kurs ToEntity(this KursModel model, Kurs destination)
        {
            return model.MapTo(destination);
        }
        #endregion
        #region Transfer
        //Konaklama
        public static TransferModel ToModel(this Transfer entity)
        {
            return entity.MapTo<Transfer, TransferModel>();
        }

        public static Transfer ToEntity(this TransferModel model)
        {
            return model.MapTo<TransferModel, Transfer>();
        }

        public static Transfer ToEntity(this TransferModel model, Transfer destination)
        {
            return model.MapTo(destination);
        }
        #endregion
        #region Kongre
        //Kongre
        public static KongreModel ToModel(this Kongreler entity)
        {
            return entity.MapTo<Kongreler, KongreModel>();
        }

        public static Kongreler ToEntity(this KongreModel model)
        {
            return model.MapTo<KongreModel, Kongreler>();
        }

        public static Kongreler ToEntity(this KongreModel model, Kongreler destination)
        {
            return model.MapTo(destination);
        }
        #endregion
        #region Kontenjan
        //Kontenjan
        public static KontenjanModel ToModel(this Kontenjan entity)
        {
            return entity.MapTo<Kontenjan, KontenjanModel>();
        }

        public static Kontenjan ToEntity(this KontenjanModel model)
        {
            return model.MapTo<KontenjanModel, Kontenjan>();
        }

        public static Kontenjan ToEntity(this KontenjanModel model, Kontenjan destination)
        {
            return model.MapTo(destination);
        }
        #endregion
        #region BankaHesapBilgileri
        public static BankaHesapBilgileriModel ToModel(this BankaHesapBilgileri entity)
        {
            return entity.MapTo<BankaHesapBilgileri, BankaHesapBilgileriModel>();
        }

        public static BankaHesapBilgileri ToEntity(this BankaHesapBilgileriModel model)
        {
            return model.MapTo<BankaHesapBilgileriModel, BankaHesapBilgileri>();
        }

        public static BankaHesapBilgileri ToEntity(this BankaHesapBilgileriModel model, BankaHesapBilgileri destination)
        {
            return model.MapTo(destination);
        }
        #endregion
        #region GelirGiderHedefi
        public static GelirGiderHedefiModel ToModel(this GelirGiderHedefi entity)
        {
            return entity.MapTo<GelirGiderHedefi, GelirGiderHedefiModel>();
        }

        public static GelirGiderHedefi ToEntity(this GelirGiderHedefiModel model)
        {
            return model.MapTo<GelirGiderHedefiModel, GelirGiderHedefi>();
        }

        public static GelirGiderHedefi ToEntity(this GelirGiderHedefiModel model, GelirGiderHedefi destination)
        {
            return model.MapTo(destination);
        }
        #endregion
        #region KontenjanBilgileri
        public static KontenjanBilgileriModel ToModel(this KontenjanBilgileri entity)
        {
            return entity.MapTo<KontenjanBilgileri, KontenjanBilgileriModel>();
        }

        public static KontenjanBilgileri ToEntity(this KontenjanBilgileriModel model)
        {
            return model.MapTo<KontenjanBilgileriModel, KontenjanBilgileri>();
        }

        public static KontenjanBilgileri ToEntity(this KontenjanBilgileriModel model, KontenjanBilgileri destination)
        {
            return model.MapTo(destination);
        }
        #endregion
        #region KayıtBilgileri
        public static KayıtBilgileriModel ToModel(this KayıtBilgileri entity)
        {
            return entity.MapTo<KayıtBilgileri, KayıtBilgileriModel>();
        }

        public static KayıtBilgileri ToEntity(this KayıtBilgileriModel model)
        {
            return model.MapTo<KayıtBilgileriModel, KayıtBilgileri>();
        }

        public static KayıtBilgileri ToEntity(this KayıtBilgileriModel model, KayıtBilgileri destination)
        {
            return model.MapTo(destination);
        }
        #endregion
        #region KursBilgileri
        public static KursBilgileriModel ToModel(this KursBilgileri entity)
        {
            return entity.MapTo<KursBilgileri, KursBilgileriModel>();
        }

        public static KursBilgileri ToEntity(this KursBilgileriModel model)
        {
            return model.MapTo<KursBilgileriModel, KursBilgileri>();
        }

        public static KursBilgileri ToEntity(this KursBilgileriModel model, KursBilgileri destination)
        {
            return model.MapTo(destination);
        }
        #endregion
        #region GenelSponsorlukBilgileri
        public static GenelSponsorlukModel ToModel(this GenelSponsorluk entity)
        {
            return entity.MapTo<GenelSponsorluk, GenelSponsorlukModel>();
        }

        public static GenelSponsorluk ToEntity(this GenelSponsorlukModel model)
        {
            return model.MapTo<GenelSponsorlukModel, GenelSponsorluk>();
        }

        public static GenelSponsorluk ToEntity(this GenelSponsorlukModel model, GenelSponsorluk destination)
        {
            return model.MapTo(destination);
        }
        #endregion
        #region Takvim
        public static TakvimModel ToModel(this Takvim entity)
        {
            return entity.MapTo<Takvim, TakvimModel>();
        }

        public static Takvim ToEntity(this TakvimModel model)
        {
            return model.MapTo<TakvimModel, Takvim>();
        }

        public static Takvim ToEntity(this TakvimModel model, Takvim destination)
        {
            return model.MapTo(destination);
        }
        #endregion
        #region SponsorlukSatışı
        public static SponsorlukSatışıModel ToModel(this SponsorlukSatışı entity)
        {
            return entity.MapTo<SponsorlukSatışı, SponsorlukSatışıModel>();
        }

        public static SponsorlukSatışı ToEntity(this SponsorlukSatışıModel model)
        {
            return model.MapTo<SponsorlukSatışıModel, SponsorlukSatışı>();
        }

        public static SponsorlukSatışı ToEntity(this SponsorlukSatışıModel model, SponsorlukSatışı destination)
        {
            return model.MapTo(destination);
        }
        #endregion
        #endregion
        #region KongreTanımlamaları
        #region KongreTedarikçi

        public static KongreTedarikçiModel ToModel(this KongreTedarikçi entity)
        {
            return entity.MapTo<KongreTedarikçi, KongreTedarikçiModel>();
        }

        public static KongreTedarikçi ToEntity(this KongreTedarikçiModel model)
        {
            return model.MapTo<KongreTedarikçiModel, KongreTedarikçi>();
        }

        public static KongreTedarikçi ToEntity(this KongreTedarikçiModel model, KongreTedarikçi destination)
        {
            return model.MapTo(destination);
        }

        #endregion
        #region Hekimler
        public static HekimlerModel ToModel(this Hekimler entity)
        {
            return entity.MapTo<Hekimler, HekimlerModel>();
        }

        public static Hekimler ToEntity(this HekimlerModel model)
        {
            return model.MapTo<HekimlerModel, Hekimler>();
        }

        public static Hekimler ToEntity(this HekimlerModel model, Hekimler destination)
        {
            return model.MapTo(destination);
        }
        #endregion
        #endregion

        #region Crm
        #region Görev
        //Görev
        public static CrmGorevModel ToModel(this CrmGorev entity)
        {
            return entity.MapTo<CrmGorev, CrmGorevModel>();
        }

        public static CrmGorev ToEntity(this CrmGorevModel model)
        {
            return model.MapTo<CrmGorevModel, CrmGorev>();
        }

        public static CrmGorev ToEntity(this CrmGorevModel model, CrmGorev destination)
        {
            return model.MapTo(destination);
        }
        #endregion
        #region Kişi
        //Kişi
        public static CrmKisiModel ToModel(this CrmKisi entity)
        {
            return entity.MapTo<CrmKisi, CrmKisiModel>();
        }

        public static CrmKisi ToEntity(this CrmKisiModel model)
        {
            return model.MapTo<CrmKisiModel, CrmKisi>();
        }

        public static CrmKisi ToEntity(this CrmKisiModel model, CrmKisi destination)
        {
            return model.MapTo(destination);
        }
        #endregion
        #region Kurum
        //Kurum
        public static CrmUnvanModel ToModel(this CrmUnvan entity)
        {
            return entity.MapTo<CrmUnvan, CrmUnvanModel>();
        }

        public static CrmUnvan ToEntity(this CrmUnvanModel model)
        {
            return model.MapTo<CrmUnvanModel, CrmUnvan>();
        }

        public static CrmUnvan ToEntity(this CrmUnvanModel model, CrmUnvan destination)
        {
            return model.MapTo(destination);
        }
        #endregion
        #region Ünvan
        //Ünvan
        public static CrmKurumModel ToModel(this CrmKurum entity)
        {
            return entity.MapTo<CrmKurum, CrmKurumModel>();
        }

        public static CrmKurum ToEntity(this CrmKurumModel model)
        {
            return model.MapTo<CrmKurumModel, CrmKurum>();
        }

        public static CrmKurum ToEntity(this CrmKurumModel model, CrmKurum destination)
        {
            return model.MapTo(destination);
        }
        #endregion
        #region Kongre
        //Kongre
        public static CrmKongreModel ToModel(this CrmKongre entity)
        {
            return entity.MapTo<CrmKongre, CrmKongreModel>();
        }

        public static CrmKongre ToEntity(this CrmKongreModel model)
        {
            return model.MapTo<CrmKongreModel, CrmKongre>();
        }

        public static CrmKongre ToEntity(this CrmKongreModel model, CrmKongre destination)
        {
            return model.MapTo(destination);
        }
        #endregion
        #region Gorusme
        //Gorusme
        public static CrmGorusmeModel ToModel(this CrmGorusme entity)
        {
            return entity.MapTo<CrmGorusme, CrmGorusmeModel>();
        }

        public static CrmGorusme ToEntity(this CrmGorusmeModel model)
        {
            return model.MapTo<CrmGorusmeModel, CrmGorusme>();
        }

        public static CrmGorusme ToEntity(this CrmGorusmeModel model, CrmGorusme destination)
        {
            return model.MapTo(destination);
        }
        #endregion
        #region Firma Gorusme
        //Firma Gorusme
        public static CrmFirmaGorusmeModel ToModel(this CrmFirmaGorusme entity)
        {
            return entity.MapTo<CrmFirmaGorusme, CrmFirmaGorusmeModel>();
        }

        public static CrmFirmaGorusme ToEntity(this CrmFirmaGorusmeModel model)
        {
            return model.MapTo<CrmFirmaGorusmeModel, CrmFirmaGorusme>();
        }

        public static CrmFirmaGorusme ToEntity(this CrmFirmaGorusmeModel model, CrmFirmaGorusme destination)
        {
            return model.MapTo(destination);
        }
        #endregion
        #region YK
        //Yk
        public static CrmYonetimKuruluModel ToModel(this CrmYonetimKurulu entity)
        {
            return entity.MapTo<CrmYonetimKurulu, CrmYonetimKuruluModel>();
        }

        public static CrmYonetimKurulu ToEntity(this CrmYonetimKuruluModel model)
        {
            return model.MapTo<CrmYonetimKuruluModel, CrmYonetimKurulu>();
        }

        public static CrmYonetimKurulu ToEntity(this CrmYonetimKuruluModel model, CrmYonetimKurulu destination)
        {
            return model.MapTo(destination);
        }
        #endregion
        #region Firma
        //Firma
        public static CrmFirmaModel ToModel(this CrmFirma entity)
        {
            return entity.MapTo<CrmFirma, CrmFirmaModel>();
        }

        public static CrmFirma ToEntity(this CrmFirmaModel model)
        {
            return model.MapTo<CrmFirmaModel, CrmFirma>();
        }

        public static CrmFirma ToEntity(this CrmFirmaModel model, CrmFirma destination)
        {
            return model.MapTo(destination);
        }
        #endregion
        #region Firma Yetkilisi
        //Firma
        public static CrmFirmaYetkilisiModel ToModel(this CrmFirmaYetkilisi entity)
        {
            return entity.MapTo<CrmFirmaYetkilisi, CrmFirmaYetkilisiModel>();
        }

        public static CrmFirmaYetkilisi ToEntity(this CrmFirmaYetkilisiModel model)
        {
            return model.MapTo<CrmFirmaYetkilisiModel, CrmFirmaYetkilisi>();
        }

        public static CrmFirmaYetkilisi ToEntity(this CrmFirmaYetkilisiModel model, CrmFirmaYetkilisi destination)
        {
            return model.MapTo(destination);
        }
        #endregion
        #endregion
        #region Ayarlar
        #region Captcha
        public static GenelAyarlarModel.CaptchaAyarlarıModel ToModel(this CaptchaAyarları entity)
        {
            return entity.MapTo<CaptchaAyarları, GenelAyarlarModel.CaptchaAyarlarıModel>();
        }
        public static CaptchaAyarları ToEntity(this GenelAyarlarModel.CaptchaAyarlarıModel model, CaptchaAyarları destination)
        {
            return model.MapTo(destination);
        }
        #endregion
        #region Kullanıcı ayarları
        public static KullanıcıAyarlarModel.KullanıcılarModel ToModel(this KullanıcıAyarları entity)
        {
            return entity.MapTo<KullanıcıAyarları, KullanıcıAyarlarModel.KullanıcılarModel>();
        }
        public static KullanıcıAyarları ToEntity(this KullanıcıAyarlarModel.KullanıcılarModel model, KullanıcıAyarları destination)
        {
            return model.MapTo(destination);
        }
        public static KullanıcıAyarlarModel.AdresAyarlarıModel ToModel(this AdresAyarları entity)
        {
            return entity.MapTo<AdresAyarları, KullanıcıAyarlarModel.AdresAyarlarıModel>();
        }
        public static AdresAyarları ToEntity(this KullanıcıAyarlarModel.AdresAyarlarıModel model, AdresAyarları destination)
        {
            return model.MapTo(destination);
        }
        #endregion
        #region  Blog ayarları
        public static BlogAyarlarModel ToModel(this BlogAyarları entity)
        {
            return entity.MapTo<BlogAyarları, BlogAyarlarModel>();
        }

        public static BlogAyarları ToEntity(this BlogAyarlarModel model)
        {
            return model.MapTo<BlogAyarlarModel, BlogAyarları>();
        }

        public static BlogAyarları ToEntity(this BlogAyarlarModel model, BlogAyarları destination)
        {
            return model.MapTo(destination);
        }
        #endregion
        #region Medya Ayarları
        public static MedyaAyarlarıModel ToModel(this MedyaAyarları entity)
        {
            return entity.MapTo<MedyaAyarları, MedyaAyarlarıModel>();
        }
        public static MedyaAyarları ToEntity(this MedyaAyarlarıModel model, MedyaAyarları destination)
        {
            return model.MapTo(destination);
        }
        #endregion
        #endregion

        #region Notlar
        public static NotModel ToModel(this Not entity)
        {
            return entity.MapTo<Not, NotModel>();
        }

        public static Not ToEntity(this NotModel model)
        {
            return model.MapTo<NotModel, Not>();
        }

        public static Not ToEntity(this NotModel model, Not destination)
        {
            return model.MapTo(destination);
        }

        #endregion
        #region Mesajlar
        public static MesajModel ToModel(this Mesaj entity)
        {
            return entity.MapTo<Mesaj, MesajModel>();
        }

        public static Mesaj ToEntity(this MesajModel model)
        {
            return model.MapTo<MesajModel, Mesaj>();
        }

        public static Mesaj ToEntity(this MesajModel model, Mesaj destination)
        {
            return model.MapTo(destination);
        }

        #endregion
        #region Test
        public static TestModel ToModel(this Test entity)
        {
            return entity.MapTo<Test, TestModel>();
        }

        public static Test ToEntity(this TestModel model)
        {
            return model.MapTo<TestModel, Test>();
        }

        public static Test ToEntity(this TestModel model, Test destination)
        {
            return model.MapTo(destination);
        }

        #endregion
    }
}