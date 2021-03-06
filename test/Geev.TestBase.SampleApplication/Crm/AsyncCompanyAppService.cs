using Geev.Application.Services;
using Geev.Domain.Repositories;

namespace Geev.TestBase.SampleApplication.Crm
{
    public class AsyncCompanyAppService : AsyncCrudAppService<Company, CompanyDto>
    {
        public AsyncCompanyAppService(IRepository<Company> repository)
            : base(repository)
        {
            GetPermissionName = "GetCompanyPermission";
            GetAllPermissionName = "GetAllCompaniesPermission";
            CreatePermissionName = "CreateCompanyPermission";
            UpdatePermissionName = "UpdateCompanyPermission";
            DeletePermissionName = "DeleteCompanyPermission";
        }
    }
}