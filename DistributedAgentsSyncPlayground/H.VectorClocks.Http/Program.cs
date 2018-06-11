using Nancy.Hosting.Self;
using System;
using System.Linq;

namespace H.VectorClocks.Http
{
    class Program
    {
        static void Main(string[] args)
        {
            if (!args.Any())
            {
                PrintUsersManual();
                Console.ReadLine();
                return;
            }

            string url = args[0];

            var host = new NancyHost(new Uri(url));
            host.Start();
            Console.WriteLine($"Running H.VectorClocks.Http server on {url} @ {DateTime.Now}");

            Console.ReadLine();

            host.Dispose();
        }

        private static void PrintUsersManual()
        {
            Console.WriteLine("Usage: H.VectorClocks.Http.exe <url>");
            Console.WriteLine();
            Console.WriteLine("Usage: H.VectorClocks.Http.exe http://localhost:9999");
        }
    }
}
