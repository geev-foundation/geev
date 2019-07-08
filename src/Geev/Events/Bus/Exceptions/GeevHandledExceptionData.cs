using System;

namespace Geev.Events.Bus.Exceptions
{
    /// <summary>
    /// This type of events are used to notify for exceptions handled by ABP infrastructure.
    /// </summary>
    public class GeevHandledExceptionData : ExceptionData
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="exception">Exception object</param>
        public GeevHandledExceptionData(Exception exception)
            : base(exception)
        {

        }
    }
}