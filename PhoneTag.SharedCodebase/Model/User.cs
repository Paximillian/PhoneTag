using Newtonsoft.Json;
using PhoneTag.SharedCodebase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PhoneTag.SharedCodebase.Model
{
    public class User
    {
        public String Username { get; set; }
        public List<User> Friends { get; set; }
        public bool IsReady { get; set; }
        public int Ammo { get; set; }

        [IgnoreDataMember]
        public Task<bool> IsActive
        {
            get
            {
                using (HttpClient client = new HttpClient())
                {
                    return client.GetMethodAsync<bool>(String.Format("users/{0}/active", Username));
                }
            }
        }

        [IgnoreDataMember]
        public Task<GameRoom> PlayingIn
        {
            get
            {
                using (HttpClient client = new HttpClient())
                {
                    return client.GetMethodAsync<GameRoom>(String.Format("rooms/{0}", client.GetMethodAsync<int>(String.Format("users/{0}/room", Username))));
                }
            }
        }

        public static async Task<bool> CreateUser(string i_Username)
        {
            User newUser = new User();

            newUser.Username = i_Username;
            newUser.Ammo = 3;
            newUser.IsReady = true;
            newUser.Friends = new List<User>();

            using (HttpClient client = new HttpClient())
            {
                bool result = await client.PostMethodAsync("users/create", newUser);

                return result;
            }
        }

        public static async Task<User> GetUser(string i_Username)
        {
            using (HttpClient client = new HttpClient())
            {
                User result = await client.GetMethodAsync(String.Format("users/{0}", i_Username));
                
                //result = BsonDocument

                return result;
            }
        }
    }
}
