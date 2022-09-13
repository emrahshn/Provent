using System;
using System.Collections.Generic;
using Web.Framework.Mvc;

namespace Web.Models.Teklif
{
    public class TeklifHariciRaporModel : TemelTSEntityModel
    {
        public IList<int> Idler { get; set; }
        public DateTime Tarih1 { get; set; }
        public DateTime Tarih2 { get; set; }
        public int Yurtici { get; set; }
        public string Text { get; set; }
    }
}