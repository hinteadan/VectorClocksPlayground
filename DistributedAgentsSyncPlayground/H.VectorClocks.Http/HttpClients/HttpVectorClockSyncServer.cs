using H.VectorClocks.Http.DTO;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text;

namespace H.VectorClocks.Http.HttpClients
{
    public class HttpVectorClockSyncServer<T> : VectorClockSyncServer<T>
    {
        #region Construct
        public readonly Uri Url;
        Process iis;

        public HttpVectorClockSyncServer(string url) : base()
        {
            this.Url = new Uri(url);
        }
        #endregion

        public override void Start()
        {
            this.iis = Process.Start($"H.VectorClocks.Http.exe", this.Url.ToString());
        }

        public override void Stop()
        {
            if (iis.HasExited) return;
            iis.CloseMainWindow();
            iis.Close();
        }

        public override bool TryRegisterNode(VectorClockNode<T> node)
        {
            using (var http = new HttpClient())
            {
                StringContent json = new StringContent(JsonConvert.SerializeObject(VectorClockNodeDto<T>.FromModel(node)), Encoding.Default, "application/json");
                var response = http.PostAsync($"{Url}sync/register", json).Result;
                return response.IsSuccessStatusCode;
            }
        }

        public override void QueueEvent(VectorClockNode<T> eventSource)
        {
            AppState<T>.Current.VectorClockSyncServer.QueueEvent(eventSource);
        }
    }
}
