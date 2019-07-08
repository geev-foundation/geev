using System.ComponentModel.DataAnnotations.Schema;
using Geev.Domain.Entities;
using Geev.Domain.Entities.Auditing;

namespace Geev.TestBase.SampleApplication.Messages
{
    [Table("Messages")]
    public class Message : FullAuditedEntity, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public string Text { get; set; }

        public Message()
        {

        }

        public Message(int? tenantId, string text)
        {
            TenantId = tenantId;
            Text = text;
        }
    }
}
