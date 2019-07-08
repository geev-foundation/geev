using System.ComponentModel.DataAnnotations.Schema;
using Geev.Domain.Entities.Auditing;

namespace Geev.Dapper.Tests.Entities
{
    [Table("Goods")]
    public class Good : FullAuditedEntity
    {
        public string Name { get; set; }

        public int ParentId { get; set; }
    }
}