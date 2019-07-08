using System;
using Geev.Dependency;
using FluentValidation;

namespace Geev.FluentValidation
{
    public class GeevFluentValidationValidatorFactory : ValidatorFactoryBase
    {
        private readonly IIocResolver _iocResolver;

        public GeevFluentValidationValidatorFactory(IIocResolver iocResolver)
        {
            _iocResolver = iocResolver;
        }

        public override IValidator CreateInstance(Type validatorType)
        {
            if (_iocResolver.IsRegistered(validatorType))
            {
                return _iocResolver.Resolve(validatorType) as IValidator;
            }

            return null;
        }
    }
}
