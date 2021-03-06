﻿using System.Reflection;
using Geev.Localization.Dictionaries.Xml;
using Geev.Localization.Sources;
using Geev.Modules;

namespace Geev.Zero
{
    [DependsOn(typeof(GeevZeroCommonModule))]
    public class GeevZeroCoreModule : GeevModule
    {
        public override void PreInitialize()
        {
            Configuration.Localization.Sources.Extensions.Add(
                new LocalizationSourceExtensionInfo(
                    GeevZeroConsts.LocalizationSourceName,
                    new XmlEmbeddedFileLocalizationDictionaryProvider(
                        Assembly.GetExecutingAssembly(), "Geev.Zero.Localization.SourceExt"
                    )
                )
            );
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
