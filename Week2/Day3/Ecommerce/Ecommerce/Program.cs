using Ecommerce.System;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Ecommerce
{
//    Hands-On Lab: Concurrent Async Operations
//1. Write 3 async methods that each simulate a delay(Task.Delay) representing a different data source.✔
//2. Call all 3 sequentially with individual awaits and measure the total elapsed time.✔
//3. Rewrite the same calls using Task.WhenAll and compare the elapsed time.✔
//4. Add a CancellationToken parameter to one method and demonstrate cancelling it mid-operation.✔
//5. Commit the concurrency demo to your GitHub repository.✔
    internal class Program
    {
        static async Task Main(string[] args)
        {

            Product product = new Product();
            product.id = 1;
            product.name ="laptop";
            product.price = 100;
            product.description ="This is a laptop";

            Console.WriteLine($"Product name : {product.name} Price : {product.price} Description : {product.description} ");



            var result = Stopwatch.StartNew();
            // Sequential : 
            await GetCustomerAsync();
            await GetOrderAsync();
            await GetCustomerWithOrder(CancellationToken.None);

            Console.WriteLine();
            result.Stop();
            Console.WriteLine($"Time : {result.ElapsedMilliseconds} ms");

            Console.WriteLine();

            // 2 Task.WhenAll :

            result.Restart();

            await Task.WhenAll(
                GetCustomerAsync(),
                GetOrderAsync(),
                GetCustomerWithOrder(CancellationToken.None)
                );
            Console.WriteLine();

            result.Stop();
            Console.WriteLine($"Time : {result.ElapsedMilliseconds} ms");
            Console.WriteLine();

            var cancel = new CancellationTokenSource();
            var test = GetCustomerWithOrder(cancel.Token);
            cancel.CancelAfter(1500);
            try
            {
                await test;
            }
            catch
            {
                Console.WriteLine("Customer With Order loading was cancelled");
            }

            //  Result 
            // Task.WhenAll completes faster all tasks run concurrently
        }


        private static async Task GetCustomerAsync()
        {
            await Task.Delay(2000);
            Console.WriteLine("Customers Loaded ");

        }
        private static async Task GetOrderAsync()
        {
            await Task.Delay(3000);
            Console.WriteLine("Orders Loaded ");

        }
        private static async Task GetCustomerWithOrder(CancellationToken token)
        {
            await Task.Delay(4000, token);
            Console.WriteLine("Customer With Order ");

        }

    }
}
