using PhoneTag.SharedCodebase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using static StackExchange.Redis.Geo.RedisGeoExtensions;

namespace PhoneTag.WebServices.Controllers
{
    public class TestController : ApiController
    {
        private string s_Message;

        [Route("api/ping")]
        [HttpGet]
        public string Ping()
        {
            return "pong";
        }
        [Route("api/init")]
        [HttpGet]
        public string Init()
        {
            init();
            return s_Message;
        }

        private async void init()
        {
            s_Message = await Redis.Init();
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