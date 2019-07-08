using Geev.MultiTenancy;

namespace Geev.Web.Models.GeevUserConfiguration
{
    public class GeevUserSessionConfigDto
    {
        public long? UserId { get; set; }

        public int? TenantId { get; set; }

        public long? ImpersonatorUserId { get; set; }

        public int? ImpersonatorTenantId { get; set; }

        public MultiTenancySides MultiTenancySide { get; set; }
    }
}