using Geev.AspNetCore.Mvc.Controllers;
using Geev.Runtime.Session;

namespace Geev.AspNetCore.App.Controllers
{
    public class MultiTenancyTestController : GeevController
    {
        private readonly IGeevSession _geevSession;

        public MultiTenancyTestController(IGeevSession geevSession)
        {
            _geevSession = geevSession;
        }

        public int? GetTenantId()
        {
            return _geevSession.TenantId;
        }
    }
}
