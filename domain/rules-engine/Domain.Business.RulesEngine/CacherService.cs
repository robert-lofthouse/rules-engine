using Microsoft.Extensions.Caching.Memory;
using System;
using Domain.RulesEngine.Interface;

namespace Domain.RulesEngine.Business
{
	public class CacherService : ICacherService
	{
		private readonly IMemoryCache _memoryCache;

		public CacherService(IMemoryCache memoryCache)
		{
			_memoryCache = memoryCache;
		}

		public T GetValue<T>(string key)
		{
			if (_memoryCache.TryGetValue(key, out var data))
			{
				return (T)data;
			}
			return default;
		}

		public void SetValue<T>(T item, string key)
		{
			//TODO: read from config or come up with better solution i.e centralized cache
			_memoryCache.Set(key, item, new MemoryCacheEntryOptions { AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(2) });
		}
	}
}
