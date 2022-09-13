using System.Collections.Generic;

namespace Core.Domain.CRM
{
    public partial class CrmKurum : TemelVarlık
    {
        private ICollection<CrmKongre> _kongreler;
        public string Adı { get; set; }
        public int Tipi { get; set; }
        public int SehirId { get; set; }
        public int IlceId { get; set; }
        public string Adres { get; set; }
        public string Web { get; set; }
        public string Email { get; set; }
        public string Tel { get; set; }
        public string Faks { get; set; }
        public virtual ICollection<CrmKongre> Kongreler
        {
            get { return _kongreler ?? (_kongreler = new List<CrmKongre>()); }
            protected set { Kongreler = value; }
        }
    }
}
