using System;
using System.Collections.Generic;
using Geev.Json;
using Geev.Runtime.Caching;
using Newtonsoft.Json;
using Shouldly;
using Xunit;

namespace Geev.Tests.Runtime.Caching
{
    public class GeevCacheData_Tests
    {
        [Fact]
        public void Serialize_List_Test()
        {
            List<string> source = new List<string>
            {
                "Stranger Things",
                "The OA",
                "Lost in Space"
            };

            var result = GeevCacheData.Serialize(source);
            result.Type.ShouldBe("System.Collections.Generic.List`1[[System.String]]");
            result.Payload.ShouldBe("[\"Stranger Things\",\"The OA\",\"Lost in Space\"]");
        }

        [Fact]
        public void Serialize_Class_Test()
        {
            var source = new MyTestClass
            {
                Field1 = 42,
                Field2 = "Stranger Things"
            };

            var result = GeevCacheData.Serialize(source);
            result.Type.ShouldBe("Geev.Tests.Runtime.Caching.GeevCacheData_Tests+MyTestClass, Geev.Tests");
            result.Payload.ShouldBe("{\"Field1\":42,\"Field2\":\"Stranger Things\"}");
        }

        [Fact]
        public void Deserialize_List_Test()
        {
            var json = "{\"Payload\":\"[\\\"Stranger Things\\\",\\\"The OA\\\",\\\"Lost in Space\\\"]\",\"Type\":\"System.Collections.Generic.List`1[[System.String]]\"}";
            var cacheData = GeevCacheData.Deserialize(json);

            cacheData.ShouldNotBeNull();
        }

        [Fact]
        public void Deserialize_Class_Test()
        {
            var json = "{\"Payload\": \"{\\\"Field1\\\": 42,\\\"Field2\\\":\\\"Stranger Things\\\"}\",\"Type\":\"Geev.Tests.Runtime.Caching.GeevCacheData_Tests+MyTestClass, Geev.Tests\"}";

            var cacheData = GeevCacheData.Deserialize(json);

            cacheData.ShouldNotBeNull();
        }

        class MyTestClass
        {
            public int Field1 { get; set; }

            public string Field2 { get; set; }
        }
    }
}
