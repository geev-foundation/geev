using Geev.Application.Services.Dto;
using Geev.AutoMapper;

namespace Geev.TestBase.SampleApplication.People.Dto
{
    [AutoMap(typeof(Person))]
    public class PersonDto : EntityDto
    {
        public int ContactListId { get; set; }

        public string Name { get; set; }
    }
}