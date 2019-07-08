using System;
using System.Collections.Generic;
using Geev.Domain.Entities;
using Geev.Domain.Entities.Auditing;
using Geev.Timing;

namespace Geev.EntityFrameworkCore.Tests.Domain
{
    public class Blog : AggregateRoot, IHasCreationTime
    {
        public string Name { get; set; }

        public string Url { get; protected set; }

        public DateTime CreationTime { get; set; }

        public ICollection<Post> Posts { get; set; }

        public Blog()
        {
            
        }

        public Blog(string name, string url)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentNullException(nameof(url));
            }

            Name = name;
            Url = url;
        }

        public void ChangeUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentNullException(nameof(url));
            }

            var oldUrl = Url;
            Url = url;

            DomainEvents.Add(new BlogUrlChangedEventData(this, oldUrl));
        }
    }

    public class BlogCategory: AggregateRoot, IHasCreationTime
    {
        public string Name { get; set; }

        [DisableDateTimeNormalization]
        public DateTime CreationTime { get; set; }

        public List<SubBlogCategory> SubCategories { get; set; }
    }

    [DisableDateTimeNormalization]
    public class SubBlogCategory : Entity, IHasCreationTime
    {
        public string Name { get; set; }

        public DateTime CreationTime { get; set; }
    }
}