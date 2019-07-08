using System;
using System.Runtime.Serialization;

namespace Geev
{
    /// <summary>
    /// Base exception type for those are thrown by Geev system for Geev specific exceptions.
    /// </summary>
    [Serializable]
    public class GeevException : Exception
    {
        /// <summary>
        /// Creates a new <see cref="GeevException"/> object.
        /// </summary>
        public GeevException()
        {

        }

        /// <summary>
        /// Creates a new <see cref="GeevException"/> object.
        /// </summary>
        public GeevException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {

        }

        /// <summary>
        /// Creates a new <see cref="GeevException"/> object.
        /// </summary>
        /// <param name="message">Exception message</param>
        public GeevException(string message)
            : base(message)
        {

        }

        /// <summary>
        /// Creates a new <see cref="GeevException"/> object.
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="innerException">Inner exception</param>
        public GeevException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
