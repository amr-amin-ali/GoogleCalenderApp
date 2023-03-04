using System.Text.Json;

namespace GoogleCalenderApp
{
    public class AppSettings
    {
        public string ApiKey { get; }
        public string CalendarId { get; }
        //public static AppSettings GetConfigurations()
        //{
        //    var jsonString = File.ReadAllText(@"appsettings.json");
        //    var settings= JsonSerializer.Deserialize<AppSettings>(jsonString);
        //    return settings;

        //}
    }
}
