namespace EdiModuleCore.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class NotMatchedException : Exception
    {
        public NotMatchedException() { }
        public NotMatchedException(string message) : base(message) { }
        public NotMatchedException(string message, Exception inner) : base(message, inner) { }
        protected NotMatchedException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
