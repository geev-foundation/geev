using System;
using Geev.Events.Bus.Handlers;

namespace Geev.Tests.Events.Bus
{
    public class MySimpleTransientEventHandler : IEventHandler<MySimpleEventData>, IDisposable
    {
        public static int HandleCount { get; set; }

        public static int DisposeCount { get; set; }

        public void HandleEvent(MySimpleEventData eventData)
        {
            ++HandleCount;
        }

        public void Dispose()
        {
            ++DisposeCount;
        }
    }
}