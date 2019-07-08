using Geev.Application.Services.Dto;
using Geev.AutoMapper;

namespace Geev.TestBase.SampleApplication.ContacLists
{
    [AutoMapFrom(typeof(ContactList))]
    public class ContactListDto : EntityDto
    {
        public virtual string Name { get; set; }
    }
}