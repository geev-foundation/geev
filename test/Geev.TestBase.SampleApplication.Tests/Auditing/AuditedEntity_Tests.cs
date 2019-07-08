﻿using System;
using System.Linq;
using Geev.Configuration.Startup;
using Geev.Domain.Repositories;
using Geev.TestBase.SampleApplication.Crm;
using Geev.TestBase.SampleApplication.Messages;
using Geev.Timing;
using Shouldly;
using Xunit;

namespace Geev.TestBase.SampleApplication.Tests.Auditing
{
    public class AuditedEntity_Tests: SampleApplicationTestBase
    {
        private readonly IRepository<Message> _messageRepository;
        private readonly IRepository<Company> _companyRepository;

        public AuditedEntity_Tests()
        {
            _messageRepository = Resolve<IRepository<Message>>();
            _companyRepository = Resolve<IRepository<Company>>();
        }

        [Fact]
        public void Should_Write_Audit_Properties()
        {
            //Arrange
            GeevSession.TenantId = 1;
            GeevSession.UserId = 2;

            //Act: Create a new entity
            var createdMessage = _messageRepository.Insert(new Message(GeevSession.TenantId, "test message 1"));

            //Assert: Check creation properties
            createdMessage.CreatorUserId.ShouldBe(GeevSession.UserId);
            createdMessage.CreationTime.ShouldBeGreaterThan(Clock.Now.Subtract(TimeSpan.FromSeconds(10)));
            createdMessage.CreationTime.ShouldBeLessThan(Clock.Now.Add(TimeSpan.FromSeconds(10)));

            //Act: Select the same entity
            var selectedMessage = _messageRepository.Get(createdMessage.Id);

            //Assert: Select should not change audit properties
            selectedMessage.ShouldBe(createdMessage); //They should be same since Entity class overrides == operator.

            selectedMessage.CreationTime.ShouldBe(createdMessage.CreationTime);
            selectedMessage.CreatorUserId.ShouldBe(createdMessage.CreatorUserId);

            selectedMessage.LastModifierUserId.ShouldBeNull();
            selectedMessage.LastModificationTime.ShouldBeNull();

            selectedMessage.IsDeleted.ShouldBeFalse();
            selectedMessage.DeleterUserId.ShouldBeNull();
            selectedMessage.DeletionTime.ShouldBeNull();

            //Act: Update the entity
            selectedMessage.Text = "test message 1 - updated";
            _messageRepository.Update(selectedMessage);

            //Assert: Modification properties should be changed
            selectedMessage.LastModifierUserId.ShouldBe(GeevSession.UserId);
            selectedMessage.LastModificationTime.ShouldNotBeNull();
            selectedMessage.LastModificationTime.Value.ShouldBeGreaterThan(Clock.Now.Subtract(TimeSpan.FromSeconds(10)));
            selectedMessage.LastModificationTime.Value.ShouldBeLessThan(Clock.Now.Add(TimeSpan.FromSeconds(10)));

            //Act: Delete the entity
            _messageRepository.Delete(selectedMessage);

            //Assert: Deletion audit properties should be set
            selectedMessage.IsDeleted.ShouldBe(true);
            selectedMessage.DeleterUserId.ShouldBe(GeevSession.UserId);
            selectedMessage.DeletionTime.ShouldNotBeNull();
            selectedMessage.DeletionTime.Value.ShouldBeGreaterThan(Clock.Now.Subtract(TimeSpan.FromSeconds(10)));
            selectedMessage.DeletionTime.Value.ShouldBeLessThan(Clock.Now.Add(TimeSpan.FromSeconds(10)));
        }

        [Fact]
        public void Should_Not_Set_Audit_User_Properties_Of_Host_Entities_By_Tenant_User()
        {
            Resolve<IMultiTenancyConfig>().IsEnabled = true;

            //Login as host
            GeevSession.TenantId = null;
            GeevSession.UserId = 42;

            //Get a company to modify
            var company = _companyRepository.GetAllList().First();
            company.LastModifierUserId.ShouldBeNull(); //initial value

            //Modify the company
            company.Name = company.Name + "1";
            _companyRepository.Update(company);

            //LastModifierUserId should be set
            company.LastModifierUserId.ShouldBe(42);

            //Login as a tenant
            GeevSession.TenantId = 1;
            GeevSession.UserId = 43;

            //Get the same company to modify
            company = _companyRepository.FirstOrDefault(company.Id);
            company.ShouldNotBeNull();
            company.LastModifierUserId.ShouldBe(42); //Previous user's id

            //Modify the company
            company.Name = company.Name + "1";
            _companyRepository.Update(company);

            //LastModifierUserId should set to null since a tenant changing a host entity
            company.LastModifierUserId.ShouldBe(null);
        }
    }
}
