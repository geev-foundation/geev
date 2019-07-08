using Geev.Domain.Repositories;
using Geev.Reflection;
using Geev.TestBase.SampleApplication.ContacLists;
using Geev.TestBase.SampleApplication.EntityFramework.Repositories;
using Shouldly;
using Xunit;

namespace Geev.TestBase.SampleApplication.Tests.Reflection
{
    public class ProxyHelper_Tests: SampleApplicationTestBase
    {
        private readonly IRepository<ContactList> _contactListRepository;

        public ProxyHelper_Tests()
        {
            _contactListRepository = Resolve<IRepository<ContactList>>();
        }

        [Fact]
        public void ProxyHelper_Should_Return_Original_Object_For_Not_Proxied_Object()
        {
            (ProxyHelper.UnProxy(new MyTestClass()) is MyTestClass).ShouldBeTrue();
        }

        [Fact]
        public void ProxyHelper_Should_Return_Unproxied_Object_For_Proxied_Object()
        {
            (ProxyHelper.UnProxy(_contactListRepository) is SampleApplicationEfRepositoryBase<ContactList>).ShouldBeTrue();
        }

        class MyTestClass
        {

        }
    }
}
