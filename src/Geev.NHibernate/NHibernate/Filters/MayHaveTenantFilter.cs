using Geev.Domain.Entities;
using Geev.Domain.Uow;
using FluentNHibernate.Mapping;
using NHibernate;

namespace Geev.NHibernate.Filters
{
    /// <summary>
    /// Add filter MayHaveTenant 
    /// </summary>
    public class MayHaveTenantFilter : FilterDefinition
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public MayHaveTenantFilter()
        {
            WithName(GeevDataFilters.MayHaveTenant)
                .AddParameter(GeevDataFilters.Parameters.TenantId, NHibernateUtil.Int32)
                .WithCondition($"{nameof(IMayHaveTenant.TenantId)} = :{GeevDataFilters.Parameters.TenantId}");
        }
    }
}