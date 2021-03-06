﻿using PhoneTag.SharedCodebase;
using PhoneTag.SharedCodebase.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.IO;
using Newtonsoft.Json;
using MongoDB.Bson.Serialization;

namespace PhoneTag.WebServices.Controllers
{
    public class UserController : ApiController
    {
        [Route("api/users/create")]
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

        [Route("api/users/{i_Id}")]
        [HttpGet]
        public async Task<User> GetUser(string i_Id)
        {
            User foundUser = null;

            try
            {
                FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq("Username", i_Id);

                using (IAsyncCursor<User> cursor = await Mongo.Database.GetCollection<BsonDocument>("Users").FindAsync<User>(filter))
                {
                    foundUser = await cursor.SingleAsync();
                    //foundUser = BsonSerializer.Deserialize<User>(resultDoc);
                }
            }
            catch (Exception e)
            {

            }

            return foundUser;
        }
    }
}