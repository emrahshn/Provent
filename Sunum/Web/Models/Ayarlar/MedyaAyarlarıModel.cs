using System.ComponentModel;
using Web.Framework.Mvc;

namespace Web.Models.Ayarlar
{
    public partial class MedyaAyarlarıModel : TemelTSModel
    {

        [DisplayName("Resim Veritabanında Depola")]
        public bool ResimVeritabanındaDepola { get; set; }

        [DisplayName("Avatar Resim Boyutu")]
        public int AvatarResimBoyutu { get; set; }

        [DisplayName("İlişkilendirilmiş Resim Boyutu")]
        public int IliskilendirilmisResimBoyutu { get; set; }

        [DisplayName("Kategori Thumb Resim Boyutu")]
        public int KategoriThumbResimBoyutu { get; set; }

        [DisplayName("AutoComplete Arama Thumb Resim Boyutu")]
        public int AutoCompleteAramaThumbResimBoyutu { get; set; }

        [DisplayName("Görüntü Kare Resim Boyutu")]
        public int GörüntüKareResimBoyutu { get; set; }

        [DisplayName("Varsayılan Resim Zoom Etkin")]
        public bool VarsayılanResimZoomEtkin { get; set; }

        [DisplayName("Maksimum Resim Boyutu")]
        public int MaksimumResimBoyutu { get; set; }

        [DisplayName("Çoklu Thumb Klasorleri")]
        public bool CokluThumbKlasorleri { get; set; }

        [DisplayName("Varsayılan Resim Kalitesi")]
        public int VarsayılanResimKalitesi { get; set; }

        [DisplayName("Azure Önbellek Kontrol Başlığı")]
        public string AzureOnbellekControlBasligi { get; set; }
    }
}