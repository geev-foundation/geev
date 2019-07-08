using Geev.Application.Services.Dto;

namespace Geev.TestBase.SampleApplication.Messages
{
    public class GetMessagesWithFilterInput : PagedAndSortedResultRequestDto
    {
        public string Text { get; set; }
    }
}