namespace DistributedAgentsSyncPlayground.Http
{
    public class HttpVectorClockNode : ConsoleVectorClockNode
    {
        #region Construct
        public HttpVectorClockNode(string nodeId) : base(nodeId)
        {
        }

        public HttpVectorClockNode(string nodeId, string payload, params VectorClockNodeVersion[] revision) : base(nodeId, payload, revision)
        {
        }
        #endregion
    }
}
