using Core.Data;
using Core.Domain.Kongre;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services.Kongre
{
    public partial class KontenjanBilgileriServisi:IKontenjanBilgileriServisi
    {
        private readonly IDepo<KontenjanBilgileri> _kontenjanDepo;
        public KontenjanBilgileriServisi(IDepo<KontenjanBilgileri> kontenjanDepo)
        {
            this._kontenjanDepo = kontenjanDepo;
        }

        public virtual KontenjanBilgileri KontenjanBilgileriAlId(int kontenjanId)
        {
            if (kontenjanId == 0)
                return null;
            return _kontenjanDepo.AlId(kontenjanId);
        }

        public virtual void KontenjanBilgileriEkle(KontenjanBilgileri kontenjanBilgileri)
        {
            if (kontenjanBilgileri == null)
                throw new ArgumentNullException("kontenjanBilgileri");
            _kontenjanDepo.Ekle(kontenjanBilgileri);
        }

        public virtual void KontenjanBilgileriGüncelle(KontenjanBilgileri kontenjanBilgileri)
        {
            if (kontenjanBilgileri == null)
                throw new ArgumentNullException("kontenjanBilgileri");
            _kontenjanDepo.Güncelle(kontenjanBilgileri);
        }

        public virtual void KontenjanBilgileriSil(KontenjanBilgileri kontenjanBilgileri)
        {
            if (kontenjanBilgileri == null)
                throw new ArgumentNullException("kontenjanBilgileri");
            _kontenjanDepo.Sil(kontenjanBilgileri);
        }
    }
}
