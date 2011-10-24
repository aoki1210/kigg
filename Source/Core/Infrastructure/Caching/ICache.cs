namespace Kigg.Infrastructure
{
    using System;
    [Obsolete("Use Kigg.Infrastructure.ICacheManager", true)]
    public interface ICache
    {
        int Count
        {
            get;
        }

        void Clear();

        bool Contains(string key);

        T Get<T>(string key);

        bool TryGet<T>(string key, out T value);

        void Set<T>(string key, T value);

        void Set<T>(string key, T value, DateTime absoluteExpiration);

        void Set<T>(string key, T value, TimeSpan slidingExpiration);

        void Remove(string key);
    }
}