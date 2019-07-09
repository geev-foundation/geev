using System;
using System.ComponentModel.DataAnnotations.Schema;
using Geev.Domain.Entities;

namespace Geev.IdentityServer4
{
    [Table("PersistedGrants")]
    public class PersistedGrantEntity : Entity<string>
    {
        public virtual string Type { get; set; }

        public virtual string SubjectId { get; set; }

        public virtual string ClientId { get; set; }

        public virtual DateTime CreationTime { get; set; }

        public virtual DateTime? Expiration { get; set; }

        public virtual string Data { get; set; }
    }
}