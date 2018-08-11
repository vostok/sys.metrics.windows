using System;
using System.Linq;
using Vostok.Sys.Metrics.Windows.Helpers;

namespace Vostok.Sys.Metrics.Windows.TestProcess
{
    class EntryPoint
    {
        static EntryPoint()
        {
            for (var i = 0; i < eatCpuTasks.Length; i++)
            {
                eatCpuTasks[i] = new EatCpuTask();
            }
        }

        static void Main(string[] args)
        {
            var shouldExit = false;
            while (!shouldExit)
            {
                try
                {
                    var line = Console.ReadLine();
                    var commands = line.Split(new []{' '}, StringSplitOptions.RemoveEmptyEntries);
                    switch (commands[0])
                    {
                        case "warmup":
                            // do nothing here. Warmup is needed for more accurate memory metrics
                            break;
                        case "eat_cpu":
                            EatCpu(ShiftArgs(commands, 1));
                            break;
                        case "eat_mem":
                            EatMem(ShiftArgs(commands, 1));
                            break;
                        case "eat_private_mem":
                            EatPrivateMem(ShiftArgs(commands, 1));
                            break;
                        case "gc":
                            DoGc(ShiftArgs(commands, 1));
                            break;
                        case "threads":
                            CreateThreads(ShiftArgs(commands, 1));
                            break;
                        case "exit":
                            DisposeAll();
                            shouldExit = true;
                            break;
                        default:
                            Console.WriteLine($"Unknown command '{commands[0]}'");
                            break;
                    }
                    Console.WriteLine(line);

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }

        private static void DoGc(string[] args)
        {
            var gen = int.Parse(args[0]);
            GC.Collect(gen, GCCollectionMode.Forced, true, true);
        }

        private static string[] ShiftArgs(string[] commands, int n)
        {
            return commands.Skip(n).ToArray();
        }

        private static void DisposeAll()
        {
            foreach (var eatCpuTask in eatCpuTasks)
            {
                eatCpuTask.Stop();
            }
            eatMemoryTask?.Stop();
            eatPrivateMemoryTask?.Stop();
        }

        private static void EatMem(string[] args)
        {
            var bytes = long.Parse(args[0]);
            var size = DataSize.FromBytes(bytes);
            eatMemoryTask = new EatMemoryTask(size);
            eatMemoryTask.Start();
            Console.WriteLine($"Have eaten {size} memory");
        }

        private static void EatPrivateMem(string[] args)
        {
            var bytes = long.Parse(args[0]);
            var size = DataSize.FromBytes(bytes);
            eatPrivateMemoryTask = new EatPrivateMemoryTask(size);
            eatPrivateMemoryTask.Start();
            Console.WriteLine($"Have eaten {size} memory");
        }

        private static void EatCpu(string[] args)
        {
            var count = int.Parse(args[0]);
            foreach (var eatCpuTask in eatCpuTasks)
            {
                eatCpuTask.Stop();
            }
            for (var i = 0; i < Math.Min(count, eatCpuTasks.Length); i++)
            {
                eatCpuTasks[i].Start();
            }
        }

        private static void CreateThreads(string[] args)
        {
            var count = int.Parse(args[0]);
            createThreadsTask.CreateThreads(count);
        }

        private static readonly EatCpuTask[] eatCpuTasks = new EatCpuTask[Environment.ProcessorCount];
        private static EatMemoryTask eatMemoryTask;
        private static EatPrivateMemoryTask eatPrivateMemoryTask;
        private static readonly CreateThreadsTask createThreadsTask = new CreateThreadsTask();
    }
}
