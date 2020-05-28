using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Plugins.Redis.Cache
{
  public class Redis:IRedis
    {
        private readonly IDistributedCache _distributedCache;

        public Redis(IDistributedCache distributedCache) 
        {
            _distributedCache = distributedCache;
        }
        public async Task<T> GetRedis<T>( string cacheKey)
        {
        


            var returnBytes = await _distributedCache.GetAsync(cacheKey.ToLower());
            string decodedString;
         
            
            if (returnBytes != null)
            {
                decodedString = Encoding.UTF8.GetString(returnBytes);
              return   JsonConvert.DeserializeObject<T>(decodedString);
            }


            return default(T);
        }

        public async Task<T> SetRedis<T>(T data, string cacheKey)
        {
            string decodedString;
          
            if (data!= null && cacheKey != null)
            {
                decodedString = JsonConvert.SerializeObject(data);

                var returnBytes = Encoding.UTF8.GetBytes(decodedString);
                var options = new DistributedCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5))
                    .SetAbsoluteExpiration(DateTime.Now.AddHours(6));
                await _distributedCache.SetAsync(cacheKey, returnBytes, options);
              
            }
            return data;


        }
        }
    }

