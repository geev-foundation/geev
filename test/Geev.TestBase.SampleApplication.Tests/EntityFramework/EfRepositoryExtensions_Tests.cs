using Geev.Domain.Repositories;
using Geev.Domain.Uow;
using Geev.EntityFramework.Repositories;
using Geev.Extensions;
using Geev.TestBase.SampleApplication.Crm;
using Geev.TestBase.SampleApplication.EntityFramework;
using Shouldly;
using Xunit;

namespace Geev.TestBase.SampleApplication.Tests.EntityFramework
{
    public class EfRepositoryExtensions_Tests : SampleApplicationTestBase
    {
        private readonly IRepository<Company> _companyRepository;

        public EfRepositoryExtensions_Tests()
        {
            _companyRepository = Resolve<IRepository<Company>>();
        }

        [Fact]
        public void Should_Get_DbContext()
        {
            using (var uow = Resolve<IUnitOfWorkManager>().Begin())
            {
                _companyRepository.GetDbContext().ShouldBeOfType<SampleApplicationDbContext>();

                uow.Complete();
            }
        }
        
        [Fact]
        public void Should_Get_IocResolver()
        {
            _companyRepository.GetIocResolver().ShouldNotBeNull();
        }
    }
}