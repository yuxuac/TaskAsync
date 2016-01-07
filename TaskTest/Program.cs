using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TaskTest
{
    class Program
    {
        #region Locker

        private static object locker = new object();

        private static int taskCount;
        public static int TaskCount 
        { 
            get 
            { 
                lock(locker)
                {
                    return taskCount;
                }
            }

            set 
            {
                lock (locker)
                {
                    taskCount = value;
                }
            }
        }

        #endregion

        static void Main(string[] args)
        {
            ProcessTasksWithMutiThreads();
        }

        /// <summary>
        /// MutiThreading process with Tasks.
        /// </summary>
        private static void ProcessTasksWithMutiThreads()
        {
            Random r = new Random();
            TaskCount = 1000;
            int maximumNum = 10;

            List<Task> tasks = new List<Task>();
            var taskFactory = new TaskFactory();

            while (TaskCount > 0)
            {
                Console.WriteLine("==========================================");
                if (tasks.Count < maximumNum)
                {
                    tasks.Add(taskFactory.StartNew(() =>
                    {
                        int sleepTime = r.Next(3, 15);
                        Console.WriteLine(Thread.CurrentThread.ManagedThreadId + " - Sleep " + sleepTime + " seconds..");
                        Thread.Sleep(sleepTime * 1000);
                        TaskCount--;
                    }));
                }
                else
                {
                    Console.WriteLine(Thread.CurrentThread.ManagedThreadId + " - Wait for tasks to complete.");
                    Task.WaitAll(tasks.ToArray());
                    tasks = tasks.Where(t => !t.IsCompleted && !t.IsCanceled && t.IsFaulted).ToList();
                }
                Console.WriteLine(Thread.CurrentThread.ManagedThreadId + " - #Tasks:" + tasks.Count);
                Console.WriteLine(Thread.CurrentThread.ManagedThreadId + " - " + TaskCount + " tasks completed.");
                Console.WriteLine("==========================================");
            }
        }

        private static void TaskMethod1()
        { 
            CancellationTokenSource cts = new CancellationTokenSource();
            
            TaskFactory<int> fac = new TaskFactory<int>();

            var task = fac.StartNew((cnt) =>
            {
                return CalculateSum(cts.Token, (int)cnt);
            }, 30);

            // task.Wait();
            while (true)
            {
                Console.WriteLine("Enter 'c' to cancel exectuion.");
                if (Console.ReadLine().Trim().ToLower() == "c")
                {
                    cts.Cancel();
                }
            }
        }

        private static int CalculateSum(CancellationToken ct, int cnt)
        {
            int sum = 0;
            for (int i = 0; i < cnt; i++)
            {
                if(ct.IsCancellationRequested)
                {
                    Console.WriteLine("Canceled");
                    break;
                }

                Thread.Sleep(500);
                sum += i;
                Console.WriteLine(i + ":" + sum);
            }
            return sum;
        }
    }
}
