using System;
using System.Threading;

#if NET_STANDARD_2_0
using System.Threading.Tasks;
#endif

namespace BestHTTP.PlatformSupport.Threading
{
    public static class ThreadedRunner
    {
        public static void RunShortLiving<T>(Action<T> job, T param)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(_ => job(param)));
        }

        public static void RunShortLiving<T1, T2>(Action<T1, T2> job, T1 param1, T2 param2)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(_ => job(param1, param2)));
        }

        public static void RunShortLiving<T1, T2, T3>(Action<T1, T2, T3> job, T1 param1, T2 param2, T3 param3)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(_ => job(param1, param2, param3)));
        }

        public static void RunShortLiving(Action job)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback((param) => job()));
        }

        public static void RunLongLiving(Action job)
        {
#if NETFX_CORE
#pragma warning disable 4014
            Windows.System.Threading.ThreadPool.RunAsync((param) => job());
#pragma warning restore 4014
#elif NET_STANDARD_2_0
            var _task = new Task(() => job(), TaskCreationOptions.LongRunning);
            _task.ConfigureAwait(false);
            _task.Start();
#else
            var thread = new Thread(new ParameterizedThreadStart((param) => job()));
            thread.Start();
#endif
        }
    }
}
