using Geev.Domain.Entities;
using Geev.Domain.Uow;
using FluentNHibernate.Mapping;
using NHibernate;

namespace Geev.NHibernate.Filters
{
    /// <summary>
    /// Add filter MustHaveTenant 
    /// </summary>
    public class MustHaveTenantFilter : FilterDefinition
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public MustHaveTenantFilter()
        {
            WithName(GeevDataFilters.MustHaveTenant)
                .AddParameter(GeevDataFilters.Parameters.TenantId, NHibernateUtil.Int32)
                .WithCondition($"{nameof(IMustHaveTenant.TenantId)} = :{GeevDataFilters.Parameters.TenantId}");
        }
    }
}