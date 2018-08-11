using System;
using Vostok.Sys.Metrics.Windows.Helpers;

namespace Vostok.Sys.Metrics.Windows.TestProcess
{
    internal class EatPrivateMemoryTask
    {
        private readonly DataSize size;
        private byte[] eaten;

        public EatPrivateMemoryTask(DataSize size)
        {
            this.size = size;
        }

        public void Start()
        {
            GC.Collect(2, GCCollectionMode.Forced, true, true);

            eaten = new byte[size.Bytes];
        }

        public void Stop()
        {
            eaten = null;
        }
    }
}