namespace Core.Domain.EkTanımlamalar
{
    public partial class GelirGiderTanımlama : TemelVarlık
    {
        public string Adı { get; set; }
        public bool Gelir { get; set; }
        public bool Anabaşlık { get; set; }
        public int? NodeId { get; set; }
    }

}
