using System;
using System.Linq;
using System.Runtime.Caching;

namespace Core.Önbellek
{
    public partial class BellekÖnbellekYönetici : IÖnbellekYönetici
    {
        protected ObjectCache Önbellek
        {
            get
            {
                return MemoryCache.Default;
            }
        }
        public virtual T Al<T>(string key)
        {
            return (T)Önbellek[key];
        }
        public virtual void Ayarla(string key, object data, int cacheTime)
        {
            if (data == null)
                return;

            var policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = DateTime.Now + TimeSpan.FromMinutes(cacheTime);
            Önbellek.Add(new CacheItem(key, data), policy);
        }
        public virtual bool Ayarlandı(string key)
        {
            return (Önbellek.Contains(key));
        }
        public virtual void Sil(string key)
        {
            Önbellek.Remove(key);
        }
        public virtual void KalıpİleSil(string pattern)
        {
            this.KalıpİleSil(pattern, Önbellek.Select(p => p.Key));
        }
        public virtual void Temizle()
        {
            foreach (var item in Önbellek)
                Sil(item.Key);
        }
        public virtual void Dispose()
        {
        }
    }
}
