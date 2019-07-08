using System;
using System.Reflection;

namespace Geev.Aspects
{
    //THIS NAMESPACE IS WORK-IN-PROGRESS

    internal abstract class AspectAttribute : Attribute
    {
        public Type InterceptorType { get; set; }

        protected AspectAttribute(Type interceptorType)
        {
            InterceptorType = interceptorType;
        }
    }

    internal interface IGeevInterceptionContext
    {
        object Target { get; }

        MethodInfo Method { get; }

        object[] Arguments { get; }

        object ReturnValue { get; }

        bool Handled { get; set; }
    }

    internal interface IGeevBeforeExecutionInterceptionContext : IGeevInterceptionContext
    {

    }


    internal interface IGeevAfterExecutionInterceptionContext : IGeevInterceptionContext
    {
        Exception Exception { get; }
    }

    internal interface IGeevInterceptor<TAspect>
    {
        TAspect Aspect { get; set; }

        void BeforeExecution(IGeevBeforeExecutionInterceptionContext context);

        void AfterExecution(IGeevAfterExecutionInterceptionContext context);
    }

    internal abstract class GeevInterceptorBase<TAspect> : IGeevInterceptor<TAspect>
    {
        public TAspect Aspect { get; set; }

        public virtual void BeforeExecution(IGeevBeforeExecutionInterceptionContext context)
        {
        }

        public virtual void AfterExecution(IGeevAfterExecutionInterceptionContext context)
        {
        }
    }

    internal class Test_Aspects
    {
        internal class MyAspectAttribute : AspectAttribute
        {
            public int TestValue { get; set; }

            public MyAspectAttribute()
                : base(typeof(MyInterceptor))
            {
            }
        }

        internal class MyInterceptor : GeevInterceptorBase<MyAspectAttribute>
        {
            public override void BeforeExecution(IGeevBeforeExecutionInterceptionContext context)
            {
                Aspect.TestValue++;
            }

            public override void AfterExecution(IGeevAfterExecutionInterceptionContext context)
            {
                Aspect.TestValue++;
            }
        }

        public class MyService
        {
            [MyAspect(TestValue = 41)] //Usage!
            public void DoIt()
            {

            }
        }

        public class MyClient
        {
            private readonly MyService _service;

            public MyClient(MyService service)
            {
                _service = service;
            }

            public void Test()
            {
                _service.DoIt();
            }
        }
    }
}
