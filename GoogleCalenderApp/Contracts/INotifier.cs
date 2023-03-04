namespace GoogleCalenderApp.Contracts
{
    public interface INotifier
    {
        Task<bool> Notify(string message);
    }
}
