using Geev.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Geev.Dapper.Tests.Entities
{
    [Table("Person")]
    public class Person : Entity, IMustHaveTenant
    {
        protected Person()
        {
        }

        public Person(string name) : this()
        {
            Name = name;
        }

        public virtual string Name { get; set; }

        public int TenantId { get; set; }
    }
}
