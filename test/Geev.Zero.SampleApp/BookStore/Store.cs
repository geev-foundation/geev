using System;
using Geev.Domain.Entities;

namespace Geev.Zero.SampleApp.BookStore
{
    public class Store : Entity<Guid>
    {
        public string Name { get; set; }
    }
}
