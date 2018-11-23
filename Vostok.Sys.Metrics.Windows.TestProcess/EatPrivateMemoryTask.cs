using System;

namespace Vostok.Sys.Metrics.Windows.TestProcess
{
    internal class EatPrivateMemoryTask
    {
        private readonly long size;
        private byte[] eaten;

        public EatPrivateMemoryTask(long size)
        {
            this.size = size;
        }

        public void Start()
        {
            GC.Collect(2, GCCollectionMode.Forced, true, true);

            eaten = new byte[size];
        }

        public void Stop()
        {
            eaten = null;
        }
    }
}