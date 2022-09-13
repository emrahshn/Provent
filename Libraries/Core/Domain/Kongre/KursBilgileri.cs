using System;

namespace Core.Domain.Kongre
{
    public partial class KursBilgileri : TemelVarlık
    {
        public int KongreId { get; set; }
        public string Adı { get; set; }
        public decimal Tutar { get; set; }
        public int Döviz { get; set; }
        public DateTime Tarih { get; set; }
    }

}
