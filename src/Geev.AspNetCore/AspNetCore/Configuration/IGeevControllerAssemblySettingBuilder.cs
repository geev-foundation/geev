using System;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Geev.AspNetCore.Configuration
{
    public interface IGeevControllerAssemblySettingBuilder
    {
        GeevControllerAssemblySettingBuilder Where(Func<Type, bool> predicate);

        GeevControllerAssemblySettingBuilder ConfigureControllerModel(Action<ControllerModel> configurer);
    }
}