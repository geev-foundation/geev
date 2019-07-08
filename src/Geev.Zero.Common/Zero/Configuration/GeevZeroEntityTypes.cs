using System;
using System.Reflection;
using Geev.Authorization.Roles;
using Geev.Authorization.Users;
using Geev.MultiTenancy;

namespace Geev.Zero.Configuration
{
    public class GeevZeroEntityTypes : IGeevZeroEntityTypes
    {
        public Type User
        {
            get { return _user; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                if (!typeof (GeevUserBase).IsAssignableFrom(value))
                {
                    throw new GeevException(value.AssemblyQualifiedName + " should be derived from " + typeof(GeevUserBase).AssemblyQualifiedName);
                }

                _user = value;
            }
        }
        private Type _user;

        public Type Role
        {
            get { return _role; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                if (!typeof(GeevRoleBase).IsAssignableFrom(value))
                {
                    throw new GeevException(value.AssemblyQualifiedName + " should be derived from " + typeof(GeevRoleBase).AssemblyQualifiedName);
                }

                _role = value;
            }
        }
        private Type _role;

        public Type Tenant
        {
            get { return _tenant; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                if (!typeof(GeevTenantBase).IsAssignableFrom(value))
                {
                    throw new GeevException(value.AssemblyQualifiedName + " should be derived from " + typeof(GeevTenantBase).AssemblyQualifiedName);
                }

                _tenant = value;
            }
        }
        private Type _tenant;
    }
}