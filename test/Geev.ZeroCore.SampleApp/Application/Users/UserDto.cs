using Geev.Application.Services.Dto;
using Geev.AutoMapper;
using Geev.ZeroCore.SampleApp.Core;

namespace Geev.ZeroCore.SampleApp.Application.Users
{
    [AutoMap(typeof(User))]
    public class UserDto : EntityDto<long>
    {
        public string UserName { get; set; }
    }
}