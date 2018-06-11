using H.VectorClocks.Http.DTO;
using H.VectorClocks.Http.HttpClients;
using Nancy;
using Nancy.ModelBinding;
using System;

namespace H.VectorClocks.Http.HttpModules
{
    public class NodeModule : NancyModule
    {
        public NodeModule() : base("/")
        {
            Get["/"] = _ => View["Index.html", AppState<string>.Current];
            Get["/ping"] = _ => Response.AsText($"Alive @ {DateTime.Now}");

            Put["/ack"] = x =>
            {
                AppState<string>.Current.VectorClockNode = AppState<string>.Current.VectorClockNode ?? new ServerSideVectorClockNode<string>(Request.Url.SiteBase, "http://localhost:60000");

                VectorClockNode<string> sender = this.Bind<VectorClockNodeDto<string>>().ToModel();

                AppState<string>.Current.LatestSync = AppState<string>.Current.VectorClockNode.Acknowledge(sender);

                return HttpStatusCode.Accepted;
            };

            Post["/say"] = x =>
            {
                AppState<string>.Current.VectorClockNode = AppState<string>.Current.VectorClockNode ?? new ServerSideVectorClockNode<string>(Request.Url.SiteBase, "http://localhost:60000");

                Telegram telegram = this.Bind<Telegram>();

                AppState<string>.Current.VectorClockNode.Say(telegram.Message);

                return HttpStatusCode.OK;
            };
        }
    }

    public class Telegram
    {
        public DateTime At { get; set; }
        public string Message { get; set; }
    }
}
