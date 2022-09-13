using Core.Domain.CRM;
using System.Collections.Generic;

namespace Services.Crm
{
    public partial interface ICrmGorevServisi
    {
        void CrmGorevSil(CrmGorev crmGorev);
        CrmGorev CrmGorevAlId(int crmGorevId);
        IList<CrmGorev> TümCrmGorevAl();
        void CrmGorevEkle(CrmGorev crmGorev);
        void CrmGorevGüncelle(CrmGorev crmGorev);
    }

}
