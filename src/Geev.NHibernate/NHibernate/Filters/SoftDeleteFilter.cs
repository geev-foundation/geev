using Geev.Domain.Entities;
using Geev.Domain.Uow;
using FluentNHibernate.Mapping;
using NHibernate;

namespace Geev.NHibernate.Filters
{
    /// <summary>
    /// Add filter SoftDelete 
    /// </summary>
    public class SoftDeleteFilter : FilterDefinition
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public SoftDeleteFilter()
        {
            WithName(GeevDataFilters.SoftDelete)
                .AddParameter(GeevDataFilters.Parameters.IsDeleted, NHibernateUtil.Boolean)
                .WithCondition($"{nameof(ISoftDelete.IsDeleted)} = :{GeevDataFilters.Parameters.IsDeleted}");
        }
    }
}