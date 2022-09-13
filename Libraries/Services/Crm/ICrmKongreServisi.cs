using Core.Domain.CRM;
using System.Collections.Generic;
namespace Services.Crm
{
    public partial interface ICrmKongreServisi
    {
        void CrmKongreSil(CrmKongre crmKongre);
        CrmKongre CrmKongreAlId(int crmKongreId);
        IList<CrmKongre> TümCrmKongreAl();
        void CrmKongreEkle(CrmKongre crmKongre);
        void CrmKongreGüncelle(CrmKongre crmKongre);
    }

}
