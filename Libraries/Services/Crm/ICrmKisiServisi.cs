using Core.Domain.CRM;
using System.Collections.Generic;

namespace Services.Crm
{
    public partial interface ICrmKisiServisi
    {
        void CrmKisiSil(CrmKisi crmKisi);
        CrmKisi CrmKisiAlId(int crmKisiId);
        IList<CrmKisi> TümCrmKisiAl();
        void CrmKisiEkle(CrmKisi crmKisi);
        void CrmKisiGüncelle(CrmKisi crmKisi);
    }

}
