using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Geev.Domain.Entities;
using Geev.Domain.Entities.Auditing;

namespace Geev.Dapper.Tests.Entities
{
    [Table("ProductDetails")]
    public class ProductDetail : FullAuditedEntity, IMustHaveTenant
    {
        protected ProductDetail()
        {
        }

        public ProductDetail(string gender) : this()
        {
            Gender = gender;
        }

        [Required]
        public string Gender { get; set; }

        public int TenantId { get; set; }
    }
}
