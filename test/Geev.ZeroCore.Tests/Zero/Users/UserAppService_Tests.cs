using System.Threading.Tasks;
using Geev.Application.Services.Dto;
using Geev.ZeroCore.SampleApp.Application.Users;
using Shouldly;
using Xunit;

namespace Geev.Zero.Users
{
    public class UserAppService_Tests : GeevZeroTestBase
    {
        private readonly IUserAppService _userAppService;

        public UserAppService_Tests()
        {
            _userAppService = Resolve<IUserAppService>();
        }

        [Fact]
        public async Task Should_Get_All_Users()
        {
            var users = await _userAppService.GetAll(new PagedAndSortedResultRequestDto());
            users.TotalCount.ShouldBeGreaterThan(0);
            users.Items.Count.ShouldBeGreaterThan(0);
        }
    }
}
