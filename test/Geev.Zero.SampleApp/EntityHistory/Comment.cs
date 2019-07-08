using Geev.Auditing;
using Geev.Domain.Entities;

namespace Geev.Zero.SampleApp.EntityHistory
{
    [Audited]
    public class Comment : Entity
    {
        public virtual Post Post { get; set; }

        public string Content { get; set; }
    }
}
