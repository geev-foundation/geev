using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using Geev.Authorization;
using Geev.Domain.Uow;
using Microsoft.AspNet.OData;

namespace Geev.WebApi.OData.Controllers
{
    public abstract class GeevODataController : ODataController
    {
        public IUnitOfWorkManager UnitOfWorkManager { get; set; }

        protected IUnitOfWorkCompleteHandle UnitOfWorkCompleteHandler { get; private set; }

        protected bool IsDisposed { get; set; }

        public IPermissionChecker PermissionChecker { protected get; set; }

        protected GeevODataController()
        {
            PermissionChecker = NullPermissionChecker.Instance;
        }

        public override Task<HttpResponseMessage> ExecuteAsync(HttpControllerContext controllerContext, CancellationToken cancellationToken)
        {
            UnitOfWorkCompleteHandler = UnitOfWorkManager.Begin();
            return base.ExecuteAsync(controllerContext, cancellationToken);
        }

        protected override void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                UnitOfWorkCompleteHandler.Complete();
                UnitOfWorkCompleteHandler.Dispose();
            }

            IsDisposed = true;

            base.Dispose(disposing);
        }
    }
}