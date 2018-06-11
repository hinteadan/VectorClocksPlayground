namespace H.VectorClocks.Http.DTO
{
    public class VectorClockNodeVersionDto
    {
        public string NodeID { get; set; }
        public ulong Version { get; set; }

        public VectorClockNodeVersion ToModel()
        {
            return new VectorClockNodeVersion(NodeID, Version);
        }

        public static VectorClockNodeVersionDto FromModel(VectorClockNodeVersion model)
        {
            return new VectorClockNodeVersionDto
            {
                NodeID = model.NodeID,
                Version = model.Version,
            };
        }
    }
}
