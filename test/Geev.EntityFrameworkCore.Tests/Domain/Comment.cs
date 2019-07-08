using Geev.Domain.Entities;

namespace Geev.EntityFrameworkCore.Tests.Domain
{
    public class Comment : Entity
    {
        public Post Post { get; set; }

        public string Content { get; set; }
    }
}