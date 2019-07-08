﻿using System.Threading.Tasks;
using Geev.Notifications;
using Geev.Runtime.Session;
using Shouldly;
using Xunit;

namespace Geev.Zero.Notifications
{
    public class NotificationDistributer_Tests : GeevZeroTestBase
    {
        private readonly INotificationPublisher _publisher;
        private readonly FakeNotificationDistributer _fakeNotificationDistributer;

        public NotificationDistributer_Tests()
        {
            _publisher = LocalIocManager.Resolve<INotificationPublisher>();
            _fakeNotificationDistributer = LocalIocManager.Resolve<FakeNotificationDistributer>();
        }

        [Fact]
        public async Task Should_Distribute_Notification_Using_Custom_Distributer()
        {
            //Arrange
            var notificationData = new NotificationData();

            //Act
            await _publisher.PublishAsync("TestNotification", notificationData, severity: NotificationSeverity.Success, userIds: new[] { GeevSession.ToUserIdentifier() });

            //Assert
            _fakeNotificationDistributer.IsDistributeCalled.ShouldBeTrue();
        }
    }
}
