﻿namespace Geev.WebApi.Controllers.Dynamic.Clients
{
    /// <summary>
    /// 
    /// </summary>
    public interface IApiClientBuilder<TService>
    {
        /// <summary>
        /// 
        /// </summary>
        void Build();
    }
}