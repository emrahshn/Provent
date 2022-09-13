using System;

namespace Core.Domain.CRM
{
    public partial class CrmYonetimKurulu : TemelVarlık
    {
        public int KurumId { get; set; }
        public int KisiId { get; set; }
        public int Gorevi { get; set; }
        public DateTime? BaslamaTarihi { get; set; }
        public DateTime? BitisTarihi { get; set; }
        public bool Onceki { get; set; }
        
    }
}
