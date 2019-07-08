using Geev.NHibernate.EntityMappings;

namespace Geev.NHibernate.Tests.Entities
{
    public class PersonMap : EntityMap<Person>
    {
        public PersonMap()
            : base("People")
        {
            Map(x => x.Name);
        }
    }
}