using System;
using System.Runtime.Serialization;

namespace Musikplatta
{
    [Serializable]
    public class SharpWintabException : Exception
    {
        public SharpWintabException()
        {
        }

        public SharpWintabException(string message) : base(message)
        {
        }

        public SharpWintabException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected SharpWintabException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}