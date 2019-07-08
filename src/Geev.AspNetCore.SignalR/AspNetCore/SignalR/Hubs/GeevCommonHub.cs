using Geev.Auditing;
using Geev.RealTime;

namespace Geev.AspNetCore.SignalR.Hubs
{
    public class GeevCommonHub : OnlineClientHubBase
    {
        public GeevCommonHub(IOnlineClientManager onlineClientManager, IClientInfoProvider clientInfoProvider) 
            : base(onlineClientManager, clientInfoProvider)
        {
        }

        public void Register()
        {
            Logger.Debug("A client is registered: " + Context.ConnectionId);
        }
    }
}
