using Geev.Domain.Entities;

namespace Geev.EntityFramework.GraphDIff.Tests.Entities
{
    public class MyDependentEntity : Entity
    {
        public virtual MyMainEntity MyMainEntity { get; set; }
    }
}
