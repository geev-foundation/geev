using System;
using System.ComponentModel.DataAnnotations.Schema;
using Geev.Domain.Entities;
using Geev.Domain.Entities.Auditing;
using Geev.Timing;

namespace Geev.TestBase.SampleApplication.Crm
{
    [Table("Rooms")]
    public class Room : Entity, IHasCreationTime
    {
        public int HotelId { get; set; }

        public string Name { get; set; }

        public int Capacity { get; set; }

        [DisableDateTimeNormalization]
        public DateTime CreationTime { get; set; }

        [ForeignKey("HotelId")]
        public virtual Hotel Hotel { get; set; }

        public Room()
        {
            
        }
    }
}