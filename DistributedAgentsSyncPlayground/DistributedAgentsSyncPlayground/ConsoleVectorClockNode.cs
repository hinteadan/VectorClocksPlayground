using System;

namespace DistributedAgentsSyncPlayground
{
    internal class ConsoleVectorClockNode : VectorClockNode<string>
    {
        #region Construct
        public ConsoleVectorClockNode(string nodeId) : base(nodeId)
        {
        }

        public ConsoleVectorClockNode(string nodeId, string payload, params VectorClockNodeVersion[] revision) : base(nodeId, payload, revision)
        {
        }
        #endregion

        public override VectorClockNode<string> Say(string payload)
        {
            Console.WriteLine($"{NodeID} says: {payload}  @ {DateTime.Now}");

            return base.Say(payload);
        }

        public override VectorClockSyncResult<string> Acknowledge(VectorClockNode<string> vectorClock)
        {
            Console.WriteLine($"{this} tries to aknowledge {vectorClock} @ {DateTime.Now}");

            var result = base.Acknowledge(vectorClock);

            Console.WriteLine($"{result} @ {DateTime.Now}");

            return result;
        }
    }
}
