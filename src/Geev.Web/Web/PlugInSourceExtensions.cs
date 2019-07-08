using System;
using System.Web.Compilation;
using Geev.Logging;
using Geev.PlugIns;

namespace Geev.Web
{
    public static class PlugInSourceListExtensions
    {
        public static void AddToBuildManager(this PlugInSourceList plugInSourceList)
        {
            foreach (var plugInAssembly in plugInSourceList.GetAllAssemblies())
            {
                try
                {
                    LogHelper.Logger.Debug($"Adding {plugInAssembly.FullName} to BuildManager");
                    BuildManager.AddReferencedAssembly(plugInAssembly);
                }
                catch (Exception ex)
                {
                    LogHelper.Logger.Warn(ex.ToString(), ex);
                }
            }
        }
    }
}
