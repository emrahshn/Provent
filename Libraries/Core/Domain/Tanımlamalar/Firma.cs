using System;
using System.Collections.Generic;

namespace Core.Domain.Tanımlamalar
{
    public partial class Firma : TemelVarlık
    {
        private ICollection<Yetkililer> _yetkililer;
        public int KategoriId { get; set; }
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
        public virtual ICollection<Yetkililer> Yetkililer
        {
            get { return _yetkililer ?? (_yetkililer = new List<Yetkililer>()); }
            protected set { Yetkililer = value; }
        }
    }

}
