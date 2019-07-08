﻿using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Geev.Collections.Extensions;
using AutoMapper;
using AutoMapper.Collection;
using AutoMapper.EquivalencyExpression;
using AutoMapper.Internal;
using Geev.Reflection.Extensions;

namespace Geev.AutoMapper
{
    public class AutoMapToAttribute : AutoMapAttributeBase
    {
        public MemberList MemberList { get; set; } = MemberList.Source;

        public AutoMapToAttribute(params Type[] targetTypes)
            : base(targetTypes)
        {

        }

        public AutoMapToAttribute(MemberList memberList, params Type[] targetTypes)
            : this(targetTypes)
        {
            MemberList = memberList;
        }

        public override void CreateMap(IMapperConfigurationExpression configuration, Type type)
        {
            if (TargetTypes.IsNullOrEmpty())
            {
                return;
            }

            configuration.CreateAutoAttributeMaps(type, TargetTypes, MemberList);
        }
    }
}