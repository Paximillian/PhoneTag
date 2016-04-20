using PhoneTag.SharedCodebase;
using PhoneTag.SharedCodebase.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using MongoDB.Bson;
using MongoDB.Driver;
using System.IO;

namespace PhoneTag.WebServices.Controllers
{
    public class RoomController : ApiController
    {
        [Route("api/rooms/create")]
        [HttpPost]
        public async Task<bool> CreateUser(User i_NewUser)
        {
            bool success = true;

            try
            {
                await Mongo.Database.GetCollection<User>("Users").InsertOneAsync(i_NewUser);
            }
            catch (Exception e)
            {
                success = false;
            }

            return success;
        }

        [Route("api/rooms/{i_Id}")]
        [HttpGet]
        public async Task<User> GetUser(string i_Id)
        {
            User foundUser = null;

            try
            {
                FilterDefinition<User> filter = Builders<User>.Filter.Eq("User:Username", i_Id);

                using (IAsyncCursor<User> cursor = await Mongo.Database.GetCollection<User>("Users").FindAsync<User>(filter))
                {
                    foundUser = await cursor.SingleAsync();
                }
            }
            catch (Exception e)
            {

            }

            return foundUser;
        }
    }
}