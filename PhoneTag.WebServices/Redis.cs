using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace PhoneTag.WebServices
{
    public static class Redis
    {
        private const string k_RedisServerAddress = "http://ec2-54-93-95-18.eu-central-1.compute.amazonaws.com:6379";

        private static ConnectionMultiplexer s_RedisInstance = null;

        public static IDatabase Database { get { return s_RedisInstance.GetDatabase(); } }

        public static bool IsReady { get { return s_RedisInstance != null && s_RedisInstance.IsConnected; } }

        public async static Task<string> Init()
        {
            string message = "";

            if(s_RedisInstance == null)
            {
                try
                {
                    s_RedisInstance = await ConnectionMultiplexer.ConnectAsync(k_RedisServerAddress);
                }
                catch (Exception e)
                {
                    message = e.Message;
                }
            }

            return message;
        }
    }
}