using System;
using Geev.MultiTenancy;
using Geev.Reflection.Extensions;
using Shouldly;
using Xunit;

namespace Geev.Tests.Reflection.Extensions
{
    public class MemberInfoExtensions_Tests
    {
        [Theory]
        [InlineData(typeof(MyClass))]
        [InlineData(typeof(MyBaseClass))]
        public void GetSingleAttributeOfTypeOrBaseTypesOrNull_Test(Type type)
        {
            var attr = type.GetSingleAttributeOfTypeOrBaseTypesOrNull<MultiTenancySideAttribute>();
            attr.ShouldNotBeNull();
            attr.Side.ShouldBe(MultiTenancySides.Host);
        }

        private class MyClass : MyBaseClass
        {
            
        }

        [MultiTenancySide(MultiTenancySides.Host)]
        private abstract class MyBaseClass
        {

        }
    }
}
