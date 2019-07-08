using Geev.NHibernate.EntityMappings;

namespace Geev.NHibernate.Tests.Entities
{
    public class BookMap : EntityMap<Book>
    {
        public BookMap() : base("Books")
        {
            Id(x => x.Id);
            Map(x => x.Name);

            this.MapFullAudited();
        }
    }
}