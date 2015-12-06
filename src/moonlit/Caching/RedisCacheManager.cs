using System;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Moonlit.Caching
{
    //public class RedisCacheManager : ICacheManager
    //{
    //    public string Host { get; set; }
    //    public int Port { get; set; }
    //    public bool Exist(string key)
    //    {
    //        key = BuildKey(key);
    //        using (var connection = MakeConnection())
    //        {
    //            return connection.Exists(key);
    //        }
    //    }

    //    public void Set(string key, object value, TimeSpan? expiredTime)
    //    {
    //        key = BuildKey(key);
    //        using (var connection = MakeConnection())
    //        {
    //            var text = JsonConvert.SerializeObject(value);
    //            expiredTime = expiredTime ?? TimeSpan.FromMinutes(5);
    //            var bytes = Encoding.UTF8.GetBytes(text);
    //            connection.Set(key, Convert.ToBase64String(bytes), expiredTime.Value);
    //        }
    //    }

    //    public object Get(string key, Type type)
    //    {
    //        key = BuildKey(key);
    //        using (var connection = MakeConnection())
    //        {
    //            var text = connection.Get(key);
    //            if (text == null)
    //            {
    //                return null;
    //            }
    //            var bytes = Convert.FromBase64String(text);
    //            text = Encoding.UTF8.GetString(bytes);
    //            return JsonConvert.DeserializeObject(text, type, new JsonSerializerSettings());
    //        }
    //    }

    //    private string BuildKey(string key)
    //    {
    //        return key;
    //    }

    //    public void Remove(string key)
    //    {
    //        key = BuildKey(key);
    //        using (var connection = MakeConnection())
    //        {
    //            var text = connection.Get(key);
    //            if (text == null)
    //                return;
    //            connection.Del(key);
    //        }
    //    }

    //    public async Task SetAsync(string key, object value, TimeSpan? expiredTime)
    //    {
    //        key = BuildKey(key);
    //        using (var connection = MakeConnection())
    //        {
    //            var text = JsonConvert.SerializeObject(value);
    //            expiredTime = expiredTime ?? TimeSpan.FromMinutes(5);
    //            var bytes = Encoding.UTF8.GetBytes(text);
    //            await connection.SetAsync(key, Convert.ToBase64String(bytes), expiredTime.Value).ConfigureAwait(false);
    //        }
    //    }

    //    public async Task<object> GetAsync(string key, Type type)
    //    {
    //        key = BuildKey(key);
    //        using (var connection = MakeConnection())
    //        {
    //            var text = await connection.GetAsync(key);
    //            if (text == null)
    //            {
    //                return null;
    //            }
    //            var bytes = Convert.FromBase64String(text);
    //            text = Encoding.UTF8.GetString(bytes);
    //            return JsonConvert.DeserializeObject(text, type, new JsonSerializerSettings());
    //        }
    //    }

    //    public Task RemoveAsync(string key)
    //    {
    //        key = BuildKey(key);
    //        using (var connection = MakeConnection())
    //        {
    //            var text = connection.Get(key);
    //            if (text == null)
    //                return Task.FromResult(0);
    //            return connection.DelAsync(key);
    //        }
    //    }

    //    private RedisClient MakeConnection()
    //    {
    //        return new RedisClient(Host, Port);
    //    }
    //}
}