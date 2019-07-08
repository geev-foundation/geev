namespace Geev.Auditing
{
    public interface IClientInfoProvider
    {
        string BrowserInfo { get; }

        string ClientIpAddress { get; }

        string ComputerName { get; }
    }
}