namespace Figlut.Server.Toolkit.Utilities
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    #endregion //Using Directives

    public class ThreadHelper
    {
        #region Methods

        /// <summary>
        /// Get the number of currently running threads in the CLR thread pool: https://blogs.msdn.microsoft.com/oldnewthing/20170724-00/?p=96675
        /// </summary>
        /// <returns></returns>
        public static void GetCurrentThreadCount(out int workerThreadsRunning, out int completionPortThreadsRunning)
        {
            ThreadPool.GetMaxThreads(out int maxWorkerThreads, out int maxCompletionPortThreads);
            ThreadPool.GetAvailableThreads(out int availableWorkerThreads, out int availableCompletionPortThreads);
            workerThreadsRunning = maxWorkerThreads - availableWorkerThreads;
            completionPortThreadsRunning = maxCompletionPortThreads - availableCompletionPortThreads;
        }

        /// <summary>
        /// Sets the minimum number of threads available in the Thread Pools (Worker Thread Pool) and IOCP (IO Completion Port) threads.
        /// Worker Threads vs IOCP (IO Completion Port) threads: https://www.infoworld.com/article/3201030/understand-the-net-clr-thread-pool.html 
        /// 
        /// The default value of the minimum setting, which is the minimum number of both worker and IOCP threads, is determined by the number of
        /// processors in your system. Hence, if your system has four cores, you would have four worker threads and four IOCP threads by default. 
        /// </summary>
        private void GetMinimumThreadsCount(out int minimumWorkerThreadCount, out int minimumCompletionPortThreadCount)
        {
            ThreadPool.GetMinThreads(out minimumWorkerThreadCount, out minimumCompletionPortThreadCount);
        }

        /// <summary>
        /// Sets the minimum number of threads available in the Thread Pools (Worker Thread Pool) and IOCP (IO Completion Port) threads.
        /// Worker Threads vs IOCP (IO Completion Port) threads: https://www.infoworld.com/article/3201030/understand-the-net-clr-thread-pool.html 
        /// 
        /// The default value of the minimum setting, which is the minimum number of both worker and IOCP threads, is determined by the number of
        /// processors in your system. Hence, if your system has four cores, you would have four worker threads and four IOCP threads by default. 
        /// 
        /// You can set the minimum configuration values for both worker and IOCP threads to any value between one and 50. 
        /// A good approach is to take a user mode process dump of the IIS worker process (W3wp.exe) and then use the !threadpool command to report 
        /// the total number of worker threads. Once you know this value, simply divide it by the number of processor cores on your system to 
        /// determine the minimum worker and IOCP thread settings. For example, if the total count of worker threads is 100 and you have four 
        /// processors in your system, you can set the minimum values for both worker and IOCP threads to 25 i.e. each CPU should be able to handle 25 
        /// threads at a minimum to handle 100 threads in total at a minimum.
        /// </summary>
        public static void SetMinimumThreadsCount(int minimumWorkerThreadCount, int minimumCompletionPortThreadCount)
        {
            ThreadPool.SetMinThreads(minimumWorkerThreadCount, minimumCompletionPortThreadCount);
        }

        /// <summary>
        /// Gets the total number of running threads in the application. This will include both managed and unmanaged threads. https://stackoverflow.com/questions/10439916/find-out-how-many-threads-my-application-is-running
        /// </summary>
        /// <returns></returns>
        public static int GetTotalThreadsRunningCountInCurrentProcess()
        {
            return System.Diagnostics.Process.GetCurrentProcess().Threads.Count;
        }

        #endregion //Methods
    }
}
