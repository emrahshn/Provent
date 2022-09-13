using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Core.Altyapı;

namespace Core.Eklentiler
{
    public class EklentiTanımlayıcı : IComparable<EklentiTanımlayıcı>
    {
        public EklentiTanımlayıcı()
        {
            this.DesteklenenSürümler = new List<string>();
            this.KısıtlıSiteler = new List<int>();
            this.KısıtlıMüsteriRolleriListesi = new List<int>();
        }
        public EklentiTanımlayıcı(Assembly referanslıAssembly, FileInfo orijinalAssemblyDosyası,
            Type eklentiTipi)
            : this()
        {
            this.ReferanslıAssembly = referanslıAssembly;
            this.OrijinalAssemblyDosyası = orijinalAssemblyDosyası;
            this.EklentiTipi = eklentiTipi;
        }
        public virtual string EklentiDosyaAdı { get; set; }
        public virtual Type EklentiTipi { get; set; }
        public virtual Assembly ReferanslıAssembly { get; internal set; }
        public virtual FileInfo OrijinalAssemblyDosyası { get; internal set; }
        public virtual string Grup { get; set; }
        public virtual string KısaAd { get; set; }
        public virtual string SistemAdı { get; set; }
        public virtual string Sürüm { get; set; }
        public virtual IList<string> DesteklenenSürümler { get; set; }
        public virtual string Yazar { get; set; }
        public virtual string Açıklama { get; set; }
        public virtual int GörüntülemeSırası { get; set; }
        public virtual IList<int> KısıtlıSiteler { get; set; }
        public virtual IList<int> KısıtlıMüsteriRolleriListesi { get; set; }
        public virtual bool Kuruldu { get; set; }
        public virtual T Model<T>() where T : class, IEklenti
        {
            object model;
            if (!EngineContext.Current.ContainerManager.TryResolve(EklentiTipi, null, out model))
            {
                //not resolved
                model = EngineContext.Current.ContainerManager.ResolveUnregistered(EklentiTipi);
            }
            var typedModel = model as T;
            if (typedModel != null)
                typedModel.EklentiTanımlayıcı = this;
            return typedModel;
        }

        public IEklenti Model()
        {
            return Model<IEklenti>();
        }

        public int CompareTo(EklentiTanımlayıcı diğer)
        {
            if (GörüntülemeSırası != diğer.GörüntülemeSırası)
                return GörüntülemeSırası.CompareTo(diğer.GörüntülemeSırası);

            return KısaAd.CompareTo(diğer.KısaAd);
        }

        public override string ToString()
        {
            return KısaAd;
        }

        public override bool Equals(object obj)
        {
            var diğer = obj as EklentiTanımlayıcı;
            return diğer != null &&
                SistemAdı != null &&
                SistemAdı.Equals(diğer.SistemAdı);
        }

        public override int GetHashCode()
        {
            return SistemAdı.GetHashCode();
        }
    }
}

