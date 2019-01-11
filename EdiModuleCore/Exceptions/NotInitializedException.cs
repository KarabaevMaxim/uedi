namespace EdiModuleCore.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class NotInitializedException : Exception
    {
        public NotInitializedException() { }
        public NotInitializedException(string message) : base(message) { }
        public NotInitializedException(string message, Exception inner) : base(message, inner) { }
        protected NotInitializedException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
