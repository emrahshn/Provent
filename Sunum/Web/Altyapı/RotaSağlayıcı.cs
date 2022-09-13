using System.Web.Mvc;
using System.Web.Routing;
using Web.Framework.Mvc.Routes;

namespace Web.Altyapı
{
    public class RotaSağlayıcı : IRotaSağlayıcı
    {

        public void RotaKaydet(RouteCollection rotalar)
        {
            rotalar.MapRoute("HomePage",
                            "",
                            new { controller = "Home", action = "Index" },
                            new[] { "Web.Controllers" });
            rotalar.MapRoute("Giriş",
                            "Giris/",
                            new { controller = "Kullanıcı", action = "Giriş" },
                            new[] { "Web.Controllers" });
            rotalar.MapRoute("Kayıt",
                            "Kayit/",
                            new { controller = "Kullanıcı", action = "Kayıt" },
                            new[] { "Web.Controllers" });
            rotalar.MapRoute("ParolaKurtarma",
                            "ParolaKurtarma",
                            new { controller = "Kullanıcı", action = "ParolaKurtarma" },
                            new[] { "Web.Controllers" });
            rotalar.MapRoute("KullanıcıBilgi",
                            "Kullanıcı/Bilgi",
                            new { controller = "Kullanıcı", action = "Bilgi" },
                            new[] { "Web.Controllers" });
            rotalar.MapRoute("Çıkış",
                            "Çıkış/",
                            new { controller = "Kullanıcı", action = "Çıkış" },
                            new[] { "Web.Controllers" });

            rotalar.MapRoute("Blog",
                            "Blog/",
                            new { controller = "Katalog", action = "Blog" },
                            new[] { "Web.Controllers" });
            rotalar.MapRoute("Forum",
                            "Forum/",
                            new { controller = "Katalog", action = "Forum" },
                            new[] { "Web.Controllers" });
            rotalar.MapRoute("İletişim",
                            "İletişim/",
                            new { controller = "Katalog", action = "İletişim" },
                            new[] { "Web.Controllers" });
            rotalar.MapRoute("Kategori",
                            "Kategori/",
                            new { controller = "Katalog", action = "Kategori" },
                            new[] { "Web.Controllers" });
            rotalar.MapRoute("Yetkililer",
                            "Yetkililer/YetkiliListe/",
                            new { controller = "Yetkililer", action = "YetkiliListe" },
                            new[] { "Web.Controllers" });
            rotalar.MapRoute("SayfalananYetkililer",
                           "Yetkililer/YetkiliListe/Sayfa/{page}",
                           new { controller = "Yetkililer", action = "YetkiliListe" },
                           new { page = @"\d+" },
                           new[] { "Web.Controllers" });
            rotalar.MapRoute("Rehber",
                            "Rehber/RehberListe/",
                            new { controller = "Rehber", action = "RehberListe" },
                            new[] { "Web.Controllers" });
            rotalar.MapRoute("SayfalananKisiler",
                           "Rehber/RehberListe/Sayfa/{page}",
                           new { controller = "Rehber", action = "RehberListe" },
                           new { page = @"\d+" },
                           new[] { "Web.Controllers" });
            rotalar.MapRoute("Kongreler",
                            "Kongre/KongreListe",
                            new { controller = "Kongre", action = "KongreListe" },
                            new[] { "Web.Controllers" });
            rotalar.MapRoute("SayfalananKongreler",
                           "Kongre/KongreListe/Sayfa/{page}",
                           new { controller = "Kongre", action = "KongreListe" },
                           new { page = @"\d+" },
                           new[] { "Web.Controllers" });
            /*
            rotalar.MapRoute("Katılımcı",
                           "Kongre/KatılımcıListe/{kongreId}",
                           new { controller = "Kongre", action = "KatılımcıListe" },
                           new { kongreId = @"\d+" },
                           new[] { "Web.Controllers" });*/
        }
        public int Öncelik
        {
            get
            {
                return 0;
            }
        }
    }
}