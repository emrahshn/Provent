using Core.Domain.CRM;
using System.Collections.Generic;

namespace Services.Crm
{
    public partial interface ICrmKurumServisi
    {
        void CrmKurumSil(CrmKurum crmKurum);
        CrmKurum CrmKurumAlId(int crmKurumId);
        IList<CrmKurum> TümCrmKurumAl();
        void CrmKurumEkle(CrmKurum crmKurum);
        void CrmKurumGüncelle(CrmKurum crmKurum);
    }

}
