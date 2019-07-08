using Geev.Authorization;
using Geev.Domain.Uow;
using Microsoft.AspNet.OData;

namespace Geev.AspNetCore.OData.Controllers
{
    public abstract class GeevODataController : ODataController
    {
        public IUnitOfWorkManager UnitOfWorkManager { get; set; }

        public IPermissionChecker PermissionChecker { protected get; set; }

        protected GeevODataController()
        {
            PermissionChecker = NullPermissionChecker.Instance;
        }
    }
}
