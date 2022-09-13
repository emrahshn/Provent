﻿using System.ComponentModel;
using System.Web.Mvc;
using Web.Framework.Mvc;

namespace Web.Models.EkTanımlamalar
{
    public partial class SponsorlukKalemleriModel : TemelTSEntityModel
    {
        [DisplayName("Adı")]
        [AllowHtml]
        public string Adı { get; set; }
    }

}