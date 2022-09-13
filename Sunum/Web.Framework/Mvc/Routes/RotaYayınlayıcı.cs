using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Routing;
using Core.Altyapı;
using Core.Eklentiler;

namespace Web.Framework.Mvc.Routes
{
    public class RotaYayınlayıcı : IRotaYayınlayıcı
    {
        protected readonly ITypeFinder tipBulucu;
        public RotaYayınlayıcı(ITypeFinder tipBulucu)
        {
            this.tipBulucu = tipBulucu;
        }
        protected virtual EklentiTanımlayıcı EklentiBul(Type sağlayıcıTipi)
        {
            if (sağlayıcıTipi == null)
                throw new ArgumentNullException("sağlayıcıTipi");

            foreach (var eklenti in EklentiYönetici.ReferenslıEklentiler)
            {
                if (eklenti.ReferanslıAssembly == null)
                    continue;

                if (eklenti.ReferanslıAssembly.FullName == sağlayıcıTipi.Assembly.FullName)
                    return eklenti;
            }

            return null;
        }
        public virtual void RotaKaydet(RouteCollection rotalar)
        {
            var rotaSağlayıcıTipleri = tipBulucu.FindClassesOfType<IRotaSağlayıcı>();
            var rotaSağlayıcıları = new List<IRotaSağlayıcı>();
            foreach (var sağlayıcıTipi in rotaSağlayıcıTipleri)
            {
                //Yüklü eklentileri yoksay
                var eklenti = EklentiBul(sağlayıcıTipi);
                if (eklenti != null && !eklenti.Kuruldu)
                    continue;

                var sağlayıcı = Activator.CreateInstance(sağlayıcıTipi) as IRotaSağlayıcı;
                rotaSağlayıcıları.Add(sağlayıcı);
            }
            rotaSağlayıcıları = rotaSağlayıcıları.OrderByDescending(rp => rp.Öncelik).ToList();
            rotaSağlayıcıları.ForEach(rp => rp.RotaKaydet(rotalar));
        }
    }
}
