using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Geev.Domain.Entities;
using Geev.Domain.Entities.Auditing;

namespace Geev.Dapper.Tests.Entities
{
    [Table("Products")]
    public class Product : FullAuditedEntity, IMayHaveTenant
    {
        protected Product()
        {
        }

        public Product(string name) : this()
        {
            Name = name;
        }

        [Required]
        public string Name { get; set; }
        
        public Status Status { get; set; }

        public int? TenantId { get; set; }
    }

    public enum Status
    {
        Active,
        Passive
    }
}
