using System;
using Microsoft.AspNetCore.Identity;

namespace Microsoft.Extensions.DependencyInjection
{
    public class GeevIdentityBuilder : IdentityBuilder
    {
        public Type TenantType { get; }

        public GeevIdentityBuilder(IdentityBuilder identityBuilder, Type tenantType)
            : base(identityBuilder.UserType, identityBuilder.RoleType, identityBuilder.Services)
        {
            TenantType = tenantType;
        }
    }
}