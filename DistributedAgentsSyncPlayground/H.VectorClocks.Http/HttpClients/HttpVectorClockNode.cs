using System;
using System.Diagnostics;

namespace H.VectorClocks.Http.HttpClients
{
    public class HttpVectorClockNode<T> : VectorClockNode<T>, IDisposable
    {
        #region Construct
        private readonly Uri url;
        Process iis;

        public HttpVectorClockNode(string url, T payload, params VectorClockNodeVersion[] revision)
            : base(url, payload, revision)
        {
            this.url = new Uri(url);
            this.iis = Process.Start($"H.VectorClocks.Http.exe", this.url.ToString());
        }
        public HttpVectorClockNode(string url) : this(url, default(T))
        {
        }
        #endregion

        public void Dispose()
        {
            iis.CloseMainWindow();
            iis.Close();
        }
    }
}
