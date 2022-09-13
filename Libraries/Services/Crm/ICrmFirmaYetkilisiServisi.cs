using Core.Domain.CRM;
using System.Collections.Generic;

namespace Services.Crm
{
    public partial interface ICrmFirmaYetkilisiServisi
    {
        void CrmFirmaYetkilisiSil(CrmFirmaYetkilisi crmFirmaYetkilisi);
        CrmFirmaYetkilisi CrmFirmaYetkilisiAlId(int crmFirmaYetkilisiId);
        IList<CrmFirmaYetkilisi> TümCrmFirmaYetkilisiAl();
        IList<CrmFirmaYetkilisi> CrmFirmaYetkilileriAl(int firmaId);
        void CrmFirmaYetkilisiEkle(CrmFirmaYetkilisi crmFirmaYetkilisi);
        void CrmFirmaYetkilisiGüncelle(CrmFirmaYetkilisi crmFirmaYetkilisi);
    }

}
