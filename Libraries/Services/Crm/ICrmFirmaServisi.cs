using Core.Domain.CRM;
using System.Collections.Generic;

namespace Services.Crm
{
    public partial interface ICrmFirmaServisi
    {
        void CrmFirmaSil(CrmFirma crmFirma);
        CrmFirma CrmFirmaAlId(int crmFirmaId);
        IList<CrmFirma> TümCrmFirmaAl();
        void CrmFirmaEkle(CrmFirma crmFirma);
        void CrmFirmaGüncelle(CrmFirma crmFirma);
    }

}
