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

            Post["/register"] = x =>
            {
                AppState<string>.Current.VectorClockSyncServer = AppState<string>.Current.VectorClockSyncServer ?? new VectorClockSyncServer<string>(new DelegateToClientConflictResolver<string>());

                AppState<string>.Current.VectorClockSyncServer.Start();

                VectorClockNode<string> sender = this.Bind<VectorClockNodeDto<string>>().ToModel();

                ServerSideVectorClockNode<string> node = new ServerSideVectorClockNode<string>(sender.NodeID, Request.Url.SiteBase, sender.Payload, sender.Revision);

                bool isSuccess = AppState<string>.Current.VectorClockSyncServer.TryRegisterNode(node);

                return isSuccess ? HttpStatusCode.OK : HttpStatusCode.BadRequest;
            };

            Put["/"] = x =>
            {
                AppState<string>.Current.VectorClockSyncServer = AppState<string>.Current.VectorClockSyncServer ?? new VectorClockSyncServer<string>(new DelegateToClientConflictResolver<string>());

                VectorClockNode<string> sender = this.Bind<VectorClockNodeDto<string>>().ToModel();

                AppState<string>.Current.VectorClockSyncServer.QueueEvent(sender);

                return HttpStatusCode.Accepted;
            };
        }
    }
}
