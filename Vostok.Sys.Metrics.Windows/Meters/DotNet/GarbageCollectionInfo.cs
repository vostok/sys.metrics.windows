using Vostok.Sys.Metrics.Windows.Helpers;

namespace Vostok.Sys.Metrics.Windows.Meters.DotNet
{
    public struct GarbageCollectionInfo
    {
        public double TimeInGCPercent;
        public long Gen0CollectionsSinceStart;
        public long Gen1CollectionsSinceStart;
        public long Gen2CollectionsSinceStart;

        public long Gen0CollectionsDelta;
        public long Gen1CollectionsDelta;
        public long Gen2CollectionsDelta;

        public override string ToString()
        {
            return $"Gen-0: {Gen0CollectionsDelta}, Gen-1: {Gen1CollectionsDelta}, Gen-2: {Gen2CollectionsDelta}, Time in GC (%): {TimeInGCPercent.Format()}";
        }
    }
}