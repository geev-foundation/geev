using System;
using System.Threading.Tasks;
using Geev.AspNetCore.SignalR.Hubs;
using Geev.Dependency;
using Geev.Notifications;
using Geev.RealTime;
using Castle.Core.Logging;
using Microsoft.AspNetCore.SignalR;

namespace Geev.AspNetCore.SignalR.Notifications
{
    /// <summary>
    /// Implements <see cref="IRealTimeNotifier"/> to send notifications via SignalR.
    /// </summary>
    public class SignalRRealTimeNotifier : IRealTimeNotifier, ITransientDependency
    {
        /// <summary>
        /// Reference to the logger.
        /// </summary>
        public ILogger Logger { get; set; }

        private readonly IOnlineClientManager _onlineClientManager;

        private readonly IHubContext<GeevCommonHub> _hubContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="SignalRRealTimeNotifier"/> class.
        /// </summary>
        public SignalRRealTimeNotifier(
            IOnlineClientManager onlineClientManager,
            IHubContext<GeevCommonHub> hubContext)
        {
            _onlineClientManager = onlineClientManager;
            _hubContext = hubContext;
            Logger = NullLogger.Instance;
        }

        /// <inheritdoc/>
        public Task SendNotificationsAsync(UserNotification[] userNotifications)
        {
            foreach (var userNotification in userNotifications)
            {
                try
                {
                    var onlineClients = _onlineClientManager.GetAllByUserId(userNotification);
                    foreach (var onlineClient in onlineClients)
                    {
                        var signalRClient = _hubContext.Clients.Client(onlineClient.ConnectionId);
                        if (signalRClient == null)
                        {
                            Logger.Debug("Can not get user " + userNotification.ToUserIdentifier() + " with connectionId " + onlineClient.ConnectionId + " from SignalR hub!");
                            continue;
                        }

                        signalRClient.SendAsync("getNotification", userNotification);
                    }
                }
                catch (Exception ex)
                {
                    Logger.Warn("Could not send notification to user: " + userNotification.ToUserIdentifier());
                    Logger.Warn(ex.ToString(), ex);
                }
            }

            return Task.FromResult(0);
        }
    }
}
