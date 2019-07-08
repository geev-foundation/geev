﻿using System.Linq;
using System.Threading.Tasks;
using Geev.Configuration.Startup;
using Geev.Domain.Repositories;
using Geev.EntityFramework.Repositories;
using Geev.TestBase.SampleApplication.ContacLists;
using Shouldly;
using Xunit;

namespace Geev.TestBase.SampleApplication.Tests.ContactLists
{
    public class ContactList_Ef_Explicit_Loading : SampleApplicationTestBase
    {
        private readonly IRepository<ContactList> _contactListRepository;

        public ContactList_Ef_Explicit_Loading()
        {
            _contactListRepository = Resolve<IRepository<ContactList>>();
        }

        protected override void CreateInitialData()
        {
            Resolve<IMultiTenancyConfig>().IsEnabled = true;
            base.CreateInitialData();
        }

        [Fact]
        public async Task Ef_Explicit_Load_Should_Work_If_Lazy_Load_Is_Disabled()
        {
            GeevSession.TenantId = 1;

            await WithUnitOfWorkAsync(async () =>
            {
                _contactListRepository.GetDbContext().Configuration.LazyLoadingEnabled = false;
                _contactListRepository.GetDbContext().Configuration.ProxyCreationEnabled = false;

                var contactList = await _contactListRepository.FirstOrDefaultAsync(cl => cl.Name == "List of Tenant-1");

                await _contactListRepository.EnsureCollectionLoadedAsync(contactList, cl => cl.People);

                contactList.People.ShouldNotBeNull();
                contactList.People.Count.ShouldBe(1);
                contactList.People.First().Name.ShouldBe("halil");
            });
        }

        [Fact]
        public async Task Ef_Explicit_Load_Should_Work_With_No_Effect_If_Lazy_Load_Is_Enabled()
        {
            GeevSession.TenantId = 1;

            await WithUnitOfWorkAsync(async () =>
            {
                _contactListRepository.GetDbContext().Configuration.LazyLoadingEnabled = true;
                _contactListRepository.GetDbContext().Configuration.ProxyCreationEnabled = true;

                var contactList = await _contactListRepository.FirstOrDefaultAsync(cl => cl.Name == "List of Tenant-1");

                await _contactListRepository.EnsureCollectionLoadedAsync(contactList, cl => cl.People);

                contactList.People.ShouldNotBeNull();
                contactList.People.Count.ShouldBe(1);
                contactList.People.First().Name.ShouldBe("halil");
            });
        }
    }
}
