using System;
using System.Collections.Generic;

namespace Core.Domain.CRM
{
    public partial class CrmKisi : TemelVarlık
    {
        private ICollection<CrmGorusme> _gorusmeler;
        public string Adı { get; set; }
        public string Soyadı { get; set; }
        public int Unvan { get; set; }
        public string CepTel { get; set; }
        public string Tel { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string Adres { get; set; }
        public int SehirId { get; set; }
        public int IlceId { get; set; }
        public int Cinsiyet { get; set; }
        public int KurumId { get; set; }
        public string Bölüm { get; set; }
        public DateTime DoğumTarihi { get; set; }
        public virtual ICollection<CrmGorusme> Gorusmeler
        {
            get { return _gorusmeler ?? (_gorusmeler = new List<CrmGorusme>()); }
            protected set { Gorusmeler = value; }
        }
    }
}
