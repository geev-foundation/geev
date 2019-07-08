using System;
using Geev.Domain.Entities;

namespace Geev.Zero.SampleApp.BookStore
{
    public class Book : Entity<Guid>
    {
        public string Name { get; set; }
    }
}
