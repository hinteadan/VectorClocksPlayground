namespace DistributedAgentsSyncPlayground
{
    class Program
    {
        static void Main(string[] args)
        {
            var alice = new VectorClockNode<string>("Alice");
            var ben = new VectorClockNode<string>("Ben");
            var dave = new VectorClockNode<string>("Dave");

            VectorClockSyncResult<string> syncResult;

            alice.SetPayload("Wednesday");
            syncResult = ben.SyncWith(alice);
            syncResult = dave.SyncWith(alice);

            ben.SetPayload("Tuesday");
            syncResult = alice.SyncWith(ben);
            syncResult = dave.SyncWith(ben);

            dave.SetPayload("Tuesday");
            alice.SyncWith(dave);
            ben.SyncWith(dave);


            syncResult = ben.SyncWith(dave);

        }
    }
}
