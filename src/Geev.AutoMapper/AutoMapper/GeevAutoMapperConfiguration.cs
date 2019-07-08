using System;
using System.Collections.Generic;
using AutoMapper;

namespace Geev.AutoMapper
{
    public class GeevAutoMapperConfiguration : IGeevAutoMapperConfiguration
    {
        public List<Action<IMapperConfigurationExpression>> Configurators { get; }

        public bool UseStaticMapper { get; set; }

        public GeevAutoMapperConfiguration()
        {
            UseStaticMapper = true;
            Configurators = new List<Action<IMapperConfigurationExpression>>();
        }
    }
}