using System;
using Geev.Domain.Entities;

namespace Geev.ZeroCore.SampleApp.Core.BookStore
{
    public class Book : Entity<Guid>
    {
        public string Name { get; set; }
    }
}
