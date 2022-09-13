using System;

namespace Core.Domain.Kongre
{
    public partial class KayıtBilgileri : TemelVarlık
    {
        public int KongreId { get; set; }
        public int KayıtTipiId { get; set; }
        public string Adı { get; set; }
        public decimal Tutar { get; set; }
        public int Döviz { get; set; }
        public DateTime Tarihinden { get; set; }
        public DateTime Tarihine { get; set; }
    }

}
