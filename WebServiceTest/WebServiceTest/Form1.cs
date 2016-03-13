using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PhoneTag.SharedCodebase;


namespace WebServiceTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            updatePosition(1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            updatePosition(2);
        }
        private void button4_Click(object sender, EventArgs e)
        {
            shoot(1);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            shoot(2);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            clear();
        }

        private async void updatePosition(int i_Id)
        {
            using (HttpClient client = new HttpClient())
            {
                Text = (await client.PostMethodAsync(String.Format("test/position/{0}", i_Id), new { X = 0.13, Y = 1.56 })).ToString();
            }
        }

        private async void shoot(int i_Id)
        {
            using (HttpClient client = new HttpClient())
            {
                Text = await client.PostMethodAsync(String.Format("test/shoot/{0}", i_Id), new { X = 0, Y = 1 }) + " hit";
            }
        }

        private async void clear()
        {
            using (HttpClient client = new HttpClient())
            {
                await client.PostMethodAsync("test/clear");
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            checkServerStatus();
        }

        private async void checkServerStatus()
        {
            timer1.Enabled = false;

            using (HttpClient client = new HttpClient())
            {
                bool result = await client.GetMethodAsync("service");
                
                button1.Enabled = result;
                button2.Enabled = result;
                button3.Enabled = result;
                button4.Enabled = result;
                button5.Enabled = result;
            }

            timer1.Enabled = true;
        }
    }
}
