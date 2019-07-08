using Geev.Application.Services.Dto;
using Geev.AutoMapper;
using GeevAspNetCoreDemo.Core.Domain;

namespace GeevAspNetCoreDemo.Core.Application.Dtos
{
    [AutoMap(typeof(Product))]
    public class ProductDto : EntityDto
    {
        public string Name { get; set; }

        public float Price { get; set; }
    }
}
