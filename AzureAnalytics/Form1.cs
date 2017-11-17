using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using AzureAnalytics.Models;
using Newtonsoft.Json;

namespace AzureAnalytics
{
    public partial class Form1 : Form
    {
        
        private string URLServer = "https://api.appcenter.ms";

        // SET THESE TWO VARIABLES OR GET ERRORS!
        private string Owner = "";
        private string APIKey = "";
        private List<string> AppNames = new List<string>();
        public Form1()
        {
            InitializeComponent();

            GetApps();
            
        }

        private async void GetApps()
        {
            var client = new HttpClient();

            client.BaseAddress = new Uri(URLServer);
            client.DefaultRequestHeaders.Add("X-API-Token", APIKey);
            var endpoint = "/v0.1/apps";
            var response = await client.GetAsync(endpoint);
            var projectsJson = response.Content.ReadAsStringAsync().Result;
            try
            {
                var translateResponse = JsonConvert.DeserializeObject<List<AppsModel>>(projectsJson);
                foreach (var eachItem in translateResponse)
                {
                    AppNames.Add(eachItem.name);
                    Apps.Items.Add(eachItem.name);
                }
                GrabButton.Enabled = true;
                PurgeButton.Enabled = true;
                Apps.SelectedIndex = 0;
            }
            catch
            {
                if (projectsJson.Contains("Unauthorized"))
                {
                    MessageBox.Show(this, "API Key Incorrect", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
         
            
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            var startdate = DateTime.Now.AddDays(-90).ToString("s");
            var enddate = DateTime.Now.ToString("s");

                var client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(10);
                client.BaseAddress = new Uri(URLServer);
                client.DefaultRequestHeaders.Add("X-API-Token", APIKey);
            var endpoint ="/v0.1/apps/" + Owner + "/" + AppNames[Apps.SelectedIndex] + "/analytics/events?start=" +
                           startdate + "&%24inlinecount=none&%24top=5000";
            try
            {
                var response = await client.GetAsync(endpoint);
                var projectsJson = await response.Content.ReadAsStringAsync();

                var translateResponse = JsonConvert.DeserializeObject<Events>(projectsJson.ToString());

                foreach (var eachItem in translateResponse.events)
                {
                    listBox1.Items.Add(eachItem.name);
                }
            }
            catch (TimeoutException ex)
            {
                Debug.WriteLine("Error: " + ex);
            }


        }

        private async void button2_Click(object sender, EventArgs e)
        {
            if (listBox1.Items.Count == 0)
            {
                MessageBox.Show(this, "Please grab your events first", "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Stop);
                return;
            }

            var client = new HttpClient();
            client.BaseAddress = new Uri(URLServer);
            client.DefaultRequestHeaders.Add("X-API-Token", APIKey);
            int count = 0;
            foreach (var eachItem in listBox1.Items)
            {
                try
                {


                        var endpoint = "/v0.1/apps/" + Owner + "/" + AppNames[Apps.SelectedIndex] + "/analytics/events/" +
                                       HttpUtility.UrlEncode(eachItem.ToString()).Replace("+", "%20");
                        var response = await client.DeleteAsync(endpoint);
                        var projectsJson = response.Content.ReadAsStringAsync().Result;
                        ServerResponse.Items.Add(endpoint);
                        ServerResponse.Items.Add(response.StatusCode + " - " + projectsJson);
                    
                    
                }
                catch
                {
                    Console.WriteLine("error");
                }
            }

            listBox1.Items.Clear();

        }

        private void Apps_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
          
        }

        static string Hash(string input)
        {
            using (SHA1Managed sha1 = new SHA1Managed())
            {
                var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(input));
                var sb = new StringBuilder(hash.Length * 2);

                foreach (byte b in hash)
                {
                    // can be "x2" if you want lowercase
                    sb.Append(b.ToString("X2"));
                }

                return sb.ToString();
            }
        }
    }
}
