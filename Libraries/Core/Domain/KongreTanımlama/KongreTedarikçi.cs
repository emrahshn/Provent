
using System;

namespace Core.Domain.KongreTanımlama
{
    public partial class KongreTedarikçi : TemelVarlık
    {
        public string Adı { get; set; }
        public string Soyadı { get; set; }
        public string FirmaAdı { get; set; }
        public string CepTel1 { get; set; }
        public string CepTel2 { get; set; }
        public string Email1 { get; set; }
        public string Email2 { get; set; }
        public int UnvanId { get; set; }
        public string Adres { get; set; }
        public int SehirId { get; set; }
        public int IlceId { get; set; }
        public string PostaKodu { get; set; }
        public DateTime DoğumTarihi { get; set; }
    }
}
