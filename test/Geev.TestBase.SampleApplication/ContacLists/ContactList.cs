using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Geev.Domain.Entities;
using Geev.TestBase.SampleApplication.People;

namespace Geev.TestBase.SampleApplication.ContacLists
{
    [Table("ContactLists")]
    public class ContactList : Entity, IMustHaveTenant
    {
        public virtual int TenantId { get; set; }

        public virtual string Name { get; set; }

        [ForeignKey("ContactListId")]
        public virtual ICollection<Person> People { get; set; }
    }
}
