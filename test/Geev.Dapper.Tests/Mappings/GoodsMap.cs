using Geev.Dapper.Tests.Entities;
using DapperExtensions.Mapper;

namespace Geev.Dapper.Tests.Mappings
{
    public sealed class GoodsMap : ClassMapper<Good>
    {
        public GoodsMap()
        {
            Table("Goods");
            Map(x => x.Id).Key(KeyType.Identity);
            AutoMap();
        }
    }
}