using System;
using System.Runtime.Serialization;

namespace Common.Web.Common.Exceptions
{
    [Serializable]
    public class DeleteFailureException : Exception
    {
        public DeleteFailureException()
        {
        }

        public DeleteFailureException(string message) : base(message)
        {
        }

        public DeleteFailureException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DeleteFailureException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
        
        public DeleteFailureException(string name, object key, string message)
            : base($"Deletion of entity \"{name}\" ({key}) failed. {message}")
        {
        }
    }
}