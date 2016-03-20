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

namespace PhoneTag.WebServices.Controllers
{
    public class UserController : ApiController
    {
        [Route("api/users/create")]
        [HttpPost]
        public async Task CreateUser(User i_NewUser)
        {
            await Mongo.Database.GetCollection<User>("Users").InsertOneAsync(i_NewUser);
        }

        [Route("api/users/{i_Id}")]
        [HttpGet]
        public async Task<User> CreateUser(string i_Id)
        {
            User foundUser = null;

            FilterDefinition<User> filter = Builders<User>.Filter.Eq("_id", i_Id);

            using (IAsyncCursor<User> cursor = await Mongo.Database.GetCollection<User>("Users").FindAsync<User>(filter))
            {
                foundUser = await cursor.SingleAsync();
            }

            return foundUser;
        }
    }
}