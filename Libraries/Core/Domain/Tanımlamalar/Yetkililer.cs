using System;

namespace Core.Domain.Tanımlamalar
{
    public partial class Yetkililer : TemelVarlık
    {
        public string Adı { get; set; }
        public string Soyadı { get; set; }
        public DateTime DoğumTarihi { get; set; }
        public string CepTel1 { get; set; }
        public string CepTel2 { get; set; }
        public string Email1 { get; set; }
        public string Email2 { get; set; }
        public int UnvanId { get; set; }
        public string Adres { get; set; }
        public int YSehirId { get; set; }
        public int YIlceId { get; set; }
        public int PostaKodu { get; set; }
        public int KategoriId { get; set; }
    }
}
