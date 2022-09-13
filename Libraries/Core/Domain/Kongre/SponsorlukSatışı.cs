namespace Core.Domain.Kongre
{
    public partial class SponsorlukSatışı : TemelVarlık
    {
        public string Adı { get; set; }
        public decimal BirimFiyat { get; set; }
        public int Adet { get; set; }
        public int Gün { get; set; }
        public decimal Tutar { get; set; }
        public int Döviz { get; set; }
        public int SponsorId { get; set; }
        public int KongreId { get; set; }
        public bool Locked { get; set; }
        public int Tipi { get; set; }
        public int OgeId { get; set; }
    }
}
