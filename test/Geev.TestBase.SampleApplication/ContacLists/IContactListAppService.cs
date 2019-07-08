using System.Collections.Generic;
using Geev.Application.Services;

namespace Geev.TestBase.SampleApplication.ContacLists
{
    public interface IContactListAppService : IApplicationService
    {
        void Test();

        List<ContactListDto> GetContactLists();
    }
}
