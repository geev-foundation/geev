using System;
using System.Collections.Generic;
using System.Reflection;
using JetBrains.Annotations;

namespace Geev.Modules
{
    /// <summary>
    /// Used to store all needed information for a module.
    /// </summary>
    public class GeevModuleInfo
    {
        /// <summary>
        /// The assembly which contains the module definition.
        /// </summary>
        public Assembly Assembly { get; }

        /// <summary>
        /// Type of the module.
        /// </summary>
        public Type Type { get; }

        /// <summary>
        /// Instance of the module.
        /// </summary>
        public GeevModule Instance { get; }

        /// <summary>
        /// Is this module loaded as a plugin.
        /// </summary>
        public bool IsLoadedAsPlugIn { get; }

        /// <summary>
        /// All dependent modules of this module.
        /// </summary>
        public List<GeevModuleInfo> Dependencies { get; }

        /// <summary>
        /// Creates a new GeevModuleInfo object.
        /// </summary>
        public GeevModuleInfo([NotNull] Type type, [NotNull] GeevModule instance, bool isLoadedAsPlugIn)
        {
            Check.NotNull(type, nameof(type));
            Check.NotNull(instance, nameof(instance));

            Type = type;
            Instance = instance;
            IsLoadedAsPlugIn = isLoadedAsPlugIn;
            Assembly = Type.GetTypeInfo().Assembly;

            Dependencies = new List<GeevModuleInfo>();
        }

        public override string ToString()
        {
            return Type.AssemblyQualifiedName ??
                   Type.FullName;
        }
    }
}