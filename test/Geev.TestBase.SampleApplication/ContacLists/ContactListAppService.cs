using System.Collections.Generic;
using Geev.Application.Features;
using Geev.Application.Services;
using Geev.AutoMapper;
using Geev.Domain.Repositories;

namespace Geev.TestBase.SampleApplication.ContacLists
{
    public class ContactListAppService : ApplicationService, IContactListAppService
    {
        private readonly IRepository<ContactList> _contactListRepository;

        public ContactListAppService(IRepository<ContactList> contactListRepository)
        {
            _contactListRepository = contactListRepository;
        }

        [RequiresFeature(SampleFeatureProvider.Names.Contacts)]
        public void Test()
        {
            //This method is called only if SampleFeatureProvider.Names.Contacts feature is enabled
        }

        public List<ContactListDto> GetContactLists()
        {
            return ObjectMapper.Map<List<ContactListDto>>(_contactListRepository.GetAllList());
        }
    }
}