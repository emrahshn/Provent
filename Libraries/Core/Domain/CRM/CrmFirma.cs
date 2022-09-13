using Core.Domain.CRM;
using System;
using System.Collections.Generic;

namespace Core.Domain.CRM
{
    public partial class CrmFirma : TemelVarlık
    {
        private ICollection<CrmFirmaGorusme> _gorusmeler;
        private ICollection<CrmFirmaYetkilisi> _yetkililer;
        public string Adı { get; set; }
        public string TicariUnvan { get; set; }
        public string Tel { get; set; }
        public string CepTel { get; set; }
        public string Faks { get; set; }
        public int SehirId { get; set; }
        public int IlceId { get; set; }
        public string Adres { get; set; }
        public string Web { get; set; }
        public string Email { get; set; }
        public string VergiDairesi { get; set; }
        public string VergiNo { get; set; }
        public DateTime? OlusturulmaTarihi { get; set; }
        public virtual ICollection<CrmFirmaGorusme> Gorusmeler
        {
            get { return _gorusmeler ?? (_gorusmeler = new List<CrmFirmaGorusme>()); }
            protected set { Gorusmeler = value; }
        }
        public virtual ICollection<CrmFirmaYetkilisi> Yetkililer
        {
            get { return _yetkililer ?? (_yetkililer = new List<CrmFirmaYetkilisi>()); }
            protected set { Yetkililer = value; }
        }
    }

}
