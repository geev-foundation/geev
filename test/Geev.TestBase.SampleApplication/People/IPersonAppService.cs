using System.Threading.Tasks;
using Geev.Application.Services;
using Geev.Application.Services.Dto;
using Geev.TestBase.SampleApplication.People.Dto;

namespace Geev.TestBase.SampleApplication.People
{
    public interface IPersonAppService : IApplicationService
    {
        ListResultDto<PersonDto> GetPeople(GetPeopleInput input);

        Task CreatePersonAsync(CreatePersonInput input);

        Task DeletePerson(EntityDto input);

        string TestPrimitiveMethod(int a, string b, EntityDto c);
    }
}