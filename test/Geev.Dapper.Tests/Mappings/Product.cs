using Geev.Dapper.Tests.Entities;

using DapperExtensions.Mapper;

namespace Geev.Dapper.Tests.Mappings
{
    public sealed class ProductMap : ClassMapper<Product>
    {
        public ProductMap()
        {
            Table("Products");
            Map(x => x.Id).Key(KeyType.Identity);
            AutoMap();
        }
    }
}
