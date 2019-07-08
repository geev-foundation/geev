using System;
using System.ComponentModel.DataAnnotations.Schema;
using Geev.Domain.Entities;

namespace Geev.Zero.SampleApp.BookStore
{
    public class Author : Entity<Guid>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override Guid Id { get; set; }

        public string Name { get; set; }
    }
}