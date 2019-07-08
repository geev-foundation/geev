using System.Collections.Generic;
using Geev.EntityFramework.GraphDiff.Mapping;

namespace Geev.EntityFramework.GraphDiff.Configuration
{
    public class GeevEntityFrameworkGraphDiffModuleConfiguration : IGeevEntityFrameworkGraphDiffModuleConfiguration
    {
        public List<EntityMapping> EntityMappings { get; set; }
    }
}