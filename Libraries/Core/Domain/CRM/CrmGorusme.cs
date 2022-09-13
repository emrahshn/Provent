using System;

namespace Core.Domain.CRM
{
    public partial class CrmGorusme : TemelVarlık
    {
        public int YetkiliId { get; set; }
        public DateTime GorusmeTarihi { get; set; }
        public int GorusmeSekli { get; set; }
        public int GorusmeSebebi { get; set; }
        public int Gorusen { get; set; }
        public string Notlar { get; set; }
    }

}
