using H.VectorClocks.Http.DTO;
using H.VectorClocks.Http.HttpClients;
using Nancy;
using Nancy.ModelBinding;
using System;

namespace H.VectorClocks.Http.HttpModules
{
    public class SyncServerModule : NancyModule
    {
        public SyncServerModule() : base("/sync")
        {
            Get["/ping"] = _ => Response.AsText($"Alive @ {DateTime.Now}");

            Get["/"] = _ => View["Index.html", AppState<string>.Current];

            Post["/"] = x =>
            {
                VectorClockNodeDto<string> sender = this.Bind<VectorClockNodeDto<string>>();

                return HttpStatusCode.OK;
            };

            Post["/register"] = x =>
            {
                AppState<string>.Current.VectorClockSyncServer.Start();

                VectorClockNode<string> sender = this.Bind<VectorClockNodeDto<string>>().ToModel();

                HttpVectorClockNode<string> node = new HttpVectorClockNode<string>(sender.NodeID, Request.Url.BasePath, sender.Payload, sender.Revision);

                bool isSuccess = AppState<string>.Current.VectorClockSyncServer.TryRegisterNode(node);

                return isSuccess ? HttpStatusCode.OK : HttpStatusCode.BadRequest;
            };
        }
    }
}
