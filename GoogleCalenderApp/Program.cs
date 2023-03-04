using GoogleCalenderApp.Contracts;
using GoogleCalenderApp.Presentation;

ILogger logger = new CalendarAppLogger();
INotifier emailSender = new EmailSender();
var calendar = new CalendarManager(new OAuthManager(logger), logger, emailSender);

logger.Log("\nWelcome to your google calendar app");
logger.Log("Choose number corresponding to the operation you want to perform:");

int operation = 0;
while (operation == 0)
{
    logger.Log(@"
         ____________________________
        | # | Operation              |
        |----------------------------|
        | 1 | Get my Upcomming events|
        | 2 | Get all my events      |
        | 3 | Create new event       |
        | 4 | Delete an event        |
        | 5 | Exit from the app      |
        | 6 | Clear screen           |
        -----------------------------");
    logger.Log("Operation number: ");

    var userInput = Console.ReadLine();

    if (int.TryParse(userInput, out operation))
    {
        if (operation == 1)
        {
            //Get upcomming events
            logger.Log("\nGetting upcomming events...");
            var upcommingEvents = await calendar.GetAllUpcommongEventsAsync();
            calendar.DisplayEventsToConsole(upcommingEvents);

        }
        if (operation == 2)
        {
            //Get all my events
            logger.Log("\nGetting all events...");
            var allEvents = await calendar.GetAllEventsAsync();
            calendar.DisplayEventsToConsole(allEvents);
        }
        if (operation == 3)
        {
            //Create new event
            logger.Log("Please enter the event information: ");
            Console.Write("Event title:       ");
            var titleUserInput = Console.ReadLine();
            while (string.IsNullOrEmpty(titleUserInput) || titleUserInput.Length < 1)
            {
                Console.Write("Title can not be empty:       ");
                titleUserInput = Console.ReadLine();
            }
            Console.Write("Event description: ");
            var descriptionUserInput = Console.ReadLine();
            while (string.IsNullOrEmpty(descriptionUserInput) || descriptionUserInput.Length < 1)
            {
                Console.Write("Description can not be empty:       ");
                descriptionUserInput = Console.ReadLine();
            }

            Console.Write("Event starts in (ex: 01/27/2022):   ");
            var startUserInput = Console.ReadLine();
            DateTime eventStart;
            while (!DateTime.TryParse(startUserInput, out eventStart))
            {
                Console.Write("Enter a valid event start date (ex: 01/27/2022):       ");
                startUserInput = Console.ReadLine();
            }


            Console.Write("Event ends in (ex: 01/27/2022):   ");
            var endUserInput = Console.ReadLine();
            DateTime eventEnd;
            while (!DateTime.TryParse(endUserInput, out eventEnd))
            {
                Console.Write("Enter a valid event end date (ex: 01/27/2022):       ");
                endUserInput = Console.ReadLine();
                if (DateTime.TryParse(endUserInput, out eventEnd) && eventStart > eventEnd)
                {
                    logger.Log("Event cant end before being started:       ");
                    endUserInput = null;
                }
            }
            logger.Log("\nCreating event....");
            calendar.CreateEvent(titleUserInput, descriptionUserInput, eventStart, eventEnd).Wait();

        }
        if (operation == 4)
        {
            //Delete an event
            logger.Log("Here your events with its ids");
            var allEventsWithId = await calendar.GetAllEventsAsync();
            calendar.DisplayEventsIdsToConsole(allEventsWithId);

            logger.Log("Enter event id to delete:");
            var eventId = Console.ReadLine();
            while (String.IsNullOrWhiteSpace(eventId))
            {
                logger.Log("Enter event id to delete:");
                eventId = Console.ReadLine();
            }

            bool result = await calendar.DeleteEvent(eventId);
            if (result)
            {
                logger.Log("Event deleted successfully");
            }
            else
            {
                logger.Log("Event was not deleted.");
            }
        }
        if (operation == 5)
        {
            //Exit from the app
            logger.Log(@"
                ++++++++++++++++++++++++++++++++++++++
                +                                    +
                +       Goodby...                    +
                +                                    +
                ++++++++++++++++++++++++++++++++++++++");
            Environment.Exit(0);
        }
        if (operation == 6)
        {
            Console.Clear();
        }
        operation = 0;
    }
    else
    {
        operation = 0;
        logger.Log("\nPlease enter a valid operation number from the table:");
    }
}