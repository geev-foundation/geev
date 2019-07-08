using Geev.Domain.Entities;

namespace Geev.NHibernate.Tests.Entities
{
    public class Person : Entity
    {
        public const int MaxNameLength = 64;

        public virtual string Name { get; set; }
    }
}