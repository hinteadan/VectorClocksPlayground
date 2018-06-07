namespace H.VectorClocks
{
    public interface ImAVectorClockNode<T>
    {
        T Payload { get; }
        string NodeID { get; }
        long Version { get; }
        VectorClockNodeVersion[] Revision { get; }

        long VersionOf(string nodeId);

        VectorClockSyncResult<T> Acknowledge(ImAVectorClockNode<T> vectorClock);
        ImAVectorClockNode<T> Say(T payload);
    }
}
