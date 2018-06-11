using H.VectorClocks.Http.DTO;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text;

namespace H.VectorClocks.Http.HttpClients
{
    public class HttpVectorClockNode<T> : SyncServerVectorClockNode<T>, IDisposable
    {
        #region Construct
        Process iis;

        public HttpVectorClockNode(string url, string syncServerUrl, T payload, params VectorClockNodeVersion[] revision)
            : base(url, syncServerUrl, payload, revision)
        {
            this.iis = Process.Start($"H.VectorClocks.Http.exe", this.url.ToString());
        }
        public HttpVectorClockNode(string url, string syncServerUrl) 
            : this(url, syncServerUrl, default(T))
        {
        }
        #endregion

        public void Dispose()
        {
            if (iis.HasExited) return;
            iis.CloseMainWindow();
            iis.Close();
        }
    }
}
