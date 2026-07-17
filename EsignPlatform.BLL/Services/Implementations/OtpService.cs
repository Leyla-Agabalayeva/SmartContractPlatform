using EsignPlatform.BLL.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsignPlatform.BLL.Services.Implementations
{
    public class OtpService : IOtpService
    {
        private readonly IMemoryCache _cache;
        private static readonly TimeSpan Ttl = TimeSpan.FromMinutes(5); // OTP expiration

        public OtpService(IMemoryCache cache) => _cache = cache;

        public string Generate(string key)
        {
            var code = Random.Shared.Next(100000, 999999).ToString();
            _cache.Set(CacheKey(key), code, Ttl);
            return code;
        }

        public bool Validate(string key, string code)
        {
            if (_cache.TryGetValue(CacheKey(key), out string? stored))
                return stored == code;
            return false;
        }

        public void Remove(string key) => _cache.Remove(CacheKey(key));

        private static string CacheKey(string key) => $"otp:{key}";
    }

}
