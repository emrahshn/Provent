using System;

namespace Core.Domain.CRM
{
    public partial class CrmFirmaGorusme : TemelVarlık
    {
        public int YetkiliId { get; set; }
        public DateTime GorusmeTarihi { get; set; }
        public int GorusmeSekli { get; set; }
        public int GorusmeSebebi { get; set; }
        public int Gorusen { get; set; }
        public string Notlar { get; set; }
        public bool UcGun { get; set; }
        public bool UcHafta { get; set; }
        public bool UcAy { get; set; }
    }

}
