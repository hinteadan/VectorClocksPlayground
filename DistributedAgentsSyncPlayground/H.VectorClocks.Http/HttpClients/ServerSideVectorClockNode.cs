using H.VectorClocks.Http.DTO;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;

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
            AppState<T>.Current.VectorClockNode.Say(payload);
            return this;
        }

        public override VectorClockSyncResult<T> Acknowledge(VectorClockNode<T> vectorClock)
        {
            var result = base.Acknowledge(vectorClock);
            result = AppState<T>.Current.VectorClockNode.Acknowledge(vectorClock);
            NotifyRemoteNode();
            return result;
        }

        private void NotifyRemoteNode()
        {
            using (var http = new HttpClient())
            {
                StringContent json = new StringContent(JsonConvert.SerializeObject(VectorClockNodeDto<T>.FromModel(AppState<T>.Current.VectorClockNode)), Encoding.Default, "application/json");
                http.PutAsync($"{syncServerUrl}/ack", json).Result.EnsureSuccessStatusCode();
            }
        }
    }
}
