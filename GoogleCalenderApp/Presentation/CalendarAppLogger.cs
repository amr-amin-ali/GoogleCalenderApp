using GoogleCalenderApp.Contracts;

namespace GoogleCalenderApp.Presentation
{
    public class CalendarAppLogger : ILogger
    {
        public void Log(string message)
        {
            Console.WriteLine(message);
        }
    }
}
