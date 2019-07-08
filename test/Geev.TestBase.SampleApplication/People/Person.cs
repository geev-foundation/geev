using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Geev.Domain.Entities.Auditing;
using Geev.TestBase.SampleApplication.ContacLists;

namespace Geev.TestBase.SampleApplication.People
{
    [Table("People")]
    public class Person : AuditedEntity, IDeletionAudited
    {
        public const int MaxNameLength = 64;

        [Required]
        [StringLength(MaxNameLength)]
        public virtual string Name { get; set; }

        [ForeignKey("ContactListId")]
        public virtual ContactList ContactList { get; set; }

        public virtual int ContactListId { get; set; }

        public virtual bool IsDeleted { get; set; }

        public virtual long? DeleterUserId { get; set; }

        public virtual DateTime? DeletionTime { get; set; }
    }
}
