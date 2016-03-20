using MongoDB.Driver;
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

            return errorMessage;
        }
    }
}