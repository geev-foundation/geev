using Geev.Application.Services;
using Geev.Domain.Repositories;

namespace Geev.TestBase.SampleApplication.Crm
{
    public class CompanyAppService : CrudAppService<Company, CompanyDto, int>
    {
        public CompanyAppService(IRepository<Company, int> repository) : base(repository)
        {
            GetPermissionName = "GetCompanyPermission";
            GetAllPermissionName = "GetAllCompaniesPermission";
            CreatePermissionName = "CreateCompanyPermission";
            UpdatePermissionName = "UpdateCompanyPermission";
            DeletePermissionName = "DeleteCompanyPermission";
        }
    }
}
