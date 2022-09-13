using Core.Data;
using Core.Domain.Kongre;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services.Kongre
{
    public partial class TakvimServisi:ITakvimServisi
    {
        private readonly IDepo<Takvim> _takvimDepo;
        public TakvimServisi(IDepo<Takvim> takvimDepo)
        {
            this._takvimDepo = takvimDepo;
        }

        public IList<Takvim> TümTakvimAl()
        {
            return _takvimDepo.Tablo.ToList();
        }

        public virtual Takvim TakvimAlId(int takvimId)
        {
            if (takvimId == 0)
                return null;
            return _takvimDepo.AlId(takvimId);
        }

        public virtual void TakvimEkle(Takvim takvim)
        {
            if (takvim == null)
                throw new ArgumentNullException("takvim");
            _takvimDepo.Ekle(takvim);
        }

        public virtual void TakvimGüncelle(Takvim takvim)
        {
            if (takvim == null)
                throw new ArgumentNullException("takvim");
            _takvimDepo.Güncelle(takvim);
        }

        public virtual void TakvimSil(Takvim takvim)
        {
            if (takvim == null)
                throw new ArgumentNullException("takvim");
            _takvimDepo.Sil(takvim);
        }
    }
}
