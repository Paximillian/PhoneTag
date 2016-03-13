using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackExchange.Redis.Geo
{
    public static class RedisGeoExtensions
    {
        public class GeoLocation
        {
            public String Name { get; set; }
            public double Longitude { get; set; }
            public double Latitude { get; set; }
        }

        public enum GeoMeasurementUnit
        {
            m,
            km,
            mi,
            ft
        }

        private static GeoMeasurementUnit s_MeasurementUnit = GeoMeasurementUnit.m;

        /// <summary>
        /// Adds geographical locations to a given key.
        /// </summary>
        /// <param name="i_KeyName">Key to store locations in.</param>
        /// <param name="i_GeoLocations">The locations you want to store.</param>
        public static void GeoAdd(this IDatabase i_Database, String i_KeyName, params GeoLocation[] i_GeoLocations)
        {
            for (int i = 0; i < i_GeoLocations.Length; ++i)
            {
                //Create the script we want to run.
                String script = "return redis.call('geoadd', @key, @longitude, @latitude, @name)";

                LuaScript geoAddScript = LuaScript.Prepare(script);

                //Create the object containing the given values.
                var requestParams = new
                {
                    key = (RedisKey)i_KeyName,
                    longitude = i_GeoLocations[i].Longitude,
                    latitude = i_GeoLocations[i].Latitude,
                    name = i_GeoLocations[i].Name
                };

                i_Database.ScriptEvaluate(geoAddScript, requestParams);
            }
        }

        /// <summary>
        /// Find any geographical locations in range of a given position.
        /// </summary>
        /// <param name="i_KeyName">The key we'll be looking for matched locations in</param>
        /// <param name="i_Longitude">The longitude position of the search start point.</param>
        /// <param name="i_Latitude">The latitude position of the search start point</param>
        /// <param name="i_Radius">The search radius.</param>
        /// <returns>A list of location names.</returns>
        public static String[] GeoRadius(this IDatabase i_Database, String i_KeyName, double i_Longitude, double i_Latitude, double i_Radius)
        {
            //Create the script we want to run.
            String script = "return redis.call('georadius', @key, @longitude, @latitude, @radius, @measurement)";

            LuaScript geoRadiusScript = LuaScript.Prepare(script);

            //Create the object containing the given values.
            var requestParams = new
            {
                key = (RedisKey)i_KeyName,
                longitude = i_Longitude,
                latitude = i_Latitude,
                radius = i_Radius,
                measurement = s_MeasurementUnit.ToString()
            };

            return (String[])i_Database.ScriptEvaluate(geoRadiusScript, requestParams);
        }

        /// <summary>
        /// Find any geographical locations in range of a given position.
        /// </summary>
        /// <param name="i_KeyName">The key we'll be looking for matched locations in</param>
        /// <param name="i_LocationName">The location from which we want to search</param>
        /// <param name="i_Radius">The search radius.</param>
        /// <returns>A list of location names.</returns>
        public static String[] GeoRadius(this IDatabase i_Database, String i_KeyName, String i_LocationName, double i_Radius)
        {
            //Create the script we want to run.
            String script = "return redis.call('georadiusbymember', @key, @locationName, @radius, @measurement)";

            LuaScript geoRadiusScript = LuaScript.Prepare(script);

            //Create the object containing the given values.
            var requestParams = new
            {
                key = (RedisKey)i_KeyName,
                locationName = (RedisKey)i_LocationName,
                radius = i_Radius,
                measurement = s_MeasurementUnit.ToString()
            };

            return (String[])i_Database.ScriptEvaluate(geoRadiusScript, requestParams);
        }

        /// <summary>
        /// Returns the distance between 2 locations in the key.
        /// </summary>
        /// <param name="i_KeyName">The key we'll be looking in.</param>
        /// <param name="i_Location1Name">Name of the first location.</param>
        /// <param name="i_Location2Name">Name of the second location.</param>
        /// <returns>The distance between the given locations.</returns>
        public static double GeoDist(this IDatabase i_Database, String i_KeyName, String i_Location1Name, String i_Location2Name)
        {
            //Create the script we want to run.
            String script = "return redis.call('geodist', @key, @location1Name, @location2Name, @measurement)";

            LuaScript geoDistScript = LuaScript.Prepare(script);

            //Create the object containing the given values.
            var requestParams = new
            {
                key = (RedisKey)i_KeyName,
                location1Name = (RedisKey)i_Location1Name,
                location2Name = (RedisKey)i_Location2Name,
                measurement = s_MeasurementUnit.ToString()
            };

            return (double)i_Database.ScriptEvaluate(geoDistScript, requestParams);
        }

        /// <summary>
        /// Gets the geohashed(Location code) for each given position in a key.
        /// This is the sort of code that's usable with: http://geohash.org/<geohash-string>
        /// </summary>
        /// <param name="i_KeyName">The key to look for the locations in.</param>
        /// <param name="i_LocationNames">Names of the locations to be encoded</param>
        /// <returns>Collelction of hashed positions in the same order as given.</returns>
        public static String[] GeoHash(this IDatabase i_Database, String i_KeyName, params String[] i_LocationNames)
        {
            List<String> hashedLocations = new List<String>();

            for (int i = 0; i < i_LocationNames.Length; ++i)
            {
                //Create the script we want to run.
                String script = "return redis.call('geohash', @key, @locationName)";

                LuaScript geoHashScript = LuaScript.Prepare(script);

                //Create the object containing the given values.
                var requestParams = new
                {
                    key = (RedisKey)i_KeyName,
                    locationName = (RedisKey)i_LocationNames[i],
                };

                //The geohash command always returns an array, but since we only gave it 1 
                //argument, we'll get an array of length 1, so we'll take the first member
                String[] hashedArray = (String[])i_Database.ScriptEvaluate(geoHashScript, requestParams);
                hashedLocations.Add(hashedArray[0]);
            }

            return hashedLocations.ToArray();
        }

        /// <summary>
        /// Returns the positions of the given locations in a key.
        /// </summary>
        /// <param name="i_KeyName">The key to search in.</param>
        /// <param name="i_LocationNames">The names of the locations we want.</param>
        /// <returns>A list of location details with their longitude and latitude.</returns>
        public static GeoLocation[] GeoPos(this IDatabase i_Database, String i_KeyName, params String[] i_LocationNames)
        {
            List<GeoLocation> hashedLocations = new List<GeoLocation>();

            for (int i = 0; i < i_LocationNames.Length; ++i)
            {
                //Create the script we want to run.
                String script = "return redis.call('geopos', @key, @locationName)";

                LuaScript geoPosScript = LuaScript.Prepare(script);

                //Create the object containing the given values.
                var requestParams = new
                {
                    key = (RedisKey)i_KeyName,
                    locationName = (RedisKey)i_LocationNames[i],
                };

                //The geopos command always returns an array, but since we only gave it 1 
                //argument, we'll get an array of length 1, so we'll take the first member
                //which in itself is an array.
                RedisResult[] hashedArray = (RedisResult[])(((RedisResult[])i_Database.ScriptEvaluate(geoPosScript, requestParams))[0]);

                //The returned array contains the longitude and latitude of the position
                hashedLocations.Add(new GeoLocation() { Name = i_LocationNames[i], Longitude = (double)hashedArray[0], Latitude = (double)hashedArray[1] });
            }

            return hashedLocations.ToArray();
        }
    }
}
