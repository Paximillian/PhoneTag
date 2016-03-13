using PhoneTag.SharedCodebase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using static StackExchange.Redis.Geo.RedisGeoExtensions;
using com.shephertz.app42.paas.sdk.csharp;
using com.shephertz.app42.paas.sdk.csharp.user;
using MongoDB.Bson;
using MongoDB.Driver;

namespace PhoneTag.WebServices.Controllers
{
    public class TestController : ApiController
    {
        protected static IMongoClient _client;
        protected static IMongoDatabase _database;

        private string s_Message;

        [Route("api/ping")]
        [HttpGet]
        public async Task<string> Ping()
        {
            return "pong " + await pong();
        }
        [Route("api/init")]
        [HttpGet]
        public string Init()
        {
            init();
            return s_Message;
        }

        private async Task<string> pong()
        {
            var collection = _database.GetCollection<BsonDocument>("myCollection");
            var filter = new BsonDocument();

            s_Message = "";

            using (var cursor = await collection.FindAsync(filter))
            {
                while (await cursor.MoveNextAsync())
                {
                    var batch = cursor.Current;
                    foreach (var document in batch)
                    {
                        // process document
                        BsonElement res = new BsonElement("x", "error");
                        document.TryGetElement("x", out res);
                        s_Message += res.Value;
                    }
                }
            }

            return s_Message;
        }

        private void init()
        {
            //App42API.Initialize("b7cce3f56c238389790ccef2a13c69fe88cb9447523730b6e93c849a6d0bd510", "e6672070bad36d0940805bff5d81fa3d9d66e440913301f1f438ad937b5d8502");
            
            _client = new MongoClient();
            _database = _client.GetDatabase("local");

            //s_Message = await Redis.Init();
        }

        [Route("api/test/clear")]
        [HttpPost]
        public void ClearPositions()
        {
            Redis.Database.KeyDelete("Test");
        }

        [Route("api/test/position/{i_PlayerId}")]
        [HttpPost]
        public Point PositionUpdate([FromBody]Point i_PlayerLocation, int i_PlayerId)
        {
            Redis.Database.GeoAdd("Test", new GeoLocation { Name = String.Format("{0}", i_PlayerId), Longitude = i_PlayerLocation.X, Latitude = i_PlayerLocation.Y });

            return i_PlayerLocation;
        }

        [Route("api/test/shoot/{i_PlayerId}")]
        [HttpPost]
        public String Shoot([FromBody]DeviceLocationInfo i_PlayerLocation, int i_PlayerId)
        {
            List<string> hits = Redis.Database.GeoRadius("Test", i_PlayerLocation.DeviceLocation.X, i_PlayerLocation.DeviceLocation.Y, 20000000).ToList();

            hits = hits.Where((hitName) => !hitName.Equals(i_PlayerId.ToString())).ToList();

            return hits.Count > 0 ? hits[0] : "No hits";
        }
    }
}