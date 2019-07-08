using Geev.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Geev.AspNetCore.App.AppServices
{
    public class NameConflictAppService : ApplicationService
    {
        [HttpGet]
        public string GetConstantString()
        {
            return "return-value-from-app-service";
        }
    }
}