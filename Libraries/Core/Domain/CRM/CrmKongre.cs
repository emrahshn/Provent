using System;

namespace Core.Domain.CRM
{
    public partial class CrmKongre : TemelVarlık
    {
        public string Adı { get; set; }
        public string Yer { get; set; }
        public DateTime Tarih { get; set; }
        public string Acenta { get; set; }
        public string Web { get; set; }
        public int KatılımcıSayısı { get; set; }
        public int StandSayısı { get; set; }
        public string KatılımUcreti { get; set; }
        public string IhaleYeri { get; set; }
        public DateTime IhaleTarihi { get; set; }
        public int Ilgili { get; set; }
        public string Notlar { get; set; }
    }

}
