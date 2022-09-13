using System.Web.Routing;

namespace Web.Framework.Mvc.Routes
{
    public interface IRotaSağlayıcı
    {
        void RotaKaydet(RouteCollection rotalar);

        int Öncelik { get; }
    }
}