﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace PhotoChannelWebAPI.Cache
{
    public class CustomMemoryCache : ICustomMemoryCache
    {
        public IMemoryCache _memoryCache;

        public CustomMemoryCache(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
            keys = new List<object>();
        }

        private List<object> keys;
        public IList<object> Keys => keys.Where(o => o != null).ToList();

        public void Dispose()
        {
            _memoryCache.Dispose();
        }

        public bool TryGetValue(object key, out object value)
        {
            return _memoryCache.TryGetValue(key, out value);
        }

        public ICacheEntry CreateEntry(object key)
        {
            return _memoryCache.CreateEntry(key);
        }

        public void Remove(object key)
        {
            keys.Remove(key);
            _memoryCache.Remove(key);
        }

        public void Clear()
        {
            foreach (var key in keys)
            {
                _memoryCache.Remove(key);
            }
        }

        public void RemoveRange(List<object> keys)
        {
            foreach (var key in keys)
            {
                _memoryCache.Remove(key);
            }
        }

        public bool Contains(object key)
        {
            return Keys.Contains(key);
        }

        public object Get(object key)
        {
            return _memoryCache.Get(key);
        }

        public TItem Get<TItem>(object key)
        {
            return _memoryCache.Get<TItem>(key);
        }

        public TItem GetOrCreate<TItem>(object key, Func<ICacheEntry, TItem> factory)
        {
            return _memoryCache.GetOrCreate<TItem>(key, factory);
        }

        public Task<TItem> GetOrCreateAsync<TItem>(object key, Func<ICacheEntry, Task<TItem>> factory)
        {
            return _memoryCache.GetOrCreateAsync(key, factory);
        }

        public TItem Set<TItem>(object key, TItem value)
        {
            keys.Add(key);
            return _memoryCache.Set<TItem>(key, value);
        }

        public TItem Set<TItem>(object key, TItem value, DateTimeOffset absoluteExpiration)
        {
            keys.Add(key);
            return _memoryCache.Set<TItem>(key, value, absoluteExpiration);
        }

        public TItem Set<TItem>(object key, TItem value, TimeSpan absoluteExpirationRelativeToNow)
        {
            keys.Add(key);
            return _memoryCache.Set<TItem>(key, value, absoluteExpirationRelativeToNow);
        }

        public TItem Set<TItem>(object key, TItem value, IChangeToken expirationToken)
        {
            keys.Add(key);
            return _memoryCache.Set<TItem>(key, value, expirationToken);
        }

        public TItem Set<TItem>(object key, TItem value, MemoryCacheEntryOptions options)
        {
            keys.Add(key);
            return _memoryCache.Set<TItem>(key, value, options);
        }

        public bool TryGetValue<TItem>(object key, out TItem value)
        {
            keys.Add(key);
            return _memoryCache.TryGetValue<TItem>(key, out value);
        }

    }
}
