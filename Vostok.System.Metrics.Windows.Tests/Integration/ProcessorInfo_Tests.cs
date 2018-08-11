using System;
using Vostok.System.Metrics.Windows.Meters.CPU;
using NUnit.Framework;

namespace Vostok.System.Metrics.Windows.IntegrationTests
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
