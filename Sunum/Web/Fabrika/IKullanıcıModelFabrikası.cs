using Core.Domain.Kullanıcılar;
using Web.Models.Kullanıcılar;

namespace Web.Fabrika
{
    public partial interface IKullanıcıModelFabrikası
    {
        KayıtModel KayıtModelHazırla(KayıtModel model, bool excludeProperties,
            string overrideCustomCustomerAttributesXml = "", bool setDefaultValues = false);
        GirişModel GirişModelHazırla();
        KullanıcıBilgiModel KullanıcıBilgiModelHazırla(KullanıcıBilgiModel model, Kullanıcı kullanıcı,
            bool excludeProperties, string overrideCustomCustomerAttributesXml = "");
    }
}