using System;
using System.Threading.Tasks;
using Geev.Notifications;

namespace Geev.Zero.Notifications
{
    public class FakeNotificationDistributer : INotificationDistributer
    {
        public bool IsDistributeCalled { get; set; }

        public async Task DistributeAsync(Guid notificationId)
        {
            IsDistributeCalled = true;
        }
    }
}