using Geev.Application.Services;

namespace Geev.Web.Api.Tests.AppServices
{
    public interface IMyFirstAppService : IApplicationService
    {
        string GetStr(int i);

        [MyIgnoreApi]
        string GetStr2(int i);
    }
}