namespace H.VectorClocks.Http
{
    internal class AppState<T>
    {
        public VectorClockNode<T> VectorClockNode;
        public VectorClockSyncResult<T> LatestSync;
        public VectorClockSyncServer<T> VectorClockSyncServer;
        public string SyncServerHead => VectorClockSyncServer?.Head?.ToString() ?? "[Zero Action]";
        public string SyncServerMesh => VectorClockSyncServer?.ToString() ?? "[No Sync Server Running]";

        public static AppState<T> Current = new AppState<T>();
    }
}
