using System;
using System.ComponentModel.DataAnnotations.Schema;
using Geev.Domain.Entities;

namespace Geev.ZeroCore.SampleApp.Core.BookStore
{
    public class Store : Entity<Guid>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        public string Name { get; set; }
    }
}
