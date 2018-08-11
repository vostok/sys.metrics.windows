// ReSharper disable InconsistentNaming
namespace Vostok.Sys.Metrics.Windows.Native.Constants
{
    internal enum LOGICAL_PROCESSOR_RELATIONSHIP {
        RelationProcessorCore = 0,
        RelationNumaNode = 1,
        RelationCache = 2,
        RelationProcessorPackage = 3,
        RelationGroup = 4,
        RelationAll = 0xffff
    }
}