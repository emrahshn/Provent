using System;

namespace Core.Domain.Kongre
{
    public partial class KongreGörüşmeRaporları : TemelVarlık
    {
        public int YetkiliId { get; set; }
        public int KongreId { get; set; }
        public int MusteriId { get; set; }
        public DateTime GörüsmeTarihi { get; set; }
        public DateTime OlusturulmaTarihi { get; set; }
        public int GörüsenId { get; set; }
        public string Durumu { get; set; }
        public string Rapor { get; set; }
    }

}
