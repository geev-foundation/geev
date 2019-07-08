using Geev.Domain.Entities;

namespace Geev.Dapper.NHibernate.Tests
{
    public class Person : Entity
    {
        protected Person()
        {
        }

        public Person(string name) : this()
        {
            Name = name;
        }

        public virtual string Name { get; set; }
    }
}
