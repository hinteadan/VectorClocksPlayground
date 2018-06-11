namespace H.VectorClocks.Http.DTO
{
    public class VectorClockConflictDto<T>
    {
        public VectorClockNodeDto<T> NodeA { get; set; }
        public VectorClockNodeDto<T> NodeB { get; set; }

        public VectorClockConflict<T> ToModel()
        {
            return new VectorClockConflict<T>(NodeA.ToModel(), NodeB.ToModel());
        }

        public static VectorClockConflictDto<T> FromModel(VectorClockConflict<T> model)
        {
            if (model == null) return null;

            return new VectorClockConflictDto<T>
            {
                NodeA = VectorClockNodeDto<T>.FromModel(model.NodeA),
                NodeB = VectorClockNodeDto<T>.FromModel(model.NodeB),
            };
        }
    }
}
