using System;

namespace Core.Domain.Kongre
{
    public partial class Transfer : TemelVarlık
    {
        public int KongreId { get; set; }
        public string Adı { get; set; }
        public string Tutar { get; set; }
        public int Döviz { get; set; }
        public string TransferAracı { get; set; }
        public string TransferNotu { get; set; }
        public DateTime Tarih { get; set; }
    }

}
