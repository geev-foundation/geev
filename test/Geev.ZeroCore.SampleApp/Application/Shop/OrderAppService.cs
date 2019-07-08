using System.Collections.Generic;
using System.Threading.Tasks;
using Geev.Application.Services;
using Geev.Application.Services.Dto;
using Geev.Domain.Repositories;
using Geev.ZeroCore.SampleApp.Core.Shop;
using Microsoft.EntityFrameworkCore;

namespace Geev.ZeroCore.SampleApp.Application.Shop
{
    public class OrderAppService : ApplicationService, IOrderAppService
    {
        private readonly IRepository<Order> _orderRepository;

        public OrderAppService(IRepository<Order> orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<ListResultDto<OrderListDto>> GetOrders()
        {
            var products = await _orderRepository.GetAll()
                .Include(p => p.Translations)
                .Include(p => p.Products)
                .ToListAsync();

            return new ListResultDto<OrderListDto>(ObjectMapper.Map<List<OrderListDto>>(products));
        }
    }
}