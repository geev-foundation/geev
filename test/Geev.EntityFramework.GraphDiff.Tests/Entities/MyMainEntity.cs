using System.Collections.Generic;
using Geev.Domain.Entities;

namespace Geev.EntityFramework.GraphDIff.Tests.Entities
{
    public class MyMainEntity : Entity
    {
        public virtual ICollection<MyDependentEntity> MyDependentEntities { get; set; }
    }
}