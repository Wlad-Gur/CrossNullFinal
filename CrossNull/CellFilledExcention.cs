using System;
using System.Runtime.Serialization;

namespace CrossNull
{
    [Serializable]
    internal class CellFilledExcention : Exception
    {
        public CellFilledExcention()
        {
        }

        public CellFilledExcention(string message) : base(message)
        {

        }

        public CellFilledExcention(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CellFilledExcention(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
