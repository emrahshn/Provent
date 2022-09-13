using Core;
using Core.Domain.Tanımlamalar;
using System.Collections.Generic;

namespace Services.Tanımlamalar
{
    public partial interface IHekimlerServisi
    {
        void HekimlerSil(Hekimler hekimler);
        Hekimler HekimlerAlId(int hekimlerId);
        IList<Hekimler> TümHekimlerAl();
        ISayfalıListe<Hekimler> HekimAra(int brans,
           string adı, string soyadı, string tckn, string email, bool enYeniler, int pageIndex = 0, int pageSize = int.MaxValue);
        void HekimlerEkle(Hekimler hekimler);
        void HekimlerGüncelle(Hekimler hekimler);
    }

}
