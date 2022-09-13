using System.Collections;
using System.Linq;
using System.Web;

namespace Core.Önbellek
{
    public partial class İstekBaşınaÖnbellekYöneticisi : IÖnbellekYönetici
    {
        private readonly HttpContextBase _context;
        public İstekBaşınaÖnbellekYöneticisi(HttpContextBase context)
        {
            this._context = context;
        }
        protected virtual IDictionary GetItems()
        {
            if (_context != null)
                return _context.Items;

            return null;
        }
        public virtual T Al<T>(string key)
        {
            var items = GetItems();
            if (items == null)
                return default(T);

            return (T)items[key];
        }
        public virtual void Ayarla(string key, object data, int cacheTime)
        {
            var items = GetItems();
            if (items == null)
                return;

            if (data != null)
            {
                if (items.Contains(key))
                    items[key] = data;
                else
                    items.Add(key, data);
            }
        }
        public virtual bool Ayarlandı(string key)
        {
            var items = GetItems();
            if (items == null)
                return false;

            return (items[key] != null);
        }
        public virtual void Sil(string key)
        {
            var items = GetItems();
            if (items == null)
                return;

            items.Remove(key);
        }
        public virtual void KalıpİleSil(string pattern)
        {
            var items = GetItems();
            if (items == null)
                return;

            this.KalıpİleSil(pattern, items.Keys.Cast<object>().Select(p => p.ToString()));
        }
        public virtual void Temizle()
        {
            var items = GetItems();
            if (items == null)
                return;

            items.Clear();
        }
        public virtual void Dispose()
        {
        }
    }
}
