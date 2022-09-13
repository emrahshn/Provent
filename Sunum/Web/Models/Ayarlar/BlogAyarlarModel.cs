using System.ComponentModel;
using Web.Framework.Mvc;

namespace Web.Models.Ayarlar
{
    public partial class BlogAyarlarModel : TemelTSModel
    {
        public int ActiveStoreScopeConfiguration { get; set; }

        [DisplayName("Blog Etkin")]
        public bool Etkin { get; set; }

        [DisplayName("Girdiler sayfa büyüklüğü")]
        public int GirdilerSayfaBüyüklüğü { get; set; }

        [DisplayName("Ziyaretçiler yorum yapabilir")]
        public bool ZiyaretçilerYorumYapabilir { get; set; }

        [DisplayName("Yeni blog yorumunda uyar")]
        public bool YeniBlogYorumunuUyar { get; set; }

        [DisplayName("Tag sayısı")]
        public int TagSayısı { get; set; }

        [DisplayName("Rss URLsi başlığını göster")]
        public bool RssURLBaşlığıGöster { get; set; }

        [DisplayName("Blog yorumları onaylanmalı")]
        public bool BlogYorumlarıOnaylanmalı { get; set; }

        [DisplayName("Blog yorumlarını tüm sitelerde göster")]
        public bool BlogYorumlarıTümSitelerdeGöster { get; set; }
    }
}