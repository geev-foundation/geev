using System;
using System.Threading.Tasks;
using Geev.Auditing;
using Geev.Dependency;
using Geev.RealTime;
using Geev.Runtime.Session;
using Castle.Core.Logging;

namespace Geev.AspNetCore.SignalR.Hubs
{
    public abstract class OnlineClientHubBase : GeevHubBase, ITransientDependency
    {
        protected IOnlineClientManager OnlineClientManager { get; }
        protected IClientInfoProvider ClientInfoProvider { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeevCommonHub"/> class.
        /// </summary>
        protected OnlineClientHubBase(
            IOnlineClientManager onlineClientManager,
            IClientInfoProvider clientInfoProvider)
        {
            OnlineClientManager = onlineClientManager;
            ClientInfoProvider = clientInfoProvider;

            Logger = NullLogger.Instance;
            GeevSession = NullGeevSession.Instance;
        }

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();

            var client = CreateClientForCurrentConnection();

            Logger.Debug("A client is connected: " + client);

            OnlineClientManager.Add(client);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await base.OnDisconnectedAsync(exception);

            Logger.Debug("A client is disconnected: " + Context.ConnectionId);

            try
            {
                OnlineClientManager.Remove(Context.ConnectionId);
            }
            catch (Exception ex)
            {
                Logger.Warn(ex.ToString(), ex);
            }
        }

        protected virtual IOnlineClient CreateClientForCurrentConnection()
        {
            return new OnlineClient(
                Context.ConnectionId,
                GetIpAddressOfClient(),
                GeevSession.TenantId,
                GeevSession.UserId
            );
        }

        protected virtual string GetIpAddressOfClient()
        {
            try
            {
                return ClientInfoProvider.ClientIpAddress;
            }
            catch (Exception ex)
            {
                Logger.Error("Can not find IP address of the client! connectionId: " + Context.ConnectionId);
                Logger.Error(ex.Message, ex);
                return "";
            }
        }
    }
}