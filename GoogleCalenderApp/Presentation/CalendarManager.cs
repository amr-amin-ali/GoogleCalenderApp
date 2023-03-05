using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using GoogleCalenderApp.Contracts;
using GoogleCalenderApp.Models;

namespace GoogleCalenderApp.Presentation
{
    public class CalendarManager : ICalendarManager
    {
        private readonly IOAuthManager _authManager;
        private readonly ILogger _logger;
        private readonly INotifier _notifier;
        private readonly string CalendarId = Models.AppSettings.GetAppSettings().CalendarId;
        public CalendarManager(IOAuthManager authManager, ILogger logger, INotifier notifier)
        {
            _authManager = authManager;
            _logger = logger;
            _notifier = notifier;
        }

        public async Task<IEnumerable<CalendarEvent>> GetAllUpcommongEventsAsync()
        {
            // create service
            var service = new CalendarService(new BaseClientService.Initializer()
            {
                ApplicationName = "Amr Amin",
                HttpClientInitializer = await _authManager.Authenticate()
            });

            // Define parameters for the request to get only the upcomming events
            EventsResource.ListRequest request = service.Events.List("primary");
            request.TimeMin = DateTime.Now;

            //Specify fields to retrieve
            request.Fields = "items(id,summary,start,end)";

            var calendarEvents = new List<CalendarEvent>();
            try
            {
                var response = await request.ExecuteAsync();

                if (response.Items != null)
                {
                    foreach (var item in response.Items)
                    {
                        var start = item.Start.Date != null ?
                            DateOnly.Parse(item.Start.Date) :
                            DateOnly.FromDateTime((DateTime)item.Start.DateTime);
                        var end = item.End.Date != null ?
                            DateOnly.Parse(item.End.Date) :
                            DateOnly.FromDateTime((DateTime)item.End.DateTime);
                        calendarEvents.Add(new CalendarEvent
                        {
                            Id = item.Id,
                            Title = item.Summary,
                            Start = start,
                            End = end,
                        });
                    }
                }
            }

            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"CalendarManager Error: {ex.Message}");
            }
            return calendarEvents;
        }

        public async Task<IEnumerable<CalendarEvent>> GetAllEventsAsync()
        {
            // create service
            var service = new CalendarService(new BaseClientService.Initializer()
            {
                //ApiKey = ApiKey,
                ApplicationName = "Amr Amin",
                HttpClientInitializer = await _authManager.Authenticate()
            });
            //send request
            var request = service.Events.List(CalendarId);
            //Specify fields to retrieve
            request.Fields = "items(id,summary,start,end)";

            var calendarEvents = new List<CalendarEvent>();
            try
            {
                var response = await request.ExecuteAsync();
                if (response.Items != null)
                {
                    foreach (var item in response.Items)
                    {
                        var start = item.Start.Date != null ?
                            DateOnly.Parse(item.Start.Date) :
                            DateOnly.FromDateTime((DateTime)item.Start.DateTime);
                        var end = item.End.Date != null ?
                            DateOnly.Parse(item.End.Date) :
                            DateOnly.FromDateTime((DateTime)item.End.DateTime);
                        calendarEvents.Add(new CalendarEvent
                        {
                            Id = item.Id,
                            Title = item.Summary,
                            Start = start,
                            End = end,
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"CalendarManager.GetAllEventsAsync.catch Error: {ex.Message}");
            }
            return calendarEvents;
        }

        public void DisplayEventsIdsToConsole(IEnumerable<CalendarEvent> events)
        {
            _logger.Log("|---------------------------------------------------------------------------|");
            _logger.Log("| Title                                         |          Id           |");
            _logger.Log("|---------------------------------------------------------------------------|");

            int count = 1;
            foreach (var ev in events)
                _logger.Log($"|{ev.Title.PadRight(44, ' ')}|{ev.Id.ToString().PadRight(12, ' ')}|");
            _logger.Log("|---------------------------------------------------------------------------|");
        }

        public void DisplayEventsToConsole(IEnumerable<CalendarEvent> events)
        {
            _logger.Log("|---------------------------------------------------------------------------|");
            _logger.Log("| # | Title                                         |  Start      |  Time   |");
            _logger.Log("|---------------------------------------------------------------------------|");

            int count = 1;
            foreach (var ev in events)
                _logger.Log($"|{(count++).ToString().PadRight(3, ' ')}|   {ev.Title.PadRight(44, ' ')}|{ev.Start.ToString().PadRight(12, ' ')}|{ev.Time.ToString().PadRight(10, ' ')}|");
            _logger.Log("|---------------------------------------------------------------------------|");
        }

        public async Task CreateEvent(string title, string description, DateTime startDateTime, DateTime endDateTime)
        {
            try
            {
                // create service
                var service = new CalendarService(new BaseClientService.Initializer()
                {
                    ApplicationName = Models.AppSettings.GetAppSettings().ApplicationName,// "Amr Amin",
                    HttpClientInitializer = await _authManager.Authenticate()
                });
                // Create a new event object
                Event newEvent = new Event()
                {
                    Summary = title,
                    Description = description,
                    Start = new EventDateTime()
                    {
                        DateTime = startDateTime,
                    },
                    End = new EventDateTime()
                    {
                        DateTime = endDateTime,
                    },

                };

                // Insert the event
                EventsResource.InsertRequest request = service.Events.Insert(newEvent, "primary");
                Event createdEvent = request.Execute();

                _logger.Log($"Event created, link to event:\n\t {createdEvent.HtmlLink}");
                bool result = await _notifier.Notify($"Event created, link to event:\n\t {createdEvent.HtmlLink}");
                if (result) _logger.Log("Email verification was sent to your inbox");
            }
            catch (Exception ex)
            {
                _logger.Log($"Error creating your event: {ex.Message}");
            }


        }

        public async Task<bool> DeleteEvent(string id)
        {
            // Create a new CalendarService object
            var service = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = await _authManager.Authenticate(),
                ApplicationName = "Amr Amin",
            });

            // Delete the event with the specified ID
            EventsResource.DeleteRequest request = service.Events.Delete("primary", id);
            try
            {
                await request.ExecuteAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.Log($"Event was not deleted, some thing went wrong,\n Error: {ex.Message}");
                return false;
            }

        }

    }
}
;