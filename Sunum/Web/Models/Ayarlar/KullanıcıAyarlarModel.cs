using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Web.Framework.Mvc;

namespace Web.Models.Ayarlar
{
    public partial class KullanıcıAyarlarModel : TemelTSModel
    {
        public KullanıcıAyarlarModel()
        {
            KullanıcıAyarları = new KullanıcılarModel();
            AdresAyarlari = new AdresAyarlarıModel();
            TarihAyarları = new TarihAyarlarıModeli();
            HariciKimlikDoğrulamaAyarları = new HariciKimlikDoğrulamaAyarlarıModel();
        }
        public KullanıcılarModel KullanıcıAyarları { get; set; }
        public AdresAyarlarıModel AdresAyarlari { get; set; }
        public TarihAyarlarıModeli TarihAyarları { get; set; }
        public HariciKimlikDoğrulamaAyarlarıModel HariciKimlikDoğrulamaAyarları { get; set; }

        #region Nested classes

        public partial class KullanıcılarModel : TemelTSModel
        {
            [DisplayName("Kullanıcı adları etkin")]
            public bool KullanıcıAdlarıEtkin { get; set; }

            [DisplayName("Kullanıcı adı değiştirebilir")]
            public bool KullanıcıKullanıcıAdıDeğiştirebilir { get; set; }

            [DisplayName("Kullanıcı adı ugunluğu kontrolü etkin")]
            public bool KullanıcıUgunluğuKontrolüEtkin { get; set; }

            [DisplayName("Kullanıcı kayıt tipi")]
            public int KullanıcıKayıtTipi { get; set; }

            [DisplayName("Kullanıcı avatar yüklemesi etkin")]
            public bool KullanıcıAvatarYüklemesiEtkin { get; set; }

            [DisplayName("Varsayılan avatar etkin")]
            public bool VarsayılanAvatarEtkin { get; set; }

            [DisplayName("Kulllanıcı konumu göster")]
            public bool KulllanıcıKonumuGöster { get; set; }

            [DisplayName("Kullanıcı kayıt tarihi göster")]
            public bool KullanıcıKayıtTarihiGöster { get; set; }

            [DisplayName("Profil görüntüleme etkin")]
            public bool ProfilGörüntülemeEtkin { get; set; }

            [DisplayName("Yeni kullanıcı kaydını bildir")]
            public bool YeniKullanıcıKaydınıBildir { get; set; }

            [DisplayName("Kullanıcı adı formatı")]
            public int KullanıcıAdıFormatı { get; set; }

            [DisplayName("Şifre uzunluğu")]
            public int ŞifreUzunluğu { get; set; }

            [DisplayName("Kopyalanmayan şifre sayıları")]
            public int KopyalanmayanŞifreSayıları { get; set; }

            [DisplayName("Şifre kurtarma linki günü uygunluğu")]
            public int ŞifreKurtarmaLinkiGünüUygunluğu { get; set; }

            [DisplayName("Varsayılan şifre formatı")]
            public int VarsayılanŞifreFormatı { get; set; }

            [DisplayName("Şifre ömrü")]
            public int ŞifreÖmrü { get; set; }

            [DisplayName("Hatalı şifre deneme sayısı")]
            public int HatalıŞifreDenemeSayısı { get; set; }

            [DisplayName("Hatalı şifre kilit dakikası")]
            public int HatalıŞifreKilitDakikası { get; set; }

            [DisplayName("Bülten etkin")]
            public bool BültenEtkin { get; set; }

            [DisplayName("Bülten varsayılan olarak tikli")]
            public bool BültenVarsayılanOlarakTikli { get; set; }

            [DisplayName("Bülten bloğunu gizle")]
            public bool BültenBloğunuGizle { get; set; }

            [DisplayName("Bülten bloğu takip bırakmaya izin verir")]
            public bool BültenBloğuTakipBırakmayaİzinVerir { get; set; }

            [DisplayName("Site son ziyaret edilen sayfa")]
            public bool SiteSonZiyaretEdilenSayfa { get; set; }

            [DisplayName("Email iki defa gir")]
            public bool EmailİkiDefaGir { get; set; }


            [DisplayName("Cinsiyet etkin")]
            public bool CinsiyetEtkin { get; set; }

            [DisplayName("Doğum günü etkin")]
            public bool DoğumGünüEtkin { get; set; }

            [DisplayName("Doğum günü gerekli")]
            public bool DoğumGünüGerekli { get; set; }

            [DisplayName("Doğum günü minimum yaş")]
            [UIHint("Int32Nullable")]
            public int? DoğumGünüMinimumYaş { get; set; }

            [DisplayName("Şirket etkin")]
            public bool ŞirketEtkin { get; set; }

            [DisplayName("Şirket gerekli")]
            public bool ŞirketGerekli { get; set; }

            [DisplayName("Sokak adresi etkin")]
            public bool SokakAdresiEtkin { get; set; }

            [DisplayName("Sokak adresi gerekli")]
            public bool SokakAdresiGerekli { get; set; }

            [DisplayName("Sokak adresi 2 etkin")]
            public bool SokakAdresi2Etkin { get; set; }

            [DisplayName("Sokak adresi 2 gerekli")]
            public bool SokakAdresi2Gerekli { get; set; }

            [DisplayName("Posta kodu etkin")]
            public bool PostaKoduEtkin { get; set; }

            [DisplayName("Posta kodu gerekli")]
            public bool PostaKoduGerekli { get; set; }

            [DisplayName("Şehir etkin")]
            public bool ŞehirEtkin { get; set; }

            [DisplayName("Şehir gerekli")]
            public bool ŞehirGerekli { get; set; }

            [DisplayName("Ülke etkin")]
            public bool ÜlkeEtkin { get; set; }

            [DisplayName("Ülke gerekli")]
            public bool ÜlkeGerekli { get; set; }

            [DisplayName("Telefon etkin")]
            public bool TelEtkin { get; set; }

            [DisplayName("Telefon gerekli")]
            public bool TelGerekli { get; set; }

            [DisplayName("Faks etkin")]
            public bool FaksEtkin { get; set; }

            [DisplayName("Faks gerekli")]
            public bool FaksGerekli { get; set; }

            [DisplayName("Gizlilik sözleşmesi onayı etkin")]
            public bool GizlilikSözleşmesiOnayıEtkin { get; set; }
        }

        public partial class AdresAyarlarıModel : TemelTSModel
        {
            [DisplayName("Şirket etkin")]
            public bool ŞirketEtkin { get; set; }

            [DisplayName("Şirket gerekli")]
            public bool ŞirketGerekli { get; set; }

            [DisplayName("Sokak adresi etkin")]
            public bool SokakAdresiEtkin { get; set; }

            [DisplayName("Sokak adresi gerekli")]
            public bool SokakAdresiGerekli { get; set; }

            [DisplayName("Sokak adresi 2 etkin")]
            public bool SokakAdresi2Etkin { get; set; }

            [DisplayName("Sokak adresi 2 gerekli")]
            public bool SokakAdresi2Gerekli { get; set; }

            [DisplayName("Posta kodu etkin")]
            public bool PostaKoduEtkin { get; set; }

            [DisplayName("Posta kodu gerekli")]
            public bool PostaKoduGerekli { get; set; }

            [DisplayName("Şehir etkin")]
            public bool ŞehirEtkin { get; set; }

            [DisplayName("Şehir gerekli")]
            public bool ŞehirGerekli { get; set; }

            [DisplayName("Ülke etkin")]
            public bool ÜlkeEtkin { get; set; }

            [DisplayName("Telefon etkin")]
            public bool TelEtkin { get; set; }

            [DisplayName("Telefon gerekli")]
            public bool TelGerekli { get; set; }

            [DisplayName("Faks etkin")]
            public bool FaksEtkin { get; set; }

            [DisplayName("Faks gerekli")]
            public bool FaksGerekli { get; set; }
        }

        public partial class TarihAyarlarıModeli : TemelTSModel
        {
            public TarihAyarlarıModeli()
            {
                MevcutZamanDilimleri = new List<SelectListItem>();
            }

            [DisplayName("Kullanıcılar Zaman Dilimi Ayarlayabilir")]
            public bool KullanıcılarZamanDilimiAyarlayabilir { get; set; }

            [DisplayName("Varsayılan Site Zaman Dilimi")]
            public string VarsayılanSiteZamanDilimiId { get; set; }

            [DisplayName("Varsayılan Zaman Dilimleri")]
            public IList<SelectListItem> MevcutZamanDilimleri { get; set; }
        }

        public partial class HariciKimlikDoğrulamaAyarlarıModel : TemelTSModel
        {
            [DisplayName("Harici Kimlik Doğrulamada Otomatik Kayıt")]
            public bool OtomatikKayıtEtkin { get; set; }
        }
        #endregion
    }
}