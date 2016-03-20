using PhoneTag.SharedCodebase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PhoneTag.SharedCodebase.Model
{
    public class User
    {
        public String Username { get; private set; }
        public List<User> Friends { get; private set; }
        public bool IsReady { get; private set; }
        public int Ammo { get; private set; }

        public User(String name)
        {
            Username = name;
            Ammo = 0;
            IsReady = false;
            Friends = new List<User>();
        }

        public Task<bool> IsActive
        {
            get
            {
                using (HttpClient client = new HttpClient())
                {
                    return client.GetMethodAsync<bool>("users/i/active");
                }
            }
        }

        //public Task<GameRoom> PlayingIn
        //{
        //    get
        //    {
        //        using (HttpClient client = new HttpClient())
        //        {
        //            return client.GetMethodAsync<GameRoom>(String.Format("rooms/{0}", client.GetMethodAsync<int>(String.Format("users/{0}/room", Id))));
        //        }
        //    }
        //}
    }
}
