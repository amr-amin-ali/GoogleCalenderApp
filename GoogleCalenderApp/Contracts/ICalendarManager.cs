using GoogleCalenderApp.Models;

namespace GoogleCalenderApp.Contracts
{
    public interface ICalendarManager
    {
        Task<IEnumerable<CalendarEvent>> GetAllUpcommongEventsAsync();
        Task<IEnumerable<CalendarEvent>> GetAllEventsAsync();
        Task CreateEvent(string title, string description, DateTime startDateTime, DateTime endDateTime);
        Task<bool> DeleteEvent(string id);
        void DisplayEventsToConsole(IEnumerable<CalendarEvent> events);
        void DisplayEventsIdsToConsole(IEnumerable<CalendarEvent> events);

    }
}
