using H.VectorClocks.Http.DTO;
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
                VectorClockNode<string> sender = this.Bind<VectorClockNodeDto<string>>().ToModel();

                AppState<string>.Current.LatestSync = AppState<string>.Current.VectorClockNode.Acknowledge(sender);

                return HttpStatusCode.Accepted;
            };
        }
    }
}
