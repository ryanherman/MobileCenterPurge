namespace AzureAnalytics.Models
{
    public class Events
    {

            public Event[] events { get; set; }
            public int total { get; set; }
            public int total_devices { get; set; }
     

        public class Event
        {
            public string id { get; set; }
            public string name { get; set; }
            public int deviceCount { get; set; }
            public int previous_device_count { get; set; }
            public int count { get; set; }
            public int previous_count { get; set; }
            public string count_per_device { get; set; }
            public string count_per_session { get; set; }
        }

    }
}