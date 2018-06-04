namespace DistributedAgentsSyncPlayground
{
    public interface ImAVectorClockNode<T>
    {
        T Payload { get; }
        string NodeID { get; }
        long Version { get; }
        VectorClockNodeVersion[] Revision { get; }

        long VersionOf(string nodeId);

        VectorClockSyncResult<T> Acknowledge(VectorClockNode<T> vectorClock);
        VectorClockNode<T> Say(T payload);
    }
}
