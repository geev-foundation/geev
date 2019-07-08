using System;
using Geev.Application.Services.Dto;
using Geev.Json;
using Geev.Timing;
using Shouldly;
using Xunit;

namespace Geev.Tests.Json
{
    public class EntityDtoSerialization_Tests
    {
        [Fact]
        public void Should_Serialize_Types_Derived_From_EntityDto()
        {
            var obj = new MyClass1
            {
                Id = 42,
                Value = new MyClass2
                {
                    Id = 42
                }
            };

            obj.ToJsonString().ShouldNotBeNull();
        }

        public class MyClass1 : EntityDto
        {
            public MyClass2 Value { get; set; }
        }

        public class MyClass2 : EntityDto
        {

        }
    }
}
