using System;
using System.Diagnostics;
using Vostok.Sys.Metrics.Windows.Helpers;
using Vostok.Sys.Metrics.Windows.Meters.Memory;

namespace Vostok.Sys.Metrics.Windows.Benchmark.Implementations
{
    /// <summary>
    /// <para>
    /// Estimates memory consumption for a given process.
    /// Reported values do not include child process consumption.
    /// </para>
    /// 
    /// <para>
    /// Internally uses <see cref="Process.PrivateMemorySize64"/> and <see cref="Process.WorkingSet64"/> values
    /// </para>
    /// </summary>
    public class ProcessMemoryMeterSlow : IDisposable
    {
        private readonly Process process;

        public ProcessMemoryMeterSlow(Process process = null)
        {
            this.process = process != null 
                ? Process.GetProcessById(process.Id) 
                : Process.GetCurrentProcess();
        }

        /// <summary>
        /// Estimates memory consumption
        /// </summary>
        /// <returns></returns>
        public ProcessMemoryInfo GetMemoryInfo()
        {
            process.Refresh();

            return new ProcessMemoryInfo
            {
                PrivateBytes = process.PrivateMemorySize64,
                WorkingSetBytes = process.WorkingSet64
            };
        }

        public void Dispose()
        {
            process.Dispose();
        }
    }
}