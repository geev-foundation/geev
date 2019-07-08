using System.Collections.Generic;
using Geev.EntityFramework.GraphDiff.Mapping;

namespace Geev.EntityFramework.GraphDiff.Configuration
{
    public interface IGeevEntityFrameworkGraphDiffModuleConfiguration
    {
        List<EntityMapping> EntityMappings { get; set; }
    }
}