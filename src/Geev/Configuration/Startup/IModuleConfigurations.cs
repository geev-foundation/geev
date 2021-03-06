﻿namespace Geev.Configuration.Startup
{
    /// <summary>
    /// Used to provide a way to configure modules.
    /// Create entension methods to this class to be used over <see cref="IGeevStartupConfiguration.Modules"/> object.
    /// </summary>
    public interface IModuleConfigurations
    {
        /// <summary>
        /// Gets the ABP configuration object.
        /// </summary>
        IGeevStartupConfiguration GeevConfiguration { get; }
    }
}