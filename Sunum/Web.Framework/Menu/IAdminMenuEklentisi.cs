using Core.Eklentiler;

namespace Web.Framework.Menu
{
    public interface IAdminMenuEklentisi:IEklenti
    {
        void SiteHaritasıYönet(SiteHaritasıNode rootNode);
    }
}
