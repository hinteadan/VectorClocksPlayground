using H.VectorClocks;
using Nancy.Hosting.Self;
using System;

namespace DistributedAgentsSyncPlayground.Http
{
    public class HttpVectorClockNode : ConsoleVectorClockNode, IDisposable
    {
        private readonly NancyHost httpServer = new NancyHost();

        #region Construct
        public HttpVectorClockNode(string nodeId) : base(nodeId)
        {
        }

        public HttpVectorClockNode(string nodeId, string payload, params VectorClockNodeVersion[] revision) : base(nodeId, payload, revision)
        {
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
