using System.Threading.Tasks;
using Geev.Application.Services;
using Geev.Application.Services.Dto;

namespace Geev.ZeroCore.SampleApp.Application.Shop
{
    public interface IProductAppService : IApplicationService
    {
        Task<ListResultDto<ProductListDto>> GetProducts();

        Task CreateProduct(ProductCreateDto input);

        Task UpdateProduct(ProductUpdateDto input);

        Task Translate(int productId, ProductTranslationDto input);
    }
}