using System.Threading.Tasks;
using Geev.Application.Services;
using Geev.Application.Services.Dto;

namespace Geev.ZeroCore.SampleApp.Application.Shop
{
    public interface IOrderAppService : IApplicationService
    {
        Task<ListResultDto<OrderListDto>> GetOrders();
    }
}