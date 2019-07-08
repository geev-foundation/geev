using Geev.Application.Services;
using Geev.Authorization;
using Geev.Domain.Repositories;

namespace Geev.TestBase.SampleApplication.Messages
{
    [GeevAuthorize("AsyncMessageAppService_Permission")]
    public class AsyncMessageAppService : AsyncCrudAppService<Message, MessageDto>, IAsyncMessageAppService
    {
        public AsyncMessageAppService(IRepository<Message> repository)
            : base(repository)
        {

        }
    }
}