using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TaskTest
{
    public class Async2
    {
        public static async void AsyncMethodCall()
        {
            Task t1 = new Task(TimeConsumingMethod_Void);
            t1.Start();
            await t1;
        }

        public static async Task<object> AsyncMethodCall2()
        {
            Task<object> t = new Task<object>(() =>
            {
                Thread.Sleep(3000);
                return "OK2";
            });

            t.Start();
            await t;
            return t;
        }

        private static void TimeConsumingMethod_Void()
        {
            Thread.Sleep(3000);
            Console.WriteLine("OK1");
        }

        public static void MainGo()
        {
            Task<object> t1 = AsyncMethodCall2();
            t1.GetAwaiter().OnCompleted(() =>
            {
                Console.WriteLine("Async work done:" + t1.Result.ToString());
            });
            Console.WriteLine("New action below...");
            Console.ReadLine();
        }
    }
}
