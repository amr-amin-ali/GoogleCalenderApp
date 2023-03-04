using Google.Apis.Auth.OAuth2;

namespace GoogleCalenderApp.Contracts
{
    public interface IOAuthManager
    {
        Task<UserCredential> Authenticate();
    }
}
