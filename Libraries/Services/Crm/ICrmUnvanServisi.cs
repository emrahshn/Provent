using Core.Domain.CRM;
using System.Collections.Generic;

namespace Services.Crm
{
    public partial interface ICrmUnvanServisi
    {
        void CrmUnvanSil(CrmUnvan crmUnvan);
        CrmUnvan CrmUnvanAlId(int crmUnvanId);
        IList<CrmUnvan> TümCrmUnvanAl();
        void CrmUnvanEkle(CrmUnvan crmUnvan);
        void CrmUnvanGüncelle(CrmUnvan crmUnvan);
    }

}
