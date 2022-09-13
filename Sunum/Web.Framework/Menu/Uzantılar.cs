using System;
using System.Linq;

namespace Web.Framework.Menu
{
    public static class Uzantılar
    {
        public static bool SistemAdıİçeriyor(this SiteHaritasıNode node, string sistemAdı)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            if (String.IsNullOrWhiteSpace(sistemAdı))
                return false;

            if (sistemAdı.Equals(node.SistemAdı, StringComparison.InvariantCultureIgnoreCase))
                return true;

            return node.ChildNodes.Any(cn => SistemAdıİçeriyor(cn, sistemAdı));
        }
    }
}
