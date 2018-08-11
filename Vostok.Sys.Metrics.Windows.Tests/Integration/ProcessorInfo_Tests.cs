using System;
using NUnit.Framework;
using Vostok.Sys.Metrics.Windows.Meters.CPU;

namespace Vostok.Sys.Metrics.Windows.Tests.Integration
{
    [TestFixture]
    public class ProcessorInfo_Tests
    {
        [Test]
        public void Does_not_throw_exceptions()
        {
            var physicalCores = ProcessorInfo.PhysicalCores;
            var logicalCores = ProcessorInfo.LogicalCores;

            Console.WriteLine($"Physical cores: {physicalCores}. Logical cores: {logicalCores}.");
        }
    }
}
