using Geev.Domain.Entities;

namespace Geev.EntityFrameworkCore.Dapper.Tests.Domain
{
    public class Comment : Entity<long>
    {
        protected Comment()
        {
        }

        public Comment(string text) : this()
        {
            Text = text;
        }

        public string Text { get; set; }
    }
}
