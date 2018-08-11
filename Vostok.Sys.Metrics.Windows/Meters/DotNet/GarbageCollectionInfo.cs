using Vostok.Sys.Metrics.Windows.Helpers;

namespace Vostok.Sys.Metrics.Windows.Meters.DotNet
{
    public struct GarbageCollectionInfo
    {
        public double TimeInGCPercent { get; internal set; }
        public long Gen0CollectionsSinceStart { get; internal set; }
        public long Gen1CollectionsSinceStart { get; internal set; }
        public long Gen2CollectionsSinceStart { get; internal set; }

        public long Gen0CollectionsDelta { get; internal set; }
        public long Gen1CollectionsDelta { get; internal set; }
        public long Gen2CollectionsDelta { get; internal set; }

        public override string ToString()
        {
            return $"Gen-0: {Gen0CollectionsDelta}, Gen-1: {Gen1CollectionsDelta}, Gen-2: {Gen2CollectionsDelta}, Time in GC (%): {TimeInGCPercent.Format()}";
        }
    }
}