using CardioTrack.Interfaces.ICache;
using Microsoft.Extensions.Caching.Distributed;

namespace CardioTrack.Services.CacheService
{
    public class PatientCacheInvalidator : IPatientCacheInvalidator
    {
        private readonly IDistributedCache cache;

        public PatientCacheInvalidator(IDistributedCache cache)
        {
            this.cache = cache;
        }

        public async Task InvalidateAsync(int doctorId)
        {
            await cache.RemoveAsync($"patients:doctor:{doctorId}");
        }
    }
}
