using Core;
using Core.Domain.Teklif;
using System;
using System.Collections.Generic;

namespace Services.Teklifler
{
    public partial interface ITeklif2Servisi
    {
        void TeklifSil(Teklif2 Teklif2);
        Teklif2 TeklifAlId(int TeklifId);
        IList<Teklif2> TümTeklifAl(bool AclYoksay = false, bool gizliOlanlarıGöster = false);
        void TeklifEkle(Teklif2 Teklif2);
        void TeklifGüncelle(Teklif2 Teklif2);
        ISayfalıListe<Teklif2> TeklifAra(DateTime? tarihinden = null,
            DateTime? tarihine = null, int hazırlayanId = 0, string adı = "",
            string Konumu = "", string açıklama = "", string durumu = "",
            int sayfaIndeksi = 0, int sayfaBüyüklüğü = int.MaxValue);
    }
}
