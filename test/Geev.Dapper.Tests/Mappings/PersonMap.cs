using Geev.Dapper.Tests.Entities;

using DapperExtensions.Mapper;

namespace Geev.Dapper.Tests.Mappings
{
    public sealed class PersonMap : ClassMapper<Person>
    {
        public PersonMap()
        {
            Table("Person");
            Map(x => x.Id).Key(KeyType.Identity);
            AutoMap();
        }
    }
}
