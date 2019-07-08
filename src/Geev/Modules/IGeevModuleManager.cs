using System;
using System.Collections.Generic;

namespace Geev.Modules
{
    public interface IGeevModuleManager
    {
        GeevModuleInfo StartupModule { get; }

        IReadOnlyList<GeevModuleInfo> Modules { get; }

        void Initialize(Type startupModule);

        void StartModules();

        void ShutdownModules();
    }
}