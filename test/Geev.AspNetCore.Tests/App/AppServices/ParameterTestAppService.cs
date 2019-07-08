using Geev.Application.Services;
using Geev.AspNetCore.App.Models;

namespace Geev.AspNetCore.App.AppServices
{
    public class ParameterTestAppService : ApplicationService
    {
        public string GetComplexInput(SimpleViewModel model, bool testBool)
        {
            return "42";
        }
    }
}