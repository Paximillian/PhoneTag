﻿using PhoneTag.SharedCodebase.Model;
using PhoneTag.SharedCodebase;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Net.Http;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;

namespace WebServiceTests
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnCreateUser_Click(object sender, EventArgs e)
        {
            createUser();
        }

        private void btnGetUser_Click(object sender, EventArgs e)
        {
            getUser();
        }

        private void btnClearUser_Click(object sender, EventArgs e)
        {
            clearUser();
        }

        private async void clearUser()
        {
            using (HttpClient client = new HttpClient())
            {
                await client.GetMethodAsync("test/clear");

                tbResult.Text = "Cleared";
            }
        }

        private async void createUser()
        {
            bool success = await User.CreateUser("TestUser");

            tbResult.Text = success.ToString();
        }

        private async void getUser()
        {
            User user = await User.GetUser("TestUser");
            
            user = BsonSerializer.Deserialize<User>((BsonDocument)null);

            //tbResult.Text = JsonConvert.SerializeObject(user);
        }
    }
}
