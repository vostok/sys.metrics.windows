using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using FluentAssertions;
using Vostok.Sys.Metrics.Windows.Meters;
using NUnit.Framework;

namespace Vostok.Sys.Metrics.Windows.IntegrationTests
{
    [TestFixture]
    public class ProcessHelper_Tests
    {
        [Test]
        public void GetAllProcesses_works_like_system_diagnostics_process()
        {
            var helper = new ProcessHelper();
            var processes = helper.GetAllProcesses();
            var processesFromSysDiag = Process.GetProcesses();

            var ids = new HashSet<int>(processes.Select(p => p.Id));
            var idsSysDiag = new HashSet<int>(processesFromSysDiag.Select(p => p.Id));

            var sameCount = new HashSet<int>(ids);
            sameCount.IntersectWith(idsSysDiag);

            (ids.Count - sameCount.Count).Should().BeLessThan(5);
            (idsSysDiag.Count - sameCount.Count).Should().BeLessThan(5);

            Console.WriteLine($"ProcHelper: {ids.Count}. SysDiag: {idsSysDiag.Count}. Intersection: {sameCount.Count}");
            foreach (var processInfo in processes)
            {
                Console.WriteLine($"{processInfo.Name}:{processInfo.Id}.");
            }
        }
    }
}