using System;
using System.Runtime.Serialization;
using Geev.Web.Models;

namespace Geev.WebApi.Client
{
    /// <summary>
    /// This exception is thrown when a remote method call made and remote application sent an error message.
    /// </summary>
    [Serializable]
    public class GeevRemoteCallException : GeevException
    {
        /// <summary>
        /// Remote error information.
        /// </summary>
        public ErrorInfo ErrorInfo { get; set; }

        /// <summary>
        /// Creates a new <see cref="GeevException"/> object.
        /// </summary>
        public GeevRemoteCallException()
        {

        }

        /// <summary>
        /// Creates a new <see cref="GeevException"/> object.
        /// </summary>
        public GeevRemoteCallException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {

        }

        /// <summary>
        /// Creates a new <see cref="GeevException"/> object.
        /// </summary>
        /// <param name="errorInfo">Exception message</param>
        public GeevRemoteCallException(ErrorInfo errorInfo)
            : base(errorInfo.Message)
        {
            ErrorInfo = errorInfo;
        }
    }
}
