using System.Threading.Tasks;
using Geev.Web.Configuration;
using Microsoft.AspNetCore.Mvc;

namespace Geev.AspNetCore.Mvc.Controllers
{
    public class GeevUserConfigurationController: GeevController
    {
        private readonly GeevUserConfigurationBuilder _geevUserConfigurationBuilder;

        public GeevUserConfigurationController(GeevUserConfigurationBuilder geevUserConfigurationBuilder)
        {
            _geevUserConfigurationBuilder = geevUserConfigurationBuilder;
        }

        public async Task<JsonResult> GetAll()
        {
            var userConfig = await _geevUserConfigurationBuilder.GetAll();
            return Json(userConfig);
        }
    }
}