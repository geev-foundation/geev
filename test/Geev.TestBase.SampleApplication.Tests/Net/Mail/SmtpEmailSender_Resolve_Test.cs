using Geev.Net.Mail;
using Geev.Net.Mail.Smtp;
using Shouldly;
using Xunit;

namespace Geev.TestBase.SampleApplication.Tests.Net.Mail
{
    public class SmtpEmailSender_Resolve_Test : GeevIntegratedTestBase<GeevKernelModule>
    {
        [Fact]
        public void Should_Resolve_EmailSenders()
        {
            Resolve<IEmailSender>().ShouldBeOfType(typeof(SmtpEmailSender));
            Resolve<ISmtpEmailSender>().ShouldBeOfType(typeof(SmtpEmailSender));
        }
    }
}
