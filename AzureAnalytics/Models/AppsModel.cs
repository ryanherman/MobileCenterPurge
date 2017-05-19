namespace AzureAnalytics.Models
{
    public class AppsModel
    {

   
           public string id { get; set; }
        public string app_secret { get; set; }
        public string azure_subscription_id { get; set; }
        public string description { get; set; }
        public string display_name { get; set; }
        public string icon_url { get; set; }
        public string name { get; set; }
        public string os { get; set; }
        public Owner owner { get; set; }
        public string platform { get; set; }
        public string origin { get; set; }


        public class Owner
        {
            public string id { get; set; }
            public string avatar_url { get; set; }
            public string display_name { get; set; }
            public string email { get; set; }
            public string name { get; set; }
            public string type { get; set; }
        }

    }
}