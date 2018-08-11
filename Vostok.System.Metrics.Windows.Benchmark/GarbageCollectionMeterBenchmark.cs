﻿using System;
using BenchmarkDotNet.Attributes;
using Vostok.System.Metrics.Windows.Benchmark.Implementations;
using Vostok.System.Metrics.Windows.Benchmark.Implementations.PerfCounters;
using Vostok.System.Metrics.Windows.Meters.DotNet;
using Vostok.System.Metrics.Windows.PerformanceCounters;
using Vostok.System.Metrics.Windows.PerformanceCounters.Batch;

namespace Vostok.System.Metrics.Windows.Benchmark
{
    [MemoryDiagnoser]
    public class GarbageCollectionMeterBenchmark
    {
        //TODO ezsilmar Fix benchmark
        private GarbageCollectionMeter_WithManyCounters dotNetMeter =
            new GarbageCollectionMeter_WithManyCounters(new PerformanceCounterFactory_Old(false), InstanceNameProviders.DotNet.ForCurrentProcess());
        private GarbageCollectionMeter_WithManyCounters pdhMeter =
            new GarbageCollectionMeter_WithManyCounters(new PerformanceCounterFactory_Old(true), InstanceNameProviders.DotNet.ForCurrentProcess());
        private GarbageCollectionMeter meter = new GarbageCollectionMeter(PerformanceCounterFactory.Default, InstanceNameProviders.DotNet.ForCurrentProcess());
        private ManagedMemoryMeter overallMeter = new ManagedMemoryMeter();

        [GlobalSetup]
        public void Setup() => GC.Collect();

        [Benchmark]
        public void DotNet() => dotNetMeter.GetGarbageCollectionInfo();
        [Benchmark]
        public void Pdh() => pdhMeter.GetGarbageCollectionInfo();
        [Benchmark]
        public void PdhBatch() => meter.GetGarbageCollectionInfo();
        [Benchmark]
        public void Overall() => overallMeter.GetManagedMemoryInfo();
        [Benchmark]
        public void CreatePdhBatch() => new GarbageCollectionMeter(PerformanceCounterFactory.Default, InstanceNameProviders.DotNet.ForCurrentProcess());
    }
}