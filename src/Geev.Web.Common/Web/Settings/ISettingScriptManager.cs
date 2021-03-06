﻿using System.Threading.Tasks;

namespace Geev.Web.Settings
{
    /// <summary>
    /// Define interface to get setting scripts
    /// </summary>
    public interface ISettingScriptManager
    {
        /// <summary>
        /// Gets JavaScript that contains setting values.
        /// </summary>
        Task<string> GetScriptAsync();
    }
}