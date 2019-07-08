using System.ComponentModel.DataAnnotations;
using Geev.AutoMapper;

namespace Geev.TestBase.SampleApplication.People.Dto
{
    [AutoMapTo(typeof(Person))]
    public class CreatePersonInput
    {
        [Range(1, int.MaxValue)]
        public int ContactListId { get; set; }

        [Required]
        [StringLength(Person.MaxNameLength)]
        public string Name { get; set; }
    }
}