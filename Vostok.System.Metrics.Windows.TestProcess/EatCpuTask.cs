using System.Threading;
using System.Threading.Tasks;

namespace Vostok.System.Metrics.Windows.TestProcess
{
    internal class EatCpuTask
    {
        private Task eatingRoutine;
        private CancellationTokenSource cts;

        public void Start()
        {
            if (eatingRoutine == null)
            {
                cts = new CancellationTokenSource();
                eatingRoutine = Task.Run(() => StartRoutine());
            }
        }

        public void Stop()
        {
            if (eatingRoutine != null)
            {
                cts.Cancel();
                eatingRoutine.Wait();

                cts = null;
                eatingRoutine = null;
            }
        }

        private void StartRoutine()
        {
            while (!cts.IsCancellationRequested)
            {
                // do nothing, just poll cts
            }
        }
    }
}