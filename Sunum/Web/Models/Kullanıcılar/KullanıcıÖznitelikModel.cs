using System.Collections.Generic;
using Web.Framework.Mvc;

namespace Web.Models.Kullanıcılar
{
    public partial class KullanıcıÖznitelikModel : TemelTSModel
    {
        public KullanıcıÖznitelikModel()
        {
            Values = new List<CustomerAttributeValueModel>();
        }

        public string Name { get; set; }

        public bool IsRequired { get; set; }

        /// <summary>
        /// Default value for textboxes
        /// </summary>
        public string DefaultValue { get; set; }

        //public AttributeControlType AttributeControlType { get; set; }

        public IList<CustomerAttributeValueModel> Values { get; set; }

    }

    public partial class CustomerAttributeValueModel : TemelTSModel
    {
        public string Name { get; set; }

        public bool IsPreSelected { get; set; }
    }
}