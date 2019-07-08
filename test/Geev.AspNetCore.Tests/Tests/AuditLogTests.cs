using System.Net;
using System.Threading.Tasks;
using Geev.AspNetCore.App.Controllers;
using Geev.AspNetCore.App.Models;
using Geev.AspNetCore.Mocks;
using Geev.Web.Models;
using Shouldly;
using Xunit;

namespace Geev.AspNetCore.Tests
{
    public class AuditLogTests : AppTestBase
    {
        private readonly MockAuditingStore _mockAuditingStore;

        public AuditLogTests()
        {
            _mockAuditingStore = Resolve<MockAuditingStore>();
        }

        [Fact]
        public async Task Should_Write_Audit_Logs()
        {
            _mockAuditingStore.Logs.Count.ShouldBe(0);

            //Act

            await GetResponseAsObjectAsync<AjaxResponse<SimpleViewModel>>(
                   GetUrl<SimpleTestController>(
                       nameof(SimpleTestController.SimpleJsonException),
                       new
                       {
                           message = "A test message",
                           userFriendly = true
                       }),
                   HttpStatusCode.InternalServerError
               );

            //Assert

            _mockAuditingStore.Logs.Count.ShouldBe(1);
            var auditLog = _mockAuditingStore.Logs.ToArray()[0];
            auditLog.MethodName.ShouldBe(nameof(SimpleTestController.SimpleJsonException));
        }
    }
}