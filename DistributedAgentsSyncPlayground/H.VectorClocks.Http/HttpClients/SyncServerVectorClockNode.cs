using H.VectorClocks.Http.DTO;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;

namespace H.VectorClocks.Http.HttpClients
{
    public class SyncServerVectorClockNode<T> : VectorClockNode<T>
    {
        #region Construct
        protected readonly Uri url;
        protected readonly Uri syncServerUrl;
        public SyncServerVectorClockNode(string url, string syncServerUrl, T payload, params VectorClockNodeVersion[] revision)
            : base(url, payload, revision)
        {
            this.url = new Uri(url);
            this.syncServerUrl = new Uri(syncServerUrl);
        }
        public SyncServerVectorClockNode(string url, string syncServerUrl) : this(url, syncServerUrl, default(T))
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
            var result = AppState<T>.Current.VectorClockNode.Acknowledge(vectorClock);

            return result;
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
