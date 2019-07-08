using System;
using Geev.Configuration.Startup;
using Geev.Dependency;
using Geev.MultiTenancy;
using Geev.Runtime;
using Geev.Runtime.Session;

namespace Geev.TestBase.Runtime.Session
{
    public class TestGeevSession : IGeevSession, ISingletonDependency
    {
        public virtual long? UserId
        {
            get
            {
                if (_sessionOverrideScopeProvider.GetValue(GeevSessionBase.SessionOverrideContextKey) != null)
                {
                    return _sessionOverrideScopeProvider.GetValue(GeevSessionBase.SessionOverrideContextKey).UserId;
                }

                return _userId;
            }
            set { _userId = value; }
        }

        public virtual int? TenantId
        {
            get
            {
                if (!_multiTenancy.IsEnabled)
                {
                    return 1;
                }

                if (_sessionOverrideScopeProvider.GetValue(GeevSessionBase.SessionOverrideContextKey) != null)
                {
                    return _sessionOverrideScopeProvider.GetValue(GeevSessionBase.SessionOverrideContextKey).TenantId;
                }

                var resolvedValue = _tenantResolver.ResolveTenantId();
                if (resolvedValue != null)
                {
                    return resolvedValue;
                }

                return _tenantId;
            }
            set
            {
                if (!_multiTenancy.IsEnabled && value != 1 && value != null)
                {
                    throw new GeevException("Can not set TenantId since multi-tenancy is not enabled. Use IMultiTenancyConfig.IsEnabled to enable it.");
                }

                _tenantId = value;
            }
        }

        public virtual MultiTenancySides MultiTenancySide { get { return GetCurrentMultiTenancySide(); } }
        
        public virtual long? ImpersonatorUserId { get; set; }
        
        public virtual int? ImpersonatorTenantId { get; set; }

        private readonly IMultiTenancyConfig _multiTenancy;
        private readonly IAmbientScopeProvider<SessionOverride> _sessionOverrideScopeProvider;
        private readonly ITenantResolver _tenantResolver;
        private int? _tenantId;
        private long? _userId;

        public TestGeevSession(
            IMultiTenancyConfig multiTenancy, 
            IAmbientScopeProvider<SessionOverride> sessionOverrideScopeProvider,
            ITenantResolver tenantResolver)
        {
            _multiTenancy = multiTenancy;
            _sessionOverrideScopeProvider = sessionOverrideScopeProvider;
            _tenantResolver = tenantResolver;
        }

        protected virtual MultiTenancySides GetCurrentMultiTenancySide()
        {
            return _multiTenancy.IsEnabled && !TenantId.HasValue
                ? MultiTenancySides.Host
                : MultiTenancySides.Tenant;
        }

        public virtual IDisposable Use(int? tenantId, long? userId)
        {
            return _sessionOverrideScopeProvider.BeginScope(GeevSessionBase.SessionOverrideContextKey, new SessionOverride(tenantId, userId));
        }
    }
}