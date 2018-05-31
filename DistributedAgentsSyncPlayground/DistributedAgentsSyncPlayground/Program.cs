namespace DistributedAgentsSyncPlayground
{
    class Program
    {
        static void Main(string[] args)
        {
            var alice = new VectorClockNode<string>("Alice");
            var ben = new VectorClockNode<string>("Ben");
            var dave = new VectorClockNode<string>("Dave");
            var cathy = new VectorClockNode<string>("Cathy");

            VectorClockSyncResult<string> syncResult;

            alice.Says("Wednesday");
            syncResult = cathy.Acknowledges(alice);
            syncResult = ben.Acknowledges(alice);
            syncResult = dave.Acknowledges(alice);

            ben.Says("Tuesday");
            syncResult = alice.Acknowledges(ben);
            syncResult = dave.Acknowledges(ben);

            dave.Says("Tuesday");
            alice.Acknowledges(dave);
            ben.Acknowledges(dave);

            cathy.Says("Thursday");

            syncResult = dave.Acknowledges(cathy);

        }
    }
}
