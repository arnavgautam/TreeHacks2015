namespace FabrikamWorker
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class CustomException : Exception
    {
        public CustomException()
            : base()
        {
        }

        public CustomException(string message)
            : base(message)
        {
        }

        public CustomException(string message, Exception inner)
            : base(message, inner)
        {
        }

        // A constructor is needed for serialization when an 
        // exception propagates from a remoting server to the client.  
        protected CustomException(SerializationInfo info, StreamingContext context)
        {
        }
    }
}