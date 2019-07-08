using System;
using System.Collections.Generic;
using Geev.Collections;
using Geev.Runtime.Validation.Interception;

namespace Geev.Configuration.Startup
{
    public class ValidationConfiguration : IValidationConfiguration
    {
        public List<Type> IgnoredTypes { get; }

        public ITypeList<IMethodParameterValidator> Validators { get; }

        public ValidationConfiguration()
        {
            IgnoredTypes = new List<Type>();
            Validators = new TypeList<IMethodParameterValidator>();
        }
    }
}