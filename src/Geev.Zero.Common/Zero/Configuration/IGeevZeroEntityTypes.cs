using System;

namespace Geev.Zero.Configuration
{
    public interface IGeevZeroEntityTypes
    {
        /// <summary>
        /// User type of the application.
        /// </summary>
        Type User { get; set; }

        /// <summary>
        /// Role type of the application.
        /// </summary>
        Type Role { get; set; }

        /// <summary>
        /// Tenant type of the application.
        /// </summary>
        Type Tenant { get; set; }
    }
}