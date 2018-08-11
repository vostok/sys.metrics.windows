using System;
using System.Collections.Generic;
using Vostok.System.Metrics.Windows.Helpers;

namespace Vostok.System.Metrics.Windows.TestProcess
{
    internal class EatMemoryTask
    {
        private readonly DataSize size;
        private readonly DataSize bucketSize = DataSize.FromKilobytes(2);
        private List<byte[]> eaten;

        public EatMemoryTask(DataSize size)
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
                eaten.Add(new byte[bucketSize.Bytes]);
            }
        }

        public void Stop()
        {
            eaten = null;
        }
    }
}