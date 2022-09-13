namespace Core.Domain.Kongre
{
    public partial class GelirGiderHedefi : TemelVarlık
    {
        public string Adı { get; set; }
        public decimal BirimFiyat { get; set; }
        public decimal GerçekleşenBirimFiyat { get; set; }
        public int Adet { get; set; }
        public int Gün { get; set; }
        public int GerçekleşenAdet { get; set; }
        public decimal Tutar { get; set; }
        public decimal GerçekleşenTutar { get; set; }
        public decimal Fark { get; set; }
        public decimal GelirYüzde { get; set; }
        public int Döviz { get; set; }
        public bool Gelir { get; set; }
        public int GelirKalemiId { get; set; }
        public bool Locked { get; set; }
    }
}
