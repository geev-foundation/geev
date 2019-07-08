using System.Linq;
using System.Threading.Tasks;
using Geev.Domain.Repositories;
using Geev.Domain.Uow;
using Geev.ZeroCore.SampleApp.Core;
using Shouldly;
using Xunit;

namespace Geev.Zero.Repository
{
    public class Repository_Hard_Delete_Test : GeevZeroTestBase
    {
        private readonly IRepository<Role> _roleRepository;

        public Repository_Hard_Delete_Test()
        {
            _roleRepository = LocalIocManager.Resolve<IRepository<Role>>();
        }

        [Fact]
        public async Task Should_Permanently_Delete_SoftDelete_Entity_With_HarDelete_Method()
        {
            LoginAsDefaultTenantAdmin();

            var uowManager = Resolve<IUnitOfWorkManager>();

            // Soft-Delete admin
            using (var uow = uowManager.Begin())
            {
                var admin = await _roleRepository.FirstOrDefaultAsync(u => u.NormalizedName == "ADMIN");
                await _roleRepository.DeleteAsync(admin);

                uow.Complete();
            }

            using (var uow = uowManager.Begin())
            {
                var roles = _roleRepository.GetAllList();

                foreach (var role in roles)
                {
                    await _roleRepository.HardDeleteAsync(role);
                }

                uow.Complete();
            }

            using (var uow = uowManager.Begin())
            {
                using (uowManager.Current.DisableFilter(GeevDataFilters.SoftDelete))
                {
                    var roles = _roleRepository.GetAllList();
                    roles.Count.ShouldBe(1);
                    roles.First().NormalizedName.ShouldBe("ADMIN");
                }

                uow.Complete();
            }
        }

        [Fact]
        public async Task Should_Permanently_Delete_Multiple_SoftDelete_Entities_With_HarDelete_Method()
        {
            LoginAsDefaultTenantAdmin();

            var uowManager = Resolve<IUnitOfWorkManager>();

            // Soft-Delete admin
            using (var uow = uowManager.Begin())
            {
                var admin = await _roleRepository.FirstOrDefaultAsync(u => u.NormalizedName == "ADMIN");
                await _roleRepository.DeleteAsync(admin);

                uow.Complete();
            }

            using (var uow = uowManager.Begin())
            {
                await _roleRepository.HardDeleteAsync(r => r.Id > 0);

                uow.Complete();
            }

            using (var uow = uowManager.Begin())
            {
                using (uowManager.Current.DisableFilter(GeevDataFilters.SoftDelete))
                {
                    var roles = _roleRepository.GetAllList();
                    roles.Count.ShouldBe(1);
                    roles.First().NormalizedName.ShouldBe("ADMIN");
                }

                uow.Complete();
            }
        }
    }
}
