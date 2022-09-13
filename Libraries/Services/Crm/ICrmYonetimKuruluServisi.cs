using Core.Domain.CRM;
using System.Collections.Generic;

namespace Services.Crm
{
    public partial interface ICrmYonetimKuruluServisi
    {
        void CrmYonetimKuruluSil(CrmYonetimKurulu crmYonetimKurulu);
        CrmYonetimKurulu CrmYonetimKuruluAlId(int crmYonetimKuruluId);
        IList<CrmYonetimKurulu> TümCrmYonetimKuruluAl();
        IList<CrmYonetimKurulu> CrmYonetimKuruluAlKisiId(int siteId);
        IList<CrmYonetimKurulu> CrmYonetimKuruluAlKurumId(int siteId);
        void CrmYonetimKuruluEkle(CrmYonetimKurulu crmYonetimKurulu);
        void CrmYonetimKuruluGüncelle(CrmYonetimKurulu crmYonetimKurulu);
    }

}
