using H.VectorClocks.Http.DTO;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading;

namespace H.VectorClocks.Http.HttpClients
{
    public class ServerSideVectorClockNode<T> : VectorClockNode<T>
    {
        #region Construct
        private readonly Uri url;
        private readonly Uri syncServerUrl;
        public ServerSideVectorClockNode(string url, string syncServerUrl, T payload, params VectorClockNodeVersion[] revision)
            : base(url, payload, revision)
        {
            this.url = new Uri(url);
            this.syncServerUrl = new Uri(syncServerUrl);
        }
        public ServerSideVectorClockNode(string url, string syncServerUrl) : this(url, syncServerUrl, default(T))
        {
        }
        #endregion

        public override VectorClockNode<T> Say(T payload)
        {
            base.Say(payload);
            NotifySyncServer();
            return this;
        }

        public override VectorClockSyncResult<T> Acknowledge(VectorClockNode<T> vectorClock)
        {
            var result = base.Acknowledge(vectorClock);
            if(AppState<T>.Current.VectorClockNode != this)
                NotifyRemoteNode();
            return result;
        }

        private void NotifyRemoteNode()
        {
            Thread.Sleep(AppState<T>.DelayByMs);

            using (var http = new HttpClient())
            {
                StringContent json = new StringContent(JsonConvert.SerializeObject(VectorClockNodeDto<T>.FromModel(this)), Encoding.Default, "application/json");
                http.PutAsync($"{url}/ack", json).Result.EnsureSuccessStatusCode();
            }
        }

        public void NotifySyncServer()
        {
            Thread.Sleep(AppState<T>.DelayByMs);

            using (var http = new HttpClient())
            {
                StringContent json = new StringContent(JsonConvert.SerializeObject(VectorClockNodeDto<T>.FromModel(this)), Encoding.Default, "application/json");
                http.PutAsync($"{syncServerUrl}/sync", json).Result.EnsureSuccessStatusCode();
            }
        }
    }
}
