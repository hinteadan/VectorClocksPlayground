using Nancy;
using System;

namespace H.VectorClocks.Http.HttpModules
{
    public class NodeModule : NancyModule
    {
        public NodeModule() : base("/")
        {
            Get["/ping"] = _ => Response.AsText($"Alive @ {DateTime.Now}");
        }
    }
}
