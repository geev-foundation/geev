using System.Threading.Tasks;
using System.Web.Mvc;
using Geev.Web.Configuration;

namespace Geev.Web.Mvc.Controllers
{
    public class GeevUserConfigurationController : GeevController
    {
        private readonly GeevUserConfigurationBuilder _geevUserConfigurationBuilder;

        public GeevUserConfigurationController(GeevUserConfigurationBuilder geevUserConfigurationBuilder)
        {
            _geevUserConfigurationBuilder = geevUserConfigurationBuilder;
        }

        public async Task<JsonResult> GetAll()
        {
            var userConfig = await _geevUserConfigurationBuilder.GetAll();
            return Json(userConfig, JsonRequestBehavior.AllowGet);
        }
    }
}