using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Common.Redis
{
    public class RedisService
    {
        public RedisService()
        {
        }

        public async Task AddOrUpdate(string key, object value, int minutes = 30)
        {
            await Set(key, value, DateTime.Now.AddMinutes(minutes) - DateTime.Now);
        }


        public async Task AddOrUpdate(string key, object value, DateTime? expiredAt = null)
        {
            await Set(key, value, (expiredAt ?? DateTime.Now.AddMinutes(30)) - DateTime.Now);
        }

        public async Task<T> Get<T>(string key) where T : class
        {
            return await ClientGet<T>(key);
        }

        public async Task<bool> Exists(string key)
        {
            using (var client = ConnectionMultiplexer.Connect(Conn))
            {
                var strValue = await client.GetDatabase().StringGetAsync(key);
                return string.IsNullOrEmpty(strValue);
            }
        }

        public async Task Refresh(string key)
        {
            using (var client = ConnectionMultiplexer.Connect(Conn))
            {
                var strValue = await client.GetDatabase().StringGetAsync(key);
                await AddOrUpdate(key, strValue, null);
            }
        }

        public  void Remove(string key,int count=10)
        {
            using (var client = ConnectionMultiplexer.Connect(Conn))
            {
                var strValue = client.GetDatabase().KeyDelete(key);
	            if (!strValue && count > 0)
	            {
		            Remove(key, --count);

	            }
            }
        }



        #region 泛型

        private static readonly string Conn = Settings.GetConfiguration("RedisConfig:Connection");
        /// <summary>
        /// 存值并设置过期时间
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">key</param>
        /// <param name="t">实体</param>
        /// <param name="ts">过期时间间隔</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static async Task<bool> Set<T>(string key, T t, TimeSpan ts)
        {
            var str = JsonConvert.SerializeObject(t);
            using (var client = ConnectionMultiplexer.Connect(Conn))
            {
                return await client.GetDatabase().StringSetAsync(key, str, ts);
            }
        }

        /// <summary>
        /// 
        /// 根据Key获取值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <returns>T.</returns>
        public static async Task<T> ClientGet<T>(string key) where T : class
        {
            using (var client = ConnectionMultiplexer.Connect(Conn))
            {
                var strValue = await client.GetDatabase().StringGetAsync(key);
                return string.IsNullOrEmpty(strValue) ? null : JsonConvert.DeserializeObject<T>(strValue);
            }
        }
        #endregion


    }
}
