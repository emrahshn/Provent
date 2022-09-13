using Core.Domain.CRM;
using System.Collections.Generic;

namespace Services.Crm
{
    public partial interface ICrmFirmaGorusmeServisi
    {
        void CrmFirmaGorusmeSil(CrmFirmaGorusme CrmFirmaGorusme);
        CrmFirmaGorusme CrmFirmaGorusmeAlId(int CrmFirmaGorusmeId);
        IList<CrmFirmaGorusme> TümCrmFirmaGorusmeAl();
        void CrmFirmaGorusmeEkle(CrmFirmaGorusme CrmFirmaGorusme);
        void CrmFirmaGorusmeGüncelle(CrmFirmaGorusme CrmFirmaGorusme);
    }

}
