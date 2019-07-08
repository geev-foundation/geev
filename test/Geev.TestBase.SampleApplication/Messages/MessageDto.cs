using Geev.Application.Services.Dto;
using Geev.AutoMapper;

namespace Geev.TestBase.SampleApplication.Messages
{
    [AutoMap(typeof(Message))]
    public class MessageDto : FullAuditedEntityDto
    {
        public int? TenantId { get; set; }

        public string Text { get; set; }
    }
}