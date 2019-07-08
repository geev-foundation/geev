namespace Geev.Web.Models.GeevUserConfiguration
{
    public class GeevMultiTenancyConfigDto
    {
        public bool IsEnabled { get; set; }

        public bool IgnoreFeatureCheckForHostUsers { get; set; }

        public GeevMultiTenancySidesConfigDto Sides { get; private set; }

        public GeevMultiTenancyConfigDto()
        {
            Sides = new GeevMultiTenancySidesConfigDto();
        }
    }
}