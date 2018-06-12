namespace H.VectorClocks.Http
{
    internal class DelegateToClientConflictResolver<T> : ImAVectorClockConflictResolver<T>
    {
        public VectorClockSyncResult<T> ResolveConflict(VectorClockSyncResult<T> conflict) => conflict;
    }
}
