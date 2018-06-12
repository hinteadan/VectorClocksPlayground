using H.VectorClocks.Http.DTO;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading;

namespace H.VectorClocks.Http.HttpClients
{
    public class ClientSideHttpVectorClockNode<T> : VectorClockNode<T>, IDisposable
    {
        #region Construct
        private readonly Uri url;
        private readonly Uri syncServerUrl;
        Process iis;

        public ClientSideHttpVectorClockNode(string url, string syncServerUrl, T payload, params VectorClockNodeVersion[] revision)
            : base(url, payload, revision)
        {
            this.url = new Uri(url);
            this.syncServerUrl = new Uri(syncServerUrl);
            this.iis = Process.Start($"H.VectorClocks.Http.exe", this.url.ToString());
            AppState<T>.Current.VectorClockNode = this;
        }
        public ClientSideHttpVectorClockNode(string url, string syncServerUrl) 
            : this(url, syncServerUrl, default(T))
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
            Thread.Sleep(3000);

            using (var http = new HttpClient())
            {
                StringContent json = new StringContent(JsonConvert.SerializeObject(VectorClockNodeDto<T>.FromModel(AppState<T>.Current.VectorClockNode)), Encoding.Default, "application/json");
                http.PutAsync($"{syncServerUrl}/sync", json).Result.EnsureSuccessStatusCode();
            }
        }
    }
}
