using System;
using System.Collections.Generic;
using Geev.Domain.Entities;
using Geev.Domain.Entities.Auditing;
using Geev.Timing;

namespace Geev.NHibernate.Tests.Entities
{
    public class Order : Entity, IHasCreationTime
    {
        public virtual decimal TotalPrice { get; set; }

        [DisableDateTimeNormalization]
        public virtual DateTime CreationTime { get; set; }

        public virtual ICollection<OrderDetail> Items { get; set; }
    }
}