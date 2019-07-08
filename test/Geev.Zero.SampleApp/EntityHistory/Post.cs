﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Geev.Auditing;
using Geev.Domain.Entities.Auditing;
using Geev.Domain.Entities;

namespace Geev.Zero.SampleApp.EntityHistory
{
    public class Post : AuditedEntity<Guid>, ISoftDelete, IMayHaveTenant
    {
        [Required]
        public virtual Blog Blog { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        [Audited]
        public int BlogId { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }

        public bool IsDeleted { get; set; }

        public int? TenantId { get; set; }

        public Post()
        {
            Id = Guid.NewGuid();
        }

        public Post(Blog blog, string title, string body)
            : this()
        {
            Blog = blog;
            Title = title;
            Body = body;
        }
    }
}