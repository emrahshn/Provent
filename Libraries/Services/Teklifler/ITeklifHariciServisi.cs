using Core;
using Core.Domain.Teklif;
using System;
using System.Collections.Generic;

namespace Services.Teklifler
{
    public partial interface ITeklifHariciServisi
    {
        void TeklifSil(TeklifHarici TeklifHarici);
        TeklifHarici TeklifAlId(int TeklifId);
        IList<TeklifHarici> TeklifAlPO(string PO);
        IList<TeklifHarici> TeklifAlIds(List<int> ids);
        IList<TeklifHarici> TümTeklifAl();
        void TeklifEkle(TeklifHarici TeklifHarici);
        void TeklifGüncelle(TeklifHarici TeklifHarici);
        ISayfalıListe<TeklifHarici> TeklifAra(string adı, string acenta,string po, string talepno, int belge,
           DateTime? tarihi, DateTime? teslimTarihi, bool enYeniler, int sayfaIndeksi = 0, int sayfaBüyüklüğü = int.MaxValue);
    }
}
