using System;
using System.Collections.Generic;
using Geev.Domain.Entities;

namespace Geev.EntityFramework
{
    public interface IDbContextEntityFinder
    {
        IEnumerable<EntityTypeInfo> GetEntityTypeInfos(Type dbContextType);
    }
}