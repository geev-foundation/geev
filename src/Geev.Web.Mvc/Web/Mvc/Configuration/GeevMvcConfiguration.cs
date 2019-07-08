using Geev.Domain.Uow;
using Geev.Web.Models;

namespace Geev.Web.Mvc.Configuration
{
    public class GeevMvcConfiguration : IGeevMvcConfiguration
    {
        public UnitOfWorkAttribute DefaultUnitOfWorkAttribute { get; }

        public WrapResultAttribute DefaultWrapResultAttribute { get; }

        public bool IsValidationEnabledForControllers { get; set; }

        public bool IsAutomaticAntiForgeryValidationEnabled { get; set; }

        public bool IsAuditingEnabled { get; set; }

        public bool IsAuditingEnabledForChildActions { get; set; }

        public GeevMvcConfiguration()
        {
            DefaultUnitOfWorkAttribute = new UnitOfWorkAttribute();
            DefaultWrapResultAttribute = new WrapResultAttribute();
            IsValidationEnabledForControllers = true;
            IsAutomaticAntiForgeryValidationEnabled = true;
            IsAuditingEnabled = true;
        }
    }
}