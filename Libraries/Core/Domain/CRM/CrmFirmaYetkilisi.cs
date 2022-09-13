using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.CRM
{
    public partial class CrmFirmaYetkilisi : TemelVarlık
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
        public int FirmaId { get; set; }
        public DateTime DoğumTarihi { get; set; }
        public virtual ICollection<CrmGorusme> Gorusmeler
        {
            get { return _gorusmeler ?? (_gorusmeler = new List<CrmGorusme>()); }
            protected set { Gorusmeler = value; }
        }
    }
}
