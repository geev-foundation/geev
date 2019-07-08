using System;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Geev.AspNetCore.Configuration
{
    public class GeevControllerAssemblySettingBuilder : IGeevControllerAssemblySettingBuilder
    {
        private readonly GeevControllerAssemblySetting _setting;

        public GeevControllerAssemblySettingBuilder(GeevControllerAssemblySetting setting)
        {
            _setting = setting;
        }

        public GeevControllerAssemblySettingBuilder Where(Func<Type, bool> predicate)
        {
            _setting.TypePredicate = predicate;
            return this;
        }

        public GeevControllerAssemblySettingBuilder ConfigureControllerModel(Action<ControllerModel> configurer)
        {
            _setting.ControllerModelConfigurer = configurer;
            return this;
        }
    }
}