using System.Collections.Generic;

namespace Web.Framework.Temalar
{
    public partial interface ITemaSağlayıcı
    {
        TemaAyarları TemaAyarıAl(string temaAdı);

        IList<TemaAyarları> TemaAyarlarıAl();

        bool TemaAyarlarıMevcut(string temaAdı);
    }
}