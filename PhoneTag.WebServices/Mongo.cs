using MongoDB.Bson;
using MongoDB.Driver;
using PhoneTag.SharedCodebase.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhoneTag.WebServices
{
    public class Mongo
    {
        private static IMongoClient s_Client;
        public static IMongoDatabase Database { get; private set; }
        public static bool IsReady { get; private set; }

        public static string Init()
        {
            String errorMessage = "All's well";

            try
            {
                s_Client = new MongoClient();
                Database = s_Client.GetDatabase("local");

                IsReady = true;
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
            }

            rebuildIndexes();

            return errorMessage;
        }

        private static void rebuildIndexes()
        {
            //If the database is out of date, rebuild the indexes.
            if (!Mongo.Database.GetCollection<BsonDocument>("FloatingValues").FindSync(Builders<BsonDocument>.Filter.Eq("Ready", "true")).Any())
            {
                Database.GetCollection<User>("Users").Indexes.DropAll();

                Database.GetCollection<User>("Users").Indexes.CreateOne(
                    Builders<User>.IndexKeys.Ascending("Username"),
                    new CreateIndexOptions<User>() { Unique = true }
                );

                Database.GetCollection<BsonDocument>("FloatingValues").InsertOne(new BsonDocument { { "Ready", "true" } });
            }
        }
    }
}