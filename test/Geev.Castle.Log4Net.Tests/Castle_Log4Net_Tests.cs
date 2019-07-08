using System.IO;
using Castle.Facilities.Logging;
using Castle.Windsor;
using Geev.Castle.Logging.Log4Net;
using Castle.Core.Logging;
using Geev.IO;
using Shouldly;
using Xunit;

namespace Geev.Castle.Log4Net.Tests
{
    public class Castle_Log4Net_Tests
    {
        [Fact]
        public void Should_Write_Logs_To_Text_File()
        {
            //Arrange
            var logFilePath = Path.Combine(Directory.GetCurrentDirectory(), "GeevCastleLog4NetTests-Logs.txt");
            FileHelper.DeleteIfExists(logFilePath); //Clean old file

            //Act
            var container = new WindsorContainer();
            container.AddFacility<LoggingFacility>(facility =>
            {
                facility.UseGeevLog4Net().WithConfig("log4net.config");
            });

            var logger = container.Resolve<ILoggerFactory>().Create(typeof(Castle_Log4Net_Tests));
            logger.Info("Should_Write_Logs_To_Text_File works!");

            //Assert
            File.Exists(logFilePath).ShouldBeTrue();
        }
    }
}
