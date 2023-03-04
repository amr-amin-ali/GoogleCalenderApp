using System.Text.Json;

namespace GoogleCalenderApp.Models
{
    public class AppSettings
    {
        public string ApplicationName { get; set; }
        public string CalendarApiKey { get; set; }
        public string CalendarId { get; set; }
        public string OAuthClientId { get; set; }
        public string OAuthClientSecret { get; set; }
        public static AppSettings GetAppSettings()
        {
            string jsonFilePath = "appsettings.json";

            // read the entire JSON file into a string
            string jsonString = File.ReadAllText(jsonFilePath);

            // deserialize the JSON string into a C# object
            return JsonSerializer.Deserialize<AppSettings>(jsonString);
        }

    }
}
