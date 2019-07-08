using Geev.Application.Services;
using Geev.Domain.Repositories;
using Geev.ZeroCore.SampleApp.Core;

namespace Geev.ZeroCore.SampleApp.Application.Users
{
    public class UserAppService : AsyncCrudAppService<User, UserDto, long>, IUserAppService
    {
        public UserAppService(IRepository<User, long> repository) 
            : base(repository)
        {
            
        }
    }
}