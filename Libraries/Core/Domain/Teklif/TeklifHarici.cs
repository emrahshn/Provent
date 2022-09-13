using Core.Domain.Notlar;
using System;
using System.Collections.Generic;

namespace Core.Domain.Teklif
{
    public partial class TeklifHarici : TemelVarlık
    {
        private ICollection<Not> _notlar;
        public string Adı { get; set; }
        public string Po { get; set; }
        public int BelgeTuru { get; set; }
        public int HazırlayanId { get; set; }
        public DateTime Tarih { get; set; }
        public DateTime TeslimTarihi { get; set; }
        public int UlkeId { get; set; }
        public int SehirId { get; set; }
        public string Acenta { get; set; }
        public string TalepNo { get; set; }
        public decimal HizmetBedeli { get; set; }
        public int Parabirimi { get; set; }
        public int Fatura { get; set; }
        public string FaturaNo { get; set; }
        public int FaturaResim { get; set; }
        public bool Onay { get; set; }
        public virtual ICollection<Not> Not
        {
            get { return _notlar ?? (_notlar = new List<Not>()); }
            protected set { Not = value; }
        }
    }
}
