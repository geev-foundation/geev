using System;
using System.ComponentModel.DataAnnotations.Schema;
using Geev.Domain.Entities;

namespace Geev.TestBase.SampleApplication.GuidEntities
{
    public class TestEntityWithGuidPkAndDbGeneratedValue : Entity<Guid>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override Guid Id { get; set; }
    }
}