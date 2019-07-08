using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Geev.Domain.Entities.Auditing;
using Geev.MultiTenancy;

namespace Geev.TestBase.SampleApplication.Crm
{
    [Table("Companies")]
    [MultiTenancySide(MultiTenancySides.Host)]
    public class Company : AuditedEntity
    {
        public string Name { get; set; }

        public virtual Address ShippingAddress { get; set; }

        public virtual Address BillingAddress { get; set; }

        [ForeignKey("CompanyId")]
        public virtual ICollection<Branch> Branches { get; set; }

        public Company()
        {
               
        }
    }
}
