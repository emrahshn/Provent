using System.Collections.Generic;
using System.Web.Optimization;

namespace Web.Framework.UI
{
    public partial class PaketSıralayıcı : IBundleOrderer
    {
        public virtual IEnumerable<BundleFile> OrderFiles(BundleContext context, IEnumerable<BundleFile> dosyalar)
        {
            return dosyalar;
        }
    }
}
