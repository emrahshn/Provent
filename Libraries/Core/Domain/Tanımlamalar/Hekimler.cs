using System;

namespace Core.Domain.Tanımlamalar
{
    public partial class Hekimler : TemelVarlık
    {
        public int BranşId { get; set; }
        public string TCKN { get; set; }
        public string Adı { get; set; }
        public string Soyadı { get; set; }
        public string CepTel1 { get; set; }
        public string CepTel2 { get; set; }
        public string İşTel { get; set; }
        public string Fax { get; set; }
        public string Email1 { get; set; }
        public string Email2 { get; set; }
        public string KurumAdresi { get; set; }
        public string EvAdresi { get; set; }
        public int UlkeId { get; set; }
        public int SehirId { get; set; }
        public int IlceId { get; set; }
        public int PostaKodu { get; set; }
        public DateTime DoğumTarihi { get; set; }
        public string MilesSmilesNo { get; set; }
        public string PasaportNo { get; set; }
        public DateTime PasaportGeçerlilikTarihi { get; set; }
        public string İlgiAlanları { get; set; }
        public int Resim { get; set; }
    }

}
