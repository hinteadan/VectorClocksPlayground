using System;
using System.Diagnostics;

namespace H.VectorClocks.Http.HttpClients
{
    public class HttpVectorClockSyncServer<T> : VectorClockSyncServer<T>
    {
        #region Construct
        private readonly Uri url;
        Process iis;

        public HttpVectorClockSyncServer(string url ) : base()
        {
            this.url = new Uri(url);
        }
        #endregion

        public override void Start()
        {
            base.Start();
            this.iis = Process.Start($"H.VectorClocks.Http.exe", this.url.ToString());
        }

        public override void Stop()
        {
            base.Stop();
            iis.CloseMainWindow();
            iis.Close();
        }
    }
}
