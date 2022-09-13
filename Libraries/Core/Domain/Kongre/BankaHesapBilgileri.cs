namespace Core.Domain.Kongre
{
    public partial class BankaHesapBilgileri : TemelVarlık
    {
        public int BankaId { get; set; }
        public string HesapAdı { get; set; }
        public string TlHesabı { get; set; }
        public string DolarHesabı { get; set; }
        public string EuroHesabı { get; set; }
        public string Swift { get; set; }
        public string VergiDairesi { get; set; }
        public string VergiNo { get; set; }

    }
}
