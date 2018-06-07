namespace H.VectorClocks
{
    public class VectorClockNodeVersion
    {
        public VectorClockNodeVersion(string nodeID, long version)
        {
            this.NodeID = nodeID;
            this.Version = version;
        }

        public string NodeID { get; }
        public long Version { get; private set; }

        public void Increment()
        {
            Version++;
        }
    }
}
