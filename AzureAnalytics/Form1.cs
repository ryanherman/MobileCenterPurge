using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
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
        
        private string URLServer = "https://api.mobile.azure.com";

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
            var startdate = DateTime.Now.AddDays(-7).ToString("s");
            var enddate = DateTime.Now.ToString("s");

                var client = new HttpClient();

                client.BaseAddress = new Uri(URLServer);
                client.DefaultRequestHeaders.Add("X-API-Token", APIKey);
                var endpoint = "/v0.1/apps/" + Owner + "/" + AppNames[Apps.SelectedIndex] + "/analytics/events?start=" + startdate + "&end=" + enddate;
                var response = await client.GetAsync(endpoint);
            var projectsJson = response.Content.ReadAsStringAsync().Result;

            var translateResponse = JsonConvert.DeserializeObject<Events>(projectsJson);

            foreach (var eachItem in translateResponse.events)
            {
                listBox1.Items.Add(eachItem.name);
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
    }
}
