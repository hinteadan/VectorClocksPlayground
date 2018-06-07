namespace H.VectorClocks
{
    public interface ImAVectorClockConflictResolver<T>
    {
        VectorClockSyncResult<T> ResolveConflict(VectorClockSyncResult<T> conflict);
    }
}
