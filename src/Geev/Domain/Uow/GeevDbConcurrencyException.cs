using System;
using System.Runtime.Serialization;

namespace Geev.Domain.Uow
{
    [Serializable]
    public class GeevDbConcurrencyException : GeevException
    {
        /// <summary>
        /// Creates a new <see cref="GeevDbConcurrencyException"/> object.
        /// </summary>
        public GeevDbConcurrencyException()
        {

        }

        /// <summary>
        /// Creates a new <see cref="GeevException"/> object.
        /// </summary>
        public GeevDbConcurrencyException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {

        }

        /// <summary>
        /// Creates a new <see cref="GeevDbConcurrencyException"/> object.
        /// </summary>
        /// <param name="message">Exception message</param>
        public GeevDbConcurrencyException(string message)
            : base(message)
        {

        }

        /// <summary>
        /// Creates a new <see cref="GeevDbConcurrencyException"/> object.
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="innerException">Inner exception</param>
        public GeevDbConcurrencyException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}