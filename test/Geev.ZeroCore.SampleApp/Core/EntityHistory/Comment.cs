using Geev.Auditing;
using Geev.Domain.Entities;

namespace Geev.ZeroCore.SampleApp.Core.EntityHistory
{
    [Audited]
    public class Comment : Entity
    {
        public Post Post { get; set; }

        public string Content { get; set; }
    }
}
