using Nancy;
using System;

namespace H.VectorClocks.Http.HttpModules
{
    public class SyncServerModule : NancyModule
    {
        public SyncServerModule() : base("/sync")
        {
            Get["/ping"] = _ => Response.AsText($"Alive @ {DateTime.Now}");
        }
    }
}
