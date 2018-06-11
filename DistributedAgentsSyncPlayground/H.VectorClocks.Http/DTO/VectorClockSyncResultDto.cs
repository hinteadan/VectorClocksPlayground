namespace H.VectorClocks.Http.DTO
{
    public class VectorClockSyncResultDto<T>
    {
        public VectorClockNodeDto<T> CurrentNode { get; set; }
        public VectorClockNodeDto<T> Solution { get; set; }
        public VectorClockConflictDto<T> Conflict { get; set; }

        public VectorClockSyncResult<T> ToModel()
        {
            if (Solution != null)
                return VectorClockSyncResult<T>.Successfull(CurrentNode.ToModel(), Solution.ToModel());
            return VectorClockSyncResult<T>.Conflictual(CurrentNode.ToModel(), Conflict.ToModel());
        }

        public static VectorClockSyncResultDto<T> FromModel(VectorClockSyncResult<T> model)
        {
            return new VectorClockSyncResultDto<T>
            {
                Solution = VectorClockNodeDto<T>.FromModel(model.Solution),
                Conflict = VectorClockConflictDto<T>.FromModel(model.Conflict),
                CurrentNode = VectorClockNodeDto<T>.FromModel(model.CurrentNode),
            };
        }
    }
}
