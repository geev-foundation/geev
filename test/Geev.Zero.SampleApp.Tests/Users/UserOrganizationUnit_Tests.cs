﻿using System.Linq;
using System.Threading.Tasks;
using Geev.IdentityFramework;
using Geev.Organizations;
using Geev.Zero.SampleApp.MultiTenancy;
using Geev.Zero.SampleApp.Users;
using Microsoft.AspNet.Identity;
using Shouldly;
using Xunit;

namespace Geev.Zero.SampleApp.Tests.Users
{
    public class UserOrganizationUnit_Tests : SampleAppTestBase
    {
        private readonly UserManager _userManager;
        private readonly Tenant _defaultTenant;
        private readonly User _defaultTenantAdmin;

        public UserOrganizationUnit_Tests()
        {
            _defaultTenant = GetDefaultTenant();
            _defaultTenantAdmin = GetDefaultTenantAdmin();

            GeevSession.TenantId = _defaultTenant.Id;
            GeevSession.UserId = _defaultTenantAdmin.Id;

            _userManager = Resolve<UserManager>();
        }

        [Fact]
        public async Task Test_IsInOrganizationUnitAsync()
        {
            //Act & Assert
            (await _userManager.IsInOrganizationUnitAsync(_defaultTenantAdmin, GetOU("OU11"))).ShouldBe(true);
            (await _userManager.IsInOrganizationUnitAsync(_defaultTenantAdmin, GetOU("OU2"))).ShouldBe(false);
        }

        [Fact]
        public async Task Test_AddToOrganizationUnitAsync()
        {
            //Arrange
            var ou2 = GetOU("OU2");

            //Act
            await _userManager.AddToOrganizationUnitAsync(_defaultTenantAdmin, ou2);

            //Assert
            (await _userManager.IsInOrganizationUnitAsync(_defaultTenantAdmin, ou2)).ShouldBe(true);
            UsingDbContext(context => context.UserOrganizationUnits.FirstOrDefault(ou => ou.UserId == _defaultTenantAdmin.Id && ou.OrganizationUnitId == ou2.Id).ShouldNotBeNull());
        }

        [Fact]
        public async Task Test_RemoveFromOrganizationUnitAsync()
        {
            //Arrange
            var ou11 = GetOU("OU11");

            //Act
            await _userManager.RemoveFromOrganizationUnitAsync(_defaultTenantAdmin, ou11);

            //Assert
            (await _userManager.IsInOrganizationUnitAsync(_defaultTenantAdmin, ou11)).ShouldBe(false);
            UsingDbContext(context => context.UserOrganizationUnits.FirstOrDefault(ou => ou.UserId == _defaultTenantAdmin.Id && ou.OrganizationUnitId == ou11.Id).IsDeleted.ShouldBeTrue());
        }

        [Fact]
        public async Task Should_Remove_User_From_Organization_When_User_Is_Deleted()
        {
            //Arrange
            var user = CreateAndGetTestUser();
            var ou11 = GetOU("OU11");

            await _userManager.AddToOrganizationUnitAsync(user, ou11);
            (await _userManager.IsInOrganizationUnitAsync(user, ou11)).ShouldBe(true);

            //Act
            (await _userManager.DeleteAsync(user)).CheckErrors();

            //Assert
            (await _userManager.IsInOrganizationUnitAsync(user, ou11)).ShouldBe(false);
        }

        [Theory]
        [InlineData(new object[] { new string[0] })]
        [InlineData(new object[] { new[] { "OU12", "OU21" } })]
        [InlineData(new object[] { new[] { "OU11", "OU12", "OU2" } })]
        public async Task Test_SetOrganizationUnitsAsync(string[] organizationUnitNames)
        {
            //Arrange
            var organizationUnitIds = organizationUnitNames.Select(oun => GetOU(oun).Id).ToArray();

            //Act
            await _userManager.SetOrganizationUnitsAsync(_defaultTenantAdmin, organizationUnitIds);

            //Assert
            UsingDbContext(context =>
            {
                context.UserOrganizationUnits
                    .Count(uou => uou.UserId == _defaultTenantAdmin.Id && organizationUnitIds.Contains(uou.OrganizationUnitId))
                    .ShouldBe(organizationUnitIds.Length);
            });
        }

        [Fact]
        public async Task Test_GetUsersInOrganizationUnit()
        {
            //Act & Assert
            (await _userManager.GetUsersInOrganizationUnit(GetOU("OU11"))).Count.ShouldBe(1);
            (await _userManager.GetUsersInOrganizationUnit(GetOU("OU1"))).Count.ShouldBe(0);
            (await _userManager.GetUsersInOrganizationUnit(GetOU("OU1"), true)).Count.ShouldBe(1);
        }

        private OrganizationUnit GetOU(string diplayName)
        {
            var organizationUnit = UsingDbContext(context => context.OrganizationUnits.FirstOrDefault(ou => ou.DisplayName == diplayName));
            organizationUnit.ShouldNotBeNull();

            return organizationUnit;
        }

        private User CreateAndGetTestUser()
        {
            WithUnitOfWork(() => _userManager.Create(
                new User
                {
                    EmailAddress = "emre@aspnetboilerplate.com",
                    Name = "Yunus",
                    Surname = "Emre",
                    UserName = "yunus.emre",
                    IsEmailConfirmed = true,
                    Password = "AM4OLBpptxBYmM79lGOX9egzZk3vIQU3d/gFCJzaBjAPXzYIK3tQ2N7X4fcrHtElTw==" //123qwe
                }));

            return UsingDbContext(
                context =>
                {
                    return context.Users.Single(u => u.UserName == "yunus.emre");
                });
        }
    }
}
