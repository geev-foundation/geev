﻿using System.Collections.Generic;
using System.Security.Claims;
using Geev.Authorization.Roles;
using Geev.ZeroCore.SampleApp.Core;
using Geev.ZeroCore.SampleApp.EntityFramework;

namespace Geev.Zero.TestData
{
    public class TestRolesBuilder
    {
        private readonly SampleAppDbContext _context;
        private readonly int _tenantId;

        public TestRolesBuilder(SampleAppDbContext context, int tenantId)
        {
            _context = context;
            _tenantId = tenantId;
        }

        public void Create()
        {
            var role = new Role(_tenantId, "ADMIN", "ADMIN")
            {
                Claims = new List<RoleClaim>()
            };

            role.Claims.Add(new RoleClaim(role, new Claim("MyClaim1", "MyClaim1Value")));
            role.Claims.Add(new RoleClaim(role, new Claim("MyClaim2", "MyClaim2Value")));

            _context.Roles.Add(role);

            _context.Roles.Add(new Role(_tenantId, "MANAGER", "MANAGER"));

            _context.SaveChanges();
        }
    }
}