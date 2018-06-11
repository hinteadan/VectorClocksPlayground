using System.Linq;

namespace H.VectorClocks.Http.DTO
{
    public class VectorClockNodeDto<T>
    {
        public T Payload { get; set; }
        public string NodeID { get; set; }
        public ulong Version { get; set; }
        public VectorClockNodeVersionDto[] Revision { get; set; } = new VectorClockNodeVersionDto[0];


        public VectorClockNode<T> ToModel()
        {
            return new VectorClockNode<T>(NodeID, Payload, Revision.Select(x => x.ToModel()).ToArray());
        }

        public static VectorClockNodeDto<T> FromModel(VectorClockNode<T> model)
        {
            return new VectorClockNodeDto<T>
            {
                Payload = model.Payload,
                NodeID = model.NodeID,
                Version = model.Version,
                Revision = model.Revision.Select(VectorClockNodeVersionDto.FromModel).ToArray(),
            };
        }
    }
}
