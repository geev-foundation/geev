using Geev.EntityFrameworkCore.Dapper.Tests.Domain;

using DapperExtensions.Mapper;

namespace Geev.EntityFrameworkCore.Dapper.Tests.Dapper
{
    public sealed class CommentMap : ClassMapper<Comment>
    {
        public CommentMap()
        {
            Table("Comments");
            Map(x => x.Id).Key(KeyType.Identity);
            AutoMap();
        }
    }
}
