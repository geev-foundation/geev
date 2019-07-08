using Geev.Application.Services;

namespace Geev.ZeroCore.SampleApp.Application.Users
{
    public interface IUserAppService : IAsyncCrudAppService<UserDto, long>
    {
        
    }
}
