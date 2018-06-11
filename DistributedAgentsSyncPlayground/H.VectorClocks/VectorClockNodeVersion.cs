namespace H.VectorClocks
{
    public class VectorClockNodeVersion
    {
        public VectorClockNodeVersion(string nodeID, ulong version)
        {
            this.NodeID = nodeID;
            this.Version = version;
        }

        public string NodeID { get; }
        public ulong Version { get; private set; }

        public void Increment()
        {
            Version++;
        }
    }
}
