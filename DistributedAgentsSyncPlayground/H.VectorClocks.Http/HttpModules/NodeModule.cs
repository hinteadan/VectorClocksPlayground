using Nancy;
using System;

namespace H.VectorClocks.Http.HttpModules
{
    public class NodeModule : NancyModule
    {
        public NodeModule() : base("/")
        {
            Get["/"] = _ => View["Index.html", AppState<string>.Current];
            Get["/ping"] = _ => Response.AsText($"Alive @ {DateTime.Now}");
        }
    }
}
