using Geev.Dependency;
using Geev.PlugIns;

namespace Geev
{
    public class GeevBootstrapperOptions
    {
        /// <summary>
        /// Used to disable all interceptors added by ABP.
        /// </summary>
        public bool DisableAllInterceptors { get; set; }

        /// <summary>
        /// IIocManager that is used to bootstrap the ABP system. If set to null, uses global <see cref="Geev.Dependency.IocManager.Instance"/>
        /// </summary>
        public IIocManager IocManager { get; set; }

        /// <summary>
        /// List of plugin sources.
        /// </summary>
        public PlugInSourceList PlugInSources { get; }

        public GeevBootstrapperOptions()
        {
            IocManager = Geev.Dependency.IocManager.Instance;
            PlugInSources = new PlugInSourceList();
        }
    }
}
