using System;
using System.Threading;
using System.Threading.Tasks;

namespace DistributedAgentsSyncPlayground
{
    class Program
    {
        static void Main(string[] args)
        {
            Random random = new Random();

            var syncServer = new SyncServer.VectorClockSyncServer<string>();

            syncServer.Start();

            VectorClockNode<string>[] nodes = new VectorClockNode<string>[]
            {
                new ConsoleVectorClockNode("Alice"),
                new ConsoleVectorClockNode("Ben"),
                new ConsoleVectorClockNode("Dave"),
                new ConsoleVectorClockNode("Cathy"),
            };

            foreach (var node in nodes)
            {
                Task.Run(() =>
                {
                    var self = node;

                    if (syncServer.TryRegisterNode(node))
                        Console.WriteLine($"Registered node {node.NodeID} @ {DateTime.Now}");
                    else
                    {
                        Console.WriteLine($"Error registering node {node.NodeID} @ {DateTime.Now}");
                        return;
                    }

                    while (true)
                    {
                        Thread.Sleep(random.Next(1000, 3000));
                        self.Say(Guid.NewGuid().ToString());
                        syncServer.QueueEvent(self);
                    }
                });
            }



            Console.WriteLine($"Running @ {DateTime.Now}");
            Console.WriteLine($"Press key to stop");
            Console.ReadLine();

            syncServer.Stop();
        }
    }
}
