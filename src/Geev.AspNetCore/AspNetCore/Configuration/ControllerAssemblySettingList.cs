using System;
using System.Collections.Generic;
using System.Linq;
using Geev.Reflection.Extensions;
using JetBrains.Annotations;

namespace Geev.AspNetCore.Configuration
{
    public class ControllerAssemblySettingList : List<GeevControllerAssemblySetting>
    {
        public List<GeevControllerAssemblySetting> GetSettings(Type controllerType)
        {
            return this.Where(controllerSetting => controllerSetting.Assembly == controllerType.GetAssembly()).ToList();
        }
    }
}