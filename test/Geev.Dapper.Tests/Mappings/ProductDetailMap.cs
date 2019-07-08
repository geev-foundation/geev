using Geev.Dapper.Tests.Entities;

using DapperExtensions.Mapper;

namespace Geev.Dapper.Tests.Mappings
{
    public sealed class ProductDetailMap : ClassMapper<ProductDetail>
    {
        public ProductDetailMap()
        {
            Table("ProductDetails");
            AutoMap();
        }
    }
}
