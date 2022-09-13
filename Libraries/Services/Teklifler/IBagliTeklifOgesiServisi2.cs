using Core.Domain.Teklif;
using System.Collections.Generic;

namespace Services.Teklifler
{
    public partial interface IBagliTeklifOgesi2Servisi
    {
        void BagliTeklifOgesi2Sil(BagliTeklifOgesi2 bagliTeklifOgesi);
        BagliTeklifOgesi2 BagliTeklifOgesi2AlId(int bagliTeklifOgesiId);
        IList<BagliTeklifOgesi2> BagliTeklifOgesi2AlTeklifId(int teklifId);
        IList<BagliTeklifOgesi2> BagliTeklifOgesi2AlTeklifId(int teklifId,string durumu);
        IList<BagliTeklifOgesi2> TümBagliTeklifOgesi2Al(bool AclYoksay = false, bool gizliOlanlarıGöster = false);
        void BagliTeklifOgesi2Ekle(BagliTeklifOgesi2 bagliTeklifOgesi);
        void BagliTeklifOgesi2Güncelle(BagliTeklifOgesi2 bagliTeklifOgesi);
    }

}
