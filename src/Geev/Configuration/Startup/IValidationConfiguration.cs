using System;
using System.Collections.Generic;
using Geev.Collections;
using Geev.Runtime.Validation.Interception;

namespace Geev.Configuration.Startup
{
    public interface IValidationConfiguration
    {
        List<Type> IgnoredTypes { get; }

        /// <summary>
        /// A list of method parameter validators.
        /// </summary>
        ITypeList<IMethodParameterValidator> Validators { get; }
    }
}