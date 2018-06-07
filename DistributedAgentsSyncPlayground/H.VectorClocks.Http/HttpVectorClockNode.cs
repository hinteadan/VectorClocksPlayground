using H.VectorClocks;
using Nancy.Hosting.Self;
using System;

namespace DistributedAgentsSyncPlayground.Http
{
    public class HttpVectorClockNode : VectorClockNode<string>, IDisposable
    {
        private readonly NancyHost httpServer = new NancyHost();

        #region Construct
        public HttpVectorClockNode(string url) : this(url, null)
        {
            
        }

        public HttpVectorClockNode(string url, string payload, params VectorClockNodeVersion[] revision) : base(url, payload, revision)
        {
            httpServer = new NancyHost(new Uri(url));
            httpServer.Start();
        }
        #endregion

        public void Dispose()
        {
            httpServer.Stop();
            httpServer.Dispose();
        }
    }
}
