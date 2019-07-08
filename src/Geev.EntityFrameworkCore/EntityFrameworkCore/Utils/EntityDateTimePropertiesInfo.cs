using System.Collections.Generic;
using System.Reflection;

namespace Geev.EntityFrameworkCore.Utils
{
    internal class EntityDateTimePropertiesInfo
    {
        public List<PropertyInfo> DateTimePropertyInfos { get; set; }

        public List<string> ComplexTypePropertyPaths { get; set; }

        public EntityDateTimePropertiesInfo()
        {
            DateTimePropertyInfos = new List<PropertyInfo>();
            ComplexTypePropertyPaths = new List<string>();
        }
    }
}