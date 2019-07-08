﻿using System.Data.Common;
using System.Data.Entity;
using Geev.Domain.Entities;
using Geev.EntityFramework;

namespace Geev.TestBase.SampleApplication.EntityFramework
{
    /* This dummy dbcontext is just to demonstrate usage of 2 dbcontextes in same UOW.
     */

    public class SecondDbContext : GeevDbContext
    {
        public virtual IDbSet<SecondDbContextEntity> SecondDbContextEntities { get; set; }
    }

    public class SecondDbContextEntity : Entity
    {
        public string Name { get; set; }
    }
}