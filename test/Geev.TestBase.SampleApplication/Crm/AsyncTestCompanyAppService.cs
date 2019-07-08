using Geev.Application.Services;
using Geev.Authorization;
using Geev.Domain.Repositories;

namespace Geev.TestBase.SampleApplication.Crm
{
    [GeevAuthorize("GetCompanyPermission")]
    public class AsyncTestCompanyAppService : AsyncCrudAppService<Company, CompanyDto>
    {
        public AsyncTestCompanyAppService(IRepository<Company> repository)
            : base(repository)
        {
            CreatePermissionName = "CreateCompanyPermission";
        }
    }
}