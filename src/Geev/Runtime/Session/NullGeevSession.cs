using Geev.Configuration.Startup;
using Geev.MultiTenancy;
using Geev.Runtime.Remoting;

namespace Geev.Runtime.Session
{
    /// <summary>
    /// Implements null object pattern for <see cref="IGeevSession"/>.
    /// </summary>
    public class NullGeevSession : GeevSessionBase
    {
        /// <summary>
        /// Singleton instance.
        /// </summary>
        public static NullGeevSession Instance { get; } = new NullGeevSession();

        /// <inheritdoc/>
        public override long? UserId => null;

        /// <inheritdoc/>
        public override int? TenantId => null;

        public override MultiTenancySides MultiTenancySide => MultiTenancySides.Tenant;

        public override long? ImpersonatorUserId => null;

        public override int? ImpersonatorTenantId => null;

        private NullGeevSession() 
            : base(
                  new MultiTenancyConfig(), 
                  new DataContextAmbientScopeProvider<SessionOverride>(new AsyncLocalAmbientDataContext())
            )
        {

        }
    }
}