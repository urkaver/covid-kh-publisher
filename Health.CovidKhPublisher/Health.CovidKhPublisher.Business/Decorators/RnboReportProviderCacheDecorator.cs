using System;
using System.Threading.Tasks;
using Health.CovidKhPublisher.Business.Interfaces;
using Health.CovidKhPublisher.Business.Models.Domain;
using Health.CovidKhPublisher.Business.Models.Domain.Reports;
using Microsoft.Extensions.Caching.Memory;

namespace Health.CovidKhPublisher.Business.Decorators
{
    public class RnboReportProviderCacheDecorator : IRnboReportProvider
    {
        private static readonly TimeSpan CacheExpirationTime = TimeSpan.FromMinutes(5);

        private readonly IRnboReportProvider _client;
        private readonly IMemoryCache _memoryCache;

        public RnboReportProviderCacheDecorator(IRnboReportProvider client, IMemoryCache memoryCache)
        {
            _client = client;
            _memoryCache = memoryCache;
        }

        public Task<DayReport[]> GetAllDayReports(Location location)
        {
            string cacheKey = location.ToString();

            return _memoryCache.GetOrCreateAsync(cacheKey, entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = CacheExpirationTime;

                return _client.GetAllDayReports(location);
            });
        }
    }
}
