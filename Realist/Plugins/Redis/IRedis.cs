using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Plugins.Redis.Cache
{
 public   interface IRedis
 {
  Task<T>  GetRedis<T>(string cacheKey);
  Task<T> SetRedis<T>(T data, string cacheKey);

 }
}
