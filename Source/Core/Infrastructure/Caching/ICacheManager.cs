namespace Kigg.Infrastructure
{
    using System;
    using System.Collections.Generic;

    public interface ICacheManager
    {
        TValue Get<TValue>(string key);

        void Set<TValue>(string key, Func<TValue> value);

        void Set<TValue>(string key, DateTime timestamp, TValue value);

        void Set<TValue>(string key, TimeSpan duration, TValue value);

        void Set<TValue>(string key, TValue value, Action<bool> onRemoveCallback);

        void Set<TValue>(string key, DateTime timestamp, TValue value, Action<bool> onRemoveCallback);

        void Set<TValue>(string key, TimeSpan duration, TValue value, Action<bool> onRemoveCallback);

        void Set<TValue>(string key, IEnumerable<string> fileDependencies, TValue value);

        void Set<TValue>(string key, DateTime timestamp, IEnumerable<string> fileDependencies, TValue value);

        void Set<TValue>(string key, TimeSpan duration, IEnumerable<string> fileDependencies, TValue value);

        void Set<TValue>(string key, IEnumerable<string> fileDependencies, TValue value, Action<bool> onRemoveCallback);

        void Set<TValue>(string key, DateTime timestamp, IEnumerable<string> fileDependencies, TValue value, Action<bool> onRemoveCallback);

        void Set<TValue>(string key, TimeSpan duration, IEnumerable<string> fileDependencies, TValue value, Action<bool> onRemoveCallback);

        TValue GetOrCreate<TValue>(string key, Func<TValue> factory);

        TValue GetOrCreate<TValue>(string key, DateTime timestamp, Func<TValue> factory);

        TValue GetOrCreate<TValue>(string key, TimeSpan duration, Func<TValue> factory);

        TValue GetOrCreate<TValue>(string key, Func<TValue> factory, Action<bool> onRemoveCallback);

        TValue GetOrCreate<TValue>(string key, DateTime timestamp, Func<TValue> factory, Action<bool> onRemoveCallback);

        TValue GetOrCreate<TValue>(string key, TimeSpan duration, Func<TValue> factory, Action<bool> onRemoveCallback);

        TValue GetOrCreate<TValue>(string key, IEnumerable<string> fileDependencies, Func<TValue> factory);

        TValue GetOrCreate<TValue>(string key, DateTime timestamp, IEnumerable<string> fileDependencies, Func<TValue> factory);

        TValue GetOrCreate<TValue>(string key, TimeSpan duration, IEnumerable<string> fileDependencies, Func<TValue> factory);

        TValue GetOrCreate<TValue>(string key, IEnumerable<string> fileDependencies, Func<TValue> factory, Action<bool> onRemoveCallback);

        TValue GetOrCreate<TValue>(string key, DateTime timestamp, IEnumerable<string> fileDependencies, Func<TValue> factory, Action<bool> onRemoveCallback);

        TValue GetOrCreate<TValue>(string key, TimeSpan duration, IEnumerable<string> fileDependencies, Func<TValue> factory, Action<bool> onRemoveCallback);

        void Remove(string key);
    }
}