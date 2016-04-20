using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PhoneTag.SharedCodebase.Model
{
    public class GameRoom
    {
        public GameDetails GameModeDetails { get; set; }
        public bool Started { get; set; }
        public bool Finished { get; set; }
        public int GameTime { get; set; }

        [IgnoreDataMember]
        public List<User> LivingUsers { get { return new List<User>(r_LivingUsers); } }
        private readonly List<User> r_LivingUsers = new List<User>();

        [IgnoreDataMember]
        public List<User> DeadUsers { get { return new List<User>(r_DeadUsers); } }
        private readonly List<User> r_DeadUsers = new List<User>();

        private GameRoom(GameDetails i_GameDetails)
        {
            GameModeDetails = i_GameDetails;
            Started = false;
            Finished = false;
            GameTime = 0;
        }

        public static async Task<bool> CreateRoom(GameDetails i_GameDetails)
        {
            GameRoom gameRoom = new GameRoom(i_GameDetails);

            using (HttpClient client = new HttpClient())
            {
                bool result = await client.PostMethodAsync("rooms/create", gameRoom);

                return result;
            }
        }
    }
}
