using System;
using Geev.Domain.Entities;

namespace Geev.NHibernate.Tests.Entities
{
    public class Hotel : Entity
    {
        public virtual string Name { get; set; }

        public virtual DateTime CreationDate { get; set; }

        public virtual DateTime? ModificationDate { get; set; }

        public virtual Location Headquarter { get; set; }
    }
}