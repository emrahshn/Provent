using System;

namespace Core.Domain.Kongre
{
    public partial class KontenjanBilgileri : TemelVarlık
    {
        public int KongreId { get; set; }
        public int OtelId { get; set; }
        public string Adı { get; set; }
        public int OdaKisiSayısı { get; set; }
        public decimal Tutar { get; set; }
        public int Döviz { get; set; }
        public DateTime Tarihinden { get; set; }
        public DateTime Tarihine { get; set; }
    }
}
