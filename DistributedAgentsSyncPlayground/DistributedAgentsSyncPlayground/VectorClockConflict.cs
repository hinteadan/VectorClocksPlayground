namespace DistributedAgentsSyncPlayground
{
    public class VectorClockConflict<T>
    {
        public VectorClockConflict(VectorClockNode<T> nodeA, VectorClockNode<T> nodeB)
        {
            NodeA = nodeA;
            NodeB = nodeB;
        }

        public VectorClockNode<T> NodeA { get; }
        public VectorClockNode<T> NodeB { get; }
    }
}
