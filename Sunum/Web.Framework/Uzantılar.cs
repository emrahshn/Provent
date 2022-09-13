using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Web.Framework.Kendoui;

namespace Web.Framework
{
    public static class Uzantılar
    {
        public static IEnumerable<T> KomutİçinSayfala<T>(this IEnumerable<T> mevcut, DataSourceİsteği komut)
        {
            return mevcut.Skip((komut.Page - 1) * komut.PageSize).Take(komut.PageSize);
        }
        public static bool SeçimMümkünDeğil(this IList<SelectListItem> öğreler, bool sıfırDeğeriniYoksay = true)
        {
            if (öğreler == null)
                throw new ArgumentNullException("öğeler");

            return öğreler.Count(x => !sıfırDeğeriniYoksay || !x.Value.ToString().Equals("0")) < 2;
        }
    }
}
