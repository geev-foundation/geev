using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Geev.Application.Services;
using Geev.Application.Services.Dto;
using Geev.Auditing;
using Geev.Authorization;
using Geev.AutoMapper;
using Geev.Domain.Repositories;
using Geev.Extensions;
using Geev.TestBase.SampleApplication.People.Dto;

namespace Geev.TestBase.SampleApplication.People
{
    public class PersonAppService : ApplicationService, IPersonAppService
    {
        private readonly IRepository<Person> _personRepository;

        public PersonAppService(IRepository<Person> personRepository)
        {
            _personRepository = personRepository;
        }

        [DisableAuditing]
        public ListResultDto<PersonDto> GetPeople(GetPeopleInput input)
        {
            var query = _personRepository.GetAll();

            if (!input.NameFilter.IsNullOrEmpty())
            {
                query = query.Where(p => p.Name.Contains(input.NameFilter));
            }

            var people = query.ToList();

            return new ListResultDto<PersonDto>(ObjectMapper.Map<List<PersonDto>>(people));
        }

        public async Task CreatePersonAsync(CreatePersonInput input)
        {
            await _personRepository.InsertAsync(ObjectMapper.Map<Person>(input));
        }

        [GeevAuthorize("CanDeletePerson")]
        public async Task DeletePerson(EntityDto input)
        {
            await _personRepository.DeleteAsync(input.Id);
        }

        public string TestPrimitiveMethod(int a, string b, EntityDto c)
        {
            return a + "#" + b + "#" + c.Id;
        }
    }
}
