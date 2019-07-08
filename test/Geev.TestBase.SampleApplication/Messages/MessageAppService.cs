using System.Linq;
using Geev.Application.Services;
using Geev.Domain.Repositories;
using Geev.Extensions;
using Geev.Linq.Extensions;

namespace Geev.TestBase.SampleApplication.Messages
{
    public class MessageAppService : CrudAppService<Message, MessageDto, int, GetMessagesWithFilterInput>
    {
        public MessageAppService(IRepository<Message, int> repository)
            : base(repository)
        {

        }

        protected override IQueryable<Message> CreateFilteredQuery(GetMessagesWithFilterInput input)
        {
            return base.CreateFilteredQuery(input)
                .WhereIf(!input.Text.IsNullOrWhiteSpace(), m => m.Text.Contains(input.Text));
        }
    }
}