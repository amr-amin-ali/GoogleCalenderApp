using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Services;
using Google.Apis.Util;
using GoogleCalenderApp.Contracts;

namespace GoogleCalenderApp.Presentation
{
    public class OAuthManager : IOAuthManager
    {
        private readonly string _oAuthClientId = Models.AppSettings.GetAppSettings().OAuthClientId;
        private readonly string _oAuthClientSecret = Models.AppSettings.GetAppSettings().OAuthClientSecret;
        private readonly ILogger _logger;
        //private readonly string _oAuthClientId = "995426710029-kjmq32lmdpn03js5pepuqi70uortavji.apps.googleusercontent.com";
        //private readonly string _oAuthClientSecret = "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";

        private readonly string[] _scopes = {
            "https://www.googleapis.com/auth/calendar",
            "https://www.googleapis.com/auth/calendar.events"
        };
        public OAuthManager(ILogger logger)
        {
            _logger = logger;
        }
        public async Task<UserCredential> Authenticate()
        {
            try
            {
                var userCredentials = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    new ClientSecrets
                    {
                        ClientId = _oAuthClientId,
                        ClientSecret = _oAuthClientSecret
                    },
                    _scopes,
                    "user",
                    CancellationToken.None
                    ).Result;

                if (userCredentials.Token.IsExpired(SystemClock.Default))
                    userCredentials.RefreshTokenAsync(CancellationToken.None).Wait();

                var calendarService = new CalendarService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = userCredentials
                });
                return userCredentials;
            }
            catch (Exception ex)
            {
                _logger.Log($"OAuth Error: {ex.Message}");
                return null;
            }
        }
    }
}
