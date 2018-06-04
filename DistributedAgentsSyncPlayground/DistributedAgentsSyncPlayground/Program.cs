using DistributedAgentsSyncPlayground.ConflictMediators;

namespace DistributedAgentsSyncPlayground
{
    class Program
    {
        static void Main(string[] args)
        {
            var syncServer = new SyncServer.VectorClockSyncServer<string>();

            syncServer.Start();

            ImAVectorClockConflictResolver<string> whateverConflictMediator = new GenericConflictMediator<string>((a, b) => b);

            var alice = new VectorClockNode<string>("Alice");
            var ben = new VectorClockNode<string>("Ben");
            var dave = new VectorClockNode<string>("Dave");
            var cathy = new VectorClockNode<string>("Cathy");

            syncServer.Stop();

            VectorClockSyncResult<string> syncResult;

            alice.Say("Wednesday");
            syncResult = cathy.Acknowledge(alice);
            syncResult = ben.Acknowledge(alice);
            syncResult = dave.Acknowledge(alice);

            ben.Say("Tuesday");
            syncResult = alice.Acknowledge(ben);
            syncResult = dave.Acknowledge(ben);

            dave.Say("Tuesday");
            alice.Acknowledge(dave);
            ben.Acknowledge(dave);


            //Conflict starts here

            cathy.Say("Thursday");

            syncResult = dave.Acknowledge(cathy);

            var resolution = whateverConflictMediator.ResolveConflict(syncResult);

            syncResult = dave.Acknowledge(resolution.Solution);
        }
    }
}
