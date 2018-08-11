using System;
using System.Runtime.Serialization;

namespace Vostok.System.Metrics.Windows.PerformanceCounters
{
    internal class InvalidInstanceException : InvalidOperationException
    {
        public InvalidInstanceException(string instanceName) : base($"Instance with name: {instanceName} doesn't exists")
        {
        }

        public InvalidInstanceException(string instanceName, Exception innerException) : base($"Instance with name: {instanceName} doesn't exists", innerException)
        {
        }

        protected InvalidInstanceException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}