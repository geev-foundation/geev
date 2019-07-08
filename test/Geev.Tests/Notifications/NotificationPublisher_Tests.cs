﻿using System.Threading.Tasks;
using Geev.BackgroundJobs;
using Geev.Domain.Uow;
using Geev.Notifications;
using NSubstitute;
using Xunit;

namespace Geev.Tests.Notifications
{
    public class NotificationPublisher_Tests : TestBaseWithLocalIocManager
    {
        private readonly NotificationPublisher _publisher;
        private readonly INotificationStore _store;
        private readonly IBackgroundJobManager _backgroundJobManager;

        public NotificationPublisher_Tests()
        {
            _store = Substitute.For<INotificationStore>();
            _backgroundJobManager = Substitute.For<IBackgroundJobManager>();
            _publisher = new NotificationPublisher(_store, _backgroundJobManager, Substitute.For<INotificationConfiguration>(), SequentialGuidGenerator.Instance, LocalIocManager);
            _publisher.UnitOfWorkManager = Substitute.For<IUnitOfWorkManager>();
            _publisher.UnitOfWorkManager.Current.Returns(Substitute.For<IActiveUnitOfWork>());
        }

        [Fact]
        public async Task Should_Publish_General_Notification()
        {
            //Arrange
            var notificationData = CreateNotificationData();

            //Act
            await _publisher.PublishAsync("TestNotification", notificationData, severity: NotificationSeverity.Success);

            //Assert
            await _store.Received()
                .InsertNotificationAsync(
                    Arg.Is<NotificationInfo>(
                        n => n.NotificationName == "TestNotification" &&
                             n.Severity == NotificationSeverity.Success &&
                             n.DataTypeName == notificationData.GetType().AssemblyQualifiedName &&
                             n.Data.Contains("42")
                        )
                );

            await _backgroundJobManager.Received()
                .EnqueueAsync<NotificationDistributionJob, NotificationDistributionJobArgs>(
                    Arg.Any<NotificationDistributionJobArgs>()
                );
        }

        private static NotificationData CreateNotificationData()
        {
            var notificationData = new NotificationData();
            notificationData["TestValue"] = 42;
            return notificationData;
        }
    }
}
