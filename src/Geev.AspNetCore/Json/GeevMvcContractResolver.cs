using System;
using System.Reflection;
using Geev.AspNetCore.Configuration;
using Geev.Dependency;
using Geev.Extensions;
using Geev.Reflection;
using Geev.Timing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Geev.Json
{
    public class GeevMvcContractResolver : DefaultContractResolver
    {
        private readonly IIocResolver _iocResolver;

        private bool? _useMvcDateTimeFormat { get; set; }

        private string _datetimeFormat { get; set; } = null;

        protected readonly object SyncObj = new object();

        public GeevMvcContractResolver(IIocResolver iocResolver)
        {
            _iocResolver = iocResolver;
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty property = base.CreateProperty(member, memberSerialization);

            ModifyProperty(member, property);

            return property;
        }

        protected virtual void ModifyProperty(MemberInfo member, JsonProperty property)
        {
            if (property.PropertyType != typeof(DateTime) && property.PropertyType != typeof(DateTime?))
            {
                return;
            }

            if (ReflectionHelper.GetSingleAttributeOfMemberOrDeclaringTypeOrDefault<DisableDateTimeNormalizationAttribute>(member) != null)
            {
                return;
            }

            var converter = new GeevDateTimeConverter();

            if (!_useMvcDateTimeFormat.HasValue)
            {
                lock (SyncObj)
                {
                    if (!_useMvcDateTimeFormat.HasValue)
                    {
                        using (var configuration = _iocResolver.ResolveAsDisposable<IGeevAspNetCoreConfiguration>())
                        {
                            _useMvcDateTimeFormat = configuration.Object.UseMvcDateTimeFormatForAppServices;

                            if (_useMvcDateTimeFormat.Value)
                            {
                                using (var mvcJsonOptions = _iocResolver.ResolveAsDisposable<IOptions<MvcJsonOptions>>())
                                {
                                    _datetimeFormat = mvcJsonOptions.Object.Value.SerializerSettings.DateFormatString;
                                }
                            }
                        }
                    }
                }
            }
            
            // apply DateTimeFormat only if not empty
            if (!_datetimeFormat.IsNullOrWhiteSpace())
            {
                converter.DateTimeFormat = _datetimeFormat;
            }

            property.Converter = converter;
        }
    }
}