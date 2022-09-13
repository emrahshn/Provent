using Core;
using Core.Domain.Tanımlamalar;

namespace Services.Tanımlamalar
{
    public partial interface IYetkililerServisi
    {
        void YetkiliSil(Yetkililer yetkililer);
        Yetkililer YetkiliAlId(int kongreYetkilileriId);
        ISayfalıListe<Yetkililer> TümYetkiliAl(int pageIndex = 0, int pageSize = int.MaxValue);
        ISayfalıListe<Yetkililer> YetkiliAra(int firma,
           string adı, string soyadı, string tckn,string email, bool enYeniler, int pageIndex = 0, int pageSize = int.MaxValue);
        void YetkiliEkle(Yetkililer kongreYetkilileri);
        void YetkiliGüncelle(Yetkililer kongreYetkilileri);
    }
}
