using Geev.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Geev.Runtime.Caching
{
    /// <summary>
    /// A class to hold the Type information and Serialized payload for data stored in the cache.
    /// </summary>
    public class GeevCacheData
    {
        private static readonly IReadOnlyList<string> SystemAssemblyNames = new List<string> { "mscorlib", "System.Private.CoreLib" };

        public GeevCacheData(
            string type, string payload)
        {
            Type = type;
            Payload = payload;
        }

        public string Payload { get; set; }

        public string Type { get; set; }

        public static GeevCacheData Deserialize(string serializedCacheData) => serializedCacheData.FromJsonString<GeevCacheData>();

        public static GeevCacheData Serialize(object obj)
        {
            return new GeevCacheData(
                SerializeType(obj.GetType()).ToString(),
                obj.ToJsonString());
        }

        private static StringBuilder SerializeType(Type type, bool withAssemblyName = true, StringBuilder typeNameBuilder = null)
        {
            typeNameBuilder = typeNameBuilder ?? new StringBuilder();

            if (type.DeclaringType != null)
            {
                SerializeType(type.DeclaringType, false, typeNameBuilder).Append('+');
            }
            else if (type.Namespace != null)
            {
                typeNameBuilder.Append(type.Namespace).Append('.');
            }

            typeNameBuilder.Append(type.Name);

            if (type.GenericTypeArguments.Length > 0)
            {
                SerializeTypes(type.GenericTypeArguments, '[', ']', typeNameBuilder);
            }

            if (!withAssemblyName)
            {
                return typeNameBuilder;
            }

            var assemblyName = type.GetTypeInfo().Assembly.GetName().Name;

            if (!SystemAssemblyNames.Contains(assemblyName))
            {
                typeNameBuilder.Append(", ").Append(assemblyName);
            }

            return typeNameBuilder;
        }

        private static StringBuilder SerializeTypes(Type[] types, char beginTypeDelimiter = '"', char endTypeDelimiter = '"', StringBuilder typeNamesBuilder = null)
        {
            if (types == null)
            {
                return null;
            }

            if (typeNamesBuilder == null)
            {
                typeNamesBuilder = new StringBuilder();
            }

            typeNamesBuilder.Append('[');

            for (int i = 0; i < types.Length; i++)
            {
                typeNamesBuilder.Append(beginTypeDelimiter);
                SerializeType(types[i], true, typeNamesBuilder);
                typeNamesBuilder.Append(endTypeDelimiter);

                if (i != types.Length - 1)
                {
                    typeNamesBuilder.Append(',');
                }
            }

            return typeNamesBuilder.Append(']');
        }
    }
}