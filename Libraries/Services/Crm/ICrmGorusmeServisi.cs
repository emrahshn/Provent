using Core.Domain.CRM;
using System.Collections.Generic;

namespace Services.Crm
{
    public partial interface ICrmGorusmeServisi
    {
        void CrmGorusmeSil(CrmGorusme crmGorusme);
        CrmGorusme CrmGorusmeAlId(int crmGorusmeId);
        IList<CrmGorusme> TümCrmGorusmeAl();
        void CrmGorusmeEkle(CrmGorusme crmGorusme);
        void CrmGorusmeGüncelle(CrmGorusme crmGorusme);
    }

}
