namespace Kigg.Web.Caching
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Caching;

    using Cache = System.Web.Caching.Cache;

    using Infrastructure;

    public class CacheManager : ICacheManager
    {
        private static readonly string keyPrefix = typeof(CacheManager).FullName;
        private static readonly object syncLock = new object();

        private readonly Cache cache;

        public CacheManager(Cache cache)
        {
            Check.Argument.IsNotNull(cache, "cache");

            this.cache = cache;
        }

        public TValue Get<TValue>(string key)
        {
            return (TValue)HttpRuntime.Cache[MakeKey(key)];
        }

        public void Set<TValue>(string key, Func<TValue> value)
        {
            Set(key, null, null, null, value, null);
        }

        public void Set<TValue>(string key, DateTime timestamp, TValue value)
        {
            Set(key, timestamp, null, null, value, null);
        }

        public void Set<TValue>(string key, TimeSpan duration, TValue value)
        {
            Set(key, null, duration, null, value, null);
        }

        public void Set<TValue>(string key, TValue value, Action<bool> onRemoveCallback)
        {
            Set(key, null, null, null, value, onRemoveCallback);
        }

        public void Set<TValue>(string key, DateTime timestamp, TValue value, Action<bool> onRemoveCallback)
        {
            Set(key, timestamp, null, null, value, onRemoveCallback);
        }

        public void Set<TValue>(string key, TimeSpan duration, TValue value, Action<bool> onRemoveCallback)
        {
            Set(key, null, duration, null, value, onRemoveCallback);
        }

        public void Set<TValue>(string key, IEnumerable<string> fileDependencies, TValue value)
        {
            Set(key, null, null, fileDependencies, value, null);
        }

        public void Set<TValue>(string key, DateTime timestamp, IEnumerable<string> fileDependencies, TValue value)
        {
            Set(key, timestamp, null, fileDependencies, value, null);
        }

        public void Set<TValue>(string key, TimeSpan duration, IEnumerable<string> fileDependencies, TValue value)
        {
            Set(key, null, duration, fileDependencies, value, null);
        }

        public void Set<TValue>(string key, IEnumerable<string> fileDependencies, TValue value, Action<bool> onRemoveCallback)
        {
            Set(key, null, null, fileDependencies, value, onRemoveCallback);
        }

        public void Set<TValue>(string key, DateTime timestamp, IEnumerable<string> fileDependencies, TValue value, Action<bool> onRemoveCallback)
        {
            Set(key, timestamp, null, fileDependencies, value, onRemoveCallback);
        }

        public void Set<TValue>(string key, TimeSpan duration, IEnumerable<string> fileDependencies, TValue value, Action<bool> onRemoveCallback)
        {
            Set(key, null, duration, fileDependencies, value, onRemoveCallback);
        }

        public TValue GetOrCreate<TValue>(string key, Func<TValue> factory)
        {
            return GetOrCreate(key, null, null, null, factory, null);
        }

        public TValue GetOrCreate<TValue>(string key, DateTime timestamp, Func<TValue> factory)
        {
            return GetOrCreate(key, timestamp, null, null, factory, null);
        }

        public TValue GetOrCreate<TValue>(string key, TimeSpan duration, Func<TValue> factory)
        {
            return GetOrCreate(key, null, duration, null, factory, null);
        }

        public TValue GetOrCreate<TValue>(string key, Func<TValue> factory, Action<bool> onRemoveCallback)
        {
            return GetOrCreate(key, null, null, null, factory, onRemoveCallback);
        }

        public TValue GetOrCreate<TValue>(string key, DateTime timestamp, Func<TValue> factory, Action<bool> onRemoveCallback)
        {
            return GetOrCreate(key, timestamp, null, null, factory, onRemoveCallback);
        }

        public TValue GetOrCreate<TValue>(string key, TimeSpan duration, Func<TValue> factory, Action<bool> onRemoveCallback)
        {
            return GetOrCreate(key, null, duration, null, factory, onRemoveCallback);
        }

        public TValue GetOrCreate<TValue>(string key, IEnumerable<string> fileDependencies, Func<TValue> factory)
        {
            return GetOrCreate(key, null, null, fileDependencies, factory, null);
        }

        public TValue GetOrCreate<TValue>(string key, DateTime timestamp, IEnumerable<string> fileDependencies, Func<TValue> factory)
        {
            return GetOrCreate(key, timestamp, null, fileDependencies, factory, null);
        }

        public TValue GetOrCreate<TValue>(string key, TimeSpan duration, IEnumerable<string> fileDependencies, Func<TValue> factory)
        {
            return GetOrCreate(key, null, duration, fileDependencies, factory, null);
        }

        public TValue GetOrCreate<TValue>(string key, IEnumerable<string> fileDependencies, Func<TValue> factory, Action<bool> onRemoveCallback)
        {
            return GetOrCreate(key, null, null, fileDependencies, factory, onRemoveCallback);
        }

        public TValue GetOrCreate<TValue>(string key, DateTime timestamp, IEnumerable<string> fileDependencies, Func<TValue> factory, Action<bool> onRemoveCallback)
        {
            return GetOrCreate(key, timestamp, null, fileDependencies, factory, onRemoveCallback);
        }

        public TValue GetOrCreate<TValue>(string key, TimeSpan duration, IEnumerable<string> fileDependencies, Func<TValue> factory, Action<bool> onRemoveCallback)
        {
            return GetOrCreate(key, null, duration, fileDependencies, factory, onRemoveCallback);
        }

        public void Remove(string key)
        {
            HttpRuntime.Cache.Remove(MakeKey(key));
        }

        private static string MakeKey(string key)
        {
            Check.Argument.IsNotNullOrEmpty(key, "key");

            return keyPrefix + ":" + key;
        }

        private void Set<TValue>(string key, DateTime? timestamp, TimeSpan? duration, IEnumerable<string> fileDependencies, TValue value, Action<bool> onRemoveCallback)
        {
            string fullKey = MakeKey(key);

            lock (syncLock)
            {
                cache.Remove(fullKey);

                InsertInCache(fullKey, value, fileDependencies, timestamp, duration, onRemoveCallback);
            }
        }

        private TValue GetOrCreate<TValue>(string key, DateTime? timestamp, TimeSpan? duration, IEnumerable<string> fileDependencies, Func<TValue> factory, Action<bool> onRemoveCallback)
        {
            string fullKey = MakeKey(key);

            object value = cache.Get(fullKey);

            if (value == null)
            {
                lock (syncLock)
                {
                    value = cache.Get(fullKey);

                    if (value == null)
                    {
                        value = factory();

                        if (value != null)
                        {
                            InsertInCache(fullKey, value, fileDependencies, timestamp, duration, onRemoveCallback);
                        }
                    }
                }
            }

            return (TValue)value;
        }

        private void InsertInCache(string key, object value, IEnumerable<string> fileDependencies, DateTime? timestamp, TimeSpan? duration, Action<bool> onRemoveCallback)
        {
            Action<string, object, CacheItemRemovedReason> raiseOnRemoveCallback = (cacheKey, state, reason) => onRemoveCallback(reason == CacheItemRemovedReason.DependencyChanged);

            cache.Add(key, value, fileDependencies != null ? new CacheDependency(fileDependencies.ToArray()) : null, timestamp ?? Cache.NoAbsoluteExpiration, duration ?? Cache.NoSlidingExpiration, CacheItemPriority.Normal, onRemoveCallback != null ? new CacheItemRemovedCallback(raiseOnRemoveCallback) : null);
        }
    }
}