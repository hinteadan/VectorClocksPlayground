using H.VectorClocks.Http.HttpClients;
using System;
using System.Diagnostics;
using System.Linq;

namespace DistributedAgentsSyncPlayground
{
    class Program
    {
        static void Main(string[] args)
        {
            Random random = new Random();

            var syncServer = new HttpVectorClockSyncServer<string>("http://localhost:60000");

            Console.WriteLine($"Spawning Sync Server @ {DateTime.Now}");
            syncServer.Start();
            Process.Start("http://localhost:60000/sync/ping");
            Console.WriteLine($"Spawned Sync Server @ {DateTime.Now}");

            Console.WriteLine($"Spawning Nodes @ {DateTime.Now}");
            HttpVectorClockNode<string>[] nodes = new HttpVectorClockNode<string>[]
            {
                new HttpVectorClockNode<string>("http://localhost:60001"),
                new HttpVectorClockNode<string>("http://localhost:60002"),
                new HttpVectorClockNode<string>("http://localhost:60003"),
                new HttpVectorClockNode<string>("http://localhost:60004"),
            };
            Console.WriteLine($"Spawned Nodes @ {DateTime.Now}");

            foreach (string addr in nodes.Select(n => n.NodeID))
            {
                Process.Start($"{addr}/ping");
            }

            Console.WriteLine($"Started @ {DateTime.Now}");
            Console.WriteLine($"Press key to stop");
            Console.ReadLine();

            foreach (var node in nodes)
            {
                node.Dispose();
            }

            syncServer.Stop();
        }
    }
}
