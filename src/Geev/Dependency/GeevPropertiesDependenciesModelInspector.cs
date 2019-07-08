using Castle.Core;
using Castle.MicroKernel.ModelBuilder.Inspectors;
using Castle.MicroKernel.SubSystems.Conversion;

namespace Geev.Dependency
{
    public class GeevPropertiesDependenciesModelInspector : PropertiesDependenciesModelInspector
    {
        public GeevPropertiesDependenciesModelInspector(IConversionManager converter) 
            : base(converter)
        {
        }

        protected override void InspectProperties(ComponentModel model)
        {
            if (model.Implementation.FullName != null && 
                model.Implementation.FullName.StartsWith("Microsoft"))
            {
                return;
            }

            base.InspectProperties(model);
        }
    }
}
