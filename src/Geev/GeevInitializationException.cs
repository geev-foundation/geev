using System;
using System.Runtime.Serialization;

namespace Geev
{
    /// <summary>
    /// This exception is thrown if a problem on ABP initialization progress.
    /// </summary>
    [Serializable]
    public class GeevInitializationException : GeevException
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public GeevInitializationException()
        {

        }

        /// <summary>
        /// Constructor for serializing.
        /// </summary>
        public GeevInitializationException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {

        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="message">Exception message</param>
        public GeevInitializationException(string message)
            : base(message)
        {

        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="innerException">Inner exception</param>
        public GeevInitializationException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
