using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Web.Framework.Mvc
{
    public partial class Parabirimi
    {
        public Parabirimi()
        {
            parabirimiDoldur();
        }
        public List<SelectListItem> parabirimiDoldur()
        {
            var parabirimi = new List<SelectListItem>();
            parabirimi.Add(new SelectListItem() { Text = "TL", Value = "1" });
            parabirimi.Add(new SelectListItem() { Text = "$", Value = "2" });
            parabirimi.Add(new SelectListItem() { Text = "€", Value = "3" });
            return parabirimi;
        }
    }
}
