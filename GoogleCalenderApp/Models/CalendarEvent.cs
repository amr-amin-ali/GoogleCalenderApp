namespace GoogleCalenderApp.Models
{
    public class CalendarEvent
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public DateOnly Start { get; set; }
        public DateOnly End { get; set; }
        public TimeOnly Time { get; set; }
    }
}
