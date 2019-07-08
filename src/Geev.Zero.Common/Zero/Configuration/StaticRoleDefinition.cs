﻿using System.Collections.Generic;
using Geev.Authorization;
using Geev.MultiTenancy;

namespace Geev.Zero.Configuration
{
    public class StaticRoleDefinition
    {
        public string RoleName { get; }

        public bool GrantAllPermissionsByDefault { get; set; }
        
        public List<string> GrantedPermissions { get; }

        public MultiTenancySides Side { get; }

        public StaticRoleDefinition(string roleName, MultiTenancySides side, bool grantAllPermissionsByDefault = false)
        {
            RoleName = roleName;
            Side = side;
            GrantAllPermissionsByDefault = grantAllPermissionsByDefault;
            GrantedPermissions = new List<string>();
        }

        public virtual bool IsGrantedByDefault(Permission permission)
        {
            return GrantAllPermissionsByDefault || GrantedPermissions.Contains(permission.Name);
        }
    }
}