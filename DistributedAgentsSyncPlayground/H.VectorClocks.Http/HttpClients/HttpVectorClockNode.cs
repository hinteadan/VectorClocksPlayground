using H.VectorClocks.Http.DTO;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text;

namespace H.VectorClocks.Http.HttpClients
{
    public class HttpVectorClockNode<T> : VectorClockNode<T>, IDisposable
    {
        #region Construct
        private readonly Uri url;
        private readonly Uri syncServerUrl;
        Process iis;

        public HttpVectorClockNode(string url, string syncServerUrl, T payload, params VectorClockNodeVersion[] revision)
            : base(url, payload, revision)
        {
            this.url = new Uri(url);
            this.syncServerUrl = new Uri(syncServerUrl);
            this.iis = Process.Start($"H.VectorClocks.Http.exe", this.url.ToString());
        }
        public HttpVectorClockNode(string url, string syncServerUrl) : this(url, syncServerUrl, default(T))
        {
        }
        #endregion

        public override VectorClockNode<T> Say(T payload)
        {
            AppState<T>.Current.VectorClockNode.Say(payload);
            NotifySyncServer();
            return this;
        }

        public override VectorClockSyncResult<T> Acknowledge(VectorClockNode<T> vectorClock)
        {
            var result =  AppState<T>.Current.VectorClockNode.Acknowledge(vectorClock);

            return result;
        }

        public void Dispose()
        {
            if (iis.HasExited) return;
            iis.CloseMainWindow();
            iis.Close();
        }

        private void NotifySyncServer()
        {
            using (var http = new HttpClient())
            {
                StringContent json = new StringContent(JsonConvert.SerializeObject(VectorClockNodeDto<T>.FromModel(AppState<T>.Current.VectorClockNode)), Encoding.Default, "application/json");
                http.PostAsync(syncServerUrl, json).Wait();
            }
        }
    }
}
