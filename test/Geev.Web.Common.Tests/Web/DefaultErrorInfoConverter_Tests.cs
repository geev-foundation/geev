using System.Threading.Tasks;
using Geev.Domain.Entities;
using Geev.TestBase;
using Geev.Web.Models;
using Xunit;

namespace Geev.Web.Common.Tests.Web
{
    public class DefaultErrorInfoConverter_Tests : GeevIntegratedTestBase<GeevWebCommonTestModule>
    {
        private readonly DefaultErrorInfoConverter _defaultErrorInfoConverter;

        public DefaultErrorInfoConverter_Tests()
        {
            _defaultErrorInfoConverter = Resolve<DefaultErrorInfoConverter>();
        }

        [Fact]
        public async Task DefaultErrorInfoConverter_Should_Work_For_EntityNotFoundException_Overload_Methods()
        {
            var message = "Test message";
            var errorInfo = _defaultErrorInfoConverter.Convert(new EntityNotFoundException(message));

            Assert.Equal(errorInfo.Message, message);

            var exceptionWithoutMessage = new EntityNotFoundException();
            errorInfo = _defaultErrorInfoConverter.Convert(exceptionWithoutMessage);

            Assert.Equal(errorInfo.Message, exceptionWithoutMessage.Message);
        }
    }
}
