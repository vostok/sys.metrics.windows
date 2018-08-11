using System.Collections.Generic;
using System.Threading;

namespace Vostok.System.Metrics.Windows.TestProcess
{
    public class CreateThreadsTask
    {
        private readonly List<Thread> threads = new List<Thread>();
        private CancellationTokenSource cts;

        public void CreateThreads(int count)
        {
            ClearThreads();

            cts = new CancellationTokenSource();
            for (var i = 0; i < count; i++)
            {
                var thread = new Thread(Routine);
                threads.Add(thread);
                thread.Start();
            }
        }

        private void Routine()
        {
            while (!cts.IsCancellationRequested)
            {
                Thread.Sleep(10);
            }
        }

        private void ClearThreads()
        {
            if (cts != null)
            {
                cts.Cancel();
                foreach (var thread in threads)
                {
                    thread.Join(100);
                }
                threads.Clear();
                cts = null;
            }
        }
    }
}