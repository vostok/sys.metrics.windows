using System;
using System.Collections.Generic;

namespace Vostok.Sys.Metrics.Windows.TestProcess
{
    internal class EatMemoryTask
    {
        private readonly long size;
        private readonly long bucketSize = 2 * 1024;
        private List<byte[]> eaten;

        public EatMemoryTask(long size)
        {
            this.size = size;
        }

        public void Start()
        {
            GC.Collect(2, GCCollectionMode.Forced, true, true);

            eaten = new List<byte[]>();
            var iterations = (int) (size / bucketSize) + 1;
            for (var i = 0; i < iterations; i++)
            {
                eaten.Add(new byte[bucketSize]);
            }
        }

        public void Stop()
        {
            eaten = null;
        }
    }
}