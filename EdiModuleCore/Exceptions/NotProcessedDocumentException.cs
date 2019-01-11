namespace EdiModuleCore.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class NotProcessedDocumentException : Exception
    {
        public NotProcessedDocumentException() { }
        public NotProcessedDocumentException(string message) : base(message) { }
        public NotProcessedDocumentException(string message, Exception inner) : base(message, inner) { }
        protected NotProcessedDocumentException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
