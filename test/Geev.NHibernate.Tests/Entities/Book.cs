using Geev.Domain.Entities.Auditing;

namespace Geev.NHibernate.Tests.Entities
{
    public class Book : FullAuditedEntity
    {
        public virtual string Name { get; set; }
    }
}