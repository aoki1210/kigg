﻿namespace Kigg.Infrastructure
{
    using System;
    using System.Diagnostics;

    public static class Cache
    {
        public static int Count
        {
            get
            {
                return InternalCache.Count;
            }
        }

        private static ICache InternalCache
        {
            [DebuggerStepThrough]
            get
            {
                return IoC.Resolve<ICache>();
            }
        }

        public static void Clear()
        {
            InternalCache.Clear();
        }

        public static bool Contains(string key)
        {
            Check.Argument.IsNotEmpty(key, "key");

            return InternalCache.Contains(key);
        }

        public static T Get<T>(string key)
        {
            Check.Argument.IsNotEmpty(key, "key");

            return InternalCache.Get<T>(key);
        }

        public static bool TryGet<T>(string key, out T value)
        {
            Check.Argument.IsNotEmpty(key, "key");

            return InternalCache.TryGet(key, out value);
        }

        public static void Set<T>(string key, T value)
        {
            Check.Argument.IsNotEmpty(key, "key");

            InternalCache.Set(key, value);
        }

        public static void Set<T>(string key, T value, DateTime absoluteExpiration)
        {
            Check.Argument.IsNotEmpty(key, "key");
            Check.Argument.IsNotInPast(absoluteExpiration, "absoluteExpiration");

            InternalCache.Set(key, value, absoluteExpiration);
        }

        public static void Set<T>(string key, T value, TimeSpan slidingExpiration)
        {
            Check.Argument.IsNotEmpty(key, "key");
            Check.Argument.IsNotNegativeOrZero(slidingExpiration, "absoluteExpiration");

            InternalCache.Set(key, value, slidingExpiration);
        }

        public static void Remove(string key)
        {
            Check.Argument.IsNotEmpty(key, "key");

            InternalCache.Remove(key);
        }
    }
}