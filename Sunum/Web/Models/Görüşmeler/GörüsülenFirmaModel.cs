using System;
using System.ComponentModel;
using System.Web.Mvc;
using Web.Framework.Mvc;

namespace Web.Models.Görüşmeler
{
    public partial class GörüsülenFirmaModel : TemelTSEntityModel
    {
        public string FirmaAdı { get; set; }
        public int Grup { get; set; }
        public int GrupId { get; set; }
    }

}