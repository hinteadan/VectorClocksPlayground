namespace H.VectorClocks.Http
{
    internal class AppState<T>
    {
        public VectorClockNode<T> VectorClockNode;
        public VectorClockSyncServer<T> VectorClockSyncServer;
        public string SyncServerHead => VectorClockSyncServer?.Head?.ToString() ?? "[No Registered Nodes]";

        public static AppState<T> Current = new AppState<T>();
    }
}
