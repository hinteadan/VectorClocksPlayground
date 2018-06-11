namespace H.VectorClocks
{
    public interface ImAVectorClockNode<T>
    {
        T Payload { get; }
        string NodeID { get; }
        ulong Version { get; }
        VectorClockNodeVersion[] Revision { get; }

        ulong VersionOf(string nodeId);

        VectorClockSyncResult<T> Acknowledge(ImAVectorClockNode<T> vectorClock);
        ImAVectorClockNode<T> Say(T payload);
    }
}
