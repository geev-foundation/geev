using Geev.Dependency;
using Geev.FluentValidation.Configuration;
using Geev.Modules;
using Geev.Reflection.Extensions;
using FluentValidation;

namespace Geev.FluentValidation
{
    [DependsOn(typeof(GeevKernelModule))]
    public class GeevFluentValidationModule : GeevModule
    {
        public override void PreInitialize()
        {
            IocManager.Register<IGeevFluentValidationConfiguration, GeevFluentValidationConfiguration>();
            IocManager.Register<GeevFluentValidationLanguageManager, GeevFluentValidationLanguageManager>();
            IocManager.Register<IValidatorFactory, GeevFluentValidationValidatorFactory>(DependencyLifeStyle.Transient);

            IocManager.AddConventionalRegistrar(new FluentValidationValidatorRegistrar());

            Configuration.Validation.Validators.Add<FluentValidationMethodParameterValidator>();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(GeevFluentValidationModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            ValidatorOptions.LanguageManager = IocManager.Resolve<GeevFluentValidationLanguageManager>();
        }
    }
}
