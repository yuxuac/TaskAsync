using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TaskTest
{
    public static class Async3
    {
        public static object Response = string.Empty;

        public static void MainGo()
        {
            // Just wait.
            Wait();

            // With return value.
            WaitReturn();

            // With real return value.
            Task<string> t = WaitReturn2();
            t.GetAwaiter().OnCompleted(() =>
            {
                Console.WriteLine(t.Result.ToString());
            });

            Console.WriteLine("Start doing other things...");
            Console.ReadLine();
        }

        public static async void Sample()
        {
            int t = await Task<int>.Run<int>(() => { return LongWaitInt(); });
            Console.WriteLine(t);
        }

        public static int LongWaitInt()
        {
            Thread.Sleep(5000);
            return new Random().Next(1, 100);
        }

        public static async void Wait()
        {
            await Task.Run(() =>
            {
                Thread.Sleep(3000);
                Console.WriteLine("Wait()=>Wait done.");
            });
        }

        public static async void WaitReturn()
        {
            string r = await Task.Run<string>(() =>
            {
                Thread.Sleep(3000); return "WaitReturn()=>Wait done.";
            });
            Console.WriteLine(r);
        }

        public static async Task<string> WaitReturn2()
        {
            Task<string> t1 = Task.Run<string>(() => { Thread.Sleep(3000); return "WaitReturn2()=>Wait done."; });
            string str = await t1;
            return str;
        }

        #region Thread calls
        public void Methdo1()
        {
            Thread t1 = CreateAndStartAThread();
            Console.WriteLine("Done");
            while (t1.IsAlive)
            {
                Console.WriteLine("Response:" + Response);
                Thread.Sleep(500);
            }

            Console.WriteLine("Response:" + Response);
            Console.ReadKey();
        }

        public static Thread CreateAndStartAThread()
        {
            Thread t1 = new Thread(LongRun);
            t1.Start();
            return t1;
        }

        public static void LongRun()
        {
            Thread.Sleep(3000);
            Response = "LongRun done.";
        }
        #endregion
    }
}
